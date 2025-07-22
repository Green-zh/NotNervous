using NotNervous.Server.Clients;

namespace NotNervous.Test
{
    [TestClass]
    public sealed class SpeechClientTest
    {
        private readonly SpeechClient speechClient = new ();


        [TestMethod]
        public async Task TextToSpeechToTextTest()
        {
            var text = "Hello, this is a test text.";
            var textBytes = System.Text.Encoding.UTF8.GetBytes(text);

            var audioStream = new MemoryStream();
            var recognizeStream = new MemoryStream();

            await speechClient.SynthesizeToStreamAsync(textBytes,
                async (a, b) =>
                {
                    await audioStream.WriteAsync(a);
                });
            //var textToSpeechTask = speechClient.SynthesizeToStreamAsync(textBytes, audioStream);
            //textToSpeechTask.Wait();
            audioStream.Position = 0; // Reset the stream position to the beginning
            Console.WriteLine($"Audio stream length: {audioStream.Length}");

            var handleChunk = await speechClient.StartRecognizeToStreamAsync(recognizeStream);
            //await speechClient.RecognizeToStreamAsync(audioStream, recognizeStream, textToSpeechTask);

            byte[] buffer = new byte[4096];
            int bytesRead;
            while ((bytesRead = audioStream.Read(buffer)) > 0)
            {
                handleChunk(buffer);
            }

            await speechClient.StopRecognizeAsync();

            var recognizedText = System.Text.Encoding.UTF8.GetString(recognizeStream.ToArray());
        }
    }
}
