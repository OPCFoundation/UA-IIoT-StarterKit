using MQTTnet;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Opc.Ua;
using System.Buffers;
using System.Collections;
using System.IO.Compression;
using System.Text;

namespace UaPubSubCommon
{
    public class PubSubField
    {
        public string Name;
        public BuiltInType BuiltInType;
        public int ValueRank;
        public NodeId DataTypeId;
        public DataValue Value;
        public Dictionary<string, Variant> Properties;
    }

    public static class PubSubUtils
    {
        private static readonly DateTime kBaseDateTime = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static readonly UTF8Encoding UTF8 = new UTF8Encoding(false);

        public static async Task<byte[]> Compress(MemoryStream istrm)
        {
            using (var ostrm = new MemoryStream())
            {
                using (var zstrm = new GZipStream(ostrm, CompressionMode.Compress))
                {
                    await istrm.CopyToAsync(zstrm);
                    zstrm.Close();
                    return ostrm.ToArray();
                }
            }
        }

        public static async Task<MemoryStream> Decompress(Stream istrm)
        {
            using (var zstrm = new GZipStream(istrm, CompressionMode.Decompress))
            {
                var ostrm = new MemoryStream();
                await zstrm.CopyToAsync(ostrm);
                ostrm.Position = 0;
                return ostrm;
            }
        }

        public static async Task<Variant> ReadValue(FieldMetaData field, CancellationToken ct)
        {
            do
            {
                try
                {
                    switch ((BuiltInType)field.BuiltInType)
                    {
                        case BuiltInType.Boolean: return await ReadValue<bool>(BuiltInType.Boolean, ct);
                        case BuiltInType.SByte: return await ReadValue<sbyte>(BuiltInType.SByte, ct);
                        case BuiltInType.Byte: return await ReadValue<byte>(BuiltInType.Byte, ct);
                        case BuiltInType.Int16: return await ReadValue<short>(BuiltInType.Int16, ct);
                        case BuiltInType.UInt16: return await ReadValue<ushort>(BuiltInType.UInt16, ct);
                        case BuiltInType.Int32: return await ReadValue<int>(BuiltInType.Int32, ct);
                        case BuiltInType.UInt32: return await ReadValue<uint>(BuiltInType.UInt32, ct);
                        case BuiltInType.Int64: return await ReadValue<int>(BuiltInType.Int64, ct);
                        case BuiltInType.UInt64: return await ReadValue<uint>(BuiltInType.UInt64, ct);
                        case BuiltInType.Float: return await ReadValue<float>(BuiltInType.Float, ct);
                        case BuiltInType.Double: return await ReadValue<double>(BuiltInType.Double, ct);
                        case BuiltInType.Guid: return (Guid)await ReadValue<Uuid>(BuiltInType.Guid, ct);
                        case BuiltInType.StatusCode: return await ReadValue<StatusCode>(BuiltInType.StatusCode, ct);
                        case BuiltInType.Enumeration: return await ReadValue<Int32>(BuiltInType.Int32, ct);
                        case BuiltInType.DateTime: return await ReadValue<DateTime>(BuiltInType.DateTime, ct);
                    }

                    return await ReadValue<String>(BuiltInType.String, ct);
                }
                catch (TaskCanceledException)
                {
                    throw;
                }
                catch (Exception)
                {
                    // ignore.
                }
            }
            while (true);
        }

        public static async Task<T> ReadChoice<T>(IList<T> choices, CancellationToken ct)
        {
            do
            {
                try
                {
                    do
                    {
                        Console.Write(">>>");
                        var choice = await ReadLine(ct);

                        if (choice == "X" || choice == "x")
                        {
                            return default;
                        }

                        if (Byte.TryParse(choice, out var value) && value <= choices.Count)
                        {
                            return choices[value - 1];
                        }
                    }
                    while (true);
                }
                catch (TaskCanceledException)
                {
                    throw;
                }
                catch (Exception)
                {
                    // ignore.
                }
            }
            while (true);
        }

        public static async Task<T> ReadValue<T>(BuiltInType type, CancellationToken ct)
        {
            string choice = null;

            do
            {
                Console.Write(">>>");
                choice = await ReadLine(ct);

                try
                {
                    return (T)TypeInfo.Cast(choice, type);
                }
                catch
                {
                    // continue
                }
            }
            while (true);
        }

        private static async Task<string> ReadLine(CancellationToken ct)
        {
            StringBuilder buffer = new();

            while (!ct.IsCancellationRequested)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(false);

                    if (key.Key == ConsoleKey.Enter)
                    {
                        return buffer.ToString().Trim();
                    }

                    buffer.Append(key.KeyChar);
                }

                await Task.Delay(100); 
            }

