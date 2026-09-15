using UnityEngine;
using TMPro;

public class VictoryZone3D : MonoBehaviour
{
    [SerializeField] private GameObject victoryPanelUI;
    [SerializeField] private TextMeshProUGUI victoryText;
    [SerializeField] private int totalCoinsInLevel = 5;
    [SerializeField] private GameEventChannelSO gameEventChannel;

    private bool isVictory = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isVictory)
        {
            isVictory = true;
            Time.timeScale = 0f; // Pausa o jogo ao vencer

            int coins = GameManager.Instance.CurrentCoins;
            victoryText.text = $"Fase Concluída!\nMoedas: {coins}/{totalCoinsInLevel}\nPressione [ESPAÇO] para Continuar";
            victoryPanelUI.SetActive(true);

            // Autosave de final de fase no Slot 0
            gameEventChannel.RaiseLevelCompleted();
            SaveSystem.SaveSlot(0, GameManager.Instance.CurrentGameData);
        }
    }

    private void Update()
    {
        if (isVictory && Input.GetKeyDown(KeyCode.Space))
        {
            Time.timeScale = 1f;
            int nextLevel = GameManager.Instance.CurrentGameData.levelIndex + 1;

            // Prepara dados da próxima fase
            GameManager.Instance.CurrentGameData.levelIndex = nextLevel;
            GameManager.Instance.CurrentGameData.hasReachedCheckpoint = false;
            GameManager.Instance.CurrentGameData.coinsAtCheckpoint = 0;
            GameManager.Instance.CurrentGameData.collectedCoinIDs.Clear();
            GameManager.Instance.CurrentCoins = 0;

            if (nextLevel <= 2)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene($"Level_{nextLevel}");
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
            }
        }
    }
}