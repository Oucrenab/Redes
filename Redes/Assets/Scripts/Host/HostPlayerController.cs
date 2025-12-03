using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HostPlayerController : NetworkBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] float _camSens;
    [SerializeField] NetworkTransform _netTransform;
    [SerializeField] NetworkTransform _camHolder;
    [SerializeField] LayerMask _wall;

    [SerializeField] CamFollow _cam;
    [SerializeField] SkinnedMeshRenderer _renderer;


    [Networked] public Team _team { get; set; } = Team.Red;

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
            SetCam(FindFirstObjectByType<CamFollow>());
    }

    public override void FixedUpdateNetwork()
    {
        if(!GetInput(out NetworkInputData input)) return;

        //Move(input.direction.normalized * (_speed * Runner.DeltaTime));
        //transform.position += (transform.forward * (input.direction.x * _speed * Runner.DeltaTime)
        //    + transform.right * (input.direction.z * _speed * Runner.DeltaTime));//a

        PlayerRotate(input.camMovement);
        Movement(input.direction);

        ColumnSelectro(input.interactPress);
    }

    Vector2 _camRotation;

    void PlayerRotate(Vector2 rot)
    {
        rot *= (Runner.DeltaTime * _camSens);

        _camRotation.y += rot.x;
        _camRotation.x -= rot.y;

        _camRotation.x = Mathf.Clamp(_camRotation.x, -89, 89);

        //_netTransform.transform.rotation = Quaternion.Euler(0, _camRotation.y, 0);
        //_netTransform.transform.eulerAngles = new Vector3(0, _camRotation.y, 0);
        _netTransform.transform.eulerAngles += new Vector3(0, rot.x, 0);
        //_camHolder.transform.rotation = Quaternion.Euler(_camRotation.x, _camRotation.y, 0);
        //_camHolder.transform.eulerAngles = new Vector3(_camRotation.x, _camRotation.y, 0);
        _camHolder.transform.eulerAngles += new Vector3(- rot.y, 0, 0);
    }

    void Movement(Vector3 dir)
    {
        dir = WallCheck(dir);
        dir = dir.normalized * (_speed * Runner.DeltaTime);

        _netTransform.transform.position += (transform.forward * dir.x) + (transform.right * dir.z);
        //transform.position += (transform.forward * dir.x) + (transform.right * dir.z);

        //Debug.Log(dir.normalized);
        UpdateAnim(dir.normalized);
    }
    [SerializeField] Animator _myAnim;
    void UpdateAnim(Vector3 moveDir)
    {
        if (!_myAnim) return;
        _myAnim.SetFloat("xMove", moveDir.x);
        _myAnim.SetFloat("zMove", moveDir.z);
    }

    Vector3 WallCheck(Vector3 dir)
    {
        Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), transform.forward, Color.green);
        Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), transform.right, Color.green);
        Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), -transform.forward, Color.red);
        Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), -transform.right, Color.red);

        if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), transform.forward, 1, _wall) && dir.x > 0)
            dir.x = 0;
        if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), -transform.forward, 1, _wall) && dir.x < 0)
            dir.x = 0;
        if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), transform.right, 1, _wall) && dir.z > 0)
            dir.z = 0;
        if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), -transform.right, 1, _wall) && dir.z < 0)
            dir.z = 0;

        return dir;
    }

    float _rayDist = 10;
    void ColumnSelectro(bool click)
    {

        //if (Physics.Raycast(_camHolder.transform.position, _camHolder.transform.forward * _rayDist, out RaycastHit hit))
        if (Runner.LagCompensation.Raycast(origin: _camHolder.transform.position, 
                                           direction: _camHolder.transform.forward,
                                           length: _rayDist,
                                           player: Object.InputAuthority,
                                           out var hit))
        {
            if (hit.Hitbox.Root.TryGetComponent<IPointable>(out var pointed))
            {
                pointed.RPC_Pointed_Host(_team, this);

                if (click)
                    pointed.RPC_Interact_Host(_team, this);
            }
        }
    }

    public HostPlayerController SetCam(CamFollow cam)
    {
        _cam = cam;
        _cam.SetCamHolder(_camHolder.transform);
        return this;
    }

    public void SetColorPicker(ColorPicker picker)
    {
        Debug.Log("Picker Seteado");
        picker.OnColorSelect.AddListener(RPC_SetColor);
    }


    public Color playerColor;
    [Networked] public NetworkBool canChangeColor { get; set; } = true;
    [Rpc]
    public void RPC_SetColor(Color color)
    {
        if (!canChangeColor) return;
        _renderer.material.color = color;
        playerColor = color;
    }
}
