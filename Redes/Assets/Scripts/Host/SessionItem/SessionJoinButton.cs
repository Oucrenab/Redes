using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SessionJoinButton : MonoBehaviour
{
    Button _button;
    SessionInfo _session;

    private void Awake()
    {
        _button = GetComponent<Button>();

        var sessionProvider = GetComponentInParent<SessionItemDefinition.IProvider>();
        sessionProvider.OnSessionUpdate += RefreshSession;
        var runnerProvider = GetComponentInParent<RunnerHandler.IProvider>();
        runnerProvider.OnRunnerUpdate += RefreshRunner;
    }

    private void RefreshRunner(RunnerHandler runner)
    {
        _button.onClick.AddListener(() => runner.JoinGame(_session));
    }

    private void RefreshSession(SessionItemDefinition session)
    {
        _button.interactable = session.info.PlayerCount < session.info.MaxPlayers;
        _session = session.info;
    }
}
