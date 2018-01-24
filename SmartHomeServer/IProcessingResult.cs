namespace SmartHomeServer
{
    public interface IProcessingResult
    {
        ISmartBrickCommand SmartBrickCommand { get; set; }
        string SocketSessionID { get; set; }
        string WebMessage { get; set; }
    }

    public class ProcessingResult : IProcessingResult
    {
        public ISmartBrickCommand SmartBrickCommand { get; set; }
        public string SocketSessionID { get; set; }
        public string WebMessage { get; set; }

        public ProcessingResult(ISmartBrickCommand brickCommand, string sessionId, string message)
        {
            SmartBrickCommand = brickCommand;
            WebMessage = message;
            SocketSessionID = sessionId;
        }
    }
}
