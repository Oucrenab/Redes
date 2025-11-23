using Fusion;
using Fusion.Sockets;
using System.Collections;
using UnityEngine;

public class HostErrorCanvas : MonoBehaviour
{
    [SerializeField] Canvas _errorCanvas;
    [SerializeField] HostGameLogic _gl;

    public void PlayerLeft()
    {
        //RPC_PlayerLeft(player);
        Debug.Log("Coño");
        //if (Runner.SessionInfo.PlayerCount > 2) return;

        //RPC_GameOver(Team.Empty);
        //GameStarted = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        _errorCanvas.enabled = true;
        //GameStarted = false;
        //_error = true;
        StopAllCoroutines();
    }
}
