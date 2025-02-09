namespace UaPubSubCommon
{
    public static class MessageTypes
    {
        public const string DataSetMessage = "ua-data";
        public const string DataSetMetaData = "ua-metadata";
        public const string ApplicationDescription = "ua-application";
        public const string ServerEndpoints = "ua-endpoints";
        public const string Status = "ua-status";
        public const string PubSubConnection = "ua-connection";
        public const string ActionRequest = "ua-action-request";
        public const string ActionResponse = "ua-action-response";
        public const string ActionMetaData = "ua-action-metadata";
        public const string ActionResponder = "ua-action-responder";
        public const string KeyFrame = "ua-keyframe";
        public const string DeltaFrame = "ua-deltaframe";
        public const string Event = "ua-event";
        public const string KeepAlive = "ua-keepalive";
    }
}
