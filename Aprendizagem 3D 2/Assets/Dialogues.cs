using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogues : MonoBehaviour
{
    [SerializeField] public string[] speechs;
    [SerializeField] Text dialogueText;
    [SerializeField] private bool onlyOnce;
    private bool check;

    private void Awake()
    {
        check = true;
    }

    public IEnumerator Speech()
    {
        if (check)
        {
            dialogueText.gameObject.SetActive(true);

            for (int i = 0; i < speechs.Length; i++)
            {
                dialogueText.text = speechs[i];
                yield return new WaitForSeconds(CalculateSpeechTime(speechs[i]));
            }

            if (onlyOnce) check = false;
        }

        dialogueText.gameObject.SetActive(false);
    }

    public void RunCoroutine(){ StartCoroutine(Speech()); }

    private float CalculateSpeechTime(string speechTotalLetters)
    {
        float totalTime = 0;
        foreach(char letters in speechTotalLetters){ totalTime += 0.1f; }
        return totalTime;
    }
}
