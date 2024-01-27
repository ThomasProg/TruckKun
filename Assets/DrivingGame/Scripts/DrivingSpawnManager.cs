using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrivingSpawnManager : MonoBehaviour
{
    private const float WaveTimer = 1.8f;
    private const float WaveSpawnDelayLimit = .2f;
    
    public Transform spawnContainer;

    public GameObject CarPrefab;
    public GameObject FastCarPrefab;
    public GameObject PickupPrefab;

    private List<List<GameObject>> _wavePatterns = new();
    private List<SpawnLane> _spawnLaneCandidate = new(){ SpawnLane.Left, SpawnLane.Middle, SpawnLane.Right };
    private Coroutine _spawnCoroutine;

    public enum SpawnLane
    {
        Left,
        Middle,
        Right
    }

    void Awake()
    {
        _wavePatterns.Add(new List<GameObject> { CarPrefab, CarPrefab, null });
        _wavePatterns.Add(new List<GameObject> { CarPrefab, CarPrefab, FastCarPrefab });
        _wavePatterns.Add(new List<GameObject> { CarPrefab, CarPrefab, PickupPrefab }); // pickup
        _wavePatterns.Add(new List<GameObject> { FastCarPrefab, PickupPrefab, null }); // pickup
        _wavePatterns.Add(new List<GameObject> { FastCarPrefab, FastCarPrefab, null });
    }

    public void InitializeGame()
    {
        _spawnCoroutine = StartCoroutine(CoSpawn());
    }

    IEnumerator CoSpawn()
    {
        while (true)
        {
            var currentWavePattern = _wavePatterns[Random.Range(0, _wavePatterns.Count - 1)];
            currentWavePattern.Shuffle();

            for (var i = 0; i < currentWavePattern.Count; i++)
            {
                var spawnCandidate = currentWavePattern[i];
                if (spawnCandidate == null) continue;
                var waveSpawnDelay = Random.Range(0, WaveSpawnDelayLimit);
                var spawnLane = _spawnLaneCandidate[i];
                StartCoroutine(Spawn(spawnCandidate, spawnLane, waveSpawnDelay));
            }
            
            yield return new WaitForSeconds(WaveTimer);
        }
    }

    IEnumerator Spawn(GameObject prefab, SpawnLane spawnLane, float waveSpawnDelay)
    {
        yield return new WaitForSeconds(waveSpawnDelay);
        
        var xSpawnPos = spawnLane switch
        {
            SpawnLane.Left => -.55f,
            SpawnLane.Middle => 0f,
            SpawnLane.Right => .55f
        };

        GameObject.Instantiate(prefab,
            new Vector3(xSpawnPos, 0f, 3f),
            Quaternion.identity,
            spawnContainer);
    }

    public void Stop()
    {
        foreach (Transform child in spawnContainer.transform)
        {
            Destroy(child.gameObject);
        }
        StopCoroutine(_spawnCoroutine);
    }
}

public static class IListExtensions {
    /// <summary>
    /// Shuffles the element order of the specified list.
    /// </summary>
    public static void Shuffle<T>(this IList<T> ts) {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i) {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }
}
