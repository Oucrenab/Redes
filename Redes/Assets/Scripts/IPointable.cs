using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPointable : IInteractuable
{
    public void Pointed(Team team);   
}
