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
using System.Text.Json.Serialization;
using Opc.Ua;

namespace UaMqttPublisher
{
    internal class PublisherConfiguration
    {
        public List<BrokerConfiguration> Brokers { get; set; }
        public List<ConnectionConfiguration> Connections { get; set; }
    }

    public class BrokerConfiguration
    {
        public string Name { get; set; }
        public string BrokerUrl { get; set; } = "broker.hivemq.com";
        public string TopicPrefix { get; set; } = "opcua";
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool? UseMqtt311 { get; set; }
        public bool? DoNotUseTls { get; set; }
        public bool? IgnoreCertificateErrors { get; set; }
    }

    public class ConnectionConfiguration
    {
        public string Name { get; set; }
        public string PublisherId { get; set; } = "(change-this-value)";
        public bool? Enabled { get; set; } = true;
        public string ServerUrl { get; set; } = "opc.tcp://localhost:48040";
        public bool? NoSecurity { get; set; }
        public bool? RenewCertificate { get; set; }
        public bool? AutoAccept { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public uint? SessionTimeout { get; set; }
        public List<GroupConfiguration> Groups { get; set; }
    }

    public static class HeaderProfiles
    {
        public const string UadpPeriodFixed = "http://opcfoundation.org/UA/PubSub-Layouts/UADP-Periodic-Fixed";
        public const string UadpDynamic = "http://opcfoundation.org/UA/PubSub-Layouts/UADP-Dynamic";
        public const string JsonMinimal = "http://opcfoundation.org/UA/PubSub-Layouts/JSON-Minimal";
        public const string JsonDataSetMessage = "http://opcfoundation.org/UA/PubSub-Layouts/JSON-DataSetMessage";
        public const string JsonNetworkMessage = "http://opcfoundation.org/UA/PubSub-Layouts/JSON-NetworkMessage";
    }

    public class GroupConfiguration
    {
        public string Name { get; set; }
        public bool? Enabled { get; set; } = true;
        public string Description { get; set; }
        public List<WriterConfiguration> Writers { get; set; }
        public int? PublishingInterval { get; set; } = 5000;
        public string DataSetClassId { get; set; }
        public int? MetaDataPublishingCount { get; set; } = 10;
        public uint? KeepAliveCount { get; set; } = 1;
        public string HeaderProfile { get; set; } = HeaderProfiles.JsonDataSetMessage;
        public string TopicForData { get; set; }
    }

    public enum FieldMasks
    {
        Raw = 32,
        Value = 0,
        ValueStatus = 1,
        ValueStatusTimestamp = 5,
        All = 63
    }

    public class WriterConfiguration
    {
        public string Name { get; set; }
        public ushort? WriterId { get; set; }
        public string DataSetName { get; set; }
        public bool? Enabled { get; set; } = true;
        public string Description { get; set; }
        public List<DataSetField> Fields { get; set; }
        public uint? KeyFrameCount { get; set; } = 12;
        public uint? ContentMasks { get; set; }
        public FieldMasks? FieldMasks { get; set; }
        public uint? SequenceNumber { get; set; }
        public int? QoSForMetaData { get; set; }
        public int? QoSForData { get; set; }
        public string TopicForData { get; set; }
        public string TopicForMetaData { get; set; }

        [JsonIgnore]
        public uint MessageCount { get; set; } = 0;
    }

    public class DataSetFieldBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public BuiltInType? BuiltInType { get; set; }
        public string DataType { get; set; }
        public int? ValueRank { get; set; }
        public string Source { get; set; }
        public int? SamplingInterval { get; set; }
    }

    public class DataSetField : DataSetFieldBase
    {
        public List<DataSetFieldBase> Properties { get; set; }
    }
}
