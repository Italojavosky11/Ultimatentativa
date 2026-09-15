using UnityEngine;

public class LevelInitializer : MonoBehaviour
{
    [SerializeField] private Transform initialSpawnPoint;
    [SerializeField] private Checkpoint3D checkpoint;
    [SerializeField] private GameObject playerPrefab;

    private void Start()
    {
        Vector3 spawnPos = initialSpawnPoint.position;

        if (GameManager.Instance.CurrentGameData.hasReachedCheckpoint && checkpoint != null)
        {
            spawnPos = checkpoint.SpawnPosition;
        }

        GameObject player = GameObject.FindWithTag("Player");
        if (player == null && playerPrefab != null)
        {
            player = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
        }
        else if (player != null)
        {
            player.transform.position = spawnPos;
        }
    }
}