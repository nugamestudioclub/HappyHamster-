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
    private Collider2D _collider;
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private Transform _pointer;
    [SerializeField]
    Overheatting overheat;
    private ComboMultiplier cm;
    [SerializeField]
    private GameObject _visualFlame;

    //skylar, adding flamethrower sounds
    private FMOD.Studio.EventInstance instance;
    public FMODUnity.EventReference flamethrowerFiringEvent;

    void Start()
    {
        _collider = this.GetComponent<Collider2D>();
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
        _spriteRenderer.enabled = false;
        _visualFlame.SetActive(false);
        cm = this.GetComponent<ComboMultiplier>();

        instance = FMODUnity.RuntimeManager.CreateInstance(flamethrowerFiringEvent);
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
                    cm.killedEnemy();
                }
            }
        }
    }
    void Fire()
    {
        _firing = true;
        _spriteRenderer.enabled = true;
        _visualFlame.SetActive(true);
        instance.start();
    }

    void Cease()
    {
        _firing = false;
        _spriteRenderer.enabled = false;
        _visualFlame.SetActive(false);
        instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    public void OnGameOver()
    {
        instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (_firing && collision.gameObject.tag.Equals("Enemy"))
        {
            collision.GetComponent<Enemy>().Kill();
            cm.killedEnemy();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, hitboxSize);
    }
}