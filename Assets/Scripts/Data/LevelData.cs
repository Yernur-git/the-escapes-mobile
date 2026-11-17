using System.Collections.Generic;
using UnityEngine;

public enum Goal { ReachExit }

[System.Serializable]
public class Wave
{
    public GameObject enemyPrefab;
    public int count;
    public float delay;
    public float facingAngle;
    public Vector2[] patrolPoints;
}

[CreateAssetMenu(fileName = "LevelData", menuName = "Game/Level Data")]
public class LevelData : ScriptableObject
{
    public string levelName;
    public float timeLimit;
    public Goal goal;
    public Vector2 playerSpawn, exitPosition;
    public Vector2[] spawnPoints;
    public GameObject layout;
    public Vector2 worldSize;
    public List<Wave> waves = new();
}
