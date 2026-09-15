using UnityEngine;

public class Checkpoint3D : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO gameEventChannel;
    [SerializeField] private Transform spawnCenterPoint;

    private bool activated = false;

    public Vector3 SpawnPosition => spawnCenterPoint != null ? spawnCenterPoint.position : transform.position;

    private void OnTriggerEnter(Collider other)
    {
        if (!activated && other.CompareTag("Player"))
        {
            activated = true;
            if (gameEventChannel != null) gameEventChannel.RaiseCheckpoint();
        }
    }
}