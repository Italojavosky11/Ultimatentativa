using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
    public int levelIndex = 1;
    public bool hasReachedCheckpoint = false;
    public int coinsAtCheckpoint = 0;
    public List<int> collectedCoinIDs = new List<int>();
    public List<int> collectedCoinIDsAtCheckpoint = new List<int>();
}
