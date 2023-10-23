using NetworkingLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateClient : Client.Client
{
    protected override void CheckAndReceivePacket(byte[] buffer, BasePacket _basePacket)
    {
        if (_basePacket.packetType == BasePacket.PacketType.Instantiation)
        {
            InstantiationPacket _instantiationPacket = new InstantiationPacket();
            _instantiationPacket = _instantiationPacket.DeserializeInstantiationdata(buffer);

            GameObject _spawnedObject = Instantiate(Resources.Load("prefabs/"+_instantiationPacket.prefabName) as GameObject, _instantiationPacket.position, _instantiationPacket.rotation);
            Debug.Log(_spawnedObject.name +"Object  deserialized and Spawned successfully");
            return;
        }
        Debug.Log( "Object instantiation data was not recived");
    }
}
