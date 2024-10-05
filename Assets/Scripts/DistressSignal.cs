using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistressSignal : MonoBehaviour
{

    float duration = 5f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DeleteMe());
    }

    IEnumerator DeleteMe() {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
