using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    [SerializeField]
    private float _maxHeat;
    [SerializeField]
    private float _heatRate;
    [SerializeField]
    private float _cooldownRate;
    [SerializeField]
    private float _fastCoolRate;
    [SerializeField]
    private float _slowCoolRate;

    [SerializeField]
    private float _overheatEventDuration;
    [SerializeField]
    private float _quickCoolStart;
    [SerializeField]
    private float _quickCoolEnd;

    [SerializeField]
    private Vector2 hitboxCenter;
    [SerializeField] 
    private Vector2 hitboxSize;


    [SerializeField]
    private float _currentHeat = 0f;
    private float _overheatTimer;
    [SerializeField]
    private bool _firing = false;
    private Collider2D _collider;
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private Transform _pointer;
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

            foreach (Collider2D obj in Physics2D.OverlapBoxAll(hitboxCenter, hitboxSize, _pointer.rotation.z))
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