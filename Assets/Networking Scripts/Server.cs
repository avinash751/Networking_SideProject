using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using UnityEngine;
using NetworkingLibrary;
using System.Text;
using System.Collections.Generic;

namespace Server
{
    internal class Server : MonoBehaviour
    {
        [SerializeField] float tickRate;
        [SerializeField] bool isCalled;

        List<Socket> clients = new List<Socket>();
        Socket queueSocket;


        private void Start()
        {
            isCalled = false;
            queueSocket = new Socket(
            AddressFamily.InterNetwork,
            SocketType.Stream,
            ProtocolType.Tcp);

            queueSocket.Blocking = false;
            queueSocket.Bind(new IPEndPoint(IPAddress.Any, 3000));
            queueSocket.Listen(10);

        }

        private void Update()
        {
            StartTheProcessOfServerToConnectAndSendData();
        }


        void StartTheProcessOfServerToConnectAndSendData()
        {
            if (isCalled) { return; }
            isCalled = true;

            TryToAcceptClient(queueSocket);
            SendDataToClient();

            Invoke(nameof(CallAgain), tickRate);
        }


        private void SendDataToClient()
        {
            for (int i = 0; i < clients.Count; i++)
            {

                Debug.Log("Client is conected to server, Sending data to client");

                //byte[] buffer = Encoding.ASCII.GetBytes("Hello from server");
                //clients[i].Send(buffer);
                byte[] serializedPosition = new PositionPacket(new Vector3(1, 2, 3)).SerializePosition();
                clients[i].Send(serializedPosition);    
            }
        }

        private void TryToAcceptClient(Socket queueSocket)
        {
            try
            {
                clients.Add(queueSocket.Accept());
            }
            catch (SocketException e)
            {
                if (e.SocketErrorCode != SocketError.WouldBlock)
                {
                    Console.WriteLine(e);
                }
            }
        }
        void CallAgain()
        {
            isCalled = false;
        }
    }
}
