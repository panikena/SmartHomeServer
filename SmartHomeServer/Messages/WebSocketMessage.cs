﻿namespace SmartHomeServer.Messages
{
    public class WebSocketMessage : IMessage
    {
        public MessageSource Source { get { return MessageSource.WebSocket; } }
        public string SocketSessionID { get; set; }
        public string Message { get; set; }
        public int WidgetID { get; set; }
    }
}
