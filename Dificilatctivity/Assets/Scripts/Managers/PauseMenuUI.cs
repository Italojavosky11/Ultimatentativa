using UnityEngine;
using UnityEngine.InputSystem; // Importante: namespace do novo Input System

public class PauseMenuUI : MonoBehaviour
{
    [Header("Painéis de UI")]
    [SerializeField] private GameObject pauseMenuCanvas;
    [SerializeField] private GameObject slotsPanelCanvas;

    private bool isPaused = false;
    private bool isSaveMode = true; // true = Salvar, false = Carregar

    private void Update()
    {
        // Usa a sintaxe do Novo Input System para ler o teclado sem erros
        if (Keyboard.current != null && (Keyboard.current.pKey.wasPressedThisFrame || Keyboard.current.escapeKey.wasPressedThisFrame))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        if (pauseMenuCanvas != null) pauseMenuCanvas.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        if (pauseMenuCanvas != null) pauseMenuCanvas.SetActive(false);
        if (slotsPanelCanvas != null) slotsPanelCanvas.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OpenSaveSlots()
    {
        isSaveMode = true;
        if (slotsPanelCanvas != null) slotsPanelCanvas.SetActive(true);
    }

    public void OpenLoadSlots()
    {
        isSaveMode = false;
        if (slotsPanelCanvas != null) slotsPanelCanvas.SetActive(true);
    }

    public void OnSelectSlot(int slotIndex)
    {
        if (isSaveMode)
        {
            // Salva no slot escolhido (e automaticamente replica no slot 0 via SaveSystem)
            SaveSystem.SaveSlot(slotIndex, GameManager.Instance.CurrentGameData);
            ResumeGame();
        }
        else
        {
            GameData data = SaveSystem.LoadSlot(slotIndex);
            if (data != null)
            {
                ResumeGame();
                GameManager.Instance.LoadFromData(data);
            }
            else
            {
                Debug.LogWarning($"O Slot {slotIndex} está vazio!");
            }
        }
    }

    public void OnClickReturnToMenu()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void OnClickCloseSlots()
    {
        if (slotsPanelCanvas != null)
        {
            slotsPanelCanvas.SetActive(false);
        }
    }
}