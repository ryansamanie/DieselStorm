using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ExtendNetworkManager : NetworkManager
{
    public GameEventArgs m_OnClientConnected;
    public GameEventArgs m_OnClientDisconnected;
    public TeamController m_TeamController;

    public Dictionary<string,NetworkIdentity> m_Connections = new Dictionary<string, NetworkIdentity>();

    public int m_PlayerConnected = 0;
    //Detect when a client connects to the Server
    public override void OnServerConnect(NetworkConnection connection)
    {
        base.OnServerConnect(connection);
        if (m_TeamController == null)
            StartCoroutine(SearchForController());
        StartCoroutine(ClientConnect(connection));
    }

    //Detect when a client connects to the Server
    public override void OnServerDisconnect(NetworkConnection connection)
    {        
        foreach (var con in m_Connections)
        {
            if (con.Value.connectionToClient.connectionId == connection.connectionId)
            {                
                m_TeamController.OnPlayerLeave(con.Value);
                m_PlayerConnected--;                
                m_Connections.Remove(con.Key);
            }
        }
        base.OnServerDisconnect(connection);
    }

    IEnumerator SearchForController()
    {
        yield return new WaitForSeconds(1.0f);
        m_TeamController = FindObjectOfType<TeamController>();
    }

    IEnumerator ClientConnect(NetworkConnection connection)
    {
        yield return new WaitForSeconds(2.0f);
        var netIDs = FindObjectsOfType<NetworkIdentity>();
        foreach (var id in netIDs)
        {
            if (id.connectionToClient == null)
                break;
            if (id.connectionToClient.connectionId == connection.connectionId)
            {
                m_TeamController.OnPlayerJoined(id);
                m_PlayerConnected++;
                m_Connections.Add("Connection::" + m_PlayerConnected, id);
            }
        }
        StopCoroutine(ClientConnect(connection));
    }
}
