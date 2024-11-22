using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Overheatting : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private RectTransform dial;
    [SerializeField]
    private Image dialFace;
    [SerializeField]
    private Color heatUpColor;
    [SerializeField]
    private Color cooldownColor;
    [SerializeField]
    private float startCooldownQTE = .5f;
    [SerializeField]
    private float cooldownTimeQTE = .5f;
    [SerializeField]
    private GameObject coolDownQTEImage;
    private float maxCooldown = 5f;
    private float curCooldown = 0;
    private float curHeat = 0f;
    public bool isOverheated = false;
    private bool hasHitQTE = false;
    private float cooldownRate = 1f;
    private bool cooling = false;

    //Noah here, added these floats for failing vs succeeding the QTE. Not sure if there's redundancy between these and the fields above
    [SerializeField]
    private float baseCoolRate = 1;
    [SerializeField]
    private float fastCoolRate = 3;
    [SerializeField]
    private float slowCoolRate = 0.5f;

    private FMOD.Studio.EventInstance flamethrowerAlarmInstance;
    public FMODUnity.EventReference flamethrowerAlarmEvent;

    private FMOD.Studio.EventInstance qteSuccessInstance;
    public FMODUnity.EventReference qteSuccessEvent;

    void Start()
    {
        flamethrowerAlarmInstance = FMODUnity.RuntimeManager.CreateInstance(flamethrowerAlarmEvent);
        qteSuccessInstance = FMODUnity.RuntimeManager.CreateInstance(qteSuccessEvent);
    }

    void RotateDial(float percentFull) {
        dial.rotation = Quaternion.Euler(0, 0, 260f * (1f-percentFull) - 130f);
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

        if (cooling)
        {
            curCooldown = Mathf.Max(curCooldown - Time.deltaTime * cooldownRate, 0f);
            if (curCooldown == 0f)
            {
                isOverheated = false;
                cooling = false;
            }
            RotateDial(curCooldown / maxCooldown);
            dialFace.color = Color.Lerp(cooldownColor, Color.white, curCooldown / maxCooldown);
        }

    }

    public void Cooldown() {
        dialFace.color = cooldownColor;
        float cooldownDif = maxCooldown - curCooldown;

        if (!cooling)
        {
            flamethrowerAlarmInstance.start();
            cooling = true;
            cooldownRate = baseCoolRate;
        }

        bool inQTETime = (cooldownDif > startCooldownQTE)  && (cooldownDif < startCooldownQTE + cooldownTimeQTE);
        if (Input.GetAxisRaw("Fire2") != 0 && !hasHitQTE && cooldownRate == baseCoolRate)
        {

            if (inQTETime)
            {
                //GUST THINGS
                cooldownRate = fastCoolRate;
                hasHitQTE = true;
                qteSuccessInstance.start();
            } else
            {
                cooldownRate = slowCoolRate;
                hasHitQTE = true;
            }
        }

        if (cooldownRate != slowCoolRate) 
        {
            coolDownQTEImage.SetActive(inQTETime);
        }
    }

    private void Firing() {
        if (Input.GetAxisRaw("Fire1") != 0)
        {
            float rate = Time.deltaTime * (curHeat + .1f);
            curHeat = Mathf.Min(curHeat + rate, 1);
        }
        else
        {
            curHeat = Mathf.Max(curHeat - Time.deltaTime / 5, 0);
        }

        RotateDial(curHeat);
        dialFace.color = Color.Lerp(Color.white, heatUpColor, curHeat);

        if (curHeat >= 1)
        {
            isOverheated = true;
            curCooldown = maxCooldown;
            cooldownRate = 1f;
            curHeat = 0;
        }
    }


}
