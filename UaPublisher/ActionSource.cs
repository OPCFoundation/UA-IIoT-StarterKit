using Opc.Ua;

namespace UaPublisher
{
    internal class ActionSource
    {
        protected readonly object m_lock = new();
        protected List<ActionTarget> m_targets = new ();
        protected DataSetMetaDataType m_request;
        protected DataSetMetaDataType m_response;

        public ActionSource()
        {
        }

        public string Name { get; protected set; }
        public string Description { get; protected set; }

        public DataSetMetaDataType Request => m_request;
        public DataSetMetaDataType Response => m_response;
        public List<ActionTarget> Targets => m_targets;

        public ConfigurationVersionDataType MetaDataVersion => m_request?.ConfigurationVersion ?? new ConfigurationVersionDataType();

        public virtual DataSetMetaDataType BuildRequestMetaData()
        {
            return null;
        }

        public virtual DataSetMetaDataType BuildResponseMetaData()
        {
            return null;
        }

        public ActionTargetDataTypeCollection GetTargets()
        {
            ActionTargetDataTypeCollection targets = new();

            foreach (var target in m_targets)
            {
                targets.Add(new ActionTargetDataType()
                { 
                    ActionTargetId = target.Id,
                    Name = target.Name,
                    Description = target.Description
                });
            }

            return targets;
        }

        public ActionMethodDataTypeCollection GetMethods()
        {
            ActionMethodDataTypeCollection methods = new();

            foreach (var target in m_targets)
            {
                methods.Add(new ActionMethodDataType()
                {
                    ObjectId = target.ObjectId,
                    MethodId = target.MethodId
                });
            }

            return methods;
        }
    }

    public class ActionTarget
    {
        public ushort Id;
        public string Name;
        public string Description;
        public NodeId ObjectId;
        public NodeId MethodId;
        public Func<ActionTarget, List<Variant>, List<Variant>> Callback;
    }
}
