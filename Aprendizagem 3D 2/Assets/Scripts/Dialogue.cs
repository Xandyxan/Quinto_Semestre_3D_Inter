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
    [SerializeField] private GameObject postProcessEffect2;
    public GameObject tutorialCrouch;

    public static bool isSomeDialogueRunning;

    public delegate void PlayerDuringDialogueOn();
    public static event PlayerDuringDialogueOn playerDuringDialogueOn;
    public delegate void PlayerDuringDialogueOff();
    public static event PlayerDuringDialogueOn playerDuringDialogueOff;

    public static Dialogue instance;

    private void Awake()
    {
        instance = this;

        dialogueTextUI = dialogueBox.transform.GetChild(1).GetComponent<Text>();
        characterNameUI = dialogueBox.transform.GetChild(2).GetComponent<Text>();      
        alreadyExecuted = false;

        if (nextDialogue != null) { nextDialogueScript = nextDialogue.GetComponent<Dialogue>(); }
    }

    private void Start()
    {
        
    }

    public IEnumerator Speech()
    {
        isSomeDialogueRunning = true;
        playerDuringDialogueOn();

        if (tutorialCrouch != null) { tutorialCrouch.SetActive(true); }
        characterNameUI.text = characterName;

        if (!alreadyExecuted)
        {
            dialogueBox.SetActive(true);
           
            Cellphone.instance.SetInDialogue(true);

            for (int i = 0; i < speechs.Length; i++)
            {
                dialogueTextUI.text = speechs[i];
                yield return new WaitForSeconds(CalculateSpeechTime(speechs[i]));
                
            }

            if (onlyOnce) alreadyExecuted = true;
            
        }

        dialogueBox.SetActive(false);
        Cellphone.instance.SetInDialogue(false);

        
        if (nextDialogueScript != null) { nextDialogueScript.RunCoroutine(); }
        else if(nextDialogueScript == null) { DelayPlayerDuringDialogueOff(); }


        if(postProcessEffect != null) { postProcessEffect.SetActive(true); }
        if (postProcessEffect2 != null) { postProcessEffect2.SetActive(true); }
        if(tutorialCrouch != null) { tutorialCrouch.SetActive(false); }

        isSomeDialogueRunning = false;
    }

    public void RunCoroutine(){ StartCoroutine(SpeechCoroutine()); }

    public IEnumerator SpeechCoroutine()
    {
        yield return new WaitUntil(DialogueIsRunning);
        StartCoroutine(Speech());
    }

    public bool DialogueIsRunning()
    {
        return !isSomeDialogueRunning;
    }

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

    private void DelayPlayerDuringDialogueOff()
    {
        playerDuringDialogueOff();
    }
}
