using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class ArenaManager : NetworkBehaviour
{
    public override void Spawned()
    {
        Debug.Log("ArenaManager spawned");
    }
}
