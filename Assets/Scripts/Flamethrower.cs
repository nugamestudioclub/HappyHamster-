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
    private float _overheatEventDuration;
    [SerializeField]
    private float _quickCoolStart;
    [SerializeField]
    private float _quickCoolEnd;

    private float _currentHeat;
    private bool _firing;
    private bool _cooling;
    private Collider2D _collider;
    private SpriteRenderer _spriteRenderer;
    void Start()
    {
        _firing = false;
        _cooling = false;
        _collider = this.GetComponent<Collider2D>();
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
        _currentHeat = 0f;
    }

    void Update()
    {
        if (Input.GetAxisRaw("Fire1") > 0 && !_firing)
        {
            Fire();
        } else if (_firing) {
            Cease();
        }

        if (_firing)
        {
            _currentHeat += _heatRate * Time.deltaTime;
            if (_currentHeat >= _maxHeat)
            {
                Overheat();
            }
        } else if (_currentHeat > 0 && _cooling){
            _currentHeat -= _cooldownRate * Time.deltaTime;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_firing && collision.gameObject.tag.Equals("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().Kill();
        }
    }

    void Overheat()
    {
        Cease();

    }

}