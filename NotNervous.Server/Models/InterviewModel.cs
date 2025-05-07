namespace NotNervous.Server.Models
{
    public class InterviewModel
    {
        public Guid SessionID { get; set; }

        public byte[] JobDescription{ get; set; }

        public byte[] Resume { get; set; }

        public IList<byte[]> Topics { get; set; } = [];

        public IList<MessageModel> Messages { get; set; } = [];

        public InterviewModel()
        {
            SessionID = Guid.NewGuid();
        }

        public static InterviewModel Create() => new();
    }
}
