using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCanvas : NetworkBehaviour
{
    [SerializeField] GameObject _redWins;
    [SerializeField] GameObject _blueWins;

    [Rpc]
    public void RPC_WinImage(Team team)
    {
        switch (team)
        {
            case Team.Red:
                _redWins.SetActive(true);
                _blueWins.SetActive(false);
                break;
            case Team.Blue:
                _blueWins.SetActive(true);
                _redWins.SetActive(false);
                break;
            default:
                break;
        }
    }

    [Rpc]
    public void RPC_TurnOff()
    {
        _redWins.SetActive(false);
        _blueWins.SetActive(false);
    }
}
