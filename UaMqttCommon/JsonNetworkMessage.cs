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
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Opc.Ua;

namespace UaMqttCommon
{
    public class JsonNetworkMessage : IJsonNetworkMessage
    {
        public JsonNetworkMessage()
        {
            MessageType = "ua-data";
        }

        public string MessageId { get; set; }
        public string MessageType { get; set; }
        public string PublisherId { get; set; }
        public string WriterGroupName { get; set; }
        public string DataSetClassId { get; set; }
        public List<JsonDataSetMessage> Messages { get; set; }

        [JsonIgnore]
        public bool ExcludeHeader { get; set; }

        [JsonIgnore]
        public bool SingleMessage { get; set; }

        private void DecodeMessages(IServiceMessageContext context, JsonNode node)
        {
            Messages = new();
            SingleMessage = !(node is JsonArray);

            if (!SingleMessage)
            {
                Messages = new();

                foreach (var message in node as JsonArray)
                {
                    Messages.Add(JsonDataSetMessage.Decode(context, message.ToJsonString()));
                }
            }
            else
            {
                Messages = new List<JsonDataSetMessage>
                {
                    JsonDataSetMessage.Decode(context, node.ToJsonString())
                };
            }
        }

        public static JsonNetworkMessage Decode(IServiceMessageContext context, string json, bool excludeHeader = false)
        {
            JsonNetworkMessage value = new();

            var root = JsonObject.Parse(json);

            if (excludeHeader || !root.AsObject().ContainsKey(nameof(JsonNetworkMessage.MessageId)))
            {
                value.ExcludeHeader = true;
                value.DecodeMessages(context, root);
                return value;
            }

            value.MessageType = null;

            foreach (var item in root.AsObject())
            {
                switch (item.Key)
                {
                    case nameof(MessageId):
                        value.MessageId = item.Value.AsString();
                        break;
                    case nameof(MessageType):
                        value.MessageType = item.Value.AsString();
                        break;
                    case nameof(PublisherId):
                        value.PublisherId = item.Value.ToString();
                        break;
                    case nameof(WriterGroupName):
                        value.WriterGroupName = item.Value.ToString();
                        break;
                    case nameof(DataSetClassId):
                        value.DataSetClassId = item.Value.ToString();
                        break;
                    case nameof(Messages):
                        value.DecodeMessages(context, item.Value);
                        break;
                };
            }

            return value;
        }

        private string EncodeMessages(IServiceMessageContext context)
        {
            if (SingleMessage)
            {
                if (Messages?.Count > 0 && Messages[0] != null)
                {
                    return Messages[0].Encode(context);
                }

                return "{}";
            }
            else
            {
                StringBuilder sb = new();
                sb.Append("[");

                if (Messages == null)
                {
                    return "[]";
                }

                foreach (var message in Messages)
                {
                    if (message != null)
                    {
                        if (sb.Length > 2)
                        {
                            sb.Append(",");
                        }

                        sb.Append(message.Encode(context));
                    }
                }

                sb.Append("]");
                return sb.ToString();
            }
        }

        public string Encode(IServiceMessageContext context)
        {
            if (ExcludeHeader)
            {
                return EncodeMessages(context);
            }

            StringBuilder sb = new();

            sb.Append("{");

            if (MessageId != null) sb.Append($"\"{nameof(MessageId)}\":\"{MessageId}\",");
            if (MessageType != null) sb.Append($"\"{nameof(MessageType)}\":\"{MessageType}\",");
            if (PublisherId != null) sb.Append($"\"{nameof(PublisherId)}\":\"{PublisherId}\",");
            if (WriterGroupName != null) sb.Append($"\"{nameof(WriterGroupName)}\":\"{WriterGroupName}\",");
            if (DataSetClassId != null) sb.Append($"\"{nameof(DataSetClassId)}\":\"{DataSetClassId}\",");

            if (Messages != null)
            {
                sb.Append($"\"{nameof(Messages)}\":{EncodeMessages(context)}");
            }

            return sb.Append("}").ToString();
        }
    }
}
