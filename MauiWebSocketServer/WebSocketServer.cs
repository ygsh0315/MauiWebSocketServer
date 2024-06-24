using System;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class WebSocketServer
{
    private HttpListener _listener;

    public event Action<string>? OnMessageReceived;

    public WebSocketServer(string uriPrefix)
    {
        _listener = new HttpListener();
        _listener.Prefixes.Add(uriPrefix);
    }

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        _listener.Start();
        Console.WriteLine("WebSocket 서버가 시작되었습니다.");

        while (!cancellationToken.IsCancellationRequested)
        {
            HttpListenerContext context = await _listener.GetContextAsync();

            if (context.Request.IsWebSocketRequest)
            {
                HttpListenerWebSocketContext webSocketContext = await context.AcceptWebSocketAsync(null);
                WebSocket webSocket = webSocketContext.WebSocket;

                _ = HandleWebSocketCommunication(webSocket, cancellationToken);
            }
            else
            {
                context.Response.StatusCode = 400;
                context.Response.Close();
            }
        }
    }

    private async Task HandleWebSocketCommunication(WebSocket webSocket, CancellationToken cancellationToken)
    {
        byte[] buffer = new byte[1024];

        while (webSocket.State == WebSocketState.Open && !cancellationToken.IsCancellationRequested)
        {
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);
            string message = Encoding.UTF8.GetString(buffer, 0, result.Count);

            OnMessageReceived?.Invoke(message);

            if (result.MessageType == WebSocketMessageType.Close)
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", cancellationToken);
            }
        }
    }

    public void Stop()
    {
        _listener.Stop();
    }
}
