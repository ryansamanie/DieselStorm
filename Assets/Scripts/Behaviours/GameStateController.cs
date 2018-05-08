﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public class GameStateController : NetworkBehaviour
{
    public List<StateScriptable> m_States;
    public StateScriptable m_CurrentState;    

    void Awake()
    {
        if(m_CurrentState == null)
            Debug.LogError("No initial state set");
        m_CurrentState.OnEnter();

        SceneManager.sceneLoaded += SceneLoaded;        
    }

    void TransitionToState(Object[] args)
    {
        var sender = args[0] as StateScriptable;
        var toState = args[1] as StateScriptable;
        if(m_CurrentState != sender)
            return;        
        m_CurrentState = sender.TryTransition(toState) ? toState : m_CurrentState;
        if (m_CurrentState == toState)
        {
            m_CurrentState.OnEnter();
            sender.OnExit();
            if (m_CurrentState.GetType() == typeof(MatchLoadStateScriptable))
            {
                var state = m_CurrentState as MatchLoadStateScriptable;
                StartCoroutine(state.StateSwitchDelay());
            }            
        }
    }    
    
    public void SceneLoaded(Scene cur, LoadSceneMode mode)
    {
        //if (cur.name == "1.Game-Play")
        //{
        //    TransitionToState(new Object[] { m_CurrentState, m_States[3] });
        //    return;
        //}
        //if (cur.name == "0.lobby")
        //{
        //    TransitionToState(new Object[] { m_CurrentState, m_States[1] });
        //    return;
        //}
    }

    public void TransitionToGame(Object[] args)
    {
        TransitionToState(new Object[] { m_CurrentState, m_States[0] });
    }
    
    public void TransitionToEnd(Object[] args)
    {
        TransitionToState(new Object[] { m_CurrentState, m_States[2] });
    }

    public void TransitionToLoad(Object[] args)
    {        
        TransitionToState(new Object[] { m_CurrentState, m_States[3] });
    }
}
