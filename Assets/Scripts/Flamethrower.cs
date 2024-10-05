using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    private bool _firing;
    private Collider2D _collider;
    private SpriteRenderer _spriteRenderer;
    void Start()
    {
        _firing = false;
        _collider = this.GetComponent<Collider2D>();
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        
    }

    void Fire()
    {
        _firing = true;
        _spriteRenderer.enabled = true;
    }

    void Cease()
    {
        _firing = false;
        _spriteRenderer.enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_firing && collision.gameObject.tag == "Enemy")
        {
            //replace with call to kill enemy
            Debug.Log("hit an enemy");
        }
    }
}