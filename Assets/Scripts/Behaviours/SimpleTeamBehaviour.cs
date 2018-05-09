using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SimpleTeamBehaviour : NetworkBehaviour
{
    public List<SimplePlayerBehaviour> m_Players;
    [SyncVar]public Color m_TeamColor;
    
    public void Add(SimplePlayerBehaviour player)
    {
        if(m_Players.Contains(player))
            return;
        m_Players.Add(player);
        player.SpawnNewTank();
        player.CmdSetTeamColor(m_TeamColor);
        //player.RpcSetTeamColor(m_TeamColor);
    }
    
    public void Remove(SimplePlayerBehaviour player)
    {
        if (m_Players.Contains(player))
            m_Players.Remove(player);
    }
}
