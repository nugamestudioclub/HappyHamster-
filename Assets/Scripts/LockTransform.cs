using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockTransform : MonoBehaviour
{
    [SerializeField]
    private Transform slider;
    private Vector3 startScale;
    // Start is called before the first frame update
    void Start()
    {       
        startScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = slider.position;
        this.transform.localScale = startScale;
    }
}
