using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Overheatting : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private Image overheatSlider;
    [SerializeField]
    private Color heatUpColor;
    [SerializeField]
    private Color cooldownColor;
    private float maxCooldown = 5f;
    private float curCooldown = 0;
    private float curHeat = 0f;
    public bool isOverheated = false;



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(isOverheated);
        if (!isOverheated)
        {
            Firing();
        }
        else
        {
            Cooldown();    
        }
    }

    public void Cooldown() {
        overheatSlider.color = cooldownColor;
        curCooldown = Mathf.Max(curCooldown - Time.deltaTime, 0f);
        if (curCooldown == 0f) {
            isOverheated = false;
        }
        slider.value = curCooldown / maxCooldown;
    }

    private void Firing() {
        overheatSlider.color = heatUpColor;
        if (Input.GetMouseButton(0))
        {
            float rate = Time.deltaTime * (curHeat + .1f);
            curHeat = Mathf.Min(curHeat + rate, 1);
        }
        else
        {
            curHeat = Mathf.Max(curHeat - Time.deltaTime / 5, 0);
        }

        slider.value = curHeat;

        if (curHeat >= 1)
        {
            isOverheated = true;
            curCooldown = maxCooldown;
            curHeat = 0;
        }
    }


}
