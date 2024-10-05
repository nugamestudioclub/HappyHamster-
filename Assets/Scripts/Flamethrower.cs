using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Flamethrower : MonoBehaviour
{

    [SerializeField]
    private Vector2 hitboxCenter;
    [SerializeField] 
    private Vector2 hitboxSize;

    private bool _firing = false;
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private Transform _pointer;
    [SerializeField]
    Overheatting overheat;

    void Start()
    {
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
        _spriteRenderer.enabled = false;
    }

    void Update()
    {
        if (LevelManager.isGameOver) { return; }
        if (Input.GetAxisRaw("Fire1") != 0 && !overheat.isOverheated && !_firing)
        {
            Fire();
        }

        if (Input.GetAxisRaw("Fire1") == 0 || overheat.isOverheated)
        {
            Cease();
        }

        if (_firing)
        {
            foreach (Collider2D obj in Physics2D.OverlapBoxAll(transform.position, hitboxSize, _pointer.rotation.z))
            {
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


    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, hitboxSize);
    }
}