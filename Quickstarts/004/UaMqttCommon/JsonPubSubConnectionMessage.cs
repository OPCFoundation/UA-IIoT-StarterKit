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

namespace UaMqttCommon
{
    public class JsonPubSubConnectionMessage
    {
        public JsonPubSubConnectionMessage()
        {
            MessageType = "ua-connection";
        }

        public string? MessageId { get; set; }
        public string? MessageType { get; set; }
        public string? PublisherId { get; set; }
        public DateTime? Timestamp { get; set; }
        public PubSubConnectionDataType? Connection { get; set; }
    }

    public class PubSubConnectionDataType
    {
        public string? Name { get; set; }
        public bool? Enabled { get; set; }
        public Variant? PublisherId { get; set; }
        public List<WriterGroupDataType>? WriterGroups { get; set; }
    }

    public class JsonDataSetWriterMessageDataType
    {
        public static readonly NodeId TypeId = new(21128);
        public int? DataSetMessageContentMask { get; set; }

        #region Parsing
        public static JsonDataSetWriterMessageDataType? FromExtensionObject(JsonElement element)
        {
            if (!element.TryGetProperty("TypeId", out var typeIdElement))
            {
                return null;
            }

            var typeId = NodeId.Parse(typeIdElement);

            if (typeId == null || !typeId.Equals(TypeId))
            {
                return null;
            }

            if (!element.TryGetProperty("Body", out var body))
            {
                return null;
            }

            return (JsonDataSetWriterMessageDataType?)JsonSerializer.Deserialize(body.GetRawText(), typeof(JsonDataSetWriterMessageDataType));
        }
        #endregion
    }

    public class BrokerDataSetWriterTransportDataType
    {
        public static readonly NodeId TypeId = new(15669);
        public string? QueueName { get; set; }
        public string? MetaDataQueueName { get; set; }
        public double? MetaDataUpdateTime { get; set; }
    }

    public class DataSetWriterDataType
    {
        public string? Name { get; set; }
        public bool? Enabled { get; set; }
        public int? DataSetWriterId { get; set; }
        public int? DataSetFieldContentMask { get; set; }
        public int? KeyFrameCount { get; set; }
        public string? DataSetName { get; set; }
        public ExtensionObject<BrokerDataSetWriterTransportDataType>? TransportSettings { get; set; }
        public ExtensionObject<JsonDataSetWriterMessageDataType>? MessageSettings { get; set; }
    }

    public class JsonWriterGroupMessageDataType
    {
        public static readonly NodeId TypeId = new(15657);
        public int? NetworkMessageContentMask { get; set; }

        #region Parsing
        public static JsonWriterGroupMessageDataType? FromExtensionObject(JsonElement element)
        {
            if (!element.TryGetProperty("TypeId", out var typeIdElement))
            {
                return null;
            }

            var typeId = NodeId.Parse(typeIdElement);

            if (typeId == null || !typeId.Equals(TypeId))
            {
                return null;
            }

            if (!element.TryGetProperty("Body", out var body))
            {
                return null;
            }

            return (JsonWriterGroupMessageDataType?)JsonSerializer.Deserialize(body.GetRawText(), typeof(JsonWriterGroupMessageDataType));
        }
        #endregion
    }

    public class BrokerWriterGroupTransportDataType
    {
        public static readonly NodeId TypeId = new(15667);
        public string? QueueName { get; set; }
    }

    public class WriterGroupDataType
    {
        public string? Name { get; set; }
        public bool? Enabled { get; set; }
        public double? PublishingInterval { get; set; }
        public double? KeepAliveTime { get; set; }
        public string? HeaderLayoutUri { get; set; }
        public ExtensionObject<BrokerWriterGroupTransportDataType>? TransportSettings { get; set; }
        public ExtensionObject<JsonWriterGroupMessageDataType>? MessageSettings { get; set; }
        public List<DataSetWriterDataType>? DataSetWriters { get; set; }
    }
}
