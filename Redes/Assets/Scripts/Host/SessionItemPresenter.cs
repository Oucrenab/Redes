using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionItemPresenter : MonoBehaviour, SessionItemDefinition.IProvider, RunnerHandler.IProvider
{
    public event Action<SessionItemDefinition> OnSessionUpdate;
    public event Action<RunnerHandler> OnRunnerUpdate;

    public void Initialize(SessionInfo session, RunnerHandler runnerHandler)
    {
        OnSessionUpdate?.Invoke(new SessionItemDefinition(session));
        OnRunnerUpdate?.Invoke(runnerHandler);
    }
}
