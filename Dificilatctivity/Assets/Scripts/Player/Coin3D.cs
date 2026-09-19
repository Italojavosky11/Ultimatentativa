using UnityEngine;

public class Coin3D : MonoBehaviour
{
    public int coinID;
    [SerializeField] private CoinEventChannelSO coinEventChannel;
    [SerializeField] private float rotationSpeed = 90f;

    private void Start()
    {
        // Se a moeda já constava como coletada até o checkpoint salvo, desativa do cenário
        if (GameManager.Instance != null &&
            GameManager.Instance.CurrentGameData != null &&
            GameManager.Instance.CurrentGameData.collectedCoinIDs != null &&
            GameManager.Instance.CurrentGameData.collectedCoinIDs.Contains(coinID))
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (coinEventChannel != null) coinEventChannel.RaiseEvent(coinID);
            gameObject.SetActive(false);
        }
    }
}
