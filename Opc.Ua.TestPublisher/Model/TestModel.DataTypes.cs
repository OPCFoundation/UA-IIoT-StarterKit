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

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Linq;
using System.Runtime.Serialization;
using Opc.Ua;

namespace TestModel
{
    #region AbstractStructure Class
    #if (!OPCUA_EXCLUDE_AbstractStructure)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [DataContract(Namespace = TestModel.Namespaces.TestModelXsd)]
    public partial class AbstractStructure : IEncodeable, IJsonEncodeable
    {
        #region Constructors
        /// <remarks />
        public AbstractStructure()
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
            m_a = (short)0;
            m_b = (double)0;
            m_c = null;
        }
        #endregion

        #region Public Properties
        /// <remarks />
        [DataMember(Name = "A", IsRequired = false, Order = 1)]
        public short A
        {
            get { return m_a;  }
            set { m_a = value; }
        }

        /// <remarks />
        [DataMember(Name = "B", IsRequired = false, Order = 2)]
        public double B
        {
            get { return m_b;  }
            set { m_b = value; }
        }

        /// <remarks />
        [DataMember(Name = "C", IsRequired = false, Order = 3)]
        public string C
        {
            get { return m_c;  }
            set { m_c = value; }
        }
        #endregion

        #region IEncodeable Members
        /// <summary cref="IEncodeable.TypeId" />
        public virtual ExpandedNodeId TypeId => DataTypeIds.AbstractStructure; 

        /// <summary cref="IEncodeable.BinaryEncodingId" />
        public virtual ExpandedNodeId BinaryEncodingId => ObjectIds.AbstractStructure_Encoding_DefaultBinary;

        /// <summary cref="IEncodeable.XmlEncodingId" />
        public virtual ExpandedNodeId XmlEncodingId => ObjectIds.AbstractStructure_Encoding_DefaultXml;
                    
        /// <summary cref="IJsonEncodeable.JsonEncodingId" />
        public virtual ExpandedNodeId JsonEncodingId => ObjectIds.AbstractStructure_Encoding_DefaultJson; 

        /// <summary cref="IEncodeable.Encode(IEncoder)" />
        public virtual void Encode(IEncoder encoder)
        {
            encoder.PushNamespace(TestModel.Namespaces.TestModelXsd);

            encoder.WriteInt16("A", A);
            encoder.WriteDouble("B", B);
            encoder.WriteString("C", C);

            encoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.Decode(IDecoder)" />
        public virtual void Decode(IDecoder decoder)
        {
            decoder.PushNamespace(TestModel.Namespaces.TestModelXsd);

            A = decoder.ReadInt16("A");
            B = decoder.ReadDouble("B");
            C = decoder.ReadString("C");

            decoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.IsEqual(IEncodeable)" />
        public virtual bool IsEqual(IEncodeable encodeable)
        {
            if (Object.ReferenceEquals(this, encodeable))
            {
                return true;
            }

            AbstractStructure value = encodeable as AbstractStructure;

            if (value == null)
            {
                return false;
            }

            if (!Utils.IsEqual(m_a, value.m_a)) return false;
            if (!Utils.IsEqual(m_b, value.m_b)) return false;
            if (!Utils.IsEqual(m_c, value.m_c)) return false;

            return true;
        }

        /// <summary cref="ICloneable.Clone" />
        public virtual object Clone()
        {
            return (AbstractStructure)this.MemberwiseClone();
        }

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            AbstractStructure clone = (AbstractStructure)base.MemberwiseClone();

            clone.m_a = (short)Utils.Clone(this.m_a);
            clone.m_b = (double)Utils.Clone(this.m_b);
            clone.m_c = (string)Utils.Clone(this.m_c);

            return clone;
        }
        #endregion

        #region Private Fields
        private short m_a;
        private double m_b;
        private string m_c;
        #endregion
    }

    #region AbstractStructureCollection Class
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [CollectionDataContract(Name = "ListOfAbstractStructure", Namespace = TestModel.Namespaces.TestModelXsd, ItemName = "AbstractStructure")]
    public partial class AbstractStructureCollection : List<AbstractStructure>, ICloneable
    {
        #region Constructors
        /// <remarks />
        public AbstractStructureCollection() {}

        /// <remarks />
        public AbstractStructureCollection(int capacity) : base(capacity) {}

        /// <remarks />
        public AbstractStructureCollection(IEnumerable<AbstractStructure> collection) : base(collection) {}
        #endregion

        #region Static Operators
        /// <remarks />
        public static implicit operator AbstractStructureCollection(AbstractStructure[] values)
        {
            if (values != null)
            {
                return new AbstractStructureCollection(values);
            }

            return new AbstractStructureCollection();
        }

        /// <remarks />
        public static explicit operator AbstractStructure[](AbstractStructureCollection values)
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
            return (AbstractStructureCollection)this.MemberwiseClone();
        }
        #endregion

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            AbstractStructureCollection clone = new AbstractStructureCollection(this.Count);

            for (int ii = 0; ii < this.Count; ii++)
            {
                clone.Add((AbstractStructure)Utils.Clone(this[ii]));
            }

            return clone;
        }
    }
    #endregion
    #endif
    #endregion

    #region ConcreteStructure Class
    #if (!OPCUA_EXCLUDE_ConcreteStructure)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [DataContract(Namespace = TestModel.Namespaces.TestModelXsd)]
    public partial class ConcreteStructure : TestModel.AbstractStructure
    {
        #region Constructors
        /// <remarks />
        public ConcreteStructure()
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
            m_d = (short)0;
            m_e = (double)0;
            m_f = null;
        }
        #endregion

        #region Public Properties
        /// <remarks />
        [DataMember(Name = "D", IsRequired = false, Order = 1)]
        public short D
        {
            get { return m_d;  }
            set { m_d = value; }
        }

        /// <remarks />
        [DataMember(Name = "E", IsRequired = false, Order = 2)]
        public double E
        {
            get { return m_e;  }
            set { m_e = value; }
        }

        /// <remarks />
        [DataMember(Name = "F", IsRequired = false, Order = 3)]
        public string F
        {
            get { return m_f;  }
            set { m_f = value; }
        }
        #endregion

        #region IEncodeable Members
        /// <summary cref="IEncodeable.TypeId" />
        public override ExpandedNodeId TypeId => DataTypeIds.ConcreteStructure; 

        /// <summary cref="IEncodeable.BinaryEncodingId" />
        public override ExpandedNodeId BinaryEncodingId => ObjectIds.ConcreteStructure_Encoding_DefaultBinary;

        /// <summary cref="IEncodeable.XmlEncodingId" />
        public override ExpandedNodeId XmlEncodingId => ObjectIds.ConcreteStructure_Encoding_DefaultXml;
            
        /// <summary cref="IJsonEncodeable.JsonEncodingId" />
        public override ExpandedNodeId JsonEncodingId => ObjectIds.ConcreteStructure_Encoding_DefaultJson; 

        /// <summary cref="IEncodeable.Encode(IEncoder)" />
        public override void Encode(IEncoder encoder)
        {
            base.Encode(encoder);

            encoder.PushNamespace(TestModel.Namespaces.TestModelXsd);

            encoder.WriteInt16("D", D);
            encoder.WriteDouble("E", E);
            encoder.WriteString("F", F);

            encoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.Decode(IDecoder)" />
        public override void Decode(IDecoder decoder)
        {
            base.Decode(decoder);

            decoder.PushNamespace(TestModel.Namespaces.TestModelXsd);

            D = decoder.ReadInt16("D");
            E = decoder.ReadDouble("E");
            F = decoder.ReadString("F");

            decoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.IsEqual(IEncodeable)" />
        public override bool IsEqual(IEncodeable encodeable)
        {
            if (Object.ReferenceEquals(this, encodeable))
            {
                return true;
            }

            ConcreteStructure value = encodeable as ConcreteStructure;

            if (value == null)
            {
                return false;
            }

            if (!Utils.IsEqual(m_d, value.m_d)) return false;
            if (!Utils.IsEqual(m_e, value.m_e)) return false;
            if (!Utils.IsEqual(m_f, value.m_f)) return false;

            return base.IsEqual(encodeable);
        }    

        /// <summary cref="ICloneable.Clone" />
        public override object Clone()
        {
            return (ConcreteStructure)this.MemberwiseClone();
        }

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            ConcreteStructure clone = (ConcreteStructure)base.MemberwiseClone();

            clone.m_d = (short)Utils.Clone(this.m_d);
            clone.m_e = (double)Utils.Clone(this.m_e);
            clone.m_f = (string)Utils.Clone(this.m_f);

            return clone;
        }
        #endregion

        #region Private Fields
        private short m_d;
        private double m_e;
        private string m_f;
        #endregion
    }

    #region ConcreteStructureCollection Class
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [CollectionDataContract(Name = "ListOfConcreteStructure", Namespace = TestModel.Namespaces.TestModelXsd, ItemName = "ConcreteStructure")]
    public partial class ConcreteStructureCollection : List<ConcreteStructure>, ICloneable
    {
        #region Constructors
        /// <remarks />
        public ConcreteStructureCollection() {}

        /// <remarks />
        public ConcreteStructureCollection(int capacity) : base(capacity) {}

        /// <remarks />
        public ConcreteStructureCollection(IEnumerable<ConcreteStructure> collection) : base(collection) {}
        #endregion

        #region Static Operators
        /// <remarks />
        public static implicit operator ConcreteStructureCollection(ConcreteStructure[] values)
        {
            if (values != null)
            {
                return new ConcreteStructureCollection(values);
            }

            return new ConcreteStructureCollection();
        }

        /// <remarks />
        public static explicit operator ConcreteStructure[](ConcreteStructureCollection values)
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
            return (ConcreteStructureCollection)this.MemberwiseClone();
        }
        #endregion

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            ConcreteStructureCollection clone = new ConcreteStructureCollection(this.Count);

            for (int ii = 0; ii < this.Count; ii++)
            {
                clone.Add((ConcreteStructure)Utils.Clone(this[ii]));
            }

            return clone;
        }
    }
    #endregion
    #endif
    #endregion

    #region EnumerationWithGaps Enumeration
    #if (!OPCUA_EXCLUDE_EnumerationWithGaps)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [DataContract(Namespace = TestModel.Namespaces.TestModelXsd)]
    public enum EnumerationWithGaps
    {
        /// <remarks />
        [EnumMember(Value = "Invalid_0")]
        Invalid = 0,

        /// <remarks />
        [EnumMember(Value = "Red_1")]
        Red = 1,

        /// <remarks />
        [EnumMember(Value = "Green_2")]
        Green = 2,

        /// <remarks />
        [EnumMember(Value = "Blue_6")]
        Blue = 6,
    }

    #region EnumerationWithGapsCollection Class
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [CollectionDataContract(Name = "ListOfEnumerationWithGaps", Namespace = TestModel.Namespaces.TestModelXsd, ItemName = "EnumerationWithGaps")]
    public partial class EnumerationWithGapsCollection : List<EnumerationWithGaps>, ICloneable
    {
        #region Constructors
        /// <remarks />
        public EnumerationWithGapsCollection() {}

        /// <remarks />
        public EnumerationWithGapsCollection(int capacity) : base(capacity) {}

        /// <remarks />
        public EnumerationWithGapsCollection(IEnumerable<EnumerationWithGaps> collection) : base(collection) {}
        #endregion

        #region Static Operators
        /// <remarks />
        public static implicit operator EnumerationWithGapsCollection(EnumerationWithGaps[] values)
        {
            if (values != null)
            {
                return new EnumerationWithGapsCollection(values);
            }

            return new EnumerationWithGapsCollection();
        }

        /// <remarks />
        public static explicit operator EnumerationWithGaps[](EnumerationWithGapsCollection values)
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
            return (EnumerationWithGapsCollection)this.MemberwiseClone();
        }
        #endregion

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            EnumerationWithGapsCollection clone = new EnumerationWithGapsCollection(this.Count);

            for (int ii = 0; ii < this.Count; ii++)
            {
                clone.Add((EnumerationWithGaps)Utils.Clone(this[ii]));
            }

            return clone;
        }
    }
    #endregion
    #endif
    #endregion

    #region ScalarStructure Class
    #if (!OPCUA_EXCLUDE_ScalarStructure)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [DataContract(Namespace = TestModel.Namespaces.TestModelXsd)]
    public partial class ScalarStructure : IEncodeable, IJsonEncodeable
    {
        #region Constructors
        /// <remarks />
        public ScalarStructure()
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
            m_a = true;
            m_b = (sbyte)0;
            m_c = (byte)0;
            m_d = (short)0;
            m_e = (ushort)0;
            m_f = (int)0;
            m_g = (uint)0;
            m_h = (long)0;
            m_i = (ulong)0;
            m_j = (float)0;
            m_k = (double)0;
            m_l = Uuid.Empty;
            m_m = DateTime.MinValue;
            m_n = null;
            m_o = null;
            m_p = null;
            m_q = null;
            m_r = null;
            m_s = null;
            m_t = StatusCodes.Good;
            m_u = null;
            m_v = new ConcreteStructure();
            m_w = EnumerationWithGaps.Invalid;
        }
        #endregion

        #region Public Properties
        /// <remarks />
        [DataMember(Name = "A", IsRequired = false, Order = 1)]
        public bool A
        {
            get { return m_a;  }
            set { m_a = value; }
        }

        /// <remarks />
        [DataMember(Name = "B", IsRequired = false, Order = 2)]
        public sbyte B
        {
            get { return m_b;  }
            set { m_b = value; }
        }

        /// <remarks />
        [DataMember(Name = "C", IsRequired = false, Order = 3)]
        public byte C
        {
            get { return m_c;  }
            set { m_c = value; }
        }

        /// <remarks />
        [DataMember(Name = "D", IsRequired = false, Order = 4)]
        public short D
        {
            get { return m_d;  }
            set { m_d = value; }
        }

        /// <remarks />
        [DataMember(Name = "E", IsRequired = false, Order = 5)]
        public ushort E
        {
            get { return m_e;  }
            set { m_e = value; }
        }

        /// <remarks />
        [DataMember(Name = "F", IsRequired = false, Order = 6)]
        public int F
        {
            get { return m_f;  }
            set { m_f = value; }
        }

        /// <remarks />
        [DataMember(Name = "G", IsRequired = false, Order = 7)]
        public uint G
        {
            get { return m_g;  }
            set { m_g = value; }
        }

        /// <remarks />
        [DataMember(Name = "H", IsRequired = false, Order = 8)]
        public long H
        {
            get { return m_h;  }
            set { m_h = value; }
        }

        /// <remarks />
        [DataMember(Name = "I", IsRequired = false, Order = 9)]
        public ulong I
        {
            get { return m_i;  }
            set { m_i = value; }
        }

        /// <remarks />
        [DataMember(Name = "J", IsRequired = false, Order = 10)]
        public float J
        {
            get { return m_j;  }
            set { m_j = value; }
        }

        /// <remarks />
        [DataMember(Name = "K", IsRequired = false, Order = 11)]
        public double K
        {
            get { return m_k;  }
            set { m_k = value; }
        }

        /// <remarks />
        [DataMember(Name = "L", IsRequired = false, Order = 12)]
        public Uuid L
        {
            get { return m_l;  }
            set { m_l = value; }
        }

        /// <remarks />
        [DataMember(Name = "M", IsRequired = false, Order = 13)]
        public DateTime M
        {
            get { return m_m;  }
            set { m_m = value; }
        }

        /// <remarks />
        [DataMember(Name = "N", IsRequired = false, Order = 14)]
        public string N
        {
            get { return m_n;  }
            set { m_n = value; }
        }

        /// <remarks />
        [DataMember(Name = "O", IsRequired = false, Order = 15)]
        public byte[] O
        {
            get { return m_o;  }
            set { m_o = value; }
        }

        /// <remarks />
        [DataMember(Name = "P", IsRequired = false, Order = 16)]
        public NodeId P
        {
            get { return m_p;  }
            set { m_p = value; }
        }

        /// <remarks />
        [DataMember(Name = "Q", IsRequired = false, Order = 17)]
        public ExpandedNodeId Q
        {
            get { return m_q;  }
            set { m_q = value; }
        }

        /// <remarks />
        [DataMember(Name = "R", IsRequired = false, Order = 18)]
        public QualifiedName R
        {
            get { return m_r;  }
            set { m_r = value; }
        }

        /// <remarks />
        [DataMember(Name = "S", IsRequired = false, Order = 19)]
        public LocalizedText S
        {
            get { return m_s;  }
            set { m_s = value; }
        }

        /// <remarks />
        [DataMember(Name = "T", IsRequired = false, Order = 20)]
        public StatusCode T
        {
            get { return m_t;  }
            set { m_t = value; }
        }

        /// <remarks />
        [DataMember(Name = "U", IsRequired = false, Order = 21)]
        public XmlElement U
        {
            get { return m_u;  }
            set { m_u = value; }
        }

        /// <remarks />
        [DataMember(Name = "V", IsRequired = false, Order = 22)]
        public ConcreteStructure V
        {
            get
            {
                return m_v;
            }

            set
            {
                m_v = value;

                if (value == null)
                {
                    m_v = new ConcreteStructure();
                }
            }
        }

        /// <remarks />
        [DataMember(Name = "W", IsRequired = false, Order = 23)]
        public EnumerationWithGaps W
        {
            get { return m_w;  }
            set { m_w = value; }
        }
        #endregion

        #region IEncodeable Members
        /// <summary cref="IEncodeable.TypeId" />
        public virtual ExpandedNodeId TypeId => DataTypeIds.ScalarStructure; 

        /// <summary cref="IEncodeable.BinaryEncodingId" />
        public virtual ExpandedNodeId BinaryEncodingId => ObjectIds.ScalarStructure_Encoding_DefaultBinary;

        /// <summary cref="IEncodeable.XmlEncodingId" />
        public virtual ExpandedNodeId XmlEncodingId => ObjectIds.ScalarStructure_Encoding_DefaultXml;
                    
        /// <summary cref="IJsonEncodeable.JsonEncodingId" />
        public virtual ExpandedNodeId JsonEncodingId => ObjectIds.ScalarStructure_Encoding_DefaultJson; 

        /// <summary cref="IEncodeable.Encode(IEncoder)" />
        public virtual void Encode(IEncoder encoder)
        {
            encoder.PushNamespace(TestModel.Namespaces.TestModelXsd);

            encoder.WriteBoolean("A", A);
            encoder.WriteSByte("B", B);
            encoder.WriteByte("C", C);
            encoder.WriteInt16("D", D);
            encoder.WriteUInt16("E", E);
            encoder.WriteInt32("F", F);
            encoder.WriteUInt32("G", G);
            encoder.WriteInt64("H", H);
            encoder.WriteUInt64("I", I);
            encoder.WriteFloat("J", J);
            encoder.WriteDouble("K", K);
            encoder.WriteGuid("L", L);
            encoder.WriteDateTime("M", M);
            encoder.WriteString("N", N);
            encoder.WriteByteString("O", O);
            encoder.WriteNodeId("P", P);
            encoder.WriteExpandedNodeId("Q", Q);
            encoder.WriteQualifiedName("R", R);
            encoder.WriteLocalizedText("S", S);
            encoder.WriteStatusCode("T", T);
            encoder.WriteXmlElement("U", U);
            encoder.WriteEncodeable("V", V, typeof(ConcreteStructure));
            encoder.WriteEnumerated("W", W);

            encoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.Decode(IDecoder)" />
        public virtual void Decode(IDecoder decoder)
        {
            decoder.PushNamespace(TestModel.Namespaces.TestModelXsd);

            A = decoder.ReadBoolean("A");
            B = decoder.ReadSByte("B");
            C = decoder.ReadByte("C");
            D = decoder.ReadInt16("D");
            E = decoder.ReadUInt16("E");
            F = decoder.ReadInt32("F");
            G = decoder.ReadUInt32("G");
            H = decoder.ReadInt64("H");
            I = decoder.ReadUInt64("I");
            J = decoder.ReadFloat("J");
            K = decoder.ReadDouble("K");
            L = decoder.ReadGuid("L");
            M = decoder.ReadDateTime("M");
            N = decoder.ReadString("N");
            O = decoder.ReadByteString("O");
            P = decoder.ReadNodeId("P");
            Q = decoder.ReadExpandedNodeId("Q");
            R = decoder.ReadQualifiedName("R");
            S = decoder.ReadLocalizedText("S");
            T = decoder.ReadStatusCode("T");
            U = decoder.ReadXmlElement("U");
            V = (ConcreteStructure)decoder.ReadEncodeable("V", typeof(ConcreteStructure));
            W = (EnumerationWithGaps)decoder.ReadEnumerated("W", typeof(EnumerationWithGaps));

            decoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.IsEqual(IEncodeable)" />
        public virtual bool IsEqual(IEncodeable encodeable)
        {
            if (Object.ReferenceEquals(this, encodeable))
            {
                return true;
            }

            ScalarStructure value = encodeable as ScalarStructure;

            if (value == null)
            {
                return false;
            }

            if (!Utils.IsEqual(m_a, value.m_a)) return false;
            if (!Utils.IsEqual(m_b, value.m_b)) return false;
            if (!Utils.IsEqual(m_c, value.m_c)) return false;
            if (!Utils.IsEqual(m_d, value.m_d)) return false;
            if (!Utils.IsEqual(m_e, value.m_e)) return false;
            if (!Utils.IsEqual(m_f, value.m_f)) return false;
            if (!Utils.IsEqual(m_g, value.m_g)) return false;
            if (!Utils.IsEqual(m_h, value.m_h)) return false;
            if (!Utils.IsEqual(m_i, value.m_i)) return false;
            if (!Utils.IsEqual(m_j, value.m_j)) return false;
            if (!Utils.IsEqual(m_k, value.m_k)) return false;
            if (!Utils.IsEqual(m_l, value.m_l)) return false;
            if (!Utils.IsEqual(m_m, value.m_m)) return false;
            if (!Utils.IsEqual(m_n, value.m_n)) return false;
            if (!Utils.IsEqual(m_o, value.m_o)) return false;
            if (!Utils.IsEqual(m_p, value.m_p)) return false;
            if (!Utils.IsEqual(m_q, value.m_q)) return false;
            if (!Utils.IsEqual(m_r, value.m_r)) return false;
            if (!Utils.IsEqual(m_s, value.m_s)) return false;
            if (!Utils.IsEqual(m_t, value.m_t)) return false;
            if (!Utils.IsEqual(m_u, value.m_u)) return false;
            if (!Utils.IsEqual(m_v, value.m_v)) return false;
            if (!Utils.IsEqual(m_w, value.m_w)) return false;

            return true;
        }

        /// <summary cref="ICloneable.Clone" />
        public virtual object Clone()
        {
            return (ScalarStructure)this.MemberwiseClone();
        }

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            ScalarStructure clone = (ScalarStructure)base.MemberwiseClone();

            clone.m_a = (bool)Utils.Clone(this.m_a);
            clone.m_b = (sbyte)Utils.Clone(this.m_b);
            clone.m_c = (byte)Utils.Clone(this.m_c);
            clone.m_d = (short)Utils.Clone(this.m_d);
            clone.m_e = (ushort)Utils.Clone(this.m_e);
            clone.m_f = (int)Utils.Clone(this.m_f);
            clone.m_g = (uint)Utils.Clone(this.m_g);
            clone.m_h = (long)Utils.Clone(this.m_h);
            clone.m_i = (ulong)Utils.Clone(this.m_i);
            clone.m_j = (float)Utils.Clone(this.m_j);
            clone.m_k = (double)Utils.Clone(this.m_k);
            clone.m_l = (Uuid)Utils.Clone(this.m_l);
            clone.m_m = (DateTime)Utils.Clone(this.m_m);
            clone.m_n = (string)Utils.Clone(this.m_n);
            clone.m_o = (byte[])Utils.Clone(this.m_o);
            clone.m_p = (NodeId)Utils.Clone(this.m_p);
            clone.m_q = (ExpandedNodeId)Utils.Clone(this.m_q);
            clone.m_r = (QualifiedName)Utils.Clone(this.m_r);
            clone.m_s = (LocalizedText)Utils.Clone(this.m_s);
            clone.m_t = (StatusCode)Utils.Clone(this.m_t);
            clone.m_u = (XmlElement)Utils.Clone(this.m_u);
            clone.m_v = (ConcreteStructure)Utils.Clone(this.m_v);
            clone.m_w = (EnumerationWithGaps)Utils.Clone(this.m_w);

            return clone;
        }
        #endregion

        #region Private Fields
        private bool m_a;
        private sbyte m_b;
        private byte m_c;
        private short m_d;
        private ushort m_e;
        private int m_f;
        private uint m_g;
        private long m_h;
        private ulong m_i;
        private float m_j;
        private double m_k;
        private Uuid m_l;
        private DateTime m_m;
        private string m_n;
        private byte[] m_o;
        private NodeId m_p;
        private ExpandedNodeId m_q;
        private QualifiedName m_r;
        private LocalizedText m_s;
        private StatusCode m_t;
        private XmlElement m_u;
        private ConcreteStructure m_v;
        private EnumerationWithGaps m_w;
        #endregion
    }

    #region ScalarStructureCollection Class
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [CollectionDataContract(Name = "ListOfScalarStructure", Namespace = TestModel.Namespaces.TestModelXsd, ItemName = "ScalarStructure")]
    public partial class ScalarStructureCollection : List<ScalarStructure>, ICloneable
    {
        #region Constructors
        /// <remarks />
        public ScalarStructureCollection() {}

        /// <remarks />
        public ScalarStructureCollection(int capacity) : base(capacity) {}

        /// <remarks />
        public ScalarStructureCollection(IEnumerable<ScalarStructure> collection) : base(collection) {}
        #endregion

        #region Static Operators
        /// <remarks />
        public static implicit operator ScalarStructureCollection(ScalarStructure[] values)
        {
            if (values != null)
            {
                return new ScalarStructureCollection(values);
            }

            return new ScalarStructureCollection();
        }

        /// <remarks />
        public static explicit operator ScalarStructure[](ScalarStructureCollection values)
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
            return (ScalarStructureCollection)this.MemberwiseClone();
        }
        #endregion

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            ScalarStructureCollection clone = new ScalarStructureCollection(this.Count);

            for (int ii = 0; ii < this.Count; ii++)
            {
                clone.Add((ScalarStructure)Utils.Clone(this[ii]));
            }

            return clone;
        }
    }
    #endregion
    #endif
    #endregion

    #region ScalarStructureWithAllowSubtypes Class
    #if (!OPCUA_EXCLUDE_ScalarStructureWithAllowSubtypes)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [DataContract(Namespace = TestModel.Namespaces.TestModelXsd)]
    public partial class ScalarStructureWithAllowSubtypes : TestModel.ScalarStructure
    {
        #region Constructors
        /// <remarks />
        public ScalarStructureWithAllowSubtypes()
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
            m_x = new DataValue();
            m_y = Variant.Null;
            m_z = null;
        }
        #endregion

        #region Public Properties
        /// <remarks />
        [DataMember(Name = "X", IsRequired = false, Order = 1)]
        public DataValue X
        {
            get { return m_x;  }
            set { m_x = value; }
        }

        /// <remarks />
        [DataMember(Name = "Y", IsRequired = false, Order = 2)]
        public Variant Y
        {
            get { return m_y;  }
            set { m_y = value; }
        }

        /// <remarks />
        [DataMember(Name = "Z", IsRequired = false, Order = 3)]
        public ExtensionObject Z
        {
            get { return m_z;  }
            set { m_z = value; }
        }
        #endregion

        #region IEncodeable Members
        /// <summary cref="IEncodeable.TypeId" />
        public override ExpandedNodeId TypeId => DataTypeIds.ScalarStructureWithAllowSubtypes; 

        /// <summary cref="IEncodeable.BinaryEncodingId" />
        public override ExpandedNodeId BinaryEncodingId => ObjectIds.ScalarStructureWithAllowSubtypes_Encoding_DefaultBinary;

        /// <summary cref="IEncodeable.XmlEncodingId" />
        public override ExpandedNodeId XmlEncodingId => ObjectIds.ScalarStructureWithAllowSubtypes_Encoding_DefaultXml;
            
        /// <summary cref="IJsonEncodeable.JsonEncodingId" />
        public override ExpandedNodeId JsonEncodingId => ObjectIds.ScalarStructureWithAllowSubtypes_Encoding_DefaultJson; 

        /// <summary cref="IEncodeable.Encode(IEncoder)" />
        public override void Encode(IEncoder encoder)
        {
            base.Encode(encoder);

            encoder.PushNamespace(TestModel.Namespaces.TestModelXsd);

            encoder.WriteDataValue("X", X);
            encoder.WriteVariant("Y", Y);
            encoder.WriteExtensionObject("Z", Z);

            encoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.Decode(IDecoder)" />
        public override void Decode(IDecoder decoder)
        {
            base.Decode(decoder);

            decoder.PushNamespace(TestModel.Namespaces.TestModelXsd);

            X = decoder.ReadDataValue("X");
            Y = decoder.ReadVariant("Y");
            Z = decoder.ReadExtensionObject("Z");

            decoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.IsEqual(IEncodeable)" />
        public override bool IsEqual(IEncodeable encodeable)
        {
            if (Object.ReferenceEquals(this, encodeable))
            {
                return true;
            }

            ScalarStructureWithAllowSubtypes value = encodeable as ScalarStructureWithAllowSubtypes;

            if (value == null)
            {
                return false;
            }

            if (!Utils.IsEqual(m_x, value.m_x)) return false;
            if (!Utils.IsEqual(m_y, value.m_y)) return false;
            if (!Utils.IsEqual(m_z, value.m_z)) return false;

            return base.IsEqual(encodeable);
        }    

        /// <summary cref="ICloneable.Clone" />
        public override object Clone()
        {
            return (ScalarStructureWithAllowSubtypes)this.MemberwiseClone();
        }

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            ScalarStructureWithAllowSubtypes clone = (ScalarStructureWithAllowSubtypes)base.MemberwiseClone();

            clone.m_x = (DataValue)Utils.Clone(this.m_x);
            clone.m_y = (Variant)Utils.Clone(this.m_y);
            clone.m_z = (ExtensionObject)Utils.Clone(this.m_z);

            return clone;
        }
        #endregion

        #region Private Fields
        private DataValue m_x;
        private Variant m_y;
        private ExtensionObject m_z;
        #endregion
    }

    #region ScalarStructureWithAllowSubtypesCollection Class
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [CollectionDataContract(Name = "ListOfScalarStructureWithAllowSubtypes", Namespace = TestModel.Namespaces.TestModelXsd, ItemName = "ScalarStructureWithAllowSubtypes")]
    public partial class ScalarStructureWithAllowSubtypesCollection : List<ScalarStructureWithAllowSubtypes>, ICloneable
    {
        #region Constructors
        /// <remarks />
        public ScalarStructureWithAllowSubtypesCollection() {}

        /// <remarks />
        public ScalarStructureWithAllowSubtypesCollection(int capacity) : base(capacity) {}

        /// <remarks />
        public ScalarStructureWithAllowSubtypesCollection(IEnumerable<ScalarStructureWithAllowSubtypes> collection) : base(collection) {}
        #endregion

        #region Static Operators
        /// <remarks />
        public static implicit operator ScalarStructureWithAllowSubtypesCollection(ScalarStructureWithAllowSubtypes[] values)
        {
            if (values != null)
            {
                return new ScalarStructureWithAllowSubtypesCollection(values);
            }

            return new ScalarStructureWithAllowSubtypesCollection();
        }

        /// <remarks />
        public static explicit operator ScalarStructureWithAllowSubtypes[](ScalarStructureWithAllowSubtypesCollection values)
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
            return (ScalarStructureWithAllowSubtypesCollection)this.MemberwiseClone();
        }
        #endregion

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            ScalarStructureWithAllowSubtypesCollection clone = new ScalarStructureWithAllowSubtypesCollection(this.Count);

            for (int ii = 0; ii < this.Count; ii++)
            {
                clone.Add((ScalarStructureWithAllowSubtypes)Utils.Clone(this[ii]));
            }

            return clone;
        }
    }
    #endregion
    #endif
    #endregion

    #region ArrayStructure Class
    #if (!OPCUA_EXCLUDE_ArrayStructure)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [DataContract(Namespace = TestModel.Namespaces.TestModelXsd)]
    public partial class ArrayStructure : IEncodeable, IJsonEncodeable
    {
        #region Constructors
        /// <remarks />
        public ArrayStructure()
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
            m_a = new BooleanCollection();
            m_b = new SByteCollection();
            m_c = new ByteCollection();
            m_d = new Int16Collection();
            m_e = new UInt16Collection();
            m_f = new Int32Collection();
            m_g = new UInt32Collection();
            m_h = new Int64Collection();
            m_i = new UInt64Collection();
            m_j = new FloatCollection();
            m_k = new DoubleCollection();
            m_l = new UuidCollection();
            m_m = new DateTimeCollection();
            m_n = new StringCollection();
            m_o = new ByteStringCollection();
            m_p = new NodeIdCollection();
            m_q = new ExpandedNodeIdCollection();
            m_r = new QualifiedNameCollection();
            m_s = new LocalizedTextCollection();
            m_t = new StatusCodeCollection();
            m_u = new XmlElementCollection();
            m_v = new ConcreteStructureCollection();
            m_w = new EnumerationWithGapsCollection();
        }
        #endregion

        #region Public Properties
        /// <remarks />
        [DataMember(Name = "A", IsRequired = false, Order = 1)]
        public BooleanCollection A
        {
            get
            {
                return m_a;
            }

            set
            {
                m_a = value;

                if (value == null)
                {
                    m_a = new BooleanCollection();
                }
            }
        }

        /// <remarks />
        [DataMember(Name = "B", IsRequired = false, Order = 2)]
        public SByteCollection B
        {
            get
            {
                return m_b;
            }

            set
            {
                m_b = value;

                if (value == null)
                {
                    m_b = new SByteCollection();
                }
            }
        }

        /// <remarks />
        [DataMember(Name = "C", IsRequired = false, Order = 3)]
        public ByteCollection C
        {
            get
            {
                return m_c;
            }

            set
            {
                m_c = value;

                if (value == null)
                {
                    m_c = new ByteCollection();
                }
            }
        }

        /// <remarks />
        [DataMember(Name = "D", IsRequired = false, Order = 4)]
        public Int16Collection D
        {
            get
            {
                return m_d;
            }

            set
            {
                m_d = value;

                if (value == null)
                {
                    m_d = new Int16Collection();
                }
            }
        }

        /// <remarks />
        [DataMember(Name = "E", IsRequired = false, Order = 5)]
        public UInt16Collection E
        {
            get
            {
                return m_e;
            }

            set
            {
                m_e = value;

                if (value == null)
                {
                    m_e = new UInt16Collection();
                }
            }
        }

        /// <remarks />
        [DataMember(Name = "F", IsRequired = false, Order = 6)]
        public Int32Collection F
        {
            get
            {
                return m_f;
            }

            set
            {
                m_f = value;

                if (value == null)
                {
                    m_f = new Int32Collection();
                }
            }
        }

        /// <remarks />
        [DataMember(Name = "G", IsRequired = false, Order = 7)]
        public UInt32Collection G
        {
            get
            {
                return m_g;
            }

            set
            {
                m_g = value;

                if (value == null)
                {
                    m_g = new UInt32Collection();
                }
            }
        }

        /// <remarks />
        [DataMember(Name = "H", IsRequired = false, Order = 8)]
        public Int64Collection H
        {
            get
            {
                return m_h;
            }

            set
            {
                m_h = value;

                if (value == null)
                {
                    m_h = new Int64Collection();
                }
            }
        }

        /// <remarks />
        [DataMember(Name = "I", IsRequired = false, Order = 9)]
        public UInt64Collection I
        {
            get
            {
                return m_i;
            }

            set
            {
                m_i = value;

                if (value == null)
                {
                    m_i = new UInt64Collection();
                }
            }
        }

        /// <remarks />
        [DataMember(Name = "J", IsRequired = false, Order = 10)]
        public FloatCollection J
        {
            get
            {
                return m_j;
            }

            set
            {
                m_j = value;

                if (value == null)
                {
                    m_j = new FloatCollection();
                }
            }
        }

        /// <remarks />
        [DataMember(Name = "K", IsRequired = false, Order = 11)]
        public DoubleCollection K
        {
            get
            {
                return m_k;
            }

            set
            {
                m_k = value;

                if (value == null)
                {
                    m_k = new DoubleCollection();
                }
            }
        }

        /// <remarks />
        [DataMember(Name = "L", IsRequired = false, Order = 12)]
        public UuidCollection L
        {
            get
            {
                return m_l;
            }

            set
            {
                m_l = value;

                if (value == null)
                {
                    m_l = new UuidCollection();
                }
            }
        }

        /// <remarks />
        [DataMember(Name = "M", IsRequired = false, Order = 13)]
        public DateTimeCollection M
        {
            get
            {
                return m_m;
            }

            set
            {
                m_m = value;

                if (value == null)
                {
                    m_m = new DateTimeCollection();
                }
            }
        }

        /// <remarks />
        [DataMember(Name = "N", IsRequired = false, Order = 14)]
        public StringCollection N
        {
            get
            {
                return m_n;
            }

            set
            {
                m_n = value;

                if (value == null)
                {
                    m_n = new StringCollection();
                }
            }
        }

        /// <remarks />
        [DataMember(Name = "O", IsRequired = false, Order = 15)]
        public ByteStringCollection O
        {
            get
            {
                return m_o;
            }

            set
            {
                m_o = value;

                if (value == null)
                {
                    m_o = new ByteStringCollection();
                }
            }
        }

        /// <remarks />
        [DataMember(Name = "P", IsRequired = false, Order = 16)]
        public NodeIdCollection P
        {
            get
            {
                return m_p;
            }

            set
            {
                m_p = value;

                if (value == null)
                {
                    m_p = new NodeIdCollection();
                }
            }
        }

        /// <remarks />
        [DataMember(Name = "Q", IsRequired = false, Order = 17)]
        public ExpandedNodeIdCollection Q
        {
            get
            {
                return m_q;
            }

            set
            {
                m_q = value;

                if (value == null)
                {
                    m_q = new ExpandedNodeIdCollection();
                }
            }
        }

        /// <remarks />
        [DataMember(Name = "R", IsRequired = false, Order = 18)]
        public QualifiedNameCollection R
        {
            get
            {
                return m_r;
            }

            set
            {
                m_r = value;

                if (value == null)
                {
                    m_r = new QualifiedNameCollection();
                }
            }
        }

        /// <remarks />
        [DataMember(Name = "S", IsRequired = false, Order = 19)]
        public LocalizedTextCollection S
        {
            get
            {
                return m_s;
            }

            set
            {
                m_s = value;

                if (value == null)
                {
                    m_s = new LocalizedTextCollection();
                }
            }
        }

        /// <remarks />
        [DataMember(Name = "T", IsRequired = false, Order = 20)]
        public StatusCodeCollection T
        {
            get
            {
                return m_t;
            }

            set
            {
                m_t = value;

                if (value == null)
                {
                    m_t = new StatusCodeCollection();
                }
            }
        }

        /// <remarks />
        [DataMember(Name = "U", IsRequired = false, Order = 21)]
        public XmlElementCollection U
        {
            get
            {
                return m_u;
            }

            set
            {
                m_u = value;

                if (value == null)
                {
                    m_u = new XmlElementCollection();
                }
            }
        }

        /// <remarks />
        [DataMember(Name = "V", IsRequired = false, Order = 22)]
        public ConcreteStructureCollection V
        {
            get
            {
                return m_v;
            }

            set
            {
                m_v = value;

                if (value == null)
                {
                    m_v = new ConcreteStructureCollection();
                }
            }
        }

        /// <remarks />
        [DataMember(Name = "W", IsRequired = false, Order = 23)]
        public EnumerationWithGapsCollection W
        {
            get
            {
                return m_w;
            }

            set
            {
                m_w = value;

                if (value == null)
                {
                    m_w = new EnumerationWithGapsCollection();
                }
            }
        }
        #endregion

        #region IEncodeable Members
        /// <summary cref="IEncodeable.TypeId" />
        public virtual ExpandedNodeId TypeId => DataTypeIds.ArrayStructure; 

        /// <summary cref="IEncodeable.BinaryEncodingId" />
        public virtual ExpandedNodeId BinaryEncodingId => ObjectIds.ArrayStructure_Encoding_DefaultBinary;

        /// <summary cref="IEncodeable.XmlEncodingId" />
        public virtual ExpandedNodeId XmlEncodingId => ObjectIds.ArrayStructure_Encoding_DefaultXml;
                    
        /// <summary cref="IJsonEncodeable.JsonEncodingId" />
        public virtual ExpandedNodeId JsonEncodingId => ObjectIds.ArrayStructure_Encoding_DefaultJson; 

        /// <summary cref="IEncodeable.Encode(IEncoder)" />
        public virtual void Encode(IEncoder encoder)
        {
            encoder.PushNamespace(TestModel.Namespaces.TestModelXsd);

            encoder.WriteBooleanArray("A", A);
            encoder.WriteSByteArray("B", B);
            encoder.WriteByteArray("C", C);
            encoder.WriteInt16Array("D", D);
            encoder.WriteUInt16Array("E", E);
            encoder.WriteInt32Array("F", F);
            encoder.WriteUInt32Array("G", G);
            encoder.WriteInt64Array("H", H);
            encoder.WriteUInt64Array("I", I);
            encoder.WriteFloatArray("J", J);
            encoder.WriteDoubleArray("K", K);
            encoder.WriteGuidArray("L", L);
            encoder.WriteDateTimeArray("M", M);
            encoder.WriteStringArray("N", N);
            encoder.WriteByteStringArray("O", O);
            encoder.WriteNodeIdArray("P", P);
            encoder.WriteExpandedNodeIdArray("Q", Q);
            encoder.WriteQualifiedNameArray("R", R);
            encoder.WriteLocalizedTextArray("S", S);
            encoder.WriteStatusCodeArray("T", T);
            encoder.WriteXmlElementArray("U", U);
            encoder.WriteEncodeableArray("V", V.ToArray(), typeof(ConcreteStructure));
            encoder.WriteEnumeratedArray("W", W.ToArray(), typeof(EnumerationWithGaps));

            encoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.Decode(IDecoder)" />
        public virtual void Decode(IDecoder decoder)
        {
            decoder.PushNamespace(TestModel.Namespaces.TestModelXsd);

            A = decoder.ReadBooleanArray("A");
            B = decoder.ReadSByteArray("B");
            C = decoder.ReadByteArray("C");
            D = decoder.ReadInt16Array("D");
            E = decoder.ReadUInt16Array("E");
            F = decoder.ReadInt32Array("F");
            G = decoder.ReadUInt32Array("G");
            H = decoder.ReadInt64Array("H");
            I = decoder.ReadUInt64Array("I");
            J = decoder.ReadFloatArray("J");
            K = decoder.ReadDoubleArray("K");
            L = decoder.ReadGuidArray("L");
            M = decoder.ReadDateTimeArray("M");
            N = decoder.ReadStringArray("N");
            O = decoder.ReadByteStringArray("O");
            P = decoder.ReadNodeIdArray("P");
            Q = decoder.ReadExpandedNodeIdArray("Q");
            R = decoder.ReadQualifiedNameArray("R");
            S = decoder.ReadLocalizedTextArray("S");
            T = decoder.ReadStatusCodeArray("T");
            U = decoder.ReadXmlElementArray("U");
            V = (ConcreteStructureCollection)decoder.ReadEncodeableArray("V", typeof(ConcreteStructure));
            W = (EnumerationWithGapsCollection)decoder.ReadEnumeratedArray("W", typeof(EnumerationWithGaps));

            decoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.IsEqual(IEncodeable)" />
        public virtual bool IsEqual(IEncodeable encodeable)
        {
            if (Object.ReferenceEquals(this, encodeable))
            {
                return true;
            }

            ArrayStructure value = encodeable as ArrayStructure;

            if (value == null)
            {
                return false;
            }

            if (!Utils.IsEqual(m_a, value.m_a)) return false;
            if (!Utils.IsEqual(m_b, value.m_b)) return false;
            if (!Utils.IsEqual(m_c, value.m_c)) return false;
            if (!Utils.IsEqual(m_d, value.m_d)) return false;
            if (!Utils.IsEqual(m_e, value.m_e)) return false;
            if (!Utils.IsEqual(m_f, value.m_f)) return false;
            if (!Utils.IsEqual(m_g, value.m_g)) return false;
            if (!Utils.IsEqual(m_h, value.m_h)) return false;
            if (!Utils.IsEqual(m_i, value.m_i)) return false;
            if (!Utils.IsEqual(m_j, value.m_j)) return false;
            if (!Utils.IsEqual(m_k, value.m_k)) return false;
            if (!Utils.IsEqual(m_l, value.m_l)) return false;
            if (!Utils.IsEqual(m_m, value.m_m)) return false;
            if (!Utils.IsEqual(m_n, value.m_n)) return false;
            if (!Utils.IsEqual(m_o, value.m_o)) return false;
            if (!Utils.IsEqual(m_p, value.m_p)) return false;
            if (!Utils.IsEqual(m_q, value.m_q)) return false;
            if (!Utils.IsEqual(m_r, value.m_r)) return false;
            if (!Utils.IsEqual(m_s, value.m_s)) return false;
            if (!Utils.IsEqual(m_t, value.m_t)) return false;
            if (!Utils.IsEqual(m_u, value.m_u)) return false;
            if (!Utils.IsEqual(m_v, value.m_v)) return false;
            if (!Utils.IsEqual(m_w, value.m_w)) return false;

            return true;
        }

        /// <summary cref="ICloneable.Clone" />
        public virtual object Clone()
        {
            return (ArrayStructure)this.MemberwiseClone();
        }

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            ArrayStructure clone = (ArrayStructure)base.MemberwiseClone();

            clone.m_a = (BooleanCollection)Utils.Clone(this.m_a);
            clone.m_b = (SByteCollection)Utils.Clone(this.m_b);
            clone.m_c = (ByteCollection)Utils.Clone(this.m_c);
            clone.m_d = (Int16Collection)Utils.Clone(this.m_d);
            clone.m_e = (UInt16Collection)Utils.Clone(this.m_e);
            clone.m_f = (Int32Collection)Utils.Clone(this.m_f);
            clone.m_g = (UInt32Collection)Utils.Clone(this.m_g);
            clone.m_h = (Int64Collection)Utils.Clone(this.m_h);
            clone.m_i = (UInt64Collection)Utils.Clone(this.m_i);
            clone.m_j = (FloatCollection)Utils.Clone(this.m_j);
            clone.m_k = (DoubleCollection)Utils.Clone(this.m_k);
            clone.m_l = (UuidCollection)Utils.Clone(this.m_l);
            clone.m_m = (DateTimeCollection)Utils.Clone(this.m_m);
            clone.m_n = (StringCollection)Utils.Clone(this.m_n);
            clone.m_o = (ByteStringCollection)Utils.Clone(this.m_o);
            clone.m_p = (NodeIdCollection)Utils.Clone(this.m_p);
            clone.m_q = (ExpandedNodeIdCollection)Utils.Clone(this.m_q);
            clone.m_r = (QualifiedNameCollection)Utils.Clone(this.m_r);
            clone.m_s = (LocalizedTextCollection)Utils.Clone(this.m_s);
            clone.m_t = (StatusCodeCollection)Utils.Clone(this.m_t);
            clone.m_u = (XmlElementCollection)Utils.Clone(this.m_u);
            clone.m_v = (ConcreteStructureCollection)Utils.Clone(this.m_v);
            clone.m_w = (EnumerationWithGapsCollection)Utils.Clone(this.m_w);

            return clone;
        }
        #endregion

        #region Private Fields
        private BooleanCollection m_a;
        private SByteCollection m_b;
        private ByteCollection m_c;
        private Int16Collection m_d;
        private UInt16Collection m_e;
        private Int32Collection m_f;
        private UInt32Collection m_g;
        private Int64Collection m_h;
        private UInt64Collection m_i;
        private FloatCollection m_j;
        private DoubleCollection m_k;
        private UuidCollection m_l;
        private DateTimeCollection m_m;
        private StringCollection m_n;
        private ByteStringCollection m_o;
        private NodeIdCollection m_p;
        private ExpandedNodeIdCollection m_q;
        private QualifiedNameCollection m_r;
        private LocalizedTextCollection m_s;
        private StatusCodeCollection m_t;
        private XmlElementCollection m_u;
        private ConcreteStructureCollection m_v;
        private EnumerationWithGapsCollection m_w;
        #endregion
    }

    #region ArrayStructureCollection Class
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [CollectionDataContract(Name = "ListOfArrayStructure", Namespace = TestModel.Namespaces.TestModelXsd, ItemName = "ArrayStructure")]
    public partial class ArrayStructureCollection : List<ArrayStructure>, ICloneable
    {
        #region Constructors
        /// <remarks />
        public ArrayStructureCollection() {}

        /// <remarks />
        public ArrayStructureCollection(int capacity) : base(capacity) {}

        /// <remarks />
        public ArrayStructureCollection(IEnumerable<ArrayStructure> collection) : base(collection) {}
        #endregion

        #region Static Operators
        /// <remarks />
        public static implicit operator ArrayStructureCollection(ArrayStructure[] values)
        {
            if (values != null)
            {
                return new ArrayStructureCollection(values);
            }

            return new ArrayStructureCollection();
        }

        /// <remarks />
        public static explicit operator ArrayStructure[](ArrayStructureCollection values)
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
            return (ArrayStructureCollection)this.MemberwiseClone();
        }
        #endregion

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            ArrayStructureCollection clone = new ArrayStructureCollection(this.Count);

            for (int ii = 0; ii < this.Count; ii++)
            {
                clone.Add((ArrayStructure)Utils.Clone(this[ii]));
            }

            return clone;
        }
    }
    #endregion
    #endif
    #endregion

    #region ArrayStructureWithAllowSubtypes Class
    #if (!OPCUA_EXCLUDE_ArrayStructureWithAllowSubtypes)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [DataContract(Namespace = TestModel.Namespaces.TestModelXsd)]
    public partial class ArrayStructureWithAllowSubtypes : TestModel.ArrayStructure
    {
        #region Constructors
        /// <remarks />
        public ArrayStructureWithAllowSubtypes()
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
            m_x = new DataValueCollection();
            m_y = new VariantCollection();
            m_z = new ExtensionObjectCollection();
        }
        #endregion

        #region Public Properties
        /// <remarks />
        [DataMember(Name = "X", IsRequired = false, Order = 1)]
        public DataValueCollection X
        {
            get
            {
                return m_x;
            }

            set
            {
                m_x = value;

                if (value == null)
                {
                    m_x = new DataValueCollection();
                }
            }
        }

        /// <remarks />
        [DataMember(Name = "Y", IsRequired = false, Order = 2)]
        public VariantCollection Y
        {
            get
            {
                return m_y;
            }

            set
            {
                m_y = value;

                if (value == null)
                {
                    m_y = new VariantCollection();
                }
            }
        }

        /// <remarks />
        [DataMember(Name = "Z", IsRequired = false, Order = 3)]
        public ExtensionObjectCollection Z
        {
            get { return m_z;  }
            set { m_z = value; }
        }
        #endregion

        #region IEncodeable Members
        /// <summary cref="IEncodeable.TypeId" />
        public override ExpandedNodeId TypeId => DataTypeIds.ArrayStructureWithAllowSubtypes; 

        /// <summary cref="IEncodeable.BinaryEncodingId" />
        public override ExpandedNodeId BinaryEncodingId => ObjectIds.ArrayStructureWithAllowSubtypes_Encoding_DefaultBinary;

        /// <summary cref="IEncodeable.XmlEncodingId" />
        public override ExpandedNodeId XmlEncodingId => ObjectIds.ArrayStructureWithAllowSubtypes_Encoding_DefaultXml;
            
        /// <summary cref="IJsonEncodeable.JsonEncodingId" />
        public override ExpandedNodeId JsonEncodingId => ObjectIds.ArrayStructureWithAllowSubtypes_Encoding_DefaultJson; 

        /// <summary cref="IEncodeable.Encode(IEncoder)" />
        public override void Encode(IEncoder encoder)
        {
            base.Encode(encoder);

            encoder.PushNamespace(TestModel.Namespaces.TestModelXsd);

            encoder.WriteDataValueArray("X", X);
            encoder.WriteVariantArray("Y", Y);
            encoder.WriteExtensionObjectArray("Z", Z);

            encoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.Decode(IDecoder)" />
        public override void Decode(IDecoder decoder)
        {
            base.Decode(decoder);

            decoder.PushNamespace(TestModel.Namespaces.TestModelXsd);

            X = decoder.ReadDataValueArray("X");
            Y = decoder.ReadVariantArray("Y");
            Z = decoder.ReadExtensionObjectArray("Z");

            decoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.IsEqual(IEncodeable)" />
        public override bool IsEqual(IEncodeable encodeable)
        {
            if (Object.ReferenceEquals(this, encodeable))
            {
                return true;
            }

            ArrayStructureWithAllowSubtypes value = encodeable as ArrayStructureWithAllowSubtypes;

            if (value == null)
            {
                return false;
            }

            if (!Utils.IsEqual(m_x, value.m_x)) return false;
            if (!Utils.IsEqual(m_y, value.m_y)) return false;
            if (!Utils.IsEqual(m_z, value.m_z)) return false;

            return base.IsEqual(encodeable);
        }    

        /// <summary cref="ICloneable.Clone" />
        public override object Clone()
        {
            return (ArrayStructureWithAllowSubtypes)this.MemberwiseClone();
        }

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            ArrayStructureWithAllowSubtypes clone = (ArrayStructureWithAllowSubtypes)base.MemberwiseClone();

            clone.m_x = (DataValueCollection)Utils.Clone(this.m_x);
            clone.m_y = (VariantCollection)Utils.Clone(this.m_y);
            clone.m_z = (ExtensionObjectCollection)Utils.Clone(this.m_z);

            return clone;
        }
        #endregion

        #region Private Fields
        private DataValueCollection m_x;
        private VariantCollection m_y;
        private ExtensionObjectCollection m_z;
        #endregion
    }

    #region ArrayStructureWithAllowSubtypesCollection Class
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [CollectionDataContract(Name = "ListOfArrayStructureWithAllowSubtypes", Namespace = TestModel.Namespaces.TestModelXsd, ItemName = "ArrayStructureWithAllowSubtypes")]
    public partial class ArrayStructureWithAllowSubtypesCollection : List<ArrayStructureWithAllowSubtypes>, ICloneable
    {
        #region Constructors
        /// <remarks />
        public ArrayStructureWithAllowSubtypesCollection() {}

        /// <remarks />
        public ArrayStructureWithAllowSubtypesCollection(int capacity) : base(capacity) {}

        /// <remarks />
        public ArrayStructureWithAllowSubtypesCollection(IEnumerable<ArrayStructureWithAllowSubtypes> collection) : base(collection) {}
        #endregion

        #region Static Operators
        /// <remarks />
        public static implicit operator ArrayStructureWithAllowSubtypesCollection(ArrayStructureWithAllowSubtypes[] values)
        {
            if (values != null)
            {
                return new ArrayStructureWithAllowSubtypesCollection(values);
            }

            return new ArrayStructureWithAllowSubtypesCollection();
        }

        /// <remarks />
        public static explicit operator ArrayStructureWithAllowSubtypes[](ArrayStructureWithAllowSubtypesCollection values)
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
            return (ArrayStructureWithAllowSubtypesCollection)this.MemberwiseClone();
        }
        #endregion

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            ArrayStructureWithAllowSubtypesCollection clone = new ArrayStructureWithAllowSubtypesCollection(this.Count);

            for (int ii = 0; ii < this.Count; ii++)
            {
                clone.Add((ArrayStructureWithAllowSubtypes)Utils.Clone(this[ii]));
            }

            return clone;
        }
    }
    #endregion
    #endif
    #endregion

    #region NestedStructure Class
    #if (!OPCUA_EXCLUDE_NestedStructure)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [DataContract(Namespace = TestModel.Namespaces.TestModelXsd)]
    public partial class NestedStructure : IEncodeable, IJsonEncodeable
    {
        #region Constructors
        /// <remarks />
        public NestedStructure()
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
            m_a = new ScalarStructure();
            m_b = new ArrayStructure();
            m_c = new ScalarStructureCollection();
            m_d = new ArrayStructureCollection();
        }
        #endregion

        #region Public Properties
        /// <remarks />
        [DataMember(Name = "A", IsRequired = false, Order = 1)]
        public ScalarStructure A
        {
            get
            {
                return m_a;
            }

            set
            {
                m_a = value;

                if (value == null)
                {
                    m_a = new ScalarStructure();
                }
            }
        }

        /// <remarks />
        [DataMember(Name = "B", IsRequired = false, Order = 2)]
        public ArrayStructure B
        {
            get
            {
                return m_b;
            }

            set
            {
                m_b = value;

                if (value == null)
                {
                    m_b = new ArrayStructure();
                }
            }
        }

        /// <remarks />
        [DataMember(Name = "C", IsRequired = false, Order = 3)]
        public ScalarStructureCollection C
        {
            get
            {
                return m_c;
            }

            set
            {
                m_c = value;

                if (value == null)
                {
                    m_c = new ScalarStructureCollection();
                }
            }
        }

        /// <remarks />
        [DataMember(Name = "D", IsRequired = false, Order = 4)]
        public ArrayStructureCollection D
        {
            get
            {
                return m_d;
            }

            set
            {
                m_d = value;

                if (value == null)
                {
                    m_d = new ArrayStructureCollection();
                }
            }
        }
        #endregion

        #region IEncodeable Members
        /// <summary cref="IEncodeable.TypeId" />
        public virtual ExpandedNodeId TypeId => DataTypeIds.NestedStructure; 

        /// <summary cref="IEncodeable.BinaryEncodingId" />
        public virtual ExpandedNodeId BinaryEncodingId => ObjectIds.NestedStructure_Encoding_DefaultBinary;

        /// <summary cref="IEncodeable.XmlEncodingId" />
        public virtual ExpandedNodeId XmlEncodingId => ObjectIds.NestedStructure_Encoding_DefaultXml;
                    
        /// <summary cref="IJsonEncodeable.JsonEncodingId" />
        public virtual ExpandedNodeId JsonEncodingId => ObjectIds.NestedStructure_Encoding_DefaultJson; 

        /// <summary cref="IEncodeable.Encode(IEncoder)" />
        public virtual void Encode(IEncoder encoder)
        {
            encoder.PushNamespace(TestModel.Namespaces.TestModelXsd);

            encoder.WriteEncodeable("A", A, typeof(ScalarStructure));
            encoder.WriteEncodeable("B", B, typeof(ArrayStructure));
            encoder.WriteEncodeableArray("C", C.ToArray(), typeof(ScalarStructure));
            encoder.WriteEncodeableArray("D", D.ToArray(), typeof(ArrayStructure));

            encoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.Decode(IDecoder)" />
        public virtual void Decode(IDecoder decoder)
        {
            decoder.PushNamespace(TestModel.Namespaces.TestModelXsd);

            A = (ScalarStructure)decoder.ReadEncodeable("A", typeof(ScalarStructure));
            B = (ArrayStructure)decoder.ReadEncodeable("B", typeof(ArrayStructure));
            C = (ScalarStructureCollection)decoder.ReadEncodeableArray("C", typeof(ScalarStructure));
            D = (ArrayStructureCollection)decoder.ReadEncodeableArray("D", typeof(ArrayStructure));

            decoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.IsEqual(IEncodeable)" />
        public virtual bool IsEqual(IEncodeable encodeable)
        {
            if (Object.ReferenceEquals(this, encodeable))
            {
                return true;
            }

            NestedStructure value = encodeable as NestedStructure;

            if (value == null)
            {
                return false;
            }

            if (!Utils.IsEqual(m_a, value.m_a)) return false;
            if (!Utils.IsEqual(m_b, value.m_b)) return false;
            if (!Utils.IsEqual(m_c, value.m_c)) return false;
            if (!Utils.IsEqual(m_d, value.m_d)) return false;

            return true;
        }

        /// <summary cref="ICloneable.Clone" />
        public virtual object Clone()
        {
            return (NestedStructure)this.MemberwiseClone();
        }

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            NestedStructure clone = (NestedStructure)base.MemberwiseClone();

            clone.m_a = (ScalarStructure)Utils.Clone(this.m_a);
            clone.m_b = (ArrayStructure)Utils.Clone(this.m_b);
            clone.m_c = (ScalarStructureCollection)Utils.Clone(this.m_c);
            clone.m_d = (ArrayStructureCollection)Utils.Clone(this.m_d);

            return clone;
        }
        #endregion

        #region Private Fields
        private ScalarStructure m_a;
        private ArrayStructure m_b;
        private ScalarStructureCollection m_c;
        private ArrayStructureCollection m_d;
        #endregion
    }

    #region NestedStructureCollection Class
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [CollectionDataContract(Name = "ListOfNestedStructure", Namespace = TestModel.Namespaces.TestModelXsd, ItemName = "NestedStructure")]
    public partial class NestedStructureCollection : List<NestedStructure>, ICloneable
    {
        #region Constructors
        /// <remarks />
        public NestedStructureCollection() {}

        /// <remarks />
        public NestedStructureCollection(int capacity) : base(capacity) {}

        /// <remarks />
        public NestedStructureCollection(IEnumerable<NestedStructure> collection) : base(collection) {}
        #endregion

        #region Static Operators
        /// <remarks />
        public static implicit operator NestedStructureCollection(NestedStructure[] values)
        {
            if (values != null)
            {
                return new NestedStructureCollection(values);
            }

            return new NestedStructureCollection();
        }

        /// <remarks />
        public static explicit operator NestedStructure[](NestedStructureCollection values)
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
            return (NestedStructureCollection)this.MemberwiseClone();
        }
        #endregion

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            NestedStructureCollection clone = new NestedStructureCollection(this.Count);

            for (int ii = 0; ii < this.Count; ii++)
            {
                clone.Add((NestedStructure)Utils.Clone(this[ii]));
            }

            return clone;
        }
    }
    #endregion
    #endif
    #endregion

    #region NestedStructureWithAllowSubtypes Class
    #if (!OPCUA_EXCLUDE_NestedStructureWithAllowSubtypes)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [DataContract(Namespace = TestModel.Namespaces.TestModelXsd)]
    public partial class NestedStructureWithAllowSubtypes : IEncodeable, IJsonEncodeable
    {
        #region Constructors
        /// <remarks />
        public NestedStructureWithAllowSubtypes()
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
            m_a = new ScalarStructureWithAllowSubtypes();
            m_b = new ArrayStructureWithAllowSubtypes();
            m_c = new ScalarStructureWithAllowSubtypesCollection();
            m_d = new ArrayStructureWithAllowSubtypesCollection();
        }
        #endregion

        #region Public Properties
        /// <remarks />
        [DataMember(Name = "A", IsRequired = false, Order = 1)]
        public ScalarStructureWithAllowSubtypes A
        {
            get
            {
                return m_a;
            }

            set
            {
                m_a = value;

                if (value == null)
                {
                    m_a = new ScalarStructureWithAllowSubtypes();
                }
            }
        }

        /// <remarks />
        [DataMember(Name = "B", IsRequired = false, Order = 2)]
        public ArrayStructureWithAllowSubtypes B
        {
            get
            {
                return m_b;
            }

            set
            {
                m_b = value;

                if (value == null)
                {
                    m_b = new ArrayStructureWithAllowSubtypes();
                }
            }
        }

        /// <remarks />
        [DataMember(Name = "C", IsRequired = false, Order = 3)]
        public ScalarStructureWithAllowSubtypesCollection C
        {
            get
            {
                return m_c;
            }

            set
            {
                m_c = value;

                if (value == null)
                {
                    m_c = new ScalarStructureWithAllowSubtypesCollection();
                }
            }
        }

        /// <remarks />
        [DataMember(Name = "D", IsRequired = false, Order = 4)]
        public ArrayStructureWithAllowSubtypesCollection D
        {
            get
            {
                return m_d;
            }

            set
            {
                m_d = value;

                if (value == null)
                {
                    m_d = new ArrayStructureWithAllowSubtypesCollection();
                }
            }
        }
        #endregion

        #region IEncodeable Members
        /// <summary cref="IEncodeable.TypeId" />
        public virtual ExpandedNodeId TypeId => DataTypeIds.NestedStructureWithAllowSubtypes; 

        /// <summary cref="IEncodeable.BinaryEncodingId" />
        public virtual ExpandedNodeId BinaryEncodingId => ObjectIds.NestedStructureWithAllowSubtypes_Encoding_DefaultBinary;

        /// <summary cref="IEncodeable.XmlEncodingId" />
        public virtual ExpandedNodeId XmlEncodingId => ObjectIds.NestedStructureWithAllowSubtypes_Encoding_DefaultXml;
                    
        /// <summary cref="IJsonEncodeable.JsonEncodingId" />
        public virtual ExpandedNodeId JsonEncodingId => ObjectIds.NestedStructureWithAllowSubtypes_Encoding_DefaultJson; 

        /// <summary cref="IEncodeable.Encode(IEncoder)" />
        public virtual void Encode(IEncoder encoder)
        {
            encoder.PushNamespace(TestModel.Namespaces.TestModelXsd);

            encoder.WriteEncodeable("A", A, typeof(ScalarStructureWithAllowSubtypes));
            encoder.WriteEncodeable("B", B, typeof(ArrayStructureWithAllowSubtypes));
            encoder.WriteEncodeableArray("C", C.ToArray(), typeof(ScalarStructureWithAllowSubtypes));
            encoder.WriteEncodeableArray("D", D.ToArray(), typeof(ArrayStructureWithAllowSubtypes));

            encoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.Decode(IDecoder)" />
        public virtual void Decode(IDecoder decoder)
        {
            decoder.PushNamespace(TestModel.Namespaces.TestModelXsd);

            A = (ScalarStructureWithAllowSubtypes)decoder.ReadEncodeable("A", typeof(ScalarStructureWithAllowSubtypes));
            B = (ArrayStructureWithAllowSubtypes)decoder.ReadEncodeable("B", typeof(ArrayStructureWithAllowSubtypes));
            C = (ScalarStructureWithAllowSubtypesCollection)decoder.ReadEncodeableArray("C", typeof(ScalarStructureWithAllowSubtypes));
            D = (ArrayStructureWithAllowSubtypesCollection)decoder.ReadEncodeableArray("D", typeof(ArrayStructureWithAllowSubtypes));

            decoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.IsEqual(IEncodeable)" />
        public virtual bool IsEqual(IEncodeable encodeable)
        {
            if (Object.ReferenceEquals(this, encodeable))
            {
                return true;
            }

            NestedStructureWithAllowSubtypes value = encodeable as NestedStructureWithAllowSubtypes;

            if (value == null)
            {
                return false;
            }

            if (!Utils.IsEqual(m_a, value.m_a)) return false;
            if (!Utils.IsEqual(m_b, value.m_b)) return false;
            if (!Utils.IsEqual(m_c, value.m_c)) return false;
            if (!Utils.IsEqual(m_d, value.m_d)) return false;

            return true;
        }

        /// <summary cref="ICloneable.Clone" />
        public virtual object Clone()
        {
            return (NestedStructureWithAllowSubtypes)this.MemberwiseClone();
        }

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            NestedStructureWithAllowSubtypes clone = (NestedStructureWithAllowSubtypes)base.MemberwiseClone();

            clone.m_a = (ScalarStructureWithAllowSubtypes)Utils.Clone(this.m_a);
            clone.m_b = (ArrayStructureWithAllowSubtypes)Utils.Clone(this.m_b);
            clone.m_c = (ScalarStructureWithAllowSubtypesCollection)Utils.Clone(this.m_c);
            clone.m_d = (ArrayStructureWithAllowSubtypesCollection)Utils.Clone(this.m_d);

            return clone;
        }
        #endregion

        #region Private Fields
        private ScalarStructureWithAllowSubtypes m_a;
        private ArrayStructureWithAllowSubtypes m_b;
        private ScalarStructureWithAllowSubtypesCollection m_c;
        private ArrayStructureWithAllowSubtypesCollection m_d;
        #endregion
    }

    #region NestedStructureWithAllowSubtypesCollection Class
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [CollectionDataContract(Name = "ListOfNestedStructureWithAllowSubtypes", Namespace = TestModel.Namespaces.TestModelXsd, ItemName = "NestedStructureWithAllowSubtypes")]
    public partial class NestedStructureWithAllowSubtypesCollection : List<NestedStructureWithAllowSubtypes>, ICloneable
    {
        #region Constructors
        /// <remarks />
        public NestedStructureWithAllowSubtypesCollection() {}

        /// <remarks />
        public NestedStructureWithAllowSubtypesCollection(int capacity) : base(capacity) {}

        /// <remarks />
        public NestedStructureWithAllowSubtypesCollection(IEnumerable<NestedStructureWithAllowSubtypes> collection) : base(collection) {}
        #endregion

        #region Static Operators
        /// <remarks />
        public static implicit operator NestedStructureWithAllowSubtypesCollection(NestedStructureWithAllowSubtypes[] values)
        {
            if (values != null)
            {
                return new NestedStructureWithAllowSubtypesCollection(values);
            }

            return new NestedStructureWithAllowSubtypesCollection();
        }

        /// <remarks />
        public static explicit operator NestedStructureWithAllowSubtypes[](NestedStructureWithAllowSubtypesCollection values)
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
            return (NestedStructureWithAllowSubtypesCollection)this.MemberwiseClone();
        }
        #endregion

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            NestedStructureWithAllowSubtypesCollection clone = new NestedStructureWithAllowSubtypesCollection(this.Count);

            for (int ii = 0; ii < this.Count; ii++)
            {
                clone.Add((NestedStructureWithAllowSubtypes)Utils.Clone(this[ii]));
            }

            return clone;
        }
    }
    #endregion
    #endif
    #endregion

    #region BasicUnion Class
    #if (!OPCUA_EXCLUDE_BasicUnion)
    /// <remarks />
    /// <exclude />
    public enum BasicUnionFields : uint
    {
        /// <remarks />
        None = 0,
        /// <remarks />
        A = 1,
        /// <remarks />
        B = 2,
        /// <remarks />
        C = 3,
        /// <remarks />
        D = 4,
        /// <remarks />
        E = 5
    }

    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [DataContract(Namespace = TestModel.Namespaces.TestModelXsd)]
    public partial class BasicUnion : IEncodeable, IJsonEncodeable
    {
        #region Constructors
        /// <remarks />
        public BasicUnion()
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
            SwitchField = BasicUnionFields.None;
            m_a = (uint)0;
            m_b = new StringCollection();
            m_c = null;
            m_d = new ConcreteStructure();
            m_e = EnumerationWithGaps.Invalid;
        }
        #endregion

        #region Public Properties
        /// <remarks />
        [DataMember(Name = "SwitchField", IsRequired = true, Order = 0)]
        public BasicUnionFields SwitchField { get; set; }

        /// <remarks />
        [DataMember(Name = "A", IsRequired = false, Order = 1)]
        public uint A
        {
            get { return m_a;  }
            set { m_a = value; }
        }

        /// <remarks />
        [DataMember(Name = "B", IsRequired = false, Order = 2)]
        public StringCollection B
        {
            get
            {
                return m_b;
            }

            set
            {
                m_b = value;

                if (value == null)
                {
                    m_b = new StringCollection();
                }
            }
        }

        /// <remarks />
        [DataMember(Name = "C", IsRequired = false, Order = 3)]
        public byte[] C
        {
            get { return m_c;  }
            set { m_c = value; }
        }

        /// <remarks />
        [DataMember(Name = "D", IsRequired = false, Order = 4)]
        public ConcreteStructure D
        {
            get
            {
                return m_d;
            }

            set
            {
                m_d = value;

                if (value == null)
                {
                    m_d = new ConcreteStructure();
                }
            }
        }

        /// <remarks />
        [DataMember(Name = "E", IsRequired = false, Order = 5)]
        public EnumerationWithGaps E
        {
            get { return m_e;  }
            set { m_e = value; }
        }
        #endregion

        #region IEncodeable Members
        /// <summary cref="IEncodeable.TypeId" />
        public virtual ExpandedNodeId TypeId => DataTypeIds.BasicUnion; 

        /// <summary cref="IEncodeable.BinaryEncodingId" />
        public virtual ExpandedNodeId BinaryEncodingId => ObjectIds.BasicUnion_Encoding_DefaultBinary;

        /// <summary cref="IEncodeable.XmlEncodingId" />
        public virtual ExpandedNodeId XmlEncodingId => ObjectIds.BasicUnion_Encoding_DefaultXml;

        /// <summary cref="IJsonEncodeable.JsonEncodingId" />
        public virtual ExpandedNodeId JsonEncodingId => ObjectIds.BasicUnion_Encoding_DefaultJson; 

        /// <summary cref="IEncodeable.Encode(IEncoder)" />
        public virtual void Encode(IEncoder encoder)
        {
            encoder.PushNamespace(TestModel.Namespaces.TestModelXsd);
            encoder.WriteSwitchField((uint)SwitchField, out var fieldName);

            switch (SwitchField)
            {
                default: { break; }
                case BasicUnionFields.A: { encoder.WriteUInt32(fieldName ?? "A", A); break; }
                case BasicUnionFields.B: { encoder.WriteStringArray(fieldName ?? "B", B); break; }
                case BasicUnionFields.C: { encoder.WriteByteString(fieldName ?? "C", C); break; }
                case BasicUnionFields.D: { encoder.WriteEncodeable(fieldName ?? "D", D, typeof(ConcreteStructure)); break; }
                case BasicUnionFields.E: { encoder.WriteEnumerated(fieldName ?? "E", E); break; }
            }
            
            encoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.Decode(IDecoder)" />
        public virtual void Decode(IDecoder decoder)
        {
            decoder.PushNamespace(TestModel.Namespaces.TestModelXsd);

            SwitchField = (BasicUnionFields)decoder.ReadSwitchField(m_FieldNames, out var fieldName);

            switch (SwitchField)
            {
                default: { break; }
                case BasicUnionFields.A: { A = decoder.ReadUInt32(fieldName ?? "A"); break; }
                case BasicUnionFields.B: { B = decoder.ReadStringArray(fieldName ?? "B"); break; }
                case BasicUnionFields.C: { C = decoder.ReadByteString(fieldName ?? "C"); break; }
                case BasicUnionFields.D: { D = (ConcreteStructure)decoder.ReadEncodeable(fieldName ?? "D", typeof(ConcreteStructure)); break; }
                case BasicUnionFields.E: { E = (EnumerationWithGaps)decoder.ReadEnumerated(fieldName ?? "E", typeof(EnumerationWithGaps)); break; }
            }

            decoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.IsEqual(IEncodeable)" />
        public virtual bool IsEqual(IEncodeable encodeable)
        {
            if (Object.ReferenceEquals(this, encodeable))
            {
                return true;
            }

            BasicUnion value = encodeable as BasicUnion;

            if (value == null)
            {
                return false;
            }

            if (value.SwitchField != this.SwitchField) return false;

            switch (SwitchField)
            {
                default: { break; }
                case BasicUnionFields.A: { if (!Utils.IsEqual(m_a, value.m_a)) return false; break; }
                case BasicUnionFields.B: { if (!Utils.IsEqual(m_b, value.m_b)) return false; break; }
                case BasicUnionFields.C: { if (!Utils.IsEqual(m_c, value.m_c)) return false; break; }
                case BasicUnionFields.D: { if (!Utils.IsEqual(m_d, value.m_d)) return false; break; }
                case BasicUnionFields.E: { if (!Utils.IsEqual(m_e, value.m_e)) return false; break; }
            }

            return true;
        }

        /// <summary cref="ICloneable.Clone" />
        public virtual object Clone()
        {
            return (BasicUnion)this.MemberwiseClone();
        }

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            BasicUnion clone = (BasicUnion)base.MemberwiseClone();

            clone.SwitchField = this.SwitchField;

            switch (SwitchField)
            {
                default: { break; }
                case BasicUnionFields.A: { clone.m_a = (uint)Utils.Clone(this.m_a); break; }
                case BasicUnionFields.B: { clone.m_b = (StringCollection)Utils.Clone(this.m_b); break; }
                case BasicUnionFields.C: { clone.m_c = (byte[])Utils.Clone(this.m_c); break; }
                case BasicUnionFields.D: { clone.m_d = (ConcreteStructure)Utils.Clone(this.m_d); break; }
                case BasicUnionFields.E: { clone.m_e = (EnumerationWithGaps)Utils.Clone(this.m_e); break; }
            }

            return clone;
        }
        #endregion

        #region Private Fields
        private uint m_a;
        private StringCollection m_b;
        private byte[] m_c;
        private ConcreteStructure m_d;
        private EnumerationWithGaps m_e;
           
        private static readonly string[] m_FieldNames = Enum.GetNames(typeof(BasicUnionFields)).Where(x => x != nameof(BasicUnionFields.None)).ToArray();
        #endregion
    }

    #region BasicUnionCollection Class
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [CollectionDataContract(Name = "ListOfBasicUnion", Namespace = TestModel.Namespaces.TestModelXsd, ItemName = "BasicUnion")]
    public partial class BasicUnionCollection : List<BasicUnion>, ICloneable
    {
        #region Constructors
        /// <remarks />
        public BasicUnionCollection() {}

        /// <remarks />
        public BasicUnionCollection(int capacity) : base(capacity) {}

        /// <remarks />
        public BasicUnionCollection(IEnumerable<BasicUnion> collection) : base(collection) {}
        #endregion

        #region Static Operators
        /// <remarks />
        public static implicit operator BasicUnionCollection(BasicUnion[] values)
        {
            if (values != null)
            {
                return new BasicUnionCollection(values);
            }

            return new BasicUnionCollection();
        }

        /// <remarks />
        public static explicit operator BasicUnion[](BasicUnionCollection values)
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
            return (BasicUnionCollection)this.MemberwiseClone();
        }
        #endregion

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            BasicUnionCollection clone = new BasicUnionCollection(this.Count);

            for (int ii = 0; ii < this.Count; ii++)
            {
                clone.Add((BasicUnion)Utils.Clone(this[ii]));
            }

            return clone;
        }
    }
    #endregion
    #endif
    #endregion

    #region StructureWithOptionalFields Class
    #if (!OPCUA_EXCLUDE_StructureWithOptionalFields)
    /// <remarks />
    /// <exclude />
    
    public enum StructureWithOptionalFieldsFields : uint
    {   
        /// <remarks />
        None = 0,
        /// <remarks />
        A = 0x1,
        /// <remarks />
        B = 0x2,
        /// <remarks />
        C = 0x4,
        /// <remarks />
        D = 0x8,
        /// <remarks />
        E = 0x10,
    }
        
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [DataContract(Namespace = TestModel.Namespaces.TestModelXsd)]
    public partial class StructureWithOptionalFields : IEncodeable, IJsonEncodeable
    {
        #region Constructors
        /// <remarks />
        public StructureWithOptionalFields()
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
            EncodingMask = (uint)StructureWithOptionalFieldsFields.None;
            m_a = (uint)0;
            m_b = new StringCollection();
            m_c = null;
            m_d = new ConcreteStructure();
            m_e = EnumerationWithGaps.Invalid;
        }
        #endregion

        #region Public Properties
        /// <remarks />
        [DataMember(Name = "EncodingMask", IsRequired = true, Order = 0)]
        public virtual uint EncodingMask { get; set; }

        /// <remarks />
        [DataMember(Name = "A", IsRequired = false, Order = 1)]
        public uint A
        {
            get { return m_a;  }
            set { m_a = value; }
        }

        /// <remarks />
        [DataMember(Name = "B", IsRequired = false, Order = 2)]
        public StringCollection B
        {
            get
            {
                return m_b;
            }

            set
            {
                m_b = value;

                if (value == null)
                {
                    m_b = new StringCollection();
                }
            }
        }

        /// <remarks />
        [DataMember(Name = "C", IsRequired = false, Order = 3)]
        public byte[] C
        {
            get { return m_c;  }
            set { m_c = value; }
        }

        /// <remarks />
        [DataMember(Name = "D", IsRequired = false, Order = 4)]
        public ConcreteStructure D
        {
            get
            {
                return m_d;
            }

            set
            {
                m_d = value;

                if (value == null)
                {
                    m_d = new ConcreteStructure();
                }
            }
        }

        /// <remarks />
        [DataMember(Name = "E", IsRequired = false, Order = 5)]
        public EnumerationWithGaps E
        {
            get { return m_e;  }
            set { m_e = value; }
        }
        #endregion

        #region IEncodeable Members
        /// <summary cref="IEncodeable.TypeId" />
        public virtual ExpandedNodeId TypeId => DataTypeIds.StructureWithOptionalFields; 

        /// <summary cref="IEncodeable.BinaryEncodingId" />
        public virtual ExpandedNodeId BinaryEncodingId => ObjectIds.StructureWithOptionalFields_Encoding_DefaultBinary;

        /// <summary cref="IEncodeable.XmlEncodingId" />
        public virtual ExpandedNodeId XmlEncodingId => ObjectIds.StructureWithOptionalFields_Encoding_DefaultXml;
            
        /// <summary cref="IJsonEncodeable.JsonEncodingId" />
        public virtual ExpandedNodeId JsonEncodingId => ObjectIds.StructureWithOptionalFields_Encoding_DefaultJson; 

        /// <summary cref="IEncodeable.Encode(IEncoder)" />
        public virtual void Encode(IEncoder encoder)
        {
            encoder.PushNamespace(TestModel.Namespaces.TestModelXsd);
            encoder.WriteEncodingMask((uint)EncodingMask);

            if ((EncodingMask & (uint)StructureWithOptionalFieldsFields.A) != 0) encoder.WriteUInt32("A", A);
            if ((EncodingMask & (uint)StructureWithOptionalFieldsFields.B) != 0) encoder.WriteStringArray("B", B);
            if ((EncodingMask & (uint)StructureWithOptionalFieldsFields.C) != 0) encoder.WriteByteString("C", C);
            if ((EncodingMask & (uint)StructureWithOptionalFieldsFields.D) != 0) encoder.WriteEncodeable("D", D, typeof(ConcreteStructure));
            if ((EncodingMask & (uint)StructureWithOptionalFieldsFields.E) != 0) encoder.WriteEnumerated("E", E);

            encoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.Decode(IDecoder)" />
        public virtual void Decode(IDecoder decoder)
        {
            decoder.PushNamespace(TestModel.Namespaces.TestModelXsd);

            EncodingMask = decoder.ReadEncodingMask(m_FieldNames);

            if ((EncodingMask & (uint)StructureWithOptionalFieldsFields.A) != 0) A = decoder.ReadUInt32("A");
            if ((EncodingMask & (uint)StructureWithOptionalFieldsFields.B) != 0) B = decoder.ReadStringArray("B");
            if ((EncodingMask & (uint)StructureWithOptionalFieldsFields.C) != 0) C = decoder.ReadByteString("C");
            if ((EncodingMask & (uint)StructureWithOptionalFieldsFields.D) != 0) D = (ConcreteStructure)decoder.ReadEncodeable("D", typeof(ConcreteStructure));
            if ((EncodingMask & (uint)StructureWithOptionalFieldsFields.E) != 0) E = (EnumerationWithGaps)decoder.ReadEnumerated("E", typeof(EnumerationWithGaps));

            decoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.IsEqual(IEncodeable)" />
        public virtual bool IsEqual(IEncodeable encodeable)
        {
            if (Object.ReferenceEquals(this, encodeable))
            {
                return true;
            }

            StructureWithOptionalFields value = encodeable as StructureWithOptionalFields;

            if (value == null)
            {
                return false;
            }

            if (value.EncodingMask != this.EncodingMask) return false;

            if ((EncodingMask & (uint)StructureWithOptionalFieldsFields.A) != 0) if (!Utils.IsEqual(m_a, value.m_a)) return false;
            if ((EncodingMask & (uint)StructureWithOptionalFieldsFields.B) != 0) if (!Utils.IsEqual(m_b, value.m_b)) return false;
            if ((EncodingMask & (uint)StructureWithOptionalFieldsFields.C) != 0) if (!Utils.IsEqual(m_c, value.m_c)) return false;
            if ((EncodingMask & (uint)StructureWithOptionalFieldsFields.D) != 0) if (!Utils.IsEqual(m_d, value.m_d)) return false;
            if ((EncodingMask & (uint)StructureWithOptionalFieldsFields.E) != 0) if (!Utils.IsEqual(m_e, value.m_e)) return false;

            return true;
        }

        /// <summary cref="ICloneable.Clone" />
        public virtual object Clone()
        {
            return (StructureWithOptionalFields)this.MemberwiseClone();
        }

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            StructureWithOptionalFields clone = (StructureWithOptionalFields)base.MemberwiseClone();

            clone.EncodingMask = this.EncodingMask;

            if ((EncodingMask & (uint)StructureWithOptionalFieldsFields.A) != 0) clone.m_a = (uint)Utils.Clone(this.m_a);
            if ((EncodingMask & (uint)StructureWithOptionalFieldsFields.B) != 0) clone.m_b = (StringCollection)Utils.Clone(this.m_b);
            if ((EncodingMask & (uint)StructureWithOptionalFieldsFields.C) != 0) clone.m_c = (byte[])Utils.Clone(this.m_c);
            if ((EncodingMask & (uint)StructureWithOptionalFieldsFields.D) != 0) clone.m_d = (ConcreteStructure)Utils.Clone(this.m_d);
            if ((EncodingMask & (uint)StructureWithOptionalFieldsFields.E) != 0) clone.m_e = (EnumerationWithGaps)Utils.Clone(this.m_e);

            return clone;
        }
        #endregion

        #region Private Fields
        private uint m_a;
        private StringCollection m_b;
        private byte[] m_c;
        private ConcreteStructure m_d;
        private EnumerationWithGaps m_e;
        
        private static readonly string[] m_FieldNames = Enum.GetNames(typeof(StructureWithOptionalFieldsFields)).Where(x => x != nameof(StructureWithOptionalFieldsFields.None)).ToArray();
        #endregion
    }

    #region StructureWithOptionalFieldsCollection Class
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [CollectionDataContract(Name = "ListOfStructureWithOptionalFields", Namespace = TestModel.Namespaces.TestModelXsd, ItemName = "StructureWithOptionalFields")]
    public partial class StructureWithOptionalFieldsCollection : List<StructureWithOptionalFields>, ICloneable
    {
        #region Constructors
        /// <remarks />
        public StructureWithOptionalFieldsCollection() {}

        /// <remarks />
        public StructureWithOptionalFieldsCollection(int capacity) : base(capacity) {}

        /// <remarks />
        public StructureWithOptionalFieldsCollection(IEnumerable<StructureWithOptionalFields> collection) : base(collection) {}
        #endregion

        #region Static Operators
        /// <remarks />
        public static implicit operator StructureWithOptionalFieldsCollection(StructureWithOptionalFields[] values)
        {
            if (values != null)
            {
                return new StructureWithOptionalFieldsCollection(values);
            }

            return new StructureWithOptionalFieldsCollection();
        }

        /// <remarks />
        public static explicit operator StructureWithOptionalFields[](StructureWithOptionalFieldsCollection values)
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
            return (StructureWithOptionalFieldsCollection)this.MemberwiseClone();
        }
        #endregion

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            StructureWithOptionalFieldsCollection clone = new StructureWithOptionalFieldsCollection(this.Count);

            for (int ii = 0; ii < this.Count; ii++)
            {
                clone.Add((StructureWithOptionalFields)Utils.Clone(this[ii]));
            }

            return clone;
        }
    }
    #endregion
    #endif
    #endregion
}