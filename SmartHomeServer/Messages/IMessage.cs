using SmartHomeServer.Enums;

namespace SmartHomeServer.Messages
{
    public interface IMessage
    {
        MessageSource Source { get; }
    }
}
