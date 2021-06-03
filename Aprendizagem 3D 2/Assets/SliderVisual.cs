using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderVisual : MonoBehaviour
{
    private Slider slider;
    private Text numberPercentage;

    [SerializeField] private string keyString;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        numberPercentage = GetComponentInChildren<Text>();

        if (keyString == "mouseSensibility")
        {
            slider.minValue = 0.1f;
            slider.maxValue = 10;
        }
    }

    private void Start()
    {
        slider.value = PlayerPrefs.GetFloat(keyString);
        UpdatePercentageText();
    }

    public void UpdatePercentageText()
    {
        if (keyString == "mouseSensibility") numberPercentage.text = Mathf.RoundToInt(slider.value * 10) + "%";
        else numberPercentage.text = Mathf.RoundToInt(slider.value * 100) + "%";
    }
}
