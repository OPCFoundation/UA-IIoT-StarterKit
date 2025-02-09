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

using System.Runtime.Serialization;
using Opc.Ua;

namespace Boiler
{
    #region EnergyConsumptionType Class
#if (!OPCUA_EXCLUDE_EnergyConsumptionType)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [DataContract(Namespace = Boiler.Namespaces.Boiler)]
    public partial class EnergyConsumptionType : IEncodeable, IJsonEncodeable
    {
        #region Constructors
        /// <remarks />
        public EnergyConsumptionType()
        {
            Initialize();
        }
            
        [OnDeserializing]
        private void Initialize(StreamingContext context)
        {
            Initialize();
        }
            
        private void Initialize()
        {
            m_period = (uint)0;
            m_maxPower = (double)0;
            m_averagePower = (double)0;
            m_consumption = (double)0;
        }
        #endregion

        #region Public Properties
        /// <remarks />
        [DataMember(Name = "Period", IsRequired = false, Order = 1)]
        public uint Period
        {
            get { return m_period;  }
            set { m_period = value; }
        }

        /// <remarks />
        [DataMember(Name = "MaxPower", IsRequired = false, Order = 2)]
        public double MaxPower
        {
            get { return m_maxPower;  }
            set { m_maxPower = value; }
        }

        /// <remarks />
        [DataMember(Name = "AveragePower", IsRequired = false, Order = 3)]
        public double AveragePower
        {
            get { return m_averagePower;  }
            set { m_averagePower = value; }
        }

        /// <remarks />
        [DataMember(Name = "Consumption", IsRequired = false, Order = 4)]
        public double Consumption
        {
            get { return m_consumption;  }
            set { m_consumption = value; }
        }
        #endregion

        #region IEncodeable Members
        /// <summary cref="IEncodeable.TypeId" />
        public virtual ExpandedNodeId TypeId => DataTypeIds.EnergyConsumptionType; 

        /// <summary cref="IEncodeable.BinaryEncodingId" />
        public virtual ExpandedNodeId BinaryEncodingId => ObjectIds.EnergyConsumptionType_Encoding_DefaultBinary;

        /// <summary cref="IEncodeable.XmlEncodingId" />
        public virtual ExpandedNodeId XmlEncodingId => ObjectIds.EnergyConsumptionType_Encoding_DefaultXml;
                    
        /// <summary cref="IJsonEncodeable.JsonEncodingId" />
        public virtual ExpandedNodeId JsonEncodingId => ObjectIds.EnergyConsumptionType_Encoding_DefaultJson; 

        /// <summary cref="IEncodeable.Encode(IEncoder)" />
        public virtual void Encode(IEncoder encoder)
        {
            encoder.PushNamespace(Boiler.Namespaces.Boiler);

            encoder.WriteUInt32("Period", Period);
            encoder.WriteDouble("MaxPower", MaxPower);
            encoder.WriteDouble("AveragePower", AveragePower);
            encoder.WriteDouble("Consumption", Consumption);

            encoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.Decode(IDecoder)" />
        public virtual void Decode(IDecoder decoder)
        {
            decoder.PushNamespace(Boiler.Namespaces.Boiler);

            Period = decoder.ReadUInt32("Period");
            MaxPower = decoder.ReadDouble("MaxPower");
            AveragePower = decoder.ReadDouble("AveragePower");
            Consumption = decoder.ReadDouble("Consumption");

            decoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.IsEqual(IEncodeable)" />
        public virtual bool IsEqual(IEncodeable encodeable)
        {
            if (Object.ReferenceEquals(this, encodeable))
            {
                return true;
            }

            EnergyConsumptionType value = encodeable as EnergyConsumptionType;

            if (value == null)
            {
                return false;
            }

            if (!Utils.IsEqual(m_period, value.m_period)) return false;
            if (!Utils.IsEqual(m_maxPower, value.m_maxPower)) return false;
            if (!Utils.IsEqual(m_averagePower, value.m_averagePower)) return false;
            if (!Utils.IsEqual(m_consumption, value.m_consumption)) return false;

            return true;
        }

        /// <summary cref="ICloneable.Clone" />
        public virtual object Clone()
        {
            return (EnergyConsumptionType)this.MemberwiseClone();
        }

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            EnergyConsumptionType clone = (EnergyConsumptionType)base.MemberwiseClone();

            clone.m_period = (uint)Utils.Clone(this.m_period);
            clone.m_maxPower = (double)Utils.Clone(this.m_maxPower);
            clone.m_averagePower = (double)Utils.Clone(this.m_averagePower);
            clone.m_consumption = (double)Utils.Clone(this.m_consumption);

            return clone;
        }
        #endregion

        #region Private Fields
        private uint m_period;
        private double m_maxPower;
        private double m_averagePower;
        private double m_consumption;
        #endregion
    }

