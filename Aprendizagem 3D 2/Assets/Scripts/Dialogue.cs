using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    [Header("General Text")]
    [SerializeField] private string characterName;
    [SerializeField] private string[] speechs;
    
    [Tooltip("This dialogue will be displayed only once?")]
    [SerializeField] private bool onlyOnce;
    private bool alreadyExecuted;

    [Header("Has a continuation dialogue?")]
    [SerializeField] private GameObject nextDialogue;
    private Dialogue nextDialogueScript;

    [Header("Dialogue Box UI")]
    [SerializeField] GameObject dialogueBox;
    private Text characterNameUI;
    private Text dialogueTextUI;

    [Header("Other Continuations")]
    [SerializeField] private GameObject postProcessEffect;

    private void Awake()
    {
        dialogueTextUI = dialogueBox.transform.GetChild(1).GetComponent<Text>();
        characterNameUI = dialogueBox.transform.GetChild(2).GetComponent<Text>();      
        alreadyExecuted = false;

        if (nextDialogue != null) { nextDialogueScript = nextDialogue.GetComponent<Dialogue>(); }
    }

    public IEnumerator Speech()
    {
        characterNameUI.text = characterName;

        if (!alreadyExecuted)
        {
            dialogueBox.SetActive(true);

            for (int i = 0; i < speechs.Length; i++)
            {
                dialogueTextUI.text = speechs[i];
                yield return new WaitForSeconds(CalculateSpeechTime(speechs[i]));
            }

            if (onlyOnce) alreadyExecuted = true;
        }

        dialogueBox.SetActive(false);

        if (nextDialogueScript != null) { nextDialogueScript.RunCoroutine(); }
        if(postProcessEffect != null) { postProcessEffect.SetActive(true); }
    }

    public void RunCoroutine(){ StartCoroutine(Speech()); }

    private float CalculateSpeechTime(string speechTotalLetters)
    {
        float totalTime = 0;
        foreach(char letters in speechTotalLetters)
        {
            if(letters != ' ')totalTime += 0.1f; 
        }
        if (totalTime < 1f) totalTime = 2f;
        return totalTime;
    }
}
