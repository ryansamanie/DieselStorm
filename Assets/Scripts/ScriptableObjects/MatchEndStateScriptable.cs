﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MatchEndStateScriptable : StateScriptable
{    
    public override void OnEnter()
    {
        m_OnStateEntered.Raise(this);
    }

    IEnumerator EnterStateDelay()
    {
        yield return new WaitForSeconds(1.0f);
        m_OnStateEntered.Raise(this);
    }

    public override void OnExit()
    {        
        m_OnStateExit.Raise(this);
        var teams = FindObjectsOfType<TeamBehaviour>();
        //var players = FindObjectsOfType<PlayerBehaviour>();
        var tanks = FindObjectsOfType<NetworkTankInputController>();
        for (int i = 0; i < teams.Length; i++)
        {
            Destroy(teams[i].gameObject);
        }

        //for (int i = 0; i < players.Length; i++)
        //{
        //    Destroy(players[i].gameObject);
        //}
        for (int i = 0; i < tanks.Length; i++)
        {
            Destroy(tanks[i].gameObject);
        }
    }
}
