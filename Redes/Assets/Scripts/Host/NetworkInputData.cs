using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{
    public Vector3 direction;
    public Vector2 camMovement;

    //public bool interactPress;
    public NetworkBool interactPress;
}
