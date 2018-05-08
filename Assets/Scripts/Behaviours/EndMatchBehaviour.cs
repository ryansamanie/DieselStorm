using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndMatchBehaviour : MonoBehaviour
{
    public MatchEndStateScriptable m_EndMatchScriptable;
    public int m_TimerDuration;
    public GameEventArgs m_OnTimerComplete;

    public void StartMatchTimer(Object[] args)
    {
        //if (args[0] as MatchEndStateScriptable == m_EndMatchScriptable)
        StartCoroutine(EndMatchTimer());
    }

    IEnumerator EndMatchTimer()
    {
        yield return new WaitForSeconds(m_TimerDuration);
        m_OnTimerComplete.Raise(this);
        StopCoroutine(EndMatchTimer());
    }
}
