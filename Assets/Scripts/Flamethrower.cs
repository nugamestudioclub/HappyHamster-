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
    private float _currentHeat = 0f;
    private float _overheatTimer;
    [SerializeField]
    private bool _firing = false;
    [SerializeField]
    private bool _reloading = false;
    [SerializeField]
    private bool _slowCooling = false;
    [SerializeField]
    private bool _normalCooling = false;
    [SerializeField]
    private bool _fastCooling = false;
    private Collider2D _collider;
    private SpriteRenderer _spriteRenderer;

    void Start()
    {
        _collider = this.GetComponent<Collider2D>();
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
        _spriteRenderer.enabled = false;
    }

    void Update()
    {
        if (Input.GetAxisRaw("Fire1") != 0 && !_firing && !(_slowCooling || _fastCooling))
        {
            _normalCooling = false;
            Fire();
        }
        else if (Input.GetAxisRaw("Fire1") == 0 && _firing)
        {
            Cease();
            _normalCooling = true;
        }

        if (_firing)
        {
            _currentHeat += _heatRate * Time.deltaTime;
            if (_currentHeat >= _maxHeat)
            {
                Overheat();
            }

            if (_reloading)
            {
                _overheatTimer -= Time.deltaTime;

                if (_overheatTimer <= 0)
                {
                    _reloading = false;
                }

                if (Input.GetAxisRaw("Fire2") != 0)
                {
                    playerReload(_overheatEventDuration - _overheatTimer);
                }
            }

            foreach (Collider obj in Physics.OverlapCapsule(Vector3.zero, Vector3.right, 0.5f))
            {
                Enemy enemy = obj.GameObject().GetComponent<Enemy>();
                if (enemy != null) 
                {
                    enemy.Kill();
                }
            }
        }

        if (_normalCooling)
        {
            _currentHeat -= _cooldownRate * Time.deltaTime;

        }

        if (_fastCooling)
        {
            _currentHeat -= _fastCoolRate * Time.deltaTime;
        }

        if (_slowCooling)
        {
            _currentHeat -= _slowCoolRate * Time.deltaTime;
        }

        if (_currentHeat <= 0)
        {
            _normalCooling = false;
            _fastCooling = false;
            _slowCooling = false;
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

        void Overheat()
        {
            Cease();
            _overheatTimer = _overheatEventDuration;
            _reloading = true;
        }

        void playerReload(float timeReloadPressed)
        {
            _reloading = false;
            if (timeReloadPressed > _quickCoolStart && timeReloadPressed < _quickCoolEnd)
            {
                _fastCooling = true;
            }
            else
            {
                _slowCooling = true;
            }
        }

    }
}