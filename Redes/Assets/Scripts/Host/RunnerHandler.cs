using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RunnerHandler : MonoBehaviour, INetworkRunnerCallbacks
{

    public interface IProvider
    {
        event Action<RunnerHandler> OnRunnerUpdate;
    }

    [SerializeField] NetworkRunner _runnerPrefab;
    NetworkRunner _currentRunner;

    public Action OnJoinLobbySuccesfully;
    public Action<List<SessionInfo>> OnSessionsUpdate;

    #region Lobby
    public void JoinLobby()
    {
        if (_currentRunner)
            Destroy(_currentRunner.gameObject);

        _currentRunner = Instantiate(_runnerPrefab);

        _currentRunner.AddCallbacks(this);

        JoinLobbyAsync();
    }

    async void JoinLobbyAsync()
    {
        var result = await _currentRunner.JoinSessionLobby(SessionLobby.Custom, "Custom Lobby");

        if (result.Ok)
        {
            Debug.Log("Connected To Lobby");
            OnJoinLobbySuccesfully?.Invoke();
        }
        else
        {
            Debug.Log(":(");
        }
    }
    #endregion

    #region Host - Client

    public void HostGame(string sessionName, string scene)
    {
        CreateGame(GameMode.Host, sessionName, SceneUtility.GetBuildIndexByScenePath($"Scenes/{scene}"));
    }

    public void JoinGame(SessionInfo sessionInfo)
    {
        CreateGame(GameMode.Client, sessionInfo.Name);
    }


    async void CreateGame(GameMode gameMode, string sessionName, int scene = 0)
    {
        _currentRunner.ProvideInput = true;

        var result = await _currentRunner.StartGame(new StartGameArgs()
        {
            GameMode = gameMode,
            SessionName = sessionName,
            Scene = SceneRef.FromIndex(scene),
            PlayerCount = 2
        });

        if (result.Ok)
        {
            Debug.Log("Connected To Session");
        }
        else
        {
            Debug.Log(":(");
        }
    }
    #endregion


    #region Callbacks

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        //if(sessionList.Count == 0) 
        //{
        //    HostGame("Sipun", "HostScene");
        //    return;
        //}

        //JoinGame(sessionList[0]);

        OnSessionsUpdate?.Invoke(sessionList);
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) {}

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }

    public void OnInput(NetworkRunner runner, NetworkInput input) { }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }

    public void OnConnectedToServer(NetworkRunner runner) { }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }

    public void OnSceneLoadDone(NetworkRunner runner) { }

    public void OnSceneLoadStart(NetworkRunner runner) { }
    #endregion

}
