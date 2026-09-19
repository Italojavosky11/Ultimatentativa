using UnityEngine;
using UnityEngine.InputSystem; // <-- ADICIONADO PARA RECONHECER O 'Keyboard'
using TMPro;

public class VictoryZone3D : MonoBehaviour
{
    [Header("UI & Eventos")]
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
            Time.timeScale = 0f; // Pausa a simulação ao vencer
            
            int coins = GameManager.Instance.CurrentCoins;
            if (victoryText != null)
            {
                victoryText.text = $"Fase Concluída!\nMoedas: {coins}/{totalCoinsInLevel}\nPressione [ESPAÇO] para Continuar";
            }

            if (victoryPanelUI != null)
            {
                victoryPanelUI.SetActive(true);
            }

            // Notifica os canais e executa o Autosave de vitória no Slot 0
            if (gameEventChannel != null)
            {
                gameEventChannel.RaiseLevelCompleted();
            }
            SaveSystem.SaveSlot(0, GameManager.Instance.CurrentGameData);
        }
    }

    private void Update()
    {
        // Verifica o pressionamento da tecla Espaço via Novo Input System
        if (isVictory && Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Time.timeScale = 1f; // Restaura o tempo
            
            int currentLevel = GameManager.Instance.CurrentGameData.levelIndex;

            // Se estiver na Fase 1, prepara e carrega a Fase 2
            if (currentLevel == 1)
            {
                GameManager.Instance.CurrentGameData.levelIndex = 2;
                GameManager.Instance.CurrentGameData.hasReachedCheckpoint = false;
                GameManager.Instance.CurrentGameData.coinsAtCheckpoint = 0;
                GameManager.Instance.CurrentGameData.collectedCoinIDs.Clear();
                GameManager.Instance.CurrentCoins = 0; // Reseta moedas para a nova fase

                UnityEngine.SceneManagement.SceneManager.LoadScene("Level_2");
            }
            else // Se já esteve na Fase 2 (Fim do Jogo), retorna ao Menu
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
            }
        }
    }
}