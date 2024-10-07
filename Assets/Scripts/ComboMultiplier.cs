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
    [HideInInspector]
    public float _scoreMult = 1;
    private int _nextKillThreshhold;

    private bool _onKillingSpree;
    private float _timeSpreeStarted;
    private float _currentTime = 0.0f;

    private FMOD.Studio.EventInstance announceInstance;
    public FMODUnity.EventReference announceEvent;

    void Start() {

        announceInstance = FMODUnity.RuntimeManager.CreateInstance(announceEvent);
        announceInstance.setParameterByName("Combo Picker", 1);
    }

    void Update()
    {
        _currentTime += Time.deltaTime;

        if (_onKillingSpree && (_currentTime - _timeSpreeStarted) >= _comboTime && _enemiesKilled < _nextKillThreshhold)
        {
            dropTheCombo();
        }
    }

    public void killedEnemy()
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
        Debug.Log("among us");
        _nextKillThreshhold *= _threshRate;
        _timeSpreeStarted = _currentTime;

        //hi skylar, this is where the audio goes
        if (_scoreMult == 2.0f)
        {
            announceInstance.setParameterByName("Combo Picker", 1);
        } else if (_scoreMult == 3.0f)
        { 
            announceInstance.setParameterByName("Combo Picker", 2);
        } else if (_scoreMult == 4.0f)
        {
            announceInstance.setParameterByName("Combo Picker", 3);
        } else if ( _scoreMult == 5.0f) 
        { 
            announceInstance.setParameterByName("Combo Picker", 4);
        } else
        {
            announceInstance.setParameterByName("Combo Picker", 5);
        }
        announceInstance.start();
    }

    void dropTheCombo()
    {
        _onKillingSpree = false;
        _scoreMult = 1;
        _enemiesKilled = 0;
        _nextKillThreshhold = _initialThresh;
    }
}
