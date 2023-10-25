using System;
using System.Net.Sockets;
using System.Net;
using UnityEngine;
using NetworkingLibrary;
using System.Collections.Generic;

namespace Server
{
    public class Server : MonoBehaviour
    {
        [SerializeField] float tickRate;
        protected bool isCalled;
        protected List<Socket> clients = new List<Socket>();
        protected Socket queueSocket;

        public static Func<int> ClientIDGenerated;
        public static Action<int, Socket> ClientAdded;


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

            TryToAcceptClient(queueSocket, out int _clientId);
            SendDataToClient(_clientId);

            Invoke(nameof(CallAgain), tickRate);
        }

        private void TryToAcceptClient(Socket _queueSocket, out int _generatedID)
        {
            _generatedID = 0;
            try
            {
                clients.Add(_queueSocket.Accept());

                var _clientID = ClientIDGenerated();
                ClientAdded(_clientID,_queueSocket);
                _generatedID = _clientID;
            }
            catch (SocketException e)
            {
                if (e.SocketErrorCode != SocketError.WouldBlock)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private void SendDataToClient(int _clientID)
        {
            for (int i = 0; i < clients.Count; i++)
            {
                SendClientIDPacket(_clientID, i);
                Debug.Log("Client is connected to server, Sending data to client");
                SendPacket(i);
            }
        }

        void SendClientIDPacket(int _clientID, int _clientIndex)
        {
            if(_clientID == 0) { return; }
            byte[] serializedID = new IDPacket(_clientID).SerializeID();
            clients[_clientIndex].Send(serializedID);

        }

        protected virtual void SendPacket(int _clientIndex)
        {
            byte[] serializedPosition = new PositionPacket(new Vector3(1, 2, 3)).SerializePosition();
            clients[_clientIndex].Send(serializedPosition);
        }


        void CallAgain()
        {
            isCalled = false;
        }
    }
}
