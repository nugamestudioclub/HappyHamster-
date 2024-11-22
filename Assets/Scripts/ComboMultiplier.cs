using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComboMultiplier : MonoBehaviour
{

    [SerializeField] private Slider nextComboSlider;
    [SerializeField] private TextMeshProUGUI scoreText;
    
    [SerializeField]
    private GameObject textObj;
    private DisplayAnnouncement announcementText;


    [SerializeField]
    private float _comboTime = 10;
    [SerializeField]
    private int _initialThresh = 10;
    [SerializeField]
    private float _threshRate = 2;

    [HideInInspector]
    public int score;


    private int _enemiesKilled = 0;
    [HideInInspector]
    public int _scoreMult = 1;
    private int _nextKillThreshhold;

    private bool _onKillingSpree;
    private float _timeSpreeStarted;
    private float _currentTime = 0.0f;

    private FMOD.Studio.EventInstance announceInstance;
    public FMODUnity.EventReference announceEvent;

    void Start() {

        announceInstance = FMODUnity.RuntimeManager.CreateInstance(announceEvent);
        announceInstance.setParameterByName("Combo Picker", 1);

        announcementText = textObj.GetComponent<DisplayAnnouncement>();
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

        nextComboSlider.value = (float)_enemiesKilled / _nextKillThreshhold;
        scoreText.text = score.ToString("n0");
    }

    void startTheCombo()
    {
        _onKillingSpree = true;
        _scoreMult = 1;
        _nextKillThreshhold = _initialThresh;
        _timeSpreeStarted = _currentTime;

    }

    void increaseTheCombo()
    {
        _scoreMult++;
        _nextKillThreshhold = (int)(_nextKillThreshhold * _threshRate);
        _timeSpreeStarted = _currentTime;

        if (_scoreMult == 2)
        {
            announceInstance.setParameterByName("Combo Picker", 1);
            announcementText.DisplayText(1);
        } else if (_scoreMult == 3)
        { 
            announceInstance.setParameterByName("Combo Picker", 2);
            announcementText.DisplayText(2);
        }
        else if (_scoreMult == 4)
        {
            announceInstance.setParameterByName("Combo Picker", 3);
            announcementText.DisplayText(3);
        }
        else if ( _scoreMult == 5) 
        { 
            announceInstance.setParameterByName("Combo Picker", 4);
            announcementText.DisplayText(4);
        }
        else
        {
            announceInstance.setParameterByName("Combo Picker", 5);
            announcementText.DisplayText(5);
        }
        announceInstance.start();
    }

    void dropTheCombo()
    {
        nextComboSlider.value = 0;
        _onKillingSpree = false;
        _scoreMult = 1;
        _enemiesKilled = 0;
        _nextKillThreshhold = _initialThresh;
    }
}
