using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SimplePlayerBehaviour : NetworkBehaviour
{
    public GameObject m_TankObjectPrefab;
    [SyncVar]public GameObject m_rtTankObject;
    [SyncVar]public Color m_TeamColor;
    public Material m_TankMaterial;


    [ClientRpc]
    public void RpcSetTeamColor(GameObject tank, Color col)
    {        
        var renders = tank.GetComponentsInChildren<SkinnedMeshRenderer>();        
        foreach (var renderer in renders)
        {
            var oldMaterial = renderer.material;
            m_TankMaterial = new Material(Shader.Find("Shader Forge/Tank_Shader"));
            m_TankMaterial.CopyPropertiesFromMaterial(oldMaterial);
            m_TankMaterial.SetColor("_ColorPicker", col);
            renderer.material = m_TankMaterial;
        }
    }

    [Command]
    public void CmdSetTeamColor(Color col)
    {
        m_TeamColor = col;
    }

    public void SpawnNewTank()
    {
        var newTank = Instantiate(m_TankObjectPrefab);
        NetworkServer.Spawn(newTank);
        m_rtTankObject = newTank;
        RpcSetTeamColor(m_rtTankObject, m_TeamColor);
        RpcSpawnTank();
    }

    [ClientRpc]
    void RpcSpawnTank()
    {
        CmdSpawnNewTank();        
    }

    [Command]
    public void CmdSpawnNewTank()
    {
        if (isLocalPlayer)
        {
            var connection = GetComponent<NetworkIdentity>().connectionToClient;
            m_rtTankObject.GetComponent<NetworkIdentity>().AssignClientAuthority(connection);
        }
    }
}
