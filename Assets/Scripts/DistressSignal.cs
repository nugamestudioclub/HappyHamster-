using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistressSignal : MonoBehaviour
{
    public DistressObjectPool distressPool; // For improved performance

    float duration = 5f;

    // Start is called before the first frame update
    void Start()
    {
        distressPool = FindObjectOfType<DistressObjectPool>();
        StartCoroutine(DeleteMe());
    }

    IEnumerator DeleteMe() {
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false); // Deactivate the enemy
        distressPool.ReturnToPool(gameObject); // Return it to the pool
    }
}
