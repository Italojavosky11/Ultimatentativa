using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Event Channels")]
    [SerializeField] private CoinEventChannelSO coinEventChannel;
    [SerializeField] private GameEventChannelSO gameEventChannel;

    public GameData CurrentGameData { get; private set; } = new GameData();
    public int CurrentCoins { get; set; } = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        if (coinEventChannel != null) coinEventChannel.OnCoinCollected += HandleCoinCollected;
        if (gameEventChannel != null) gameEventChannel.OnCheckpointReached += HandleCheckpointReached;
    }

    private void OnDisable()
    {
        if (coinEventChannel != null) coinEventChannel.OnCoinCollected -= HandleCoinCollected;
        if (gameEventChannel != null) gameEventChannel.OnCheckpointReached -= HandleCheckpointReached;
    }

    public void StartNewGame()
    {
        CurrentGameData = new GameData();
        CurrentCoins = 0;
        SceneManager.LoadScene("Level_1");
    }

    public void LoadFromData(GameData data)
    {
        CurrentGameData = data;
        CurrentCoins = data.coinsAtCheckpoint; // Restaura as moedas salvas no checkpoint

        // Clona dados carregados no Slot 0 (Autosave)
        SaveSystem.SaveSlot(0, CurrentGameData);

        SceneManager.LoadScene($"Level_{CurrentGameData.levelIndex}");
    }

    private void HandleCoinCollected(int coinID)
    {
        if (!CurrentGameData.collectedCoinIDs.Contains(coinID))
        {
            CurrentGameData.collectedCoinIDs.Add(coinID);
            CurrentCoins++;
        }
    }

    private void HandleCheckpointReached()
    {
        CurrentGameData.hasReachedCheckpoint = true;
        CurrentGameData.coinsAtCheckpoint = CurrentCoins;

        // Autosave no Slot 0
        SaveSystem.SaveSlot(0, CurrentGameData);
        Debug.Log("Autosave executado no Slot 0 ao atingir o Checkpoint!");
    }
}