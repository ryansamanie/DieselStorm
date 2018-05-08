using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GamePlayBehaviour : NetworkBehaviour
{
    public GameplayStateScriptable m_PlayStateScriptable;
    public float m_GamePlayTimer;
    public GameEventArgs m_GamePlayOver;

    public void StartGamePlayTimer(Object[] args)
    {
        StartCoroutine(GamePlayTimer());
    }

    IEnumerator GamePlayTimer()
    {
        yield return new WaitForSeconds(m_GamePlayTimer);
        m_GamePlayOver.Raise(this);
        StopCoroutine(GamePlayTimer());
    }
}
