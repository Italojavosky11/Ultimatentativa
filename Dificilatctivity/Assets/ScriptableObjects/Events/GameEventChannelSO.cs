

using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "GameEventChannel", menuName = "Events/Game Event Channel")]
public class GameEventChannelSO : ScriptableObject
{
    public UnityAction OnCheckpointReached;
    public UnityAction OnLevelCompleted;

    public void RaiseCheckpoint() => OnCheckpointReached?.Invoke();
    public void RaiseLevelCompleted() => OnLevelCompleted?.Invoke();
}
