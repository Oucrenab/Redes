using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCanvas : NetworkBehaviour
{
    [SerializeField] GameObject _redWins;
    [SerializeField] GameObject _blueWins;
    [SerializeField] GameObject _draw;

    [Rpc]
    public void RPC_WinImage(Team team)
    {
        switch (team)
        {
            case Team.Red:
                _redWins.SetActive(true);
                _blueWins.SetActive(false);
                _draw.SetActive(false);
                break;
            case Team.Blue:
                _blueWins.SetActive(true);
                _redWins.SetActive(false);
                _draw.SetActive(false);
                break;
            default:
                _blueWins.SetActive(false);
                _redWins.SetActive(false);
                _draw.SetActive(true);
                break;
        }
    }

    [Rpc]
    public void RPC_TurnOff()
    {
        _redWins.SetActive(false);
        _blueWins.SetActive(false);
        _draw.SetActive(false);
    }
}
