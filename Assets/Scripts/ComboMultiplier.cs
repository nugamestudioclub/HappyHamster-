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


    private int _enemiesKilled = 0;
    private float _scoreMult;
    private int _nextKillThreshhold;
    private int _comboValIndex = 0;

    private bool _onKillingSpree;
    private float _timeSpreeStarted;
    private float _currentTime = 0.0f;

    void Update()
    {
        _currentTime += Time.deltaTime;

        if (_onKillingSpree && (_currentTime - _timeSpreeStarted) >= _comboTime && _enemiesKilled < _nextKillThreshhold)
        {
            startTheCombo();
        }
    }

    void killedEnemy()
    {
        _enemiesKilled += 1;

        if (!_onKillingSpree)
        {
            startTheCombo();
        } 
        else if (_onKillingSpree && (_enemiesKilled >= _nextKillThreshhold))
        {
            if (_comboValIndex <= _comboValues.mults.Count - 1)
            {
                increaseTheCombo();
            }
        }

        score += _scoreMult;
    }

    void startTheCombo()
    {
        _onKillingSpree = true;
        _scoreMult = 1.0f;
        _nextKillThreshhold = _comboValues.threshholds[0];
        _timeSpreeStarted = _currentTime;

    }

    void increaseTheCombo()
    {
        _comboValIndex++;
        _scoreMult = _comboValues.mults[_comboValIndex];
        _nextKillThreshhold = _comboValues.threshholds [_comboValIndex];
        _timeSpreeStarted = _currentTime;
    }

    void dropTheCombo()
    {
        _onKillingSpree = false;
        _comboValIndex = 0;
        _scoreMult = _comboValues.mults[0];
        _enemiesKilled = 0;
        _nextKillThreshhold = _comboValues.threshholds[0];
    }
}
