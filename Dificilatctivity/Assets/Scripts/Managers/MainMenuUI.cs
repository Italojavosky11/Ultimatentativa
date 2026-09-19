using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("Referências de UI")]
    [SerializeField] private Button continueButton;
    [SerializeField] private GameObject slotsPanel;

    private void Start()
    {
        // Regra do enunciado: Se o slot 0 (autosave) não estiver vazio, exibe o botão Continuar
        bool hasAutosave = SaveSystem.SlotExists(0);
        if (continueButton != null)
        {
            continueButton.gameObject.SetActive(hasAutosave);
        }

        if (slotsPanel != null)
        {
            slotsPanel.SetActive(false);
        }
    }

    // Botão: Continuar Jogo (Carrega o Slot 0)
    public void OnClickContinue()
    {
        GameData data = SaveSystem.LoadSlot(0);
        if (data != null)
        {
            GameManager.Instance.LoadFromData(data);
        }
    }

    // Botão: Novo Jogo (Começa na Fase 1 do zero)
    public void OnClickNewGame()
    {
        GameManager.Instance.StartNewGame();
    }

    // Botão: Carregar Jogo (Abre a tela de seleção de slots)
    public void OnClickOpenSlots()
    {
        if (slotsPanel != null)
        {
            slotsPanel.SetActive(true);
        }
    }

    // Chamado pelos botões de slot individuais (Slot 1, Slot 2, Slot 3)
    public void OnSelectLoadSlot(int slotIndex)
    {
        GameData data = SaveSystem.LoadSlot(slotIndex);
        if (data != null)
        {
            GameManager.Instance.LoadFromData(data);
        }
        else
        {
            Debug.LogWarning($"O Slot {slotIndex} está vazio!");
        }
    }

    // Botão: Sair do Jogo
    public void OnClickExit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}