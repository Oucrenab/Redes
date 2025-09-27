using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : SimulationBehaviour,IPlayerJoined
{
    [SerializeField] NetworkPrefabRef _playerPrefab;
    [SerializeField] Transform[] _spawnPoints;
    [SerializeField] CamFollow _cam;

    public void PlayerJoined(PlayerRef player)
    {
        if(player == Runner.LocalPlayer)
        {
            var spawnIdex = Runner.SessionInfo.PlayerCount - 1;

            var spawnPlayer = Runner.Spawn(_playerPrefab, _spawnPoints[spawnIdex].position, _spawnPoints[spawnIdex].rotation)
                .GetComponent<Player>().SetCam(_cam);

            if (Runner.SessionInfo.PlayerCount == 1)
                spawnPlayer.RPC_SetTeam(Team.Red);
            else
                spawnPlayer.RPC_SetTeam(Team.Blue);
        }

    }
}
