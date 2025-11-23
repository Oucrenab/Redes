using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    [SerializeField] Canvas _canvas;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            SetActive(!_canvas.enabled);
        }   
    }

    void SetActive(bool aja)
    {
        if (aja)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        _canvas.enabled = aja;
    }


}
