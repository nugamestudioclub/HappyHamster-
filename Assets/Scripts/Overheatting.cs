using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField]
    private float startCooldownQTE = .5f;
    [SerializeField]
    private float cooldownTimeQTE = .5f;
    [SerializeField]
    private TMP_Text coolDownQTEText;
    private float maxCooldown = 5f;
    private float curCooldown = 0;
    private float curHeat = 0f;
    public bool isOverheated = false;
    private bool hasHitQTE = false;
    private float cooldownRate = 1f;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOverheated)
        {
            hasHitQTE = false;
            Firing();
        }
        else
        {
            Cooldown();    
        }
    }

    public void Cooldown() {
        overheatSlider.color = cooldownColor;
        float cooldownDif = maxCooldown - curCooldown;

        bool inQTETime = (cooldownDif > startCooldownQTE)  && (cooldownDif < startCooldownQTE + cooldownTimeQTE);
        if (inQTETime && Input.GetMouseButtonDown(1) && !hasHitQTE)
        {
            
            if (!hasHitQTE) 
            {
                //GUST THINGS
                cooldownRate = 3f;
                hasHitQTE = true;
            }
        }
        
        coolDownQTEText.gameObject.SetActive(inQTETime);
        
        curCooldown = Mathf.Max(curCooldown - Time.deltaTime * cooldownRate, 0f);
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
            cooldownRate = 1f;
            curHeat = 0;
        }
    }


}
