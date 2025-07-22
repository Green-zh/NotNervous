using System.Text;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.CognitiveServices.Speech;
using NotNervous.Server.Handlers;
using System.Threading.Channels;

namespace NotNervous.Server.Clients
{
    public class SpeechClient
    {
        private readonly string subscriptionKey = "6F87vnLs1WuHJUnl9g48eiLVYm3QLbGjic11hFtFVB3Nd7Tc8EevJQQJ99BCACqBBLyXJ3w3AAAYACOGmAWd";
        private readonly string region = "southeastasia";

        private SpeechRecognizer recognizer;
        private TaskCompletionSource<bool> recognizeTask;

        //public async Task RecognizeToStreamAsync(Stream inputStream, Stream outputStream, Task inputStreamTask)
        public async Task<Action<byte[]>> StartRecognizeToStreamAsync(Stream outputStream)
        {
            // 2. Configure Cognitive Services
            var speechConfig = SpeechConfig.FromSubscription(subscriptionKey, region);
            speechConfig.SpeechRecognitionLanguage = "en-US";

            // 3. Push audio protocol
            var audioFormat = AudioStreamFormat.GetWaveFormatPCM(16000, 16, 1);
            using var pushStream = AudioInputStream.CreatePushStream(audioFormat);
            using var audioConfig = AudioConfig.FromStreamInput(pushStream);
            recognizer = new SpeechRecognizer(speechConfig, audioConfig);

            recognizeTask = new ();

            // 4. Start continuous recognition
            recognizer.Recognizing += (s, e) =>
            {
                if (!string.IsNullOrEmpty(e.Result.Text))
                {
                    outputStream.Write(Encoding.UTF8.GetBytes(e.Result.Text));
                    Console.WriteLine($"RECOGNIZED: {e.Result.Text}");
                }
            };
            recognizer.Recognized += (s, e) =>
            {
                if (!string.IsNullOrEmpty(e.Result.Text))
                {
                    outputStream.Write(Encoding.UTF8.GetBytes(e.Result.Text));
                    Console.WriteLine($"RECOGNIZED: {e.Result.Text}");
                }
                
                Console.WriteLine($"RECOGNIZE FINISHED.");
                recognizeTask.TrySetResult(true);
            };

            //var startTask = recognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);
            await recognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);

            // 5. Send PCM data to Azure
            //byte[] opusChunk = new byte[4096];
            // Example: 16-bit, 16kHz, mono Opus
            var opusDecoder = OpusHandler.CreateInstance();
            //int bytesRead = 0;
            //while (((bytesRead = inputStream.Read(opusChunk)) > 0) || !inputStreamTask.IsCompleted)
            //{
            //    if (bytesRead == 0)
            //    {
            //        continue;
            //    }
            return (opusChunk) =>
            {
                // Opus -> PCM
                //var pcmChunk = opusDecoder.DecodeOpusToPCM(opusChunk);

                pushStream.Write(opusChunk);
                //pushStream.Write(pcmChunk);
            };
            //    await Task.Delay(1000);
            //}

            //await recognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);

            //await recognizeTask.Task;
        }

        public async Task StopRecognizeAsync()
        {
            await recognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);

            await recognizeTask.Task;
        }

        //public async Task SynthesizeToStreamAsync(byte[] text, Stream outputStream)
        public async Task SynthesizeToStreamAsync(byte[] text, Func<byte[], bool, Task> handleOuputChunk)
        {
            // Convert byte array to string
            string textString = Encoding.UTF8.GetString(text);

            var config = SpeechConfig.FromSubscription(subscriptionKey, region);
            config.SpeechSynthesisLanguage = "en-US";
            // Example: 16-bit, 16kHz, mono Opus
            //config.SetSpeechSynthesisOutputFormat(SpeechSynthesisOutputFormat.Ogg16Khz16BitMonoOpus);
            config.SetSpeechSynthesisOutputFormat(SpeechSynthesisOutputFormat.Riff16Khz16BitMonoPcm);

            // Pull stream for event-based synthesis
            using var pullStream = AudioOutputStream.CreatePullStream();
            using var audioConfig = AudioConfig.FromStreamOutput(pullStream);
            using var synthesizer = new SpeechSynthesizer(config, audioConfig);
            int cnt = 0;

            // Handle partial chunks as they're generated
            synthesizer.Synthesizing += (sender, e) =>
            {
                if (e.Result.AudioData != null && e.Result.AudioData.Length > 0)
                {
                    //outputStream.Write(e.Result.AudioData);
                    handleOuputChunk(e.Result.AudioData, false).Wait();
                    Console.WriteLine(e.Result.AudioData);
                    cnt++;
                }
            };
            synthesizer.SynthesisCompleted += (sender, e) =>
            {
                handleOuputChunk(e.Result.AudioData, true).Wait();
                Console.WriteLine($"SYNTHESIZE FINISHED.");
                cnt++;
            };

            // Begin speaking text
            //var synthesizeTask = synthesizer.SpeakTextAsync(textString);
            await synthesizer.SpeakTextAsync(textString);
            //await synthesizer.StopSpeakingAsync();
            //synthesizeTask.Wait();
            //byte[] buffer = new byte[4096];
            //uint bytesRead = 0;
            //while ((bytesRead = pullStream.Read(buffer)) > 0 || !synthesizeTask.IsCompleted)
            //{
            //    if (bytesRead == 0)
            //    {
            //        continue;
            //    }
            //    await outputStream.WriteAsync(buffer);

            //    await Task.Delay(1000);
            //}
        }
    }
}
