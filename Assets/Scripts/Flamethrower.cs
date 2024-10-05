using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    [SerializeField]
    private bool _firing = false;
    private Collider2D _collider;
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    Overheatting overheat;

    void Start()
    {
        _collider = this.GetComponent<Collider2D>();
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
        _spriteRenderer.enabled = false;
    }

    void Update()
    {
        if (Input.GetAxisRaw("Fire1") != 0 && !overheat.isOverheated)
        {
            Fire();
        }
        else 
        {
            Cease();
        }

        if (_firing)
        {

            Debug.Log(Physics.OverlapCapsule(Vector3.zero, Vector3.right, 0.5f).Length);
            foreach (Collider obj in Physics.OverlapCapsule(Vector3.zero, Vector3.right, 0.5f))
            {
                Debug.Log("Hit");
                Enemy enemy = obj.gameObject.GetComponent<Enemy>();
                if (enemy != null) 
                {
                    enemy.Kill();
                }
            }
        }
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (_firing && collision.gameObject.tag.Equals("Enemy"))
        {
            collision.GetComponent<Enemy>().Kill();
        }
    }
}