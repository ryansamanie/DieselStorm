using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SimpleUIController : NetworkBehaviour
{
    public GameObject m_MathEndPanel;
    public Text m_MatchEndText;
    public string m_Textdata;    
    public Color m_TextColor;

    [ClientRpc]
    public void RpcDisplayMatchEndUI(bool status)
    {
        m_MathEndPanel.SetActive(status);
    }

    [ClientRpc]
    public void RpcSetText(string data, Color col)
    {
        CmdSetText(data, col);
    }

    [Command]
    void CmdSetText(string data, Color col)
    {
        m_Textdata = data;
        m_TextColor = col;
    }

    void Update()
    {
        if(!isServer)
            return;
        //m_MatchEndText.text = m_Textdata;   
    }

    void LateUpdate()
    {
        m_MatchEndText.text = m_Textdata;
        m_MatchEndText.color = m_TextColor;
    }
}
