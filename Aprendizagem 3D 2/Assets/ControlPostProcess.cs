using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ControlPostProcess : MonoBehaviour
{
    private PostProcessVolume postProcessVolume;
    private GameManager gameManager;

    [Header("Time Post Process Values")]
    [SerializeField] private float intervalTime;
    [SerializeField] private int numberToRepeat;

    [Header("Weight Post Process Values")]
    [SerializeField] private float initialValue, endValue, midValue;    //1, 0, 0.4

    [Header("Other Values")]
    [SerializeField] private int action;
    [SerializeField] private int sceneToLoad;

    private void Awake()
    {
        postProcessVolume = GetComponent<PostProcessVolume>();
        gameManager = GetComponentInParent<GameManager>();
    }

    private void OnEnable()
    {
        StartCoroutine(PostProcessAnimation(intervalTime, numberToRepeat));
    }

    private IEnumerator PostProcessAnimation(float durationTime, int repeatCount)
    {
        for (int count = 0; count < repeatCount; count++)
        {
            for (float t = 0f; t < durationTime; t += Time.deltaTime)
            {
                float normalizedTime = t / durationTime;
                postProcessVolume.weight = Mathf.Lerp(initialValue, midValue, normalizedTime);
                yield return null;
            }
            postProcessVolume.weight = midValue;

            yield return new WaitForSeconds(0.1f);

            for (float t = 0f; t < durationTime; t += Time.deltaTime)
            {
                float normalizedTime = t / durationTime;
                postProcessVolume.weight = Mathf.Lerp(midValue, initialValue, normalizedTime);
                yield return null;
            }
            postProcessVolume.weight = initialValue;
        }
        
        for (float t = 0f; t < durationTime; t += Time.deltaTime)
        {
            float normalizedTime = t / durationTime;
            postProcessVolume.weight = Mathf.Lerp(initialValue, endValue, normalizedTime);
            yield return null;
        }
        postProcessVolume.weight = endValue;

        if (action > 0) ExecuteAnAction(action);
        if(action != 1) this.gameObject.SetActive(false);
    }

    private void ExecuteAnAction(int action)
    {
        switch(action)
        {
            case 1:
                gameManager.LoadScene(sceneToLoad);
                break;
        }

    }
}


