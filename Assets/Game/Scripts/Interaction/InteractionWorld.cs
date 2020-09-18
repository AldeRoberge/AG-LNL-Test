using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Game.Scripts.Utils;
using UnityEngine;

/// <summary>
/// Keeps a reference to the interactive objects in the world.
/// It is also a Singleton for ease of access.
/// </summary>
public class InteractionWorld : Singleton<InteractionWorld>
{
    public List<Grabbable> Grabbables = new List<Grabbable>();

    public void AddGrabbable(Grabbable grabbable)
    {
        Grabbables.Add(grabbable);
    }
}
