using System;
using UnityEngine;

namespace NetworkingLibrary
{
    public class PositionPacket : BasePacket
    {
        public Vector3 position { get; private set; }


        public PositionPacket()
        {
            position = Vector3.zero;
        }
        public PositionPacket(Vector3 _position) : base(PacketType.Position)
        {
            position = _position;
        }


        public byte[] SerializePosition()
        {
            SerializePacketType();

            binaryWriter.Write(position.x);
            binaryWriter.Write(position.y);
            binaryWriter.Write(position.z);

           return  writeMemoryStream.ToArray();
        }


        public PositionPacket DeserializePosition(byte[] _positionToDeserialize)
        {
            DeserializePacketType(_positionToDeserialize);

            float x = binaryReader.ReadSingle();
            float y = binaryReader.ReadSingle();
            float z = binaryReader.ReadSingle();

            position = new Vector3(x, y, z);
            return this;
        }
    }
}
