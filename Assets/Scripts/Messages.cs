using System;
using UnityEngine;

public static class Messages
{
    public static Action<Vector3, Vector3> NewPlayerMovementTerrainDestination = delegate { };
    public static Action PlayerArrivedTerrainDestination = delegate { };
    public static Action NewPlayerFollowTarget = delegate { };
    public static Action PlayerReachedFollowTarget = delegate { };
    public static Action<Entity> EntityDied = delegate { };
    public static Action PlayerDied = delegate { };
}