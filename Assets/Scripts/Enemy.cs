using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Move away from player
        Vector2 direction = transform.position - player.transform.position;
        direction.Normalize();
        transform.position += new Vector3(direction.x, direction.y, 0) * Time.deltaTime;
    }

    public void Kill()
    {
        // TODO: Death vfx and animations
        Destroy(gameObject);
    }
}
