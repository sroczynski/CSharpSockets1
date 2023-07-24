using System.Net;
using System.Net.Sockets;
using System.Text;

string serverIp = "127.0.0.1";
int port = 8787;

try
{
    // Create a socket
    Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

    // Connect to the server
    client.Connect(new IPEndPoint(IPAddress.Parse(serverIp), port));
    Console.WriteLine("Connected to server.");

    // Start a continuous chat loop
    while (true)
    {
        // Prompt for a client message
        Console.Write("Client: ");
        string clientMessage = Console.ReadLine();

        // Send the client message to the server
        client.Send(Encoding.ASCII.GetBytes(clientMessage));
        Console.WriteLine("Sent message: " + clientMessage);

        // Receive the response from the server
        byte[] buffer = new byte[1024];
        int bytesRead = client.Receive(buffer);
        string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);
        Console.WriteLine("Server: " + response);

        // Check if the server wants to end the chat
        if (response.Equals("Goodbye!", StringComparison.OrdinalIgnoreCase))
        {
            break;
        }
    }

    // Close the connection
    client.Close();
}
catch (Exception ex)
{
    Console.WriteLine("Error: " + ex.Message);
}

Console.WriteLine("Press any key to exit.");
Console.ReadKey();