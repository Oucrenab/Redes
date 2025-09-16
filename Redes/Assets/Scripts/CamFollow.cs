using Cinemachine;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CamFollow : MonoBehaviour
{ 
    //seguir solo al que no es proxy
    [SerializeField] Transform _camHolder;
    [SerializeField] Player _player;

    CinemachineVirtualCamera _cam;

    private void Awake()
    {
        _cam = GetComponent<CinemachineVirtualCamera>();
    }

    public void LateUpdate()
    {
        MoveCam();
    }

    void MoveCam()
    {
        if (!_camHolder) return;
        transform.position = _camHolder.position;
        transform.rotation = _camHolder.rotation;
    }

    public CamFollow SetCamHolder(Transform newHolder)
    {
        _camHolder = newHolder;
        _cam.Follow = _camHolder;
        return this;
    }

    public CamFollow SetPlayer(Player player)
    {
        _player = player;
        return this;
    }
}
