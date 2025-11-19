using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPointable : IInteractuable
{
    public void RPC_Pointed(Team team, Player player);   
    public void RPC_Pointed_Host(Team team, HostPlayerController player);   
}
