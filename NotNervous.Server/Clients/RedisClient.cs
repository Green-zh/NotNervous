using NotNervous.Server.Models;

namespace NotNervous.Server.Clients
{
    public class RedisClient
    {
        private InterviewModel mockInterview = new()
        {
            SessionID = Guid.NewGuid(),
            JobDescription = new byte[0],
            Resume = new byte[0],
            Topics = new List<byte[]> { new byte[0] },
            Messages = new List<MessageModel>()
        };

        public RedisClient()
        {
            // Initialize Redis connection
        }

        public InterviewModel GetInterviewData(string sessionId)
        {
            // Retrieve dialog data from Redis
            // For now, return a mock dialog model
            return mockInterview;
        }

        public void SaveInterviewData(InterviewModel interview)
        {
            // Save dialog data to Redis
            // For now, just print the session ID
            mockInterview = interview;
            Console.WriteLine($"Saving interview data for session: {interview.SessionID}");
        }
    }
}
