using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ComboMultiplier : MonoBehaviour
{
    [SerializeField]
    private float _comboTime = 10;
    [SerializeField]
    private int _initialThresh = 10;
    [SerializeField]
    private int _threshRate = 2;

    [HideInInspector]
    public float score;


    private int _enemiesKilled = 0;
    private float _scoreMult = 1;
    private int _nextKillThreshhold;

    private bool _onKillingSpree;
    private float _timeSpreeStarted;
    private float _currentTime = 0.0f;

    void Update()
    {
        _currentTime += Time.deltaTime;

        if (_onKillingSpree && (_currentTime - _timeSpreeStarted) >= _comboTime && _enemiesKilled < _nextKillThreshhold)
        {
            dropTheCombo();
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
            increaseTheCombo();
        }

        score += _scoreMult;
    }

    void startTheCombo()
    {
        _onKillingSpree = true;
        _scoreMult = 1.0f;
        _nextKillThreshhold = _initialThresh;
        _timeSpreeStarted = _currentTime;

    }

    void increaseTheCombo()
    {
        _scoreMult++;
        _nextKillThreshhold *= _threshRate;
        _timeSpreeStarted = _currentTime;
    }

    void dropTheCombo()
    {
        _onKillingSpree = false;
        _scoreMult = 1;
        _enemiesKilled = 0;
        _nextKillThreshhold = _initialThresh;
    }
}
