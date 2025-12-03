using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyButton : NetworkBehaviour, IPointable
{
    [SerializeField] HostGameLogic _gameLogic;
    [SerializeField, Range(0,1)] int _player;
    [SerializeField] MeshRenderer _mesh;

    bool _ready = false;

    [SerializeField] AudioSource _as;
    [SerializeField] AudioClip _rdy, _cnl;
    [SerializeField] ParticleSystem _buttonParticle;

    public void RPC_Interact(Team team, Player player) { }

    public void RPC_Interact_Host(Team team, HostPlayerController player)
    {
        if (_ready)
        {
            _ready = false;
            _gameLogic.PlayerReadyCancel(_player);
            _mesh.material.color = Color.red;

            _as.PlayOneShot(_cnl);
            _buttonParticle.startColor = Color.red;
            _buttonParticle.Play();
        }
        else
        {
            _ready = true;
            _gameLogic.PlayerReady(_player , player);
            _mesh.material.color = Color.green;

            _as.PlayOneShot(_rdy);
            _buttonParticle.startColor= Color.green;
            _buttonParticle.Play();
        }

    }



    public void RPC_Pointed(Team team, Player player) { }

    public void RPC_Pointed_Host(Team team, HostPlayerController player) { }
}
