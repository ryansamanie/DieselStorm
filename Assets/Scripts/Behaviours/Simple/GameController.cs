using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameController : NetworkBehaviour
{
    public SimpleScoreBehaviour m_ScoreKeeper;
    public bool m_AtLeastTwoFilledTeams;

    [Tooltip("timer for players to join game before match termination")]
    public float m_GameTimerOut;

    [SerializeField]
    private float m_Timer;

    public bool m_GameOver;

    void Awake()
    {
        if (m_GameTimerOut <= 0)
            m_GameTimerOut = 10;
    }

    void Update()
    {
        if (m_GameOver)
        {
            var tanks = FindObjectsOfType<NetworkTankInputController>();
            foreach (var tank in tanks)
            {
                tank.enabled = false;
                tank.GetComponent<TurrentAimBehaviour>().enabled = false;
            }
            return;            
        }
        int numFilledTeams = 0;
        foreach (var team in m_ScoreKeeper.m_TeamController.m_Teams)
        {
            if (team.m_PlayerOnTeam)
                numFilledTeams++;
        }        
        m_AtLeastTwoFilledTeams = numFilledTeams > 1;

        if (!m_AtLeastTwoFilledTeams)
        {
            m_Timer += Time.deltaTime;
            if (m_Timer >= m_GameTimerOut)
            {
                m_GameOver = true;                
            }
            return;
        }

        if (m_ScoreKeeper.m_OnlyOneTeamRemaining)
        {
            m_GameOver = true;
        }
    }
}
