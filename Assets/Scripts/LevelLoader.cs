using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] LevelData[] levels;
    [SerializeField] PlayerHealth player;
    [SerializeField] PrisonExit exit;
    [SerializeField] GameTimer timer;
    [SerializeField] Text objective;
    [SerializeField] CameraFollow view;
    [SerializeField] GameObject preview;

    public int LevelCount => levels.Length;

    IEnumerator Start()
    {
        if (GameSession.levelIndex < 0 || GameSession.levelIndex >= levels.Length ||
            !SaveSystem.Load().unlockedLevels.Contains(GameSession.levelIndex + 1))
            GameSession.levelIndex = 0;
        LevelData level = levels[GameSession.levelIndex];
        if (preview != null)
        {
            preview.SetActive(false);
            Destroy(preview);
        }
        Instantiate(level.layout, transform);
        player.GetComponent<Rigidbody2D>().position = level.playerSpawn;
        player.transform.position = level.playerSpawn;
        exit.transform.position = level.exitPosition;
        exit.gameObject.SetActive(level.goal == Goal.ReachExit);
        objective.text = level.levelName + " - reach exit";
        view.Configure(player.transform, level.worldSize);
        Physics2D.SyncTransforms();

        timer.Begin(level.timeLimit);
        int spawnIndex = 0;
        foreach (Wave wave in level.waves)
        {
            yield return new WaitForSeconds(wave.delay);
            Transform[] points = CreatePatrolPoints(wave.patrolPoints);
            for (int i = 0; i < wave.count; i++)
            {
                Vector2 position = level.spawnPoints[spawnIndex % level.spawnPoints.Length];
                GameObject enemy = Instantiate(wave.enemyPrefab, position,
                    Quaternion.Euler(0f, 0f, wave.facingAngle));
                if (enemy.TryGetComponent<Patroller>(out var patroller)) patroller.Configure(player.transform, points);
                if (enemy.TryGetComponent<Shooter>(out var shooter)) shooter.Configure(player.transform);
                if (enemy.TryGetComponent<Charger>(out var charger)) charger.Configure(player.transform);
                enemy.SetActive(true);
                spawnIndex++;
            }
        }
    }

    Transform[] CreatePatrolPoints(Vector2[] route)
    {
        var points = new Transform[route.Length];
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = new GameObject("patrol point").transform;
            points[i].SetParent(transform);
            points[i].position = route[i];
        }
        return points;
    }
}
