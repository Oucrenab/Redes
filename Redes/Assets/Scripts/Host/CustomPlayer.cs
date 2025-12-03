using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPlayer : MonoBehaviour
{
    [SerializeField] HostPlayerController _playerRef;
    [SerializeField] Color _color;

    public void ChangePlayerColor(Color color)
    {
        _color = color;
        //_playerRef.SetColor(color);
    }

}
