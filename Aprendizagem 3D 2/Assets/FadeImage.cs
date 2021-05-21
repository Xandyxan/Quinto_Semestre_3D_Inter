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

    [SerializeField] bool loadScene;
    [SerializeField] int sceneIndex;

    bool fadeIn;
    private void Awake()
    {
        imagem = GetComponent<Image>();
        imagem.color = startColor;
    }

    private void Start()
    {
        StartCoroutine(Fade(3f));
    }

    public IEnumerator Fade(float tempoFade) // se for true, faz um fade in / se for falso, faz um fade out
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
           
           
        }
        if (loadScene) { GameManager.instance.LoadScene(sceneIndex); }
    }

    // Setters
    public void SetHasNextFade(bool value) { hasNextFade = value; }
    public void SetIntervalTime(float interval) { interval = intervalTime; }
    public void SetHasSceneLoad(bool value) { loadScene = value; }
    public void SetSceneIndex(int index) { sceneIndex = index; }

    public void SetFadeIn(bool value) 
    { 
        fadeIn = value;
        if (fadeIn)
        {
            startColor = new Color(imagem.color.r, imagem.color.g, imagem.color.b, 0f);
            endColor = new Color(imagem.color.r, imagem.color.g, imagem.color.b, 255f);
            imagem.color = startColor;
        }
        else
        {
            startColor = new Color(imagem.color.r, imagem.color.g, imagem.color.b, 255f);
            endColor = new Color(imagem.color.r, imagem.color.g, imagem.color.b, 0f);
            imagem.color = startColor;
        }
    }
   
}
