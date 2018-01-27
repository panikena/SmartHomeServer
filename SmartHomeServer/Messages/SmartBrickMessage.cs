using System.IO;

namespace SmartHomeServer.Messages
{
    public class SmartBrickMessage : IMessage
    {
        public MessageSource Source { get { return MessageSource.UnixSocket; } }
        public short SmartBrickID { get; set; }
        public short CommandCode { get; set; }
        public byte[] Payload { get; set; }

        public byte[] Serialize()
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write(SmartBrickID);
                    writer.Write(CommandCode);
                    writer.Write(Payload);
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
                    obj.SmartBrickID = reader.ReadInt16();
                    obj.CommandCode = reader.ReadInt16();
                    obj.Payload = new byte[32];
                    reader.Read(obj.Payload, 0, 32);
                }
            }

            return obj;
        }

    }
}
