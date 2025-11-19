using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnTrigger : NetworkBehaviour, IPointable
{
    [SerializeField] GameLogic _gameLogic;
    [SerializeField, Range(0, 7)]public int _column;//a
    public int lareputaqueteparionetwork;

    [Rpc(sources: RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_Interact(Team team, Player player)
    {
        //EventManager.Trigger("OnColumnInteract", _column - 1, team, player);
        //if (GameLogic.Instance.GameStarted)
        //    GameLogic.Instance.Dropear(_column, team);
        _gameLogic.Dropear(_column-1, team);
        //Debug.Log($"{name} Interact {_column} {player.name} {team}");
    }

    [Rpc(sources: RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_Pointed(Team team, Player player)
    {
        //EventManager.Trigger("OnColumnPoint", _column - 1, team, player);
        _gameLogic.Pintar(_column-1, team);
        //Debug.Log($"{name} Pointed {_column} {player.name} {team}");
    }

    public void RPC_Pointed_Host(Team team, HostPlayerController player)
    {
        _gameLogic.Pintar(_column - 1, team);
    }

    public void RPC_Interact_Host(Team team, HostPlayerController player)
    {
        _gameLogic.Dropear(_column - 1, team);
    }
}
