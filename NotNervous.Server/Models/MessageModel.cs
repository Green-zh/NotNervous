using NotNervous.Server.Enums;

namespace NotNervous.Server.Models
{
    public class MessageModel
    {
        public RoleEnum Role { get; set; }

        public byte[] Content { get; set; }

        public MessageModel(RoleEnum role, byte[] content)
        {
            Role = role;
            Content = content;
        }

        public static MessageModel CreateFromStream(RoleEnum role, MemoryStream stream) =>
            new(role, stream.ToArray());
    }
}
