using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }

    [Header("Spawn Point Parent")]
    [SerializeField] private Transform spawnParent;  

    private List<Transform> spawnPoints;

    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Collect spawn points from children
        spawnPoints = spawnParent.GetComponentsInChildren<Transform>()
            .Where(t => t != spawnParent)
            .ToList();
    }

    // Return spawn point and remove it from the list of available spawns
    public Transform GetUniqueSpawnPoint()
    {
        if (spawnPoints.Count == 0)
        {
            Debug.LogError("No spawn points remaining!");
            return null;
        }

        int index = Random.Range(0, spawnPoints.Count);
        Transform chosen = spawnPoints[index];
        spawnPoints.RemoveAt(index);
        return chosen;
    }
    
    // Get without removing
    public Transform GetRandomSpawnPoint()
    {
        int index = Random.Range(0, spawnPoints.Count);
        return spawnPoints[index];
    }
}