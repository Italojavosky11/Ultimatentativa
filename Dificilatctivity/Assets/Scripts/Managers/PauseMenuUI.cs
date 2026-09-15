using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuCanvas;
    [SerializeField] private GameObject slotsPanelCanvas;

    private bool isPaused = false;
    private bool isSaveMode = true; // true = Salvar, false = Carregar

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        pauseMenuCanvas.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenuCanvas.SetActive(false);
        slotsPanelCanvas.SetActive(false);
        Time.timeScale = 1f;
    }


    public void OpenSaveSlots()
    {
        isSaveMode = true;
        slotsPanelCanvas.SetActive(true);
    }

    public void OpenLoadSlots()
    {
        isSaveMode = false;
        slotsPanelCanvas.SetActive(true);
    }

    public void OnSelectSlot(int slotIndex)
    {
        if (isSaveMode)
        {
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
        }
    }

    public void OnClickReturnToMenu()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
