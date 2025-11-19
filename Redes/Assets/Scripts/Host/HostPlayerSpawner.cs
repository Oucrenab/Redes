using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HostPlayerSpawner : NetworkBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] NetworkPrefabRef _playerPrefab;
    [SerializeField] Transform[] _spawnPos;
    [SerializeField] CamFollow _cam;
    [SerializeField] Canvas _errorCanvas;

    //a
    public override void Spawned()
    {
        Runner.AddCallbacks(this);
    }

    

    #region Callbacks
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) 
    {
        if (runner.IsServer)
        {
            var p = runner.Spawn(_playerPrefab,
                _spawnPos[runner.SessionInfo.PlayerCount - 1].transform.position,
                //_spawnPos[runner.SessionInfo.PlayerCount - 1].transform.rotation,
                Quaternion.identity,
                player).GetComponent<HostPlayerController>();

            if(runner.SessionInfo.PlayerCount < 2)
                p._team = Team.Red;
            else
                p._team = Team.Blue;

        }
    }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) 
    {
        //Canvas Error
        _errorCanvas.enabled = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //Volver al menu
    }

    public void OnInput(NetworkRunner runner, NetworkInput input) 
    {
        if (!LocalInputs.Instance) return;

        input.Set(LocalInputs.Instance.SetInputs());
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }

    public void OnConnectedToServer(NetworkRunner runner) { }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }

    public void OnSceneLoadDone(NetworkRunner runner) { }

    public void OnSceneLoadStart(NetworkRunner runner) { }
    #endregion

}
