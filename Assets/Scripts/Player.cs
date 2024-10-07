using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed = 100f;
    public Rigidbody2D rigidBody;
    public Camera camera;
    private float _minCameraY;
    private float _maxCameraY;

    private void Start()
    {
        _maxCameraY = 60/2 - camera.orthographicSize; // half the map size (this is so hardcoded)
        _minCameraY = -60/2 + camera.orthographicSize;
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
        camera.transform.position = new Vector3(0, Mathf.Clamp(transform.position.y, _minCameraY, _maxCameraY), -10);
    }

}
