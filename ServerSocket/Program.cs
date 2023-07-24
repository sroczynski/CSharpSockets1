
using System.Net;
using System.Net.Sockets;
using System.Text;

string ipAddress = "127.0.0.1";
int port = 8787;

// Create a socket
Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

try
{
    // Bind the socket to the IP address and port
    listener.Bind(new IPEndPoint(IPAddress.Parse(ipAddress), port));

    // Start listening for client requests
    listener.Listen(10);
    Console.WriteLine("Server started. Waiting for clients...");

    // Accept the client connection
    Socket client = listener.Accept();
    Console.WriteLine("Client connected.");

    // Start a continuous chat loop
    while (true)
    {
        // Receive the message from the client
        byte[] buffer = new byte[1024];
        int bytesRead = client.Receive(buffer);
        string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
        Console.WriteLine($"Client: {message}");

        // Check if the client wants to end the chat
        if (message.Equals("bye", StringComparison.OrdinalIgnoreCase))
        {
            // Send a goodbye message and close the connection
            string response = "Goodbye!";
            client.Send(Encoding.ASCII.GetBytes(response));
            Console.WriteLine("Sent response: " + response);
            break;
        }

        // Prompt for a server message
        Console.Write("Server: ");
        string serverMessage = Console.ReadLine();

        // Send the server message to the client
        client.Send(Encoding.ASCII.GetBytes(serverMessage));
        Console.WriteLine($"Sent message: {serverMessage}");
    }

    // Close the connection
    client.Close();
}
catch (Exception ex)
{
    Console.WriteLine("Error: " + ex.Message);
}
finally
{
    // Stop listening for client requests and close the socket
    listener.Close();
}

Console.WriteLine("Server stopped. Press any key to exit.");
Console.ReadKey();