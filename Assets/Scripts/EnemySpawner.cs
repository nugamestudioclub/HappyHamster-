using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    GameObject[] spawnPoints;
    public GameObject enemyPrefab;
    // Start is called before the first frame update
    void Start()
    {
        spawnPoints = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            spawnPoints[i] = transform.GetChild(i).gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject spawnPoint in spawnPoints)
        {
            if (Random.Range(0, 1000) < 10)
            {
                GameObject enemy = Instantiate(enemyPrefab, spawnPoint.transform.position, Quaternion.identity) as GameObject;
                // Set the parent of the enemy to the spawn point
                enemy.transform.parent = spawnPoint.transform;
            }
        }
    }
}
