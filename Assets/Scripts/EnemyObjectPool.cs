using System.Collections.Generic;
using UnityEngine;

public class EnemyObjectPool : MonoBehaviour
{
    public GameObject prefab;
    [SerializeField] private LevelManager lm;
    private Queue<GameObject> pool = new Queue<GameObject>();

    public GameObject GetFromPool()
    {
        // Increment enemy count
        lm.currentHamsters += 1;

        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            return Instantiate(prefab);
        }
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);

        // Deincrement enemy count
        lm.currentHamsters--;
    }

    public void DestroyAllInPool()
    {
        while (pool.Count > 0)
        {
            Debug.Log("DESTROY");
            GameObject obj = pool.Dequeue();
            Destroy(obj); // Destroy the GameObject
        }
    }
}
