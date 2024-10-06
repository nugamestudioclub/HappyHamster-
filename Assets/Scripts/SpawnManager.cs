using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Transform player; // Player object
    public GameObject distressSignals; // Player object
    List<GameObject> spawners; // List of spawners
    public int active_spawners; // How many spawners will be active at once
    public GameObject enemyPrefab;

    void Start()
    {
        // TODO: make the list of children without this array thing
        spawners = new List<GameObject>(new GameObject[transform.childCount]);
        for (int i = 0; i < transform.childCount; i++)
        {
            spawners[i] = transform.GetChild(i).gameObject;
        }
    }

    // Sorts the points based on how far they are from the player and returns a sorted list
    List<GameObject> SortObjectsByDistance(List<GameObject> spawners, Transform player)
    {
        // Sort by furthest to closest // Can change to OrderBy for the reverse order
        return spawners.OrderByDescending(spawner => Vector3.Distance(player.position, spawner.transform.position)).ToList();
    }

    // Get the furthest GameObject from the player
    List<GameObject> GetFurthestN(List<GameObject> spawners, int n)
    {
        // Make sure n and spawners are valid
        if (spawners == null || spawners.Count < n) return null;

        List<GameObject> furthest_spawners = SortObjectsByDistance(spawners, player);

        return furthest_spawners.GetRange(0, n);
    }

    void SpawnEnemy(GameObject spawnPoint) {
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.transform.position, Quaternion.identity);
        // Set the parent of the enemy to the spawn point
        // Debug.Log(Enemy.enemyCount);
        enemy.transform.parent = spawnPoint.transform;
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        enemyScript.player = player;
        enemyScript.distressSignals = distressSignals;
    }
    

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.isGameOver) { return;  }
        List<GameObject> furthestN = GetFurthestN(spawners, active_spawners);

        if (furthestN != null) {
            foreach (GameObject point in furthestN) {
                if (Random.Range(0, 100) < 30) {
                    SpawnEnemy(point);
                }
            }
        }
    }
}
