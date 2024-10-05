using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ComboMultiplier : MonoBehaviour
{
    [SerializeField]
    private float _comboTime = 10;
    [SerializeField]
    private ComboValues _comboValues;

    [HideInInspector]
    public float score;


    private int _enemiesKilled;
    private float _scoreMult;
    private int _nextKillThreshhold;

    private bool _onKillingSpree;
    private float _timeSpreeStarted;
    private float _timer;

    private void Start()
    {
        _timer = 0.0f;
        _scoreMult = 1.0f;
        _enemiesKilled = 0;
    }

    void Update()
    {
        _timer += Time.deltaTime;

        if (_onKillingSpree && (_timer - _timeSpreeStarted) >= _comboTime && _enemiesKilled < _nextKillThreshhold)
        {
            _onKillingSpree = false;
            _scoreMult = 1.0f;
            _enemiesKilled = 0;
            _nextKillThreshhold = _comboValues.threshholds[1];
        }
    }

    void killedEnemy()
    {
        _enemiesKilled += 1;

        if (!_onKillingSpree)
        {
            _onKillingSpree = true;
            _scoreMult = _comboValues.mults[0];
            _nextKillThreshhold = _comboValues.threshholds[0];
            _timeSpreeStarted = _timer;
        } 
        else if (_onKillingSpree && (_enemiesKilled >= _nextKillThreshhold))
        {
            if (_comboValues.mults.IndexOf(_scoreMult) + 1 <= _comboValues.mults.Count)
            {
                _scoreMult = _comboValues.mults[_comboValues.mults.IndexOf(_scoreMult) + 1];
                _nextKillThreshhold = _comboValues.threshholds[_comboValues.threshholds.IndexOf(_nextKillThreshhold) + 1];
                _timeSpreeStarted = _timer;
            }
        }

        score += _scoreMult;
    }
}
