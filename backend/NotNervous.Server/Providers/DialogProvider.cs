using NotNervous.Server.Clients;
using NotNervous.Server.Enums;
using NotNervous.Server.Handlers;
using NotNervous.Server.Models;

namespace NotNervous.Server.Providers
{
    public class DialogProvider
    {
        private SpeechClient speechClient;
        private RedisClient redisClient;
        private AIFoundryClient aiFoundryClient;

        public DialogProvider(SpeechClient speechClient, RedisClient redisClient, AIFoundryClient aiFoundryClient)
        {
            this.speechClient = speechClient;
            this.redisClient = redisClient;
            this.aiFoundryClient = aiFoundryClient;
        }

        public async Task Process(HttpContext context)
        {
            // 1. Get WebSocket connection
            using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            var wsHandler = new WebSocketHandler(webSocket);

            // 2. Retrieve candidate data from Redis
            var interview = redisClient.GetInterviewData(context.Request.Headers["X-Session-ID"]);

            // 3. Send prologue message to the client
            var prologue = aiFoundryClient.RaisePrologue();
            await SendQuestion(prologue, wsHandler);

            // 4. Loop topics
            foreach (var topic in interview.Topics)
            {
                var currentDialog = new List<MessageModel>();

                // One initial question and two follow-up questions
                for (var i = 0; i < 3; i++)
                {
                    // 4.1 Prepare current question and add to dialog
                    var question = aiFoundryClient.RaiseQuestion(topic, currentDialog);
                    currentDialog.Add(new (RoleEnum.Interviewer, question));

                    // 4.2. Send current question to the client
                    await SendQuestion(prologue, wsHandler);

                    // 4.3. Wait for the client to respond with an answer
                    var answer = await ReceiveAnswer(wsHandler);

                    // 4.4 Add the answer to the current dialog
                    currentDialog.Add(new (RoleEnum.Candidate, answer));
                }

                interview.Messages = [.. currentDialog ];
            }

            // 5. Save the dialog to Redis
            redisClient.SaveInterviewData(interview);

            // 6. Send final message to the client
            var ending = aiFoundryClient.RaiseEnding();
            await SendQuestion(ending, wsHandler);

            // 6. Close the WebSocket connection
            await wsHandler.CloseWebSocket();
        }

        private async Task SendQuestion(byte[] text, WebSocketHandler wsHandler)
        {
            using var audioStream = new MemoryStream();
            var synthesizeTask = speechClient.SynthesizeToStreamAsync(text, wsHandler.SendDataChunk);

            //var synthesizeTask = speechClient.SynthesizeToStreamAsync(text, audioStream);
            //await wsHandler.SendDataFromStreamAsync(audioStream, synthesizeTask);
        }

        private async Task<byte[]> ReceiveAnswer(WebSocketHandler wsHandler)
        {
            //using var pushStream = new MemoryStream();
            using var pullStream = new MemoryStream();

            var handleChunk = await speechClient.StartRecognizeToStreamAsync(pullStream);

            await wsHandler.ReceiveDataAndHandleChunkAsync(handleChunk);

            await speechClient.StopRecognizeAsync();

            //var receiveDataTask = wsHandler.ReceiveDataToStreamAsync(pushStream);
            //await speechClient.RecognizeToStreamAsync(pushStream, pullStream, receiveDataTask);

            return pullStream.ToArray();
        }
    }
}
