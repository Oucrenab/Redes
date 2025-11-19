using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionItemDefinition
{
    public interface IProvider
    {
        event Action<SessionItemDefinition> OnSessionUpdate;
    }

    public SessionInfo info;

    public SessionItemDefinition(SessionInfo info)
    {
        this.info = info;
    }
}
