using System;
using System.Net.Sockets;
using System.Net;
using NetworkingLibrary;
using UnityEngine;
using System.Text;

namespace Client
{
    public class Client : MonoBehaviour
    {
        [SerializeField] float tickRate;
        [SerializeField] bool isCalled;

        protected Socket socket;
        public NetworkComponent networkComponent;

        protected virtual void Start()
        {
            isCalled = false;
            socket = new Socket(
             AddressFamily.InterNetwork,
             SocketType.Stream,
             ProtocolType.Tcp);

            Debug.Log("Client is trying to connect to server");
            socket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3000));
            socket.Blocking = false;
            networkComponent = GetComponent<NetworkComponent>();
        }

        private void Update()
        {
            ProcessOfClientToConnectAndReceiveData();
        }

        void ProcessOfClientToConnectAndReceiveData()
        {
            if (isCalled) { return; }
            isCalled = true;

            try
            {
                ReceiveTheBufferFromServer();
            }
            catch (SocketException e)
            {
                if (e.SocketErrorCode != SocketError.WouldBlock)
                {
                    Console.WriteLine(e);
                }
            }
            Invoke(nameof(CallAgain), tickRate);
        }


        private void ReceiveTheBufferFromServer()
        {
            byte[] buffer = new byte[1024];
            socket.Receive(buffer);

            BasePacket _basePacket = new BasePacket();
            _basePacket = _basePacket.DeserializePacketType(buffer);

            ReceivePacketID(buffer, _basePacket);
            ReceivePacket(buffer, _basePacket);
        }

        protected virtual void ReceivePacket(byte[] buffer, BasePacket _basePacket)
        {
            if (_basePacket.packetType == BasePacket.PacketType.Position)
            {
                PositionPacket positionPacket = new PositionPacket();
                positionPacket = positionPacket.DeserializePosition(buffer);
                Debug.Log(positionPacket.position);
            }
        }

        private void ReceivePacketID(byte[] buffer, BasePacket _basePacket)
        {
            if (networkComponent.ClientID != 0) return;

            if (_basePacket.packetType == BasePacket.PacketType.ID)
            {
                IDPacket _idPacket = new IDPacket();
                _idPacket = _idPacket.DeserializeID(buffer);
                networkComponent.ClientID = _idPacket.ID;
                Debug.Log("Client ID is " + networkComponent.ClientID);
            }
        }

        void CallAgain()
        {
            isCalled = false;
        }
    }
}
