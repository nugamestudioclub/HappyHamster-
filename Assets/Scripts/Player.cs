using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed = 100f;
    public Rigidbody2D rigidBody;

    private void Start()
    {
        
    }

    void Update()
    {
        // Get the horizontal and vertical axis.
        // The input map have "Horizontal" and "Vertical" as default.
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector2 movement = new Vector2(horizontal, vertical);
        movement.Normalize();
        rigidBody.velocity = movement * speed;
    }

}
