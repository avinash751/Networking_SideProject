using NetworkingLibrary;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class IDPacket : BasePacket
{
    public int ID;


    public IDPacket() 
    {
        ID = 0;
    }
    public IDPacket(int _id) 
        : base(PacketType.ID)
    {
        ID = _id;
    }

    public byte[] SerializeID()
    {
        SerializePacketType();
        binaryWriter.Write(ID);
        return writeMemoryStream.ToArray();
    }

    public IDPacket DeserializeID(byte[] _dataToDeserialize)
    {
        DeserializePacketType(_dataToDeserialize);
        ID = binaryReader.ReadInt32();
        return this;
    }


}
