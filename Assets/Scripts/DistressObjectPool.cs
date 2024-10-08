using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistressObjectPool : MonoBehaviour
{
    public GameObject prefab;
    private Queue<GameObject> pool = new Queue<GameObject>();

    public GameObject GetFromPool()
    {
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
    }

    public IEnumerator ReturnToPoolAfterDelay(GameObject obj, float duration)
    {
        yield return new WaitForSeconds(duration);
    
        // Return the enemy GameObject to the pool
        ReturnToPool(obj);
    }
}
