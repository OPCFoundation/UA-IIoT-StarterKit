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
    public class JsonPubSubConnectionMessage
    {
        public JsonPubSubConnectionMessage()
        {
            MessageType = "ua-connection";
        }

        public string MessageId { get; set; }
        public string MessageType { get; set; }
        public string PublisherId { get; set; }
        public DateTime? Timestamp { get; set; }
        public PubSubConnectionDataType Connection { get; set; }

        public static JsonPubSubConnectionMessage Decode(IServiceMessageContext context, string json)
        {
            JsonPubSubConnectionMessage value = new();
            value.MessageType = null;

            using (var decoder = new JsonDecoder(json, context))
            {
                value.MessageId = decoder.ReadString(nameof(MessageId));
                value.MessageType = decoder.ReadString(nameof(MessageType));
                value.PublisherId = decoder.ReadString(nameof(PublisherId));
                value.Timestamp = decoder.ReadDateTime(nameof(Timestamp));
                if (value.Timestamp == DateTime.MinValue) value.Timestamp = null;
                value.Connection = (PubSubConnectionDataType)decoder.ReadEncodeable(nameof(Connection), typeof(PubSubConnectionDataType));
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
                if (Timestamp != null) encoder.WriteDateTime(nameof(Timestamp), Timestamp ?? DateTime.MinValue);

                encoder.WriteEncodeable(nameof(Connection), Connection, typeof(PubSubConnectionDataType));
                return encoder.CloseAndReturnText();
            }
        }
    }
}
