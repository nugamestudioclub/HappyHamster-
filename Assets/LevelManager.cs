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
    private int maxHamsters = 500;
    [SerializeField]
    private int currentHamsters;
    [SerializeField]
    private float maxGracePeriod = 5f;
    private float curTime = 1f;
    private bool isInGrace;
    [SerializeField]
    private GameObject endScreen;
    [SerializeField]
    private TMP_Text timer;
    [SerializeField]
    private Slider enemiesOnScreen;
    public static bool isGameOver = false;

    [SerializeField]
    private string mainMenuGame;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (curTime <= 0)
        {
            FinishGame();
            return;
        }
        if (maxHamsters <= currentHamsters && isInGrace)
        {
            curTime -= Time.deltaTime;
            timer.text = curTime.ToString("0.00");
        }
        else if (maxHamsters <= currentHamsters)
        {
            timer.gameObject.SetActive(true);
            isInGrace = true;
            curTime = maxGracePeriod;
        }
        else
        {
            timer.gameObject.SetActive(false);
            curTime = maxGracePeriod;
            isInGrace = false;
        }
        enemiesOnScreen.value = Mathf.Min((currentHamsters / (float)maxHamsters), 1);
    }

    void FinishGame()
    {
        isGameOver = true;
        Time.timeScale = 0;
        endScreen.SetActive(true);
    }


    public void onFinishClick() 
    {
        SceneManager.LoadScene("");
    }
}
