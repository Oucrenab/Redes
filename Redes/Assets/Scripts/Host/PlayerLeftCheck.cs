using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLeftCheck : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] Canvas _errorCanvas;

    Action DoCheck;

    bool _active = false;

    NetworkRunner runner;

    private void Start()
    {
        DoCheck = () =>
        {
            int count = FindObjectsOfType<HostPlayerController>().Length;
            if (count > 1)
            {
                StartCheck();
            }
        };

        //runner = FindObjectOfType<NetworkRunner>();
        //runner.AddCallbacks(this);
    }

    public void StartCheck()
    {
        Debug.Log($"<color=#ffc0c0>Player detection started</color>");

        if (_active) return;

        _active = true;

        DoCheck = () =>
        {
            int count = FindObjectsOfType<HostPlayerController>().Length;
            if (count < 2)
            {
                _active = false;
                Debug.Log("<color=00ffff> Pantalla azul </color>");
                _errorCanvas.enabled = true;

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                DoCheck = delegate { };
            }
        };
    }

    private void FixedUpdate()
    {
        DoCheck?.Invoke();
    }

    #region Callbacks
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }

    public void OnInput(NetworkRunner runner, NetworkInput input) { }

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
