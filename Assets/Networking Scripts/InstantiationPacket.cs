using NetworkingLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiationPacket : BasePacket
{
    string prefabName;
    Vector3 position;
    Quaternion rotation;

    InstantiationPacket()
    {
        prefabName = string.Empty;
        position = Vector3.zero;
        rotation = Quaternion.identity;
    }

    public InstantiationPacket(string _prefabName, Vector3 _position, Quaternion _rotation) 
        : base(PacketType.Instantiation)
    {
        prefabName = _prefabName;
        position = _position;
        rotation = _rotation;
    }

    public byte[] SerializePosition()
    {
        SerializePacketType();

        binaryWriter.Write(prefabName);

        binaryWriter.Write(position.x);
        binaryWriter.Write(position.y);
        binaryWriter.Write(position.z);

        binaryWriter.Write(rotation.x);
        binaryWriter.Write(rotation.y);
        binaryWriter.Write(rotation.z);
        binaryWriter.Write(rotation.w);

        return writeMemoryStream.ToArray();
    }

    public InstantiationPacket DeserializePosition(byte[] _buffer)
    {
        DeserializePacketType(_buffer);

        prefabName = binaryReader.ReadString();
        position = new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
        rotation = new Quaternion(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());

        return this;
    }
}