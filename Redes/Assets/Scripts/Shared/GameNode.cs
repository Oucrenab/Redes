using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameNode : NetworkBehaviour, IPlayerLeft
{
    [SerializeField] Team _team;
    [SerializeField] MeshRenderer _renderer;

    [SerializeField] AudioSource _as;
    [SerializeField] AudioClip _paint, _drop;
    [SerializeField] ParticleSystem _dropParticle;

    public Team Team { get { return _team; } }

    [Rpc]
    public void RPC_SetTeam(Team newT, Color color)
    {
        _team = newT;
        //var color = Color.white;
        if(_team == Team.Empty)
        {
            RPC_SetColor(Team.Empty);
            return;
        }

        Debug.Log($"Esto no deberisa salir si empty {newT}");

        RPC_SetColor(color, false);

        _as.PlayOneShot(_drop);
        _dropParticle.startColor = color;
        _dropParticle.Play();
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
                color.a = 0;
                break;
        }
        _renderer.material.color = color;

        //return this;
    }
    [Rpc]
    public void RPC_SetColor(Color color, bool playSound = true)
    {
        _renderer.material.color = color;

        if (playSound)
            _as.PlayOneShot(_paint);
    }

    public void PlayerLeft(PlayerRef player)
    {
        Debug.Log("WAAAAAAAA");
        if (Runner.SessionInfo.PlayerCount >= 2) return;

        Runner.Despawn(Object);
    }
}

public enum Team
{
    Red, Blue, Empty
}
