using Concentus;

namespace NotNervous.Server.Handlers
{
    public class OpusHandler
    {
        private readonly IOpusDecoder decoder;
        private readonly int sampleRate;
        private readonly int channels;

        public static OpusHandler CreateInstance(int sampleRate = 16000, int channels = 1) =>
            new OpusHandler(sampleRate, channels);

        public OpusHandler(int sampleRate, int channels)
        {
            decoder = OpusCodecFactory.CreateDecoder(sampleRate, channels);
            this.sampleRate = sampleRate;
            this.channels = channels;
        }

        public byte[] DecodeOpusToPCM(byte[] opusData)
        {
            using var inputStream = new MemoryStream(opusData);
            using var outputStream = new MemoryStream();
            var packet = new byte[1276]; // typical max Opus packet size
            var pcmBuffer = new short[sampleRate / 50 * channels]; // 960 samples per frame at 48kHz, adjust if needed

            int bytesRead;
            while ((bytesRead = inputStream.Read(packet, 0, packet.Length)) > 0)
            {
                int samplesDecoded = decoder.Decode(packet, pcmBuffer, pcmBuffer.Length / channels, false);
                for (int i = 0; i < samplesDecoded * channels; i++)
                {
                    outputStream.WriteByte((byte)(pcmBuffer[i] & 0xFF));
                    outputStream.WriteByte((byte)(pcmBuffer[i] >> 8 & 0xFF));
                }
            }

            return outputStream.ToArray();
        }
    }
}
