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
    public enum BuiltInType : int
    {
        Null = 0,
        Boolean = 1,
        SByte = 2,
        Byte = 3,
        Int16 = 4,
        UInt16 = 5,
        Int32 = 6,
        UInt32 = 7,
        Int64 = 8,
        UInt64 = 9,
        Float = 10,
        Double = 11,
        String = 12,
        DateTime = 13,
        Guid = 14,
        ByteString = 15,
        XmlElement = 16,
        NodeId = 17,
        ExpandedNodeId = 18,
        StatusCode = 19,
        QualifiedName = 20,
        LocalizedText = 21,
        ExtensionObject = 22,
        DataValue = 23,
        Variant = 24
    }

    public class QualifiedName
    {
        public string? Name { get; set; }
        public object? Uri { get; set; }

        #region Parsing
        public static QualifiedName Parse(JsonElement element)
        {
            QualifiedName qualifiedName = new();

            if (element.TryGetProperty(nameof(Name), out var name))
            {
                qualifiedName.Name = name.ToString();
            }

            if (element.TryGetProperty(nameof(Uri), out var uri))
            {
                if (uri.TryGetInt16(out var index))
                {
                    qualifiedName.Uri = index;
                }
                else
                {
                    qualifiedName.Uri = uri.ToString();
                }
            }

            return qualifiedName;
        }
        #endregion
    }

    public class LocalizedText
    {
        public string? Locale { get; set; }
        public string? Text { get; set; }

        #region Parsing
        public static LocalizedText Parse(JsonElement element)
        {
            LocalizedText localizedText = new();

            if (element.TryGetProperty(nameof(Locale), out var locale))
            {
                localizedText.Locale = locale.ToString();
            }

            if (element.TryGetProperty(nameof(Text), out var text))
            {
                localizedText.Text = text.ToString();
            }

            return localizedText;
        }
        #endregion
    }

    public class KeyValuePair
    {
        public QualifiedName? Key { get; set; }
        public Variant? Value { get; set; }
    }

    public class NodeId
    {
        public int? IdType { get; private set; }
        public object? Id { get; private set; }
        public int? Namespace { get; private set; }

        public NodeId()
        {
            IdType = 0;
            Id = 0U;
            Namespace = 0;
        }

        public NodeId(object id, int idType = 0, int ns = 0)
        {
            IdType = (idType == 0) ? 0 : idType;
            Id = (id == null || (id is uint number && number == 0)) ? 0U : id;
            Namespace = (ns == 0) ? 0 : ns;
        }

        #region Parsing
        public static NodeId Parse(JsonElement element)
        {
            NodeId nodeId = new(0);

            if (element.TryGetProperty(nameof(IdType), out var idType))
            {
                if (idType.TryGetInt32(out var idTypeValue))
                {
                    nodeId.IdType = idTypeValue;
                }
            }
            else
            {
                nodeId.IdType = 0;
            }

            if (element.TryGetProperty(nameof(Id), out var id))
            {
                if (id.TryGetInt32(out var idValue))
                {
                    nodeId.Id = idValue;
                }
                else
                {
                    nodeId.Id = id.ToString();
                }
            }
            else
            {
                nodeId.Id = (uint)0;
            }

            if (element.TryGetProperty(nameof(Namespace), out var ns))
            {
                if (ns.TryGetInt32(out var nsValue))
                {
                    nodeId.Namespace = nsValue;
                }
            }
            else
            {
                nodeId.Namespace = 0;
            }

            return nodeId;
        }
        #endregion

        #region Overridden Methods
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            if (Object.ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj is NodeId other)
            {
                if (IdType != other.IdType || other.Id?.GetType() != Id?.GetType())
                {
                    return false;
                }

                switch (other.IdType)
                {
                    case 0:
                    {
                        if (Id is int? && other.Id is int?)
                        {
                            return (int)Id == (int)other.Id;
                        }

                        return false;
                    }

                    case 1:
                    case 3:
                    {
                        if (Id is string id)
                        {
                            return id == (string?)other.Id;
                        }

                        return false;
                    }

                    case 2:
                    {
                        if (Id is string id)
                        {
                            if (Guid.TryParse(id, out var guid1))
                            {
                                if (Guid.TryParse((string?)other.Id, out var guid2))
                                {
                                    return guid1 == guid2;
                                }
                            }
                        }

                        return false;
                    }
                }
            }

            return false;
        }
        #endregion
    }

    public class ExtensionObject<T> where T : class
    {
        public NodeId? TypeId { get; set; }
        public int? Encoding { get; set; }
        public T? Body { get; set; }
    }

    public class Variant
    {
        public int? Type { get; set; }
        public object? Body { get; set; }
    }

    public class DataValue
    {
        public Variant? Value { get; set; }
        public DateTime? SourceTimestamp { get; set; }
        public DateTime? ServerTimestamp { get; set; }
        public uint? StatusCode { get; set; }
    }

    public class Range
    {
        public static readonly NodeId TypeId = new(884);

        public int? Low { get; set; }
        public int? High { get; set; }
    }

    public class EUInformation
    {
        public static readonly NodeId TypeId = new(887);

        public string? NamespaceUri { get; set; }
        public int? UnitId { get; set; }
        public LocalizedText? DisplayName { get; set; }
        public LocalizedText? Description { get; set; }

        #region Parsing
        public static EUInformation? FromExtensionObject(JsonElement element)
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

            return (EUInformation?)JsonSerializer.Deserialize(body.GetRawText(), typeof(EUInformation));
        }
        #endregion
    }
}
