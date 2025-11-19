using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractuable
{
    public void RPC_Interact(Team team, Player player);
    public void RPC_Interact_Host(Team team, HostPlayerController player);
}
