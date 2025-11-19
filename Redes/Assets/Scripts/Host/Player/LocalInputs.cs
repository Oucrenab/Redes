using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalInputs : NetworkBehaviour
{
    public static LocalInputs Instance { get; private set; }

    bool interactPress = false;

    NetworkInputData _inputData;

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            _inputData = new NetworkInputData();
            Instance = this;
            return;
        }

        enabled = false;
    }

    Vector2 movement;
    Vector2 camMovement;

    private void Update()
    {
        //if (!Object.HasInputAuthority) return;

        if (Input.GetMouseButtonDown(0))
            interactPress = true;

        movement.x = Input.GetAxisRaw("Vertical");
        movement.y = Input.GetAxisRaw("Horizontal");

        camMovement.x = Input.GetAxisRaw("Mouse X");
        camMovement.y = Input.GetAxisRaw("Mouse Y");
    }

    public NetworkInputData SetInputs()
    {
        //var inputData = new NetworkInputData();
        _inputData.direction.x = movement.x;
        _inputData.direction.z = movement.y;

        _inputData.camMovement.x = camMovement.x;
        _inputData.camMovement.y = camMovement.y;

        _inputData.interactPress = interactPress;

        interactPress = false;
        return _inputData;
    }
}
