using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PointToMouse : MonoBehaviour
{
    [SerializeField]
    GameObject player;

    private Transform _transform;
    private Transform _playerTransform;
    private Vector3 _mousePos;
    private float _rad2Deg = Mathf.Rad2Deg;
    private float _angle;
    void Start()
    {
        _transform = this.GetComponent<Transform>();
        _playerTransform = player.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        _angle = Mathf.Atan2(_mousePos.y - _playerTransform.position.y, _mousePos.x - _playerTransform.position.x) * _rad2Deg;

        _transform.rotation = Quaternion.Euler(0, 0, _angle);
    }
}
