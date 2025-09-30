using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : SimulationBehaviour,IPlayerJoined
{
    [SerializeField] NetworkPrefabRef _playerPrefab;
    [SerializeField] Transform[] _spawnPoints;
    [SerializeField] CamFollow _cam;
    //[SerializeField] GameLogic _gameLogic;

    public void PlayerJoined(PlayerRef player)
    {
        if (Runner.SessionInfo.PlayerCount > 2) return;
        if(player == Runner.LocalPlayer)
        {
            //var point = _spawnPoints[0];
            //if (_gameLogic._redTaken)
            //    point = _spawnPoints[1];

            var i = Runner.SessionInfo.PlayerCount - 1;

            //var spawnPlayer = Runner.Spawn(_playerPrefab, point.position, point.rotation)
            //    .GetComponent<Player>().SetCam(_cam);
            var spawnPlayer = Runner.Spawn(_playerPrefab, _spawnPoints[i].position, _spawnPoints[i].rotation)
                .GetComponent<Player>().SetCam(_cam);

            if (i == 0)
            {
                //_gameLogic._redTaken = true;
                spawnPlayer.RPC_SetTeam(Team.Red);
            }
            else
            {
                //_gameLogic._redTaken = false;
                spawnPlayer.RPC_SetTeam(Team.Blue);
            }
        }

    }
}
