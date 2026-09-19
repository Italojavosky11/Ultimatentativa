using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;

    private void Update()
    {
        if (GameManager.Instance != null && coinText != null)
        {
            coinText.text = $"Moedas: {GameManager.Instance.CurrentCoins}";
        }
    }
}
