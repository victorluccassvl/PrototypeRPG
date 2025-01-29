using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    [field: SerializeField] public Entity Entity { get; private set; }

    [field: SerializeField] public PlayerControl Control { get; private set; }
    [field: SerializeField] public PlayerMove Move { get; private set; }

    [field: SerializeField] public NavMeshAgent NavMeshAgent { get; private set; }
}
