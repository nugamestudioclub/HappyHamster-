using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayMultiplier : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    private ComboMultiplier comboMult;
    private TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        comboMult = player.GetComponent<ComboMultiplier>();
        text = this.GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = comboMult._scoreMult + "x";
    }
}
