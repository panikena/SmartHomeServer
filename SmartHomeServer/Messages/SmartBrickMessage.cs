using System.IO;

namespace SmartHomeServer.Messages
{
    public class SmartBrickMessage : IMessage
    {
        public MessageSource Source { get { return MessageSource.UnixSocket; } }
        public byte SmartBrickID { get; set; }
        public byte CommandCode { get; set; }
        public byte[] Payload { get; set; }

        public byte[] Serialize()
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write(SmartBrickID);
                    writer.Write(CommandCode);
                    if (Payload != null && Payload.Length > 0)
                    {
                        writer.Write(Payload);
                    }
                }
                return m.ToArray();
            }
        }

        public static SmartBrickMessage Deserialize(byte []array)
        {
            var obj = new SmartBrickMessage();

            using (MemoryStream m = new MemoryStream(array))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    obj.SmartBrickID = reader.ReadByte();
                    obj.CommandCode = reader.ReadByte();
                    obj.Payload = new byte[30];
                    reader.Read(obj.Payload, 0, 30);
                }
            }

            return obj;
        }

    }
}
