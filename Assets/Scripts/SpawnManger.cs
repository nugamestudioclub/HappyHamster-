using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject player; // Player object
    public List<GameObject> spawners; // List of spawners
    public int active_spawners; // How many spawners will be active at once

    void Start()
    {
        List<GameObject> furthest3 = GetFurthestN(spawners, active_spawners);

        if (furthest3 != null)
        {
            foreach (GameObject point in furthest3)
            {
                Debug.Log("SPAWN AT: " + point.name);
            }
        }
    }

    // Sorts the points based on how far they are from the player and returns a sorted list
    List<GameObject> SortObjectsByDistance(List<GameObject> spawners, GameObject player)
    {
        // Sort by furthest to closest // Can change to OrderBy for the reverse order
        return spawners.OrderByDescending(spawner => Vector3.Distance(player.transform.position, spawner.transform.position)).ToList();
    }

    // Get the furthest GameObject from the player
    List<GameObject> GetFurthestN(List<GameObject> spawners, int n)
    {
        // Make sure n and spawners are valid
        if (spawners == null || spawners.Count < n) return null;

        List<GameObject> furthest_spawners = SortObjectsByDistance(spawners, player);

        return furthest_spawners.GetRange(0, n);
    }
}
