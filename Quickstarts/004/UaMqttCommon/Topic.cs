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

namespace UaMqttCommon
{
    public class Topic
    {
        public string? TopicPrefix { get; set; }
        public string? Encoding { get; set; }
        public string? MessageType { get; set; }
        public string? PublisherId { get; set; }
        public string? GroupName { get; set; }
        public string? WriterName { get; set; }

        public string Build()
        {
            StringBuilder topic = new();

            topic.Append(TopicPrefix);
            topic.Append(Encoding ?? "/json");
            topic.Append('/');
            topic.Append(MessageType);
            topic.Append('/');
            topic.Append(PublisherId);

            if (!String.IsNullOrEmpty(GroupName))
            {
                topic.Append('/');
                topic.Append(GroupName);

                if (!String.IsNullOrEmpty(WriterName))
                {
                    topic.Append('/');
                    topic.Append(WriterName);
                }
            }

            return topic.ToString();
        }

        public static Topic Parse(string topic, string prefix = "opcua")
        {
            Topic parsed = new();

            int index = topic.IndexOf(prefix);

            if (index != 0)
            {
                throw new ArgumentException($"Topic must start with {prefix}.");
            }

            topic = topic[(prefix.Length + 1)..^0];
            parsed.TopicPrefix = prefix;

            string[] parts = topic.Split('/');

            if (parts.Length > 0)
            {
                parsed.Encoding = parts[0];

                if (parts.Length > 1)
                {
                    parsed.MessageType = parts[1];

                    if (parts.Length > 2)
                    {
                        parsed.PublisherId = parts[2];

                        if (parts.Length > 3)
                        {
                            parsed.GroupName = parts[3];

                            if (parts.Length > 4)
                            {
                                parsed.WriterName = parts[4];
                            }
                        }
                    }
                }
            }

            return parsed;
        }
    }

    public static class MessageTypes
    {
        public const string Status = "status";
        public const string Data = "data";
        public const string Connection = "connection";
        public const string DataSetMetaData = "metadata";
    }
}
