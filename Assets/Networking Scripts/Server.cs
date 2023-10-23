using System;
using System.Net.Sockets;
using System.Net;
using UnityEngine;
using NetworkingLibrary;
using System.Collections.Generic;

namespace Server
{
    public  class Server : MonoBehaviour
    {
        [SerializeField] float tickRate;
        protected bool isCalled;
        protected List<Socket> clients = new List<Socket>();
        protected Socket queueSocket;


        protected virtual void Start()
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

                Debug.Log("Client is connected to server, Sending data to client");
                SendPacket(i);
            }
        }


        protected virtual void SendPacket(int _clientIndex)
        {
            byte[] serializedPosition = new PositionPacket(new Vector3(1, 2, 3)).SerializePosition();
            clients[_clientIndex].Send(serializedPosition);
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