            throw new TaskCanceledException();
        }

        public static uint GetVersionTime()
        {
            var ticks = (DateTime.UtcNow - kBaseDateTime).TotalMilliseconds;
            return (uint)ticks;
        }

        public static List<PubSubField> ReadFields(DataSetMetaDataType metadata)
        {
            List<PubSubField> fields = new();

            if (metadata?.Fields != null)
            {
                foreach (var ii in metadata.Fields)
                {
                    var field = new PubSubField()
                    {
                        Name = ii.Name,
                        BuiltInType = (BuiltInType)ii.BuiltInType,
                        ValueRank = ii.ValueRank,
                        Value = new DataValue() { StatusCode = StatusCodes.BadWaitingForInitialData },
                        Properties = new()
                    };

                    foreach (var property in ii?.Properties)
                    {
                        field.Properties[property.Key.Name] = property.Value;
                    }

                    fields.Add(field);
                }
            }

            return fields;
        }

        public static async Task<Dictionary<string, PubSubField>> ReadFieldValues(
            IServiceMessageContext context,
            JsonReader reader,
            IList<PubSubField> knownFields,
            CancellationToken ct)
        {
            Dictionary<string, PubSubField> fields = new();

            if (await reader.ReadAsync(ct) && reader.TokenType == JsonToken.StartObject)
            {
                while (await reader.ReadAsync(ct) && reader.TokenType == JsonToken.PropertyName)
                {
                    var fieldName = reader.Value as string;

                    var field = knownFields?.Where(x => x.Name == fieldName).FirstOrDefault();

                    if (field == null)
                    {
                        field = new PubSubField()
                        {
                            Name = fieldName,
                            BuiltInType = BuiltInType.Variant,
                            ValueRank = ValueRanks.Scalar,
                            DataTypeId = DataTypeIds.BaseDataType,
                            Properties = new()
                        };
                    }

                    fields[fieldName] = field;

                    try
                    {
                        field.Value = await ReadDataValue(context, reader, field, ct);
                    }
                    catch (Exception e)
                    {
                        Log.Error($"[{fieldName}]: ({e.GetType().Name}) {e.Message}.");
                        field.Value = new DataValue() { StatusCode = StatusCodes.BadDecodingError };
                    }
                }
            }

            return fields;
        }

        public static bool IsDataValue(JObject jobject)
        {
            if (jobject.Count == 0) return false;

            foreach (var x in jobject)
            {
                switch (x.Key)
                {
                    case "UaType":
                    case nameof(DataValue.Value):
                    case nameof(DataValue.StatusCode):
                    case nameof(DataValue.SourceTimestamp):
                    case nameof(DataValue.SourcePicoseconds):
                    case nameof(DataValue.ServerTimestamp):
                    case nameof(DataValue.ServerPicoseconds):
                    {
                        break;
                    }


                    default:
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static StatusCode ReadStatusCode(JToken jtoken)
        {
            if (jtoken is JObject jobject && jobject.Count <= 2 && jobject.ContainsKey(nameof(StatusCode.Code)))
            {
                return new StatusCode((uint)(jobject[nameof(StatusCode.Code)].Value<long>()));
            }

            return StatusCodes.Good;
        }

        public static ExtensionObject ReadExtensionObject(IServiceMessageContext context, JsonDecoder decoder, JToken jtoken, NodeId dataTypeId)
        {
            NodeId typeId = dataTypeId;

            // check for TypeId specified inline (typical for subtypes of the DataType).
            if (jtoken is JObject jobject)
            {
                if (jobject.TryGetValue("UaTypeId", out var token))
                {
                    typeId = NodeId.Parse(context, token.Value<string>());
                }
            }

            // check if a pre-generated class exists.
            var type = context.Factory.GetSystemType(typeId);

            if (type != null)
            {
                var eo = Activator.CreateInstance(type) as IEncodeable;
                eo.Decode(decoder);
                return new ExtensionObject(typeId, eo);
            }

            // return as JObject if no pre-generated class exists.
            else
            {
                return new ExtensionObject(typeId, jtoken);
            }
        }

        public static async Task<DataValue> ReadDataValue(IServiceMessageContext context, JsonReader reader, PubSubField field, CancellationToken ct)
        {
            var dv = new DataValue();

            if (await reader.ReadAsync(ct))
            {
                if (reader.TokenType == JsonToken.StartObject)
                {
                    var jobject = JObject.Load(reader);

                    if (IsDataValue(jobject))
                    {
                        dv = new DataValue()
                        {
                            StatusCode = ReadStatusCode(jobject[nameof(StatusCode.Code)]),
                            SourceTimestamp = jobject[nameof(DataValue.SourceTimestamp)]?.Value<DateTime>() ?? DateTime.MinValue,
                            SourcePicoseconds = jobject[nameof(DataValue.SourcePicoseconds)]?.Value<ushort>() ?? 0,
                            ServerTimestamp = jobject[nameof(DataValue.ServerTimestamp)]?.Value<DateTime>() ?? DateTime.MinValue,
                            ServerPicoseconds = jobject[nameof(DataValue.ServerPicoseconds)]?.Value<ushort>() ?? 0
                        };

                        reader = jobject[nameof(DataValue.Value)].CreateReader();
                    }
                    else
                    {
                        reader = jobject.CreateReader();
                    }
                }

                if (field == null)
                {
                    dv.WrappedValue = await ReadUntypedValue(reader, ct);
                }
                else
                {
                    var value = await ReadTypedValue(context, reader, field, ct);

                    if (value.TypeInfo.BuiltInType == BuiltInType.StatusCode && field.BuiltInType != BuiltInType.StatusCode)
                    {
                        dv.StatusCode = (StatusCode)value.Value;
                        dv.WrappedValue = Variant.Null;
                    }
                    else
                    {
                        dv.WrappedValue = value; 
                    }
                }
            }

            return dv;
        }

        public static async Task<Variant> ReadTypedValue(IServiceMessageContext context, JsonReader reader, PubSubField field, CancellationToken ct)
        {
            var jtoken = await JToken.LoadAsync(reader, ct);

            // check for a StatusCode.
            if (
                jtoken is JObject status && 
                status.Count <= 2 && 
                status.TryGetValue(nameof(StatusCode.Code), out var code) &&
                code.Type == JTokenType.Integer)
            {
                return new Variant(new StatusCode((uint)code));
            }

            const string placeholder = "X";

            using (var decoder = new JsonDecoder($"{{\"{placeholder}\": {jtoken.ToString(Formatting.None)}}}", context))
            {
                if (field.ValueRank == ValueRanks.Scalar)
                {
                    switch (field.BuiltInType)
                    {
                        case BuiltInType.Boolean: { return decoder.ReadBoolean(placeholder); }
                        case BuiltInType.SByte: { return decoder.ReadSByte(placeholder); }
                        case BuiltInType.Byte: { return decoder.ReadByte(placeholder); }
                        case BuiltInType.Int16: { return decoder.ReadInt16(placeholder); }
                        case BuiltInType.UInt16: { return decoder.ReadUInt16(placeholder); }
                        case BuiltInType.Int32: { return decoder.ReadInt32(placeholder); }
                        case BuiltInType.UInt32: { return decoder.ReadUInt32(placeholder); }
                        case BuiltInType.Int64: { return decoder.ReadInt32(placeholder); }
                        case BuiltInType.Float: { return decoder.ReadFloat(placeholder); }
                        case BuiltInType.Double: { return decoder.ReadDouble(placeholder); }
                        case BuiltInType.String: { return decoder.ReadString(placeholder); }
                        case BuiltInType.ByteString: { return decoder.ReadByteString(placeholder); }
                        case BuiltInType.DateTime: { return decoder.ReadDateTime(placeholder); }
                        case BuiltInType.Guid: { return decoder.ReadGuid(placeholder); }
                        case BuiltInType.NodeId: { return decoder.ReadNodeId(placeholder); }
                        case BuiltInType.ExpandedNodeId: { return decoder.ReadExpandedNodeId(placeholder); }
                        case BuiltInType.QualifiedName: { return decoder.ReadQualifiedName(placeholder); }
                        case BuiltInType.LocalizedText: { return decoder.ReadLocalizedText(placeholder); }
                        case BuiltInType.StatusCode: { return decoder.ReadStatusCode(placeholder); }
                        case BuiltInType.XmlElement: { return decoder.ReadXmlElement(placeholder); }
                        case BuiltInType.ExtensionObject: { return ReadExtensionObject(context, decoder, jtoken, field.DataTypeId); }
                    }
                }
                else
                {
                    switch (field.BuiltInType)
                    {
                        case BuiltInType.Boolean: { return new Variant(decoder.ReadBooleanArray(placeholder)); }
                        case BuiltInType.SByte: { return new Variant(decoder.ReadSByteArray(placeholder)); }
                        case BuiltInType.Byte: { return new Variant(decoder.ReadByteArray(placeholder)); }
                        case BuiltInType.Int16: { return new Variant(decoder.ReadInt16Array(placeholder)); }
                        case BuiltInType.UInt16: { return new Variant(decoder.ReadUInt16Array(placeholder)); }
                        case BuiltInType.Int32: { return new Variant(decoder.ReadInt32Array(placeholder)); }
                        case BuiltInType.UInt32: { return new Variant(decoder.ReadUInt32Array(placeholder)); }
                        case BuiltInType.Int64: { return new Variant(decoder.ReadInt32Array(placeholder)); }
                        case BuiltInType.Float: { return new Variant(decoder.ReadFloatArray(placeholder)); }
                        case BuiltInType.Double: { return new Variant(decoder.ReadDoubleArray(placeholder)); }
                        case BuiltInType.String: { return new Variant(decoder.ReadStringArray(placeholder)); }
                        case BuiltInType.ByteString: { return new Variant(decoder.ReadByteStringArray(placeholder)); }
                        case BuiltInType.DateTime: { return new Variant(decoder.ReadDateTimeArray(placeholder)); }
                        case BuiltInType.Guid: { return new Variant(decoder.ReadGuidArray(placeholder)); }
                        case BuiltInType.NodeId: { return new Variant(decoder.ReadNodeIdArray(placeholder)); }
                        case BuiltInType.ExpandedNodeId: { return new Variant(decoder.ReadExpandedNodeIdArray(placeholder)); }
                        case BuiltInType.QualifiedName: { return new Variant(decoder.ReadQualifiedNameArray(placeholder)); }
                        case BuiltInType.LocalizedText: { return new Variant(decoder.ReadLocalizedTextArray(placeholder)); }
                        case BuiltInType.StatusCode: { return new Variant(decoder.ReadStatusCodeArray(placeholder)); }
                        case BuiltInType.XmlElement: { return new Variant(decoder.ReadXmlElementArray(placeholder)); }
                        case BuiltInType.Variant: { return new Variant(decoder.ReadVariantArray(placeholder)); }

                        case BuiltInType.ExtensionObject:
                        {
                            List<ExtensionObject> list = new();

                            if (jtoken is JArray jarray)
                            {
                                foreach (var ii in jarray)
                                {
                                    list.Add(ReadExtensionObject(context, decoder, jtoken, field.DataTypeId));
                                }
                            }

                            return new Variant(list);
                        }
                    }
                }
            }

            // default to untyped.
            return await ReadUntypedValue(jtoken.CreateReader(), ct);
        }

        public static async Task<Variant> ReadUntypedValue(JsonReader reader, CancellationToken ct)
        {
            var jtoken = await JToken.LoadAsync(reader, ct);

            switch (jtoken.Type)
            {
                default:
                case JTokenType.Guid:
                case JTokenType.Bytes:
                case JTokenType.Date:
                case JTokenType.String: return jtoken.Value<string>();

                case JTokenType.Integer: return jtoken.Value<long>();
                case JTokenType.Float: return jtoken.Value<double>();
                case JTokenType.Boolean: return jtoken.Value<bool>();
                case JTokenType.Null: return Variant.Null;

                case JTokenType.Array:
                {
                    List<Variant> list = new();

                    foreach (var ii in jtoken.Children())
                    {
                        list.Add(await ReadUntypedValue(ii.CreateReader(), ct));
                    }

                    return new Variant(list);
                }

                case JTokenType.Object:
                {
                    var jobject = jtoken as JObject;

                    // check for empty object.
                    if (jobject.Count == 0)
                    {
                        return Variant.Null;
                    }

                    // check for StatusCode.
                    if (jobject.Count <= 2 && jobject.ContainsKey(nameof(StatusCode.Code)))
                    {
                        return new StatusCode((uint)(jtoken[nameof(StatusCode.Code)]?.Value<long>() ?? 0));
                    }

                    // check for LocalizedText.
                    if (jobject.Count <= 2 &&
                        jobject.ContainsKey(nameof(LocalizedText.Text)) &&
                        jobject.ContainsKey(nameof(LocalizedText.Locale)))
                    {
                        return new LocalizedText(
                            jtoken[nameof(LocalizedText.Locale)]?.Value<string>(),
                            jtoken[nameof(LocalizedText.Text)]?.Value<string>());
                    }

                    // check for Variant.
                    if (jobject.ContainsKey(nameof(DataValue.Value)))
                    {
                        return await ReadUntypedValue(jobject[nameof(DataValue.Value)].CreateReader(), ct);
                    }

                    return new Variant(new ExtensionObject(jobject));
                }
            }
        }

        public static async Task<string> ParseMessage(MqttApplicationMessage message, CancellationToken ct)
        {
            var payload = message.Payload.ToArray();
            var istrm = new MemoryStream(payload);

            try
            {
                if (message.ContentType == null || message.ContentType == "application/json+gzip")
                {
                    // these bytes are well-known start to GZIP encoded data.
                    // this is a sanity check and deals with a missing or incorrect ContentType.
                    if (payload.Length > 2 && payload[0] == 0x1F && payload[1] == 0x8B)
                    {
                        using (var zstrm = new GZipStream(istrm, CompressionMode.Decompress, true))
                        {
                            var ostrm = new MemoryStream();
                            await zstrm.CopyToAsync(ostrm, ct);
                            ostrm.Position = 0;
                            istrm = ostrm;
                        }
                    }
                }

                istrm.Close();
                return UTF8.GetString(istrm.ToArray());
            }
            finally
            {
                istrm.Dispose();
            }
        }
    }
}
