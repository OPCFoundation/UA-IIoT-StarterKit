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
using System.Text.Json.Nodes;
using Opc.Ua;

namespace UaMqttCommon
{
    static class Extensions
    {
        public static string AsString(this JsonNode node)
        {
            if (node == null) return null;

            switch (node)
            {
                case JsonValue value:
                    return value.ToString();
                default:
                    return node.ToJsonString();
            }
        }

        public static bool? AsBoolean(this JsonNode node)
        {
            if (node == null) return null;

            switch (node)
            {
                case JsonValue value:
                    if (value.TryGetValue(out bool number))
                    {
                        return number;
                    }
                    break;
            }

            return null;
        }

        public static ulong? AsUInteger(this JsonNode node)
        {
            if (node == null) return null;

            switch (node)
            {
                case JsonValue value:
                    if (value.TryGetValue(out ulong number))
                    {
                        return number;
                    }
                    break;
            }

            return null;
        }

        public static DateTime? AsDateTime(this JsonNode node)
        {
            if (node == null) return null;

            switch (node)
            {
                case JsonValue value:
                    if (value.TryGetValue(out DateTime dt))
                    {
                        return dt;
                    }
                    break;
            }

            return null;
        }

        public static T AsEncodeable<T>(this JsonNode node, IServiceMessageContext context) where T : IEncodeable
        {
            if (node == null) return default(T);

            switch (node)
            {
                case JsonObject value:
                    using (var decoder = new JsonDecoder(value.ToJsonString(), context))
                    {
                        return (T)decoder.ReadEncodeable(null, typeof(T));
                    }
            }

            return default(T);
        }

        public static Dictionary<string, JsonNode> AsMap(this JsonNode node)
        {
            Dictionary<string, JsonNode> map = new();

            if (node != null)
            {
                foreach (var field in node.AsObject())
                {
                    map.Add(field.Key, field.Value);
                }
            }

            return map;
        }
    }
}
