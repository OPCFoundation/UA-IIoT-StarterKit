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
namespace UaMqttCommon
{
    public class JsonDataSetMetaDataMessage
    {
        public string? MessageId { get; set; }
        public string? MessageType { get; set; }
        public string? PublisherId { get; set; }
        public int? DataSetWriterId { get; set; }
        public string? DataSetWriterName { get; set; }
        public DataSetMetaDataType? MetaData { get; set; }
    }

    public class DataSetMetaDataType
    {
        public List<string>? Namespaces { get; set; }
        public string? Name { get; set; }
        public LocalizedText? Description { get; set; }
        public List<FieldMetaData>? Fields { get; set; }
        public string? DataSetClassId { get; set; }
        public ConfigurationVersionDataType? ConfigurationVersion { get; set; }
    }

    public class ConfigurationVersionDataType
    {
        public int? MinorVersion { get; set; }
        public int? MajorVersion { get; set; }
    }

    public class FieldMetaData
    {
        public string? Name { get; set; }
        public LocalizedText? Description { get; set; }
        public int? FieldFlags { get; set; }
        public int? BuiltInType { get; set; }
        public NodeId? DataType { get; set; }
        public int? ValueRank { get; set; }
        public List<int>? ArrayDimensions { get; set; }
        public int? MaxStringLength { get; set; }
        public string? DataSetFieldId { get; set; }
        public List<KeyValuePair>? Properties { get; set; }
    }
}
