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
    public class JsonDataSetMessage
    {
        public int? DataSetWriterId { get; set; }
        public string? DataSetWriterName { get; set; }
        public string? PublisherId { get; set; }
        public string? WriterGroupName { get; set; }
        public int? SequenceNumber { get; set; }
        public ConfigurationVersionDataType? MetaDataVersion { get; set; }
        public int? MinorVersion { get; set; }
        public DateTime? Timestamp { get; set; }
        public int? Status { get; set; }
        public string? MessageType { get; set; }
        public object? Payload { get; set; }

        public static JsonDataSetMessage? Parse(string json)
        {
            try
            {
                if (json.Contains(@"""Payload""") || json.Contains(@"""ua-keepalive"""))
                {
                    return (JsonDataSetMessage?)JsonSerializer.Deserialize(json, typeof(JsonDataSetMessage));
                }

                return null;
            }
            catch
            {
                Console.WriteLine($"Failed to deserialize DataSetMessage.");
                return null;
            }
        }

        public static List<JsonDataSetMessage>? ParseArray(object json)
        {
            try
            {
                var element = json as JsonElement?;

                if (element != null)
                {
                    List<JsonDataSetMessage> messages = new();

                    foreach (var item in element.Value.EnumerateArray())
                    {
                        var dm = Parse(item.GetRawText());

                        if (dm != null)
                        {
                            messages.Add(dm);
                        }
                    }

                    return messages;
                }

                return null;
            }
            catch
            {
                Console.WriteLine($"Failed to deserialize List<DataSetMessage>.");
                return null;
            }
        }
    }

    public class NetworkMessage
    {
        public NetworkMessage()
        {
            MessageType = "ua-data";
        }

        public string? MessageId { get; set; }
        public string? MessageType { get; set; }
        public string? PublisherId { get; set; }
        public string? WriterGroupName { get; set; }
        public string? DataSetClassId { get; set; }
        public object? Messages { get; set; }

        public static NetworkMessage? Parse(string json)
        {
            try
            {
                if (json.Contains(@"""Messages"""))
                {
                    return (NetworkMessage?)JsonSerializer.Deserialize(json, typeof(NetworkMessage));
                }

                return null;
            }
            catch
            {
                Console.WriteLine($"Failed to deserialize NetworkMessage.");
                return null;
            }
        }
    }
}
