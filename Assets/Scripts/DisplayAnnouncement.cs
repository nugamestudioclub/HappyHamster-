using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DisplayAnnouncement : MonoBehaviour
{
    public enum TextStates
    {
        MULTIKILL,
        BLOODBATH,
        MASSACRE,
        DEARGOD,
        RANDOM
    }

    public float maxSize = 5f;
    public int times = 15;

    private Array enumValues;
    private TMPro.TextMeshProUGUI textObj;
    

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start ran");

        enumValues = Enum.GetValues(typeof(TextStates));
        textObj = GetComponent<TMPro.TextMeshProUGUI>();

        Debug.Log(textObj);

        textObj.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void DisplayText(int comboIndex)
    {
        Debug.Log("display ran");

        TextStates chosenState = (TextStates)enumValues.GetValue(comboIndex-1);


        if (chosenState == TextStates.RANDOM)
        {
            textObj.text = "???";
        } else
        {
            string text = chosenState.ToString();

            textObj.text = text;
        }

        textObj.color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);

        textObj.gameObject.SetActive(true);

        StartCoroutine(PlayAnimation());

    }


    IEnumerator PlayAnimation()
    {
        float duration = 1.5f;
        
        float timeElapsed = 0f; // Time counter

        // Animate until the duration is completed
        while (timeElapsed < duration)
        {
            // Calculate the percentage of completion
            timeElapsed += Time.deltaTime;
            float t = Mathf.Clamp01(timeElapsed / duration) * (maxSize * 10);


            float size = Mathf.PingPong(t, maxSize) + 0.5f;


            transform.localScale = new Vector2(size, size);

            yield return null; // Wait for the next frame
        }

        textObj.gameObject.SetActive(false);
    }
}
