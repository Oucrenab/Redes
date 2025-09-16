using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnTrigger : MonoBehaviour, IPointable
{
    [SerializeField, Range(1, 7)] int _column;

    public void Interact(Team team)
    {
        EventManager.Trigger("OnColumnInteract", _column - 1, team);
    }

    public void Pointed(Team team)
    {
        EventManager.Trigger("OnColumnPoint", _column - 1, team);
    }
}
