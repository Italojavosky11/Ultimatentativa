using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button continueButton;
    [SerializeField] private GameObject slotsPanel;

    private void Start()
    {
        // Exibe o botão "Continuar" somente se houver autosave no Slot 0
        bool hasAutosave = SaveSystem.SlotExists(0);
        continueButton.gameObject.SetActive(hasAutosave);
    }

    public void OnClickContinue()
    {
        GameData data = SaveSystem.LoadSlot(0);
        if (data != null)
        {
            GameManager.Instance.LoadFromData(data);
        }
    }

    public void OnClickNewGame()
    {
        GameManager.Instance.StartNewGame();
    }

    public void OnClickOpenSlots()
    {
        slotsPanel.SetActive(true);
    }

    public void OnClickExit()
    {
        Application.Quit();
    }
}
