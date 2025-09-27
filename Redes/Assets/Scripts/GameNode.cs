using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameNode : NetworkBehaviour
{
    [SerializeField] Team _team;
    [SerializeField] MeshRenderer _renderer;

    public Team Team { get { return _team; } }

    [Rpc]
    public void RPC_SetTeam(Team newT)
    {
        _team = newT;
        var color = Color.white;

        switch (_team)
        {
            case Team.Red:
                color = Color.red;
                break;
            case Team.Blue:
                color = Color.blue;
                break;
            case Team.Empty:
                break;
        }
        _renderer.material.color = color;

        //print("b " + color);

        //return this;
    }

    [Rpc]
    public void RPC_SetColor(Team newT)
    {
        var color = Color.white;
        switch (newT)
        {
            case Team.Red:
                color = Color.red;
                break;
            case Team.Blue:
                color = Color.blue;
                break;
            case Team.Empty:
                break;
        }

        _renderer.material.color = color;

        //return this;
    }
}

public enum Team
{
    Red, Blue, Empty
}
