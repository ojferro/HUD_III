using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using TMPro;

public class NetworkManager : MonoBehaviour
{

    public string serverAddress = "2.tcp.ngrok.io";
    public int serverPort = 10845;

    private TcpClient client;
    // private TcpListener listener;
    private NetworkStream stream;
    private byte[] receiveBuffer = new byte[1024];


    private static Mutex watsonOutputMutex = new Mutex();

    string watsonOutput = "...";
    public TextMeshPro watsonOutputAsset;
    string watsonOutputPrev;
    float watsonOutputClearTime = 0.0f;
    float watsonOutputTimeBetweenClears = 5.0f;

    void Start()
    {
        ConnectToServer();
    }

    void Update()
    {
        // Wait up to 100 ms to acquire mutex, else continue
        if (watsonOutputMutex.WaitOne(100))
        {
            // Here, the mutex is acquired
            if (!String.IsNullOrEmpty(watsonOutput))
            {
                if (watsonOutput.Equals(watsonOutputPrev))
                {
                    if (Time.time > watsonOutputClearTime)
                    {
                        watsonOutput = "";
                        watsonOutputAsset.text = watsonOutput;
                    }
                }
                else // New message received
                {
                    watsonOutputAsset.text = watsonOutput;
                    watsonOutputPrev = watsonOutput;
                    watsonOutputClearTime = Time.time + watsonOutputTimeBetweenClears;
                }
            }

            // Release mutex
            watsonOutputMutex.ReleaseMutex();
        }

        Debug.Log("Current time" + Time.time + "Watson clear time: " + watsonOutputClearTime);
    }

    private void ConnectToServer()
    {
        // client = new TcpClient("127.0.0.1", 8888); // Connect to Python server on localhost, port 8888
        client = new TcpClient(serverAddress, serverPort); // Connect to Python server on localhost, port 8888
        stream = client.GetStream();

        // Start listening for messages from the server in a background thread
        Thread receiveThread = new Thread(ReceiveThread);
        receiveThread.Start();
    }

    private void ReceiveThread()
    {
        while (true)
        {
            int bytesRead = stream.Read(receiveBuffer, 0, receiveBuffer.Length);
            // int bytesRead = await stream.ReadAsync(receiveBuffer);
            string receivedMessage = Encoding.UTF8.GetString(receiveBuffer, 0, bytesRead);

            if (String.IsNullOrEmpty(receivedMessage))
            {
                continue;
            }

            // Wait up to 100 ms to acquire mutex, else continue
            if (watsonOutputMutex.WaitOne(100))
            {
                // Here, the mutex is held
                watsonOutput = receivedMessage;

                // Release mutex
                watsonOutputMutex.ReleaseMutex();
            }
        }
    }

    private void OnDestroy()
    {
        if (client != null)
        {
            client.Close();
        }
    }
}
