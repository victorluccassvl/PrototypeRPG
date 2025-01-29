using System;
using UnityEngine;

public static class Messages
{
    public static Action<Vector3, Vector3> NewPlayerMovementDestination = delegate { };
    public static Action PlayerArrivedDestination = delegate { };
    public static Action<Entity> EntityDied = delegate { };
    public static Action PlayerDied = delegate { };
}