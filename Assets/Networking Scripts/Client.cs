using System;
using System.Net.Sockets;
using System.Net;
using NetworkingLibrary;
using UnityEngine;
using System.Text;

namespace Client
{
    internal class Client : MonoBehaviour
    {
        [SerializeField] float tickRate;
        [SerializeField] bool isCalled;
        Socket socket;


        private void Start()
        {
            isCalled = false;
            socket = new Socket(
             AddressFamily.InterNetwork,
             SocketType.Stream,
             ProtocolType.Tcp);

            Debug.Log("Client is trying to connect to server");
            socket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3000));
            socket.Blocking = false;
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

            //string data = Encoding.ASCII.GetString(buffer);
            //Debug.Log(data);

            BasePacket _basePacket = new BasePacket();
            _basePacket = _basePacket.DeserializePacketType(buffer);

            if (_basePacket.packetType == BasePacket.PacketType.Position)
            {
                PositionPacket positionPacket = new PositionPacket();
                positionPacket = positionPacket.DeserializePosition(buffer);
                Debug.Log(positionPacket.position);
            }
        }
        void CallAgain()
        {
            isCalled = false;
        }
    }
}
