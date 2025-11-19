using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogicSpawner : SimulationBehaviour, IPlayerJoined, IPlayerLeft
{
    [SerializeField] NetworkPrefabRef _gl;
    NetworkObject _lastGL;

    public void PlayerJoined(PlayerRef player)
    {
        if (_lastGL != default) return;
        if(Runner.SessionInfo.PlayerCount >= 1)
            _lastGL = Runner.Spawn(_gl, Vector3.zero, Quaternion.identity).GetComponent<NetworkObject>();

        
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (Runner.SessionInfo.PlayerCount < 2)
            Runner.Despawn(_lastGL);

        _lastGL = default;
    }
}
