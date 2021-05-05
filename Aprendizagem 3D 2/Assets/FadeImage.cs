using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeImage : MonoBehaviour
{
    [Header("Duration of fade")]
    [SerializeField] private float durationTime;

    [Header("Color fade")]
    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;

    private Image imagem;
    private Menu menu;

    private void Awake()
    {
        imagem = GetComponent<Image>();
        menu = GetComponent<Menu>();

        imagem.color = startColor;
    }

    private void Start()
    {
        StartCoroutine(Fade(durationTime));
    }

    private IEnumerator Fade(float tempoFade)
    {
        for (float t = 0f; t < tempoFade; t += Time.deltaTime)
        {
            float normalizedTime = t / tempoFade;
            imagem.color = Color.Lerp(startColor, endColor, normalizedTime);
            yield return null;
        }
        imagem.color = endColor;
    }
}
