using System;
using System.Net.WebSockets;

namespace NotNervous.Server.Handlers
{
    public class WebSocketHandler
    {
        private WebSocket webSocket;

        public WebSocketHandler(WebSocket webSocket)
        {
            this.webSocket = webSocket;
        }

        public async Task ReceiveDataAndHandleChunkAsync(Action<byte[]> handleChunk)
        {
            var buffer = new byte[4096];

            while (true)
            {
                var result = await this.webSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer),
                    CancellationToken.None
                );

                handleChunk(buffer);

                if (result.EndOfMessage)
                {
                    break;
                }

                await Task.Delay(1000);
            }
        }

        //public async Task SendDataFromStreamAsync(Stream inputStream, Task inputStreamTask)
        //{
        //    var buffer = new byte[4096];

        //    while (!inputStreamTask.IsCompleted)
        //    {
        //        await inputStream.ReadAsync(buffer);

        //        await SendData(buffer, false);
        //    }
            
        //    await inputStream.ReadAsync(buffer);
            
        //    await SendData(buffer, true);
        //}

        public async Task CloseWebSocket() =>
            await webSocket.CloseAsync(
                WebSocketCloseStatus.NormalClosure,
                "Closing",
                CancellationToken.None
            );

        public async Task SendDataChunk(byte[] chunk, bool endOfMessage) =>
            await webSocket.SendAsync(
                new ArraySegment<byte>(chunk),
                WebSocketMessageType.Binary,
                endOfMessage,
                CancellationToken.None
                );
    }
}
