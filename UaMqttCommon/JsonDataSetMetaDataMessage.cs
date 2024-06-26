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
using Opc.Ua;

namespace UaMqttCommon
{
    public class JsonDataSetMetaDataMessage : IJsonNetworkMessage
    {
        public string MessageId { get; set; }
        public string MessageType { get; set; }
        public string PublisherId { get; set; }
        public ushort? DataSetWriterId { get; set; }
        public string DataSetWriterName { get; set; }
        public DateTime Timestamp { get; set; }
        public DataSetMetaDataType MetaData { get; set; }
        public JsonDataSetMetaDataMessage()
        {
            MessageType = "ua-metadata";
        }

        public static JsonDataSetMetaDataMessage Decode(IServiceMessageContext context, string json)
        {
            JsonDataSetMetaDataMessage value = new();
            value.MessageType = null;

            using (var decoder = new JsonDecoder(json, context))
            {
                value.MessageId = decoder.ReadString(nameof(MessageId));
                value.MessageType = decoder.ReadString(nameof(MessageType));
                value.PublisherId = decoder.ReadString(nameof(PublisherId));
                value.DataSetWriterId = decoder.ReadUInt16(nameof(DataSetWriterId));
                value.DataSetWriterName = decoder.ReadString(nameof(DataSetWriterName));
                value.Timestamp = decoder.ReadDateTime(nameof(Timestamp));
                value.MetaData = (DataSetMetaDataType)decoder.ReadEncodeable(nameof(MetaData), typeof(DataSetMetaDataType));
            }

            return value;
        }

        public string Encode(IServiceMessageContext context)
        {
            using (var encoder = new JsonEncoder(context, true))
            {
                if (MessageId != null) encoder.WriteString(nameof(MessageId), MessageId);
                if (MessageType != null) encoder.WriteString(nameof(MessageType), MessageType);
                if (PublisherId != null) encoder.WriteString(nameof(PublisherId), PublisherId);
                if (DataSetWriterId != null) encoder.WriteUInt16(nameof(DataSetWriterId), DataSetWriterId ?? 0);
                if (DataSetWriterName != null) encoder.WriteString(nameof(DataSetWriterName), DataSetWriterName);
                if (Timestamp != DateTime.MinValue) encoder.WriteDateTime(nameof(Timestamp), Timestamp);

                encoder.WriteEncodeable(nameof(MetaData), MetaData, typeof(DataSetMetaDataType));
                return encoder.CloseAndReturnText();
            }
        }
    }

    public static class DataSetMessageTypes
    {
        public const string KeyFrame = "ua-keyframe";
        public const string DeltaFrame = "ua-deltaframe";
        public const string KeepAlive = "ua-keepalive";
        public const string Event = "ua-event";
    }
}
