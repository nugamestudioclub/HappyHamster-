using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    [SerializeField]
    private float _maxFuel;
    [SerializeField]
    private float _fuelSpendRate;
    [SerializeField]
    private float _fuelRefillRate;

    private float _currentFuel;
    private bool _firing;
    private Collider2D _collider;
    private SpriteRenderer _spriteRenderer;
    void Start()
    {
        _firing = false;
        _collider = this.GetComponent<Collider2D>();
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
        _currentFuel = _maxFuel;
    }

    void Update()
    {
        if (_firing)
        {
            _currentFuel -= _fuelSpendRate * Time.deltaTime;
            if (_currentFuel <= 0)
            {
                Cease();
            }
        } else if (_currentFuel < _maxFuel){
            _currentFuel += _fuelRefillRate * Time.deltaTime;
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
}