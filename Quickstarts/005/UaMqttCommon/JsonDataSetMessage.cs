/* ========================================================================
 * Copyright (c) 2005-2024 The OPC Foundation, Inc. All rights reserved.
 *
 * OPC Foundation MIT License 1.00
 * 
 * Permission is hereby granted, free of charge, to any person
 * obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use,
 * copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following
 * conditions:
 * 
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 *
 * The complete license agreement can be found here:
 * http://opcfoundation.org/License/MIT/1.00/
 * ======================================================================*/
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Opc.Ua;

namespace UaMqttCommon
{
    public class JsonDataSetMessage
    {
        public ushort? DataSetWriterId { get; set; }
        public string DataSetWriterName { get; set; }
        public string PublisherId { get; set; }
        public string WriterGroupName { get; set; }
        public uint? SequenceNumber { get; set; }
        public ConfigurationVersionDataType MetaDataVersion { get; set; }
        public uint? MinorVersion { get; set; }
        public DateTime? Timestamp { get; set; }
        public StatusCode? Status { get; set; }
        public string MessageType { get; set; }
        public Dictionary<string, JsonNode> Payload { get; set; }

        [JsonIgnore]
        public bool ExcludeHeader { get; set; }

        public static JsonDataSetMessage Decode(IServiceMessageContext context, string json, bool excludeHeader = false)
        {
            JsonDataSetMessage value = new();

            var root = JsonObject.Parse(json);

            var @object = root.AsObject();

            if (excludeHeader || !@object.ContainsKey(nameof(Payload)))
            {
                value.ExcludeHeader = true;
                value.Payload = @object.AsMap();
                return value;
            }

            value.ExcludeHeader = false;
            value.MessageType = null;

            foreach (var item in @object)
            {
                switch (item.Key)
                {
                    case nameof(DataSetWriterId):
                        value.DataSetWriterId = (ushort?)item.Value.AsUInteger();
                        break;
                    case nameof(DataSetWriterName):
                        value.DataSetWriterName = item.Value.ToString();
                        break;
                    case nameof(PublisherId):
                        value.PublisherId = item.Value.ToString();
                        break;
                    case nameof(WriterGroupName):
                        value.WriterGroupName = item.Value.ToString();
                        break;
                    case nameof(SequenceNumber):
                        value.SequenceNumber = (uint?)item.Value.AsUInteger();
                        break;
                    case nameof(MetaDataVersion):
                        value.MetaDataVersion = item.Value.AsEncodeable<ConfigurationVersionDataType>(context);
                        break;
                    case nameof(MinorVersion):
                        value.MinorVersion = (uint?)item.Value.AsUInteger();
                        break;
                    case nameof(Timestamp):
                        value.Timestamp = item.Value.AsDateTime();
                        break;
                    case nameof(Status):
                        value.Status = (StatusCode?)(uint?)item.Value.AsUInteger();
                        break;
                    case nameof(MessageType):
                        value.MessageType = item.Value.AsString();
                        break;
                    case nameof(Payload):
                        value.Payload = item.Value.AsMap();
                        break;
                };
            }

            return value;
        }

        public string Encode(IServiceMessageContext context)
        {
            if (ExcludeHeader)
            {
                JsonObject root = new();

                if (Payload != null)
                {
                    foreach (var item in Payload)
                    {
                        root.Add(item.Key, item.Value);
                    }
                }

                return root.ToJsonString();
            }

            return JsonSerializer.Serialize<JsonDataSetMessage>(this, new JsonSerializerOptions()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
            });
        }
    }

    public class DataSetField
    {
        public object Value { get; set; }
        public DateTime? SourceTimestamp { get; set; }
        public StatusCode? StatusCode { get; set; }

        public static DataSetField Decode(IServiceMessageContext context, string json)
        {
            DataSetField value = new();

            var root = JsonObject.Parse(json);

            if (root is JsonObject @object)
            {
                if (!@object.ContainsKey(nameof(Value)) && !@object.ContainsKey(nameof(StatusCode)))
                {
                    value.Value = @object;
                    return value;
                }

                foreach (var item in @object)
                {
                    switch (item.Key)
                    {
                        case nameof(Value):
                            value.Value = item.Value;
                            break;
                        case nameof(SourceTimestamp):
                            value.SourceTimestamp = item.Value.AsDateTime();
                            break;
                        case nameof(StatusCode):
                            value.StatusCode = (StatusCode?)(uint?)item.Value.AsUInteger();
                            break;
                    };
                }

                return value;
            }

            value.Value = root;
            return value;
        }
    }
}
