using System;
using UnityEngine;

public static class Messages
{
    public static Action<RaycastHit> NewPlayerMovementDestination = delegate { };
    public static Action PlayerArrivedDestination = delegate { };
}