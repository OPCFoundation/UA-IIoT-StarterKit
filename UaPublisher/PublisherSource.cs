using Opc.Ua;
using System.Text;
using System.Globalization;

namespace UaPublisher
{
    internal class PublisherSource
    {
        protected readonly object m_lock = new();
        protected Dictionary<string, FieldMetaData> m_fields = new();
        protected DateTime m_lastScanTime = DateTime.MinValue;
        protected DataSetMetaDataType m_metadata;

        public PublisherSource()
        {
        }

        public int Id { get; protected set; }
        public string Name { get; protected set; }

        public DataSetMetaDataType MetaData => m_metadata;
        public ConfigurationVersionDataType MetaDataVersion => m_metadata?.ConfigurationVersion ?? new ConfigurationVersionDataType();

        private static readonly DateTime kBaseDateTime = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        protected static uint GetVersionTime()
        {
            var ticks = (DateTime.UtcNow - kBaseDateTime).TotalMilliseconds;
            return (uint)ticks;
        }

        protected string GetPath(params string[] paths)
        {
            if (paths == null || paths.Length == 0)
            {
                return String.Empty;
            }

            StringBuilder sb = new();

            foreach (var path in paths)
            {
                if (sb.Length > 0) sb.Append("/");
                sb.Append(String.Format(CultureInfo.InvariantCulture, path, Id));
            }

            return sb.ToString();
        }

        protected DataValue GetDataValue(IDictionary<string, Variant> cache, string path, Variant newValue, DateTime timestamp, bool isKeyFrame = false)
        {
            if (!isKeyFrame && cache.TryGetValue(path, out var lastValue))
            {
                if (newValue.Equals(lastValue))
                {
                    return null;
                }
            }

            cache[path] = newValue;

            return new DataValue()
            {
                WrappedValue = newValue,
                SourceTimestamp = timestamp,
                ServerTimestamp = DateTime.UtcNow
            };
        }

        protected bool ReadFieldValue(
            List<KeyValuePair<FieldMetaData, DataValue>> fields,
            IDictionary<string, Variant> cache,
            string path,
            Variant newValue,
            DateTime timestamp,
            bool isKeyFrame = false)
        {
            var dv = GetDataValue(
                cache,
                path,
                newValue,
                timestamp,
                isKeyFrame);

            if (dv != null)
            {
                lock (m_lock)
                {
                    if (m_fields.TryGetValue(path, out var field))
                    {
                        fields.Add(new(field, dv));
                        return true;
                    }
                }
            }

            return false;
        }

        public virtual List<KeyValuePair<FieldMetaData, DataValue>> ReadChangedFields(
            IDictionary<string, Variant> cache,
            bool isKeyFrame)
        {
            return new List<KeyValuePair<FieldMetaData, DataValue>>();
        }

        public virtual DataSetMetaDataType BuildMetaData()
        {
            return null;
        }

        public virtual Task Update()
        {
            return Task.CompletedTask;
        }
    }
}
