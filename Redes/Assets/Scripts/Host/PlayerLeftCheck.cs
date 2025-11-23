using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System;

public class PlayerLeftCheck : MonoBehaviour
{
    [SerializeField] Canvas _errorCanavs;

    Action DoCheck;

    bool _active = false;

    private void Start()
    {
        DoCheck = () =>
        {
            int count = FindObjectsOfType<HostPlayerController>().Length;
            if(count > 1)
            {
                StartCheck();
            }
        };
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
                _errorCanavs.enabled = true;

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
}
