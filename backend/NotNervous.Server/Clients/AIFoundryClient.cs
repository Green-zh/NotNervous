using NotNervous.Server.Models;
using NotNervous.Server.Utils;

namespace NotNervous.Server.Clients
{
    public class AIFoundryClient
    {
        public AIFoundryClient()
        {
        }

        public byte[] RaiseQuestion(byte[] topic, IEnumerable<MessageModel> context)
        {
            var question = PromptUtil.RaiseQuestionWithContext(topic, context);

            return question;
        }

        public byte[] RaisePrologue()
        {
            return new byte[2];
        }

        public byte[] RaiseEnding()
        {
            return new byte[2];
        }
    }
}