    #region EnergyConsumptionTypeCollection Class
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [CollectionDataContract(Name = "ListOfEnergyConsumptionType", Namespace = Boiler.Namespaces.Boiler, ItemName = "EnergyConsumptionType")]
    public partial class EnergyConsumptionTypeCollection : List<EnergyConsumptionType>, ICloneable
    {
        #region Constructors
        /// <remarks />
        public EnergyConsumptionTypeCollection() {}

        /// <remarks />
        public EnergyConsumptionTypeCollection(int capacity) : base(capacity) {}

        /// <remarks />
        public EnergyConsumptionTypeCollection(IEnumerable<EnergyConsumptionType> collection) : base(collection) {}
        #endregion

        #region Static Operators
        /// <remarks />
        public static implicit operator EnergyConsumptionTypeCollection(EnergyConsumptionType[] values)
        {
            if (values != null)
            {
                return new EnergyConsumptionTypeCollection(values);
            }

            return new EnergyConsumptionTypeCollection();
        }

        /// <remarks />
        public static explicit operator EnergyConsumptionType[](EnergyConsumptionTypeCollection values)
        {
            if (values != null)
            {
                return values.ToArray();
            }

            return null;
        }
        #endregion

        #region ICloneable Methods
        /// <remarks />
        public object Clone()
        {
            return (EnergyConsumptionTypeCollection)this.MemberwiseClone();
        }
        #endregion

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            EnergyConsumptionTypeCollection clone = new EnergyConsumptionTypeCollection(this.Count);

            for (int ii = 0; ii < this.Count; ii++)
            {
                clone.Add((EnergyConsumptionType)Utils.Clone(this[ii]));
            }

            return clone;
        }
    }
    #endregion
    #endif
    #endregion

    #region SetPointMask Enumeration
    #if (!OPCUA_EXCLUDE_SetPointMask)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [DataContract(Namespace = Boiler.Namespaces.Boiler)][Flags]
    public enum SetPointMask : Byte
    {
        /// <remarks />
        [EnumMember(Value = "None_0")]
        None = 0,

        /// <remarks />
        [EnumMember(Value = "Level_1")]
        Level = 1,

        /// <remarks />
        [EnumMember(Value = "Flow_2")]
        Flow = 2,
    }

    #region SetPointMaskCollection Class
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [CollectionDataContract(Name = "ListOfSetPointMask", Namespace = Boiler.Namespaces.Boiler, ItemName = "SetPointMask")]
    public partial class SetPointMaskCollection : List<SetPointMask>, ICloneable
    {
        #region Constructors
        /// <remarks />
        public SetPointMaskCollection() {}

        /// <remarks />
        public SetPointMaskCollection(int capacity) : base(capacity) {}

        /// <remarks />
        public SetPointMaskCollection(IEnumerable<SetPointMask> collection) : base(collection) {}
        #endregion

        #region Static Operators
        /// <remarks />
        public static implicit operator SetPointMaskCollection(SetPointMask[] values)
        {
            if (values != null)
            {
                return new SetPointMaskCollection(values);
            }

            return new SetPointMaskCollection();
        }

        /// <remarks />
        public static explicit operator SetPointMask[](SetPointMaskCollection values)
        {
            if (values != null)
            {
                return values.ToArray();
            }

            return null;
        }
        #endregion

        #region ICloneable Methods
        /// <remarks />
        public object Clone()
        {
            return (SetPointMaskCollection)this.MemberwiseClone();
        }
        #endregion

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            SetPointMaskCollection clone = new SetPointMaskCollection(this.Count);

            for (int ii = 0; ii < this.Count; ii++)
            {
                clone.Add((SetPointMask)Utils.Clone(this[ii]));
            }

