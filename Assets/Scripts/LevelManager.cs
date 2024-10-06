using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private int maxHamsters;
    [SerializeField]
    private int currentHamsters;
    [SerializeField]
    private float maxGracePeriod = 5f;
    private float curTime = 1f;
    private bool isInGrace;
    [SerializeField]
    private GameObject endScreen;
    [SerializeField]
    private GameObject slider;
    [SerializeField]
    private Image sliderFill;
    [SerializeField]
    private TMP_Text timer;
    [SerializeField]
    private Slider enemiesOnScreen;
    public static bool isGameOver = false;

    private float _elapsedTime = 0f;
    private bool _doRestartGame = false;



    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (_doRestartGame) {
            currentHamsters = 0;
            isGameOver = false;
            _doRestartGame = false;
            Enemy.enemyCount = 0;
            _elapsedTime = 0f;
            SceneManager.LoadScene("spawner_test");
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("spawner_test"));
            return;
        }
        currentHamsters = Enemy.enemyCount;
        if (curTime <= 0)
        {
            FinishGame();
            return;
        }
        _elapsedTime += Time.deltaTime*2;
        if (maxHamsters <= currentHamsters && isInGrace)
        {
            curTime -= Time.deltaTime;
            timer.text = curTime.ToString("0.00");
            // Flash the slider and turn it red
            sliderFill.color = Color.red;
            if (Mathf.Floor(_elapsedTime) != Mathf.Floor(_elapsedTime - Time.deltaTime))
            {
                sliderFill.gameObject.SetActive(!sliderFill.gameObject.activeSelf);
            }
        }
        else if (maxHamsters <= currentHamsters)
        {
            // timer.gameObject.SetActive(true);
            isInGrace = true;
            curTime = maxGracePeriod;
        }
        else
        {
            timer.gameObject.SetActive(false);
            curTime = maxGracePeriod;
            isInGrace = false;
            sliderFill.color = Color.Lerp(Color.white, Color.yellow, enemiesOnScreen.value);
        }
        enemiesOnScreen.value = Mathf.Min(currentHamsters / (float)maxHamsters, 1);
    }

    void FinishGame()
    {
        isGameOver = true;
        endScreen.SetActive(true);
        timer.gameObject.SetActive(false);
        slider.gameObject.SetActive(false);
        Time.timeScale = 0f;
    }


    public void OnFinishClick()
    {
        Debug.Log("Finish Clicked");
        Time.timeScale = 1f;
        _doRestartGame = true;
    }
}
