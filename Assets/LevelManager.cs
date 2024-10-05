using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private int maxHamsers = 500;
    private int currentHamsters;
    [SerializeField]
    private float maxGracePeriod = 5f;
    private float curTime;
    [SerializeField]
    Slider enemiesOnScreen;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentHamsters = Enemy.enemyCount;
        if (maxHamsers >= currentHamsters)
        {
            curTime += Time.deltaTime;
        }
        else { curTime = 0f; }
        if (curTime >= maxGracePeriod) 
        {
            //LOSING
        }
        enemiesOnScreen.value = Mathf.Max(1 - (currentHamsters / maxHamsers), 0);
    }
}