            return clone;
        }
    }
    #endregion
    #endif
    #endregion

    #region ChangeSetPointsRequest Class
    #if (!OPCUA_EXCLUDE_ChangeSetPointsRequest)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [DataContract(Namespace = Boiler.Namespaces.Boiler)]
    public partial class ChangeSetPointsRequest : IEncodeable, IJsonEncodeable
    {
        #region Constructors
        /// <remarks />
        public ChangeSetPointsRequest()
        {
            Initialize();
        }
            
        [OnDeserializing]
        private void Initialize(StreamingContext context)
        {
            Initialize();
        }
            
        private void Initialize()
        {
            m_selectedValues = 0;
            m_levelSetPoint = (double)0;
            m_flowSetPoint = (double)0;
        }
        #endregion

        #region Public Properties
        /// <remarks />
        [DataMember(Name = "SelectedValues", IsRequired = false, Order = 1)]
        public byte SelectedValues
        {
            get { return m_selectedValues;  }
            set { m_selectedValues = value; }
        }

        /// <remarks />
        [DataMember(Name = "LevelSetPoint", IsRequired = false, Order = 2)]
        public double LevelSetPoint
        {
            get { return m_levelSetPoint;  }
            set { m_levelSetPoint = value; }
        }

        /// <remarks />
        [DataMember(Name = "FlowSetPoint", IsRequired = false, Order = 3)]
        public double FlowSetPoint
        {
            get { return m_flowSetPoint;  }
            set { m_flowSetPoint = value; }
        }
        #endregion

        #region IEncodeable Members
        /// <summary cref="IEncodeable.TypeId" />
        public virtual ExpandedNodeId TypeId => DataTypeIds.ChangeSetPointsRequest; 

        /// <summary cref="IEncodeable.BinaryEncodingId" />
        public virtual ExpandedNodeId BinaryEncodingId => ObjectIds.ChangeSetPointsRequest_Encoding_DefaultBinary;

        /// <summary cref="IEncodeable.XmlEncodingId" />
        public virtual ExpandedNodeId XmlEncodingId => ObjectIds.ChangeSetPointsRequest_Encoding_DefaultXml;
                    
        /// <summary cref="IJsonEncodeable.JsonEncodingId" />
        public virtual ExpandedNodeId JsonEncodingId => ObjectIds.ChangeSetPointsRequest_Encoding_DefaultJson; 

        /// <summary cref="IEncodeable.Encode(IEncoder)" />
        public virtual void Encode(IEncoder encoder)
        {
            encoder.PushNamespace(Boiler.Namespaces.Boiler);

            encoder.WriteByte("SelectedValues", SelectedValues);
            encoder.WriteDouble("LevelSetPoint", LevelSetPoint);
            encoder.WriteDouble("FlowSetPoint", FlowSetPoint);

            encoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.Decode(IDecoder)" />
        public virtual void Decode(IDecoder decoder)
        {
            decoder.PushNamespace(Boiler.Namespaces.Boiler);

            SelectedValues = decoder.ReadByte("SelectedValues");
            LevelSetPoint = decoder.ReadDouble("LevelSetPoint");
            FlowSetPoint = decoder.ReadDouble("FlowSetPoint");

            decoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.IsEqual(IEncodeable)" />
        public virtual bool IsEqual(IEncodeable encodeable)
        {
            if (Object.ReferenceEquals(this, encodeable))
            {
                return true;
            }

            ChangeSetPointsRequest value = encodeable as ChangeSetPointsRequest;

            if (value == null)
            {
                return false;
            }

            if (!Utils.IsEqual(m_selectedValues, value.m_selectedValues)) return false;
            if (!Utils.IsEqual(m_levelSetPoint, value.m_levelSetPoint)) return false;
            if (!Utils.IsEqual(m_flowSetPoint, value.m_flowSetPoint)) return false;

            return true;
        }

        /// <summary cref="ICloneable.Clone" />
        public virtual object Clone()
        {
            return (ChangeSetPointsRequest)this.MemberwiseClone();
        }

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            ChangeSetPointsRequest clone = (ChangeSetPointsRequest)base.MemberwiseClone();

            clone.m_selectedValues = (byte)Utils.Clone(this.m_selectedValues);
            clone.m_levelSetPoint = (double)Utils.Clone(this.m_levelSetPoint);
            clone.m_flowSetPoint = (double)Utils.Clone(this.m_flowSetPoint);

            return clone;
        }
        #endregion

        #region Private Fields
        private byte m_selectedValues;
        private double m_levelSetPoint;
        private double m_flowSetPoint;
        #endregion
    }

    #region ChangeSetPointsRequestCollection Class
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [CollectionDataContract(Name = "ListOfChangeSetPointsRequest", Namespace = Boiler.Namespaces.Boiler, ItemName = "ChangeSetPointsRequest")]
    public partial class ChangeSetPointsRequestCollection : List<ChangeSetPointsRequest>, ICloneable
    {
        #region Constructors
        /// <remarks />
        public ChangeSetPointsRequestCollection() {}

        /// <remarks />
        public ChangeSetPointsRequestCollection(int capacity) : base(capacity) {}

        /// <remarks />
        public ChangeSetPointsRequestCollection(IEnumerable<ChangeSetPointsRequest> collection) : base(collection) {}
        #endregion

        #region Static Operators
        /// <remarks />
        public static implicit operator ChangeSetPointsRequestCollection(ChangeSetPointsRequest[] values)
        {
            if (values != null)
            {
                return new ChangeSetPointsRequestCollection(values);
            }

            return new ChangeSetPointsRequestCollection();
        }

        /// <remarks />
        public static explicit operator ChangeSetPointsRequest[](ChangeSetPointsRequestCollection values)
        {
            if (values != null)
            {
                return values.ToArray();
            }

            return null;
        }
        #endregion

        #region ICloneable Methods
        /// <remarks />
        public object Clone()
        {
            return (ChangeSetPointsRequestCollection)this.MemberwiseClone();
        }
        #endregion

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            ChangeSetPointsRequestCollection clone = new ChangeSetPointsRequestCollection(this.Count);

            for (int ii = 0; ii < this.Count; ii++)
            {
                clone.Add((ChangeSetPointsRequest)Utils.Clone(this[ii]));
            }

            return clone;
        }
    }
    #endregion
    #endif
    #endregion

    #region ChangeSetPointsResponse Class
    #if (!OPCUA_EXCLUDE_ChangeSetPointsResponse)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [DataContract(Namespace = Boiler.Namespaces.Boiler)]
    public partial class ChangeSetPointsResponse : IEncodeable, IJsonEncodeable
    {
        #region Constructors
        /// <remarks />
        public ChangeSetPointsResponse()
        {
            Initialize();
        }
            
        [OnDeserializing]
        private void Initialize(StreamingContext context)
        {
            Initialize();
        }
            
        private void Initialize()
        {
            m_levelSetPoint = (double)0;
            m_levelMeasurement = (double)0;
            m_flowSetPoint = (double)0;
            m_flowMeasurement = (double)0;
        }
        #endregion

        #region Public Properties
        /// <remarks />
        [DataMember(Name = "LevelSetPoint", IsRequired = false, Order = 1)]
        public double LevelSetPoint
        {
            get { return m_levelSetPoint;  }
            set { m_levelSetPoint = value; }
        }

        /// <remarks />
        [DataMember(Name = "LevelMeasurement", IsRequired = false, Order = 2)]
        public double LevelMeasurement
        {
            get { return m_levelMeasurement;  }
            set { m_levelMeasurement = value; }
        }

        /// <remarks />
        [DataMember(Name = "FlowSetPoint", IsRequired = false, Order = 3)]
        public double FlowSetPoint
        {
            get { return m_flowSetPoint;  }
            set { m_flowSetPoint = value; }
        }

        /// <remarks />
        [DataMember(Name = "FlowMeasurement", IsRequired = false, Order = 4)]
        public double FlowMeasurement
        {
            get { return m_flowMeasurement;  }
            set { m_flowMeasurement = value; }
        }
        #endregion

        #region IEncodeable Members
        /// <summary cref="IEncodeable.TypeId" />
        public virtual ExpandedNodeId TypeId => DataTypeIds.ChangeSetPointsResponse; 

        /// <summary cref="IEncodeable.BinaryEncodingId" />
        public virtual ExpandedNodeId BinaryEncodingId => ObjectIds.ChangeSetPointsResponse_Encoding_DefaultBinary;

        /// <summary cref="IEncodeable.XmlEncodingId" />
        public virtual ExpandedNodeId XmlEncodingId => ObjectIds.ChangeSetPointsResponse_Encoding_DefaultXml;
                    
        /// <summary cref="IJsonEncodeable.JsonEncodingId" />
        public virtual ExpandedNodeId JsonEncodingId => ObjectIds.ChangeSetPointsResponse_Encoding_DefaultJson; 

        /// <summary cref="IEncodeable.Encode(IEncoder)" />
        public virtual void Encode(IEncoder encoder)
        {
            encoder.PushNamespace(Boiler.Namespaces.Boiler);

            encoder.WriteDouble("LevelSetPoint", LevelSetPoint);
            encoder.WriteDouble("LevelMeasurement", LevelMeasurement);
            encoder.WriteDouble("FlowSetPoint", FlowSetPoint);
            encoder.WriteDouble("FlowMeasurement", FlowMeasurement);

            encoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.Decode(IDecoder)" />
        public virtual void Decode(IDecoder decoder)
        {
            decoder.PushNamespace(Boiler.Namespaces.Boiler);

            LevelSetPoint = decoder.ReadDouble("LevelSetPoint");
            LevelMeasurement = decoder.ReadDouble("LevelMeasurement");
            FlowSetPoint = decoder.ReadDouble("FlowSetPoint");
            FlowMeasurement = decoder.ReadDouble("FlowMeasurement");

            decoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.IsEqual(IEncodeable)" />
        public virtual bool IsEqual(IEncodeable encodeable)
        {
            if (Object.ReferenceEquals(this, encodeable))
            {
                return true;
            }

            ChangeSetPointsResponse value = encodeable as ChangeSetPointsResponse;

            if (value == null)
            {
                return false;
            }

            if (!Utils.IsEqual(m_levelSetPoint, value.m_levelSetPoint)) return false;
            if (!Utils.IsEqual(m_levelMeasurement, value.m_levelMeasurement)) return false;
            if (!Utils.IsEqual(m_flowSetPoint, value.m_flowSetPoint)) return false;
            if (!Utils.IsEqual(m_flowMeasurement, value.m_flowMeasurement)) return false;

            return true;
        }

        /// <summary cref="ICloneable.Clone" />
        public virtual object Clone()
        {
            return (ChangeSetPointsResponse)this.MemberwiseClone();
        }

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            ChangeSetPointsResponse clone = (ChangeSetPointsResponse)base.MemberwiseClone();

            clone.m_levelSetPoint = (double)Utils.Clone(this.m_levelSetPoint);
            clone.m_levelMeasurement = (double)Utils.Clone(this.m_levelMeasurement);
            clone.m_flowSetPoint = (double)Utils.Clone(this.m_flowSetPoint);
            clone.m_flowMeasurement = (double)Utils.Clone(this.m_flowMeasurement);

            return clone;
        }
        #endregion

        #region Private Fields
        private double m_levelSetPoint;
        private double m_levelMeasurement;
        private double m_flowSetPoint;
        private double m_flowMeasurement;
        #endregion
    }

    #region ChangeSetPointsResponseCollection Class
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [CollectionDataContract(Name = "ListOfChangeSetPointsResponse", Namespace = Boiler.Namespaces.Boiler, ItemName = "ChangeSetPointsResponse")]
    public partial class ChangeSetPointsResponseCollection : List<ChangeSetPointsResponse>, ICloneable
    {
        #region Constructors
        /// <remarks />
        public ChangeSetPointsResponseCollection() {}

        /// <remarks />
        public ChangeSetPointsResponseCollection(int capacity) : base(capacity) {}

        /// <remarks />
        public ChangeSetPointsResponseCollection(IEnumerable<ChangeSetPointsResponse> collection) : base(collection) {}
        #endregion

        #region Static Operators
        /// <remarks />
        public static implicit operator ChangeSetPointsResponseCollection(ChangeSetPointsResponse[] values)
        {
            if (values != null)
            {
                return new ChangeSetPointsResponseCollection(values);
            }

            return new ChangeSetPointsResponseCollection();
        }

        /// <remarks />
        public static explicit operator ChangeSetPointsResponse[](ChangeSetPointsResponseCollection values)
        {
            if (values != null)
            {
                return values.ToArray();
            }

            return null;
        }
        #endregion

        #region ICloneable Methods
        /// <remarks />
        public object Clone()
        {
            return (ChangeSetPointsResponseCollection)this.MemberwiseClone();
        }
        #endregion

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            ChangeSetPointsResponseCollection clone = new ChangeSetPointsResponseCollection(this.Count);

            for (int ii = 0; ii < this.Count; ii++)
            {
                clone.Add((ChangeSetPointsResponse)Utils.Clone(this[ii]));
            }

            return clone;
        }
    }
    #endregion
    #endif
    #endregion
}