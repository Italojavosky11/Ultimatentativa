using UnityEngine;
using UnityEngine.SceneManagement;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Descarta a coleta feita após o checkpoint antes de recriar a fase.
            if (GameManager.Instance != null)
            {
                GameManager.Instance.RestoreCheckpoint();
            }

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
