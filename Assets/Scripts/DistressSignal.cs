using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistressSignal : MonoBehaviour
{
    public DistressObjectPool distressPool; // For improved performance

    float duration = 5f;

    void Awake() // cannot use Start() because OnEnable is called before it
    {
        distressPool = FindObjectOfType<DistressObjectPool>();
    }

    void OnEnable() 
    {
        distressPool = FindObjectOfType<DistressObjectPool>();
        StartCoroutine(distressPool.ReturnToPoolAfterDelay(gameObject, duration));
    }

    // void OnDisable() 
    // {
    //     distressPool.ReturnToPool(gameObject)
    // }

    // Start is called before the first frame update
    // void Start()
    // {
    //     distressPool = FindObjectOfType<DistressObjectPool>();
    //     StartCoroutine(DeleteMe());
    // }

    // IEnumerator DeleteMe() {
    //     yield return new WaitForSeconds(duration);
    //     //gameObject.transform.position = new Vector3(0,0,1); // no clue how to toggle visibility of ash
    //     //gameObject.SetActive(false); // Deactivate the enemy
    //     distressPool.ReturnToPool(gameObject); // Return it to the pool
    // }
}
