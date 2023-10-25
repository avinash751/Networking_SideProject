using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class NetworkingManager : MonoBehaviour
{
    [SerializeField] int maxIDRange;
    public static  Dictionary<int, Socket> clientIDs = new Dictionary<int, Socket>();

    public static object ClientSpawnedAnObject;

    private void OnEnable()
    {
        Server.Server.ClientIDGenerated += GenerateUniqueClientID;
        Server.Server.ClientAdded += AddClientIDToDictionary;
    }

    private void OnDisable()
    {
        Server.Server.ClientIDGenerated -= GenerateUniqueClientID;
        Server.Server.ClientAdded -= AddClientIDToDictionary;
    }

    int GenerateUniqueClientID()
    {
        int uniqueID = 0;
        do
        {
            uniqueID = Random.Range(1, maxIDRange);
        } while (clientIDs.ContainsKey(uniqueID));
        return uniqueID;
    }

    public  void AddClientIDToDictionary(int _clientID, Socket _socket)
    {
        clientIDs.Add(_clientID, _socket);
    }


}
