using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameNode : MonoBehaviour
{
    [SerializeField] Team _team;
    [SerializeField] MeshRenderer _renderer;

    public Team Team { get { return _team; } }

    public GameNode SetTeam(Team newT)
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

        return this;
    }
}

public enum Team
{
    Red, Blue, Empty
}
