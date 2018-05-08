﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Prototype.NetworkLobby;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Networking;

public class TeamSetupBehaviour : NetworkBehaviour
{
    public TeamSetupSingleton m_TeamSetupSingleton;
    public TeamSriptable m_TeamConfig;
    public GameObject m_TeamObject;
    public GameEventArgs m_OnSetupComplete;
    public GameEventArgs m_OnReadyForSetup;

    void Start()
    {
        m_OnReadyForSetup.Raise(this);
    }

    public void InitializeSetup(Object[] args)
    {
        //m_TeamSetupSingleton.ClearTeams();
        if (m_TeamSetupSingleton.m_Teams.Count <= 0)
        {
            CmdCreateTeams();
            m_TeamSetupSingleton.m_Players = new List<PlayerBehaviour>();
            StartCoroutine(GetPlayers());
            return;
        }
        foreach (var team in FindObjectsOfType<TeamBehaviour>())
        {
            StartCoroutine(team.InitalSpawn());
        }
    }

    public IEnumerator GetPlayers()
    {
        while(true)
        {
            yield return new WaitForSeconds(2.0f);
            foreach (var player in FindObjectsOfType<PlayerBehaviour>())
            {
                m_TeamSetupSingleton.AddPlayer(player);
            }
            m_TeamSetupSingleton.BalanceTeams();
            break;
        }
    }

    [Command]
    void CmdCreateTeams()
    {
        m_TeamSetupSingleton.m_Teams = new List<TeamSriptable>();
        m_TeamSetupSingleton.m_TeamColors = new List<Color>();
        for (int i = 0; i < m_TeamSetupSingleton.m_MaxTeams; i++)
        {
            if (GameObject.Find("Name" + i) != null)
                return;
            var newTeam = m_TeamConfig.CreateInstance();            
            if (m_TeamSetupSingleton.AddTeam(newTeam))
            {
                var teamObj = Instantiate(m_TeamObject);
                var teamBeahviour = teamObj.GetComponent<TeamBehaviour>();
                teamBeahviour.m_TeamScriptable = newTeam;
                teamBeahviour.m_TicketsRemaining = newTeam.m_TicketsRemaining;                
                teamObj.GetComponent<TeamBehaviour>().Setup("Name" + i);
                teamBeahviour.m_MaxPlayers = teamBeahviour.m_TeamScriptable.m_MaxPlayers;
                teamBeahviour.m_HeavyTankPrefab = teamBeahviour.m_TeamScriptable.m_HeavyTankPrefab;
                teamBeahviour.m_LightTankPrefab = teamBeahviour.m_TeamScriptable.m_LightTankPrefab;
                NetworkServer.Spawn(teamObj);                             
            }
        }
        StartCoroutine(SetUptimer());
    }

    IEnumerator SetUptimer()
    {
        yield return new WaitForSeconds(6.0f);
        m_OnSetupComplete.Raise(this);        
    }
}
