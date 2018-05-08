using System.Collections;
using System.Collections.Generic;
using Prototype.NetworkLobby;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class LobbyStateScriptable : StateScriptable
{            
    public override void OnEnter()
    {
        m_OnStateEntered.Raise(this);
        //if (SceneManager.GetActiveScene().name != "0.lobby")
        //{
        //    SceneManager.LoadScene("0.lobby");
        //}
        //ClearPlayers();
    }

    public override void OnExit()
    {
        m_OnStateExit.Raise(this);
    }

    void ClearPlayers()
    {
        var players = FindObjectsOfType<PlayerBehaviour>();
        for (int i = 0; i < players.Length; i++)
        {
            Destroy(players[i].gameObject);
        }
    }
}
