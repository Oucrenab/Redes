using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SerializeField] NetworkTransform _netTransform;
    [SerializeField] float _speed;
    [SerializeField] Transform _camHolder;
    [SerializeField] Transform _camTarget;
    [SerializeField] LayerMask _wall;
    public Transform camHolder { get { return _camHolder; } }
    public Transform camTarget { get { return _camTarget; } }

    [SerializeField] CamFollow _cam;
    [SerializeField] float _sens;

    Vector3 _dirInputs;
    Vector2 _mouseInput;

    Vector2 _camRotation;

    private void Update()
    {
        _dirInputs.x = Input.GetAxisRaw("Vertical");
        _dirInputs.z = Input.GetAxisRaw("Horizontal");

        _mouseInput.x = Input.GetAxisRaw("Mouse X") * Time.deltaTime * _sens;
        _mouseInput.y = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * _sens;

        _camRotation.y += _mouseInput.x;
        _camRotation.x -= _mouseInput.y;

        _camRotation.x = Mathf.Clamp(_camRotation.x, -89, 89);

    }



    public override void FixedUpdateNetwork()
    {
        PlayerRotate();
        Movement(_dirInputs);

        ColumnSelectro();

    }

    float _rayDist = 10;
    void ColumnSelectro()
    {
         
        if(Physics.Raycast(_camHolder.transform.position ,_camHolder.transform.forward * _rayDist, out RaycastHit hit))
        {
            if(hit.transform.TryGetComponent<IPointable>(out var pointed)) 
            {
                pointed.RPC_Pointed(_team, this);

                if (Input.GetMouseButtonDown(0))
                    pointed.RPC_Interact(_team, this);
            }
        }
    }

    void PlayerRotate()
    {
        _netTransform.transform.rotation = Quaternion.Euler(0, _camRotation.y, 0);
        _camHolder.rotation = Quaternion.Euler(_camRotation.x, _camRotation.y, 0);
    }

    void Movement(Vector3 dir)
    {
        dir = WallCheck(dir);
        dir = dir.normalized * (_speed * Runner.DeltaTime);

        _netTransform.transform.position += (transform.forward * dir.x) + (transform.right * dir.z);
        //transform.position += (transform.forward * dir.x) + (transform.right * dir.z);
    }

    Vector3 WallCheck(Vector3 dir)
    {
        Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), transform.forward, Color.green);
        Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), transform.right, Color.green);
        Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), -transform.forward, Color.red);
        Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), -transform.right, Color.red);

        if(Physics.Raycast(transform.position + new Vector3(0,0.5f,0), transform.forward, 1, _wall) && dir.x > 0)
            dir.x = 0;
        if(Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), -transform.forward, 1, _wall) && dir.x < 0)
            dir.x = 0;
        if(Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), transform.right, 1, _wall) && dir.z > 0)
            dir.z = 0;
        if(Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), -transform.right, 1, _wall) && dir.z < 0)
            dir.z = 0;

        return dir;
    }

    public Player SetCam(CamFollow cam)
    {
        _cam = cam;
        _cam.SetCamHolder(_camHolder)
            .SetPlayer(this);
        return this;
    }

    [Networked] Team _team { get; set; } = Team.Red;

    [SerializeField] Color _redColor;
    [SerializeField] Color _blueColor;
    [SerializeField] MeshRenderer _renderer;

    [Rpc]
    public void RPC_SetTeam(Team team)
    {
        _team = team;
        switch (_team)
        {
            case Team.Red:
                _renderer.material.color = _redColor;
                break;
            case Team.Blue:
                _renderer.material.color = _blueColor;
                break;
            case Team.Empty:
                break;
            default:
                break;
        }
        //return this;
    }
}
