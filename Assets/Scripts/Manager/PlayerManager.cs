using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AI;

public class PlayerManager : SingletonMonoBehaviour<PlayerManager>
{
    [SerializeField] private Player playerPrefab;
    [SerializeField] private Transform defaultSpawnPoint;

    [SerializeField] private CinemachineCamera cam;

    public Player Player { get; private set; }

    public void Start()
    {
        Messages.PlayerDied += DestroyKilledPlayer;

        SpawnPlayer();
    }

    public void OnDestroy()
    {
        Messages.PlayerDied -= DestroyKilledPlayer;
    }

    private void DestroyKilledPlayer()
    {
        if (Player != null) Destroy(Player.gameObject);
        Player = null;

        CameraTarget target = new CameraTarget();
        target.TrackingTarget = defaultSpawnPoint;
        cam.Target = target;

        respawnCoroutine = StartCoroutine(RespawnRoutine());
    }

    private void SpawnPlayer()
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(defaultSpawnPoint.position, out hit, 1f, 1 << NavMesh.GetAreaFromName("Walkable")))
        {
            Player = Instantiate(playerPrefab, hit.position, defaultSpawnPoint.rotation);
            CameraTarget target = new CameraTarget();
            target.TrackingTarget = Player.transform;
            cam.Target = target;
        }
        else
        {
            Debug.LogError("Invalid Spawn Position for Player");
        }
    }

    private Coroutine respawnCoroutine = null;
    private IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(5f);

        SpawnPlayer();
    }
}
