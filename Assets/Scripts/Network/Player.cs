using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Player : NetworkBehaviour
{
    [Networked] public NetworkString<_32> Name { get; set; }
    [Networked] public NetworkBool Ready { get; set; }

    public override void Spawned()
    {
        App.Instance.SetPlayer(Object.InputAuthority, this);
    }

    public void GetRandomName()
    {
        Name = "Jena";
    }

    public PlayerRef GetPlayerRef()
    {
        return Object.InputAuthority;
    }
}
