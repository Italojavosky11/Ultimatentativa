using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "CoinEventChannel", menuName = "Events/Coin Event Channel")]
public class CoinEventChannelSO : ScriptableObject
{
    public UnityAction<int> OnCoinCollected;
    public void RaiseEvent(int coinID) => OnCoinCollected?.Invoke(coinID);
}