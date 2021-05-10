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

    [Header("Has another fade?")]
    [SerializeField] private bool hasNextFade;
    [SerializeField] private float intervalTime;

    private Image imagem;
    private GameManager gameManager;

    private void Awake()
    {
        imagem = GetComponent<Image>();
        gameManager = GetComponent<GameManager>();

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

        if(hasNextFade)
        {
            yield return new WaitForSeconds(intervalTime);
            for (float t = 0f; t < tempoFade; t += Time.deltaTime)
            {
                float normalizedTime = t / tempoFade;
                imagem.color = Color.Lerp(endColor, startColor, normalizedTime);
                yield return null;
            }
            imagem.color = startColor;
            gameManager.LoadScene(0);
        }
    }
}
