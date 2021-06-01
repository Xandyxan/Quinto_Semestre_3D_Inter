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
    private DialogueManager2 dialogueManager;
    
    [Header("Restrict Character Movement?")]
    [SerializeField] private bool restrictCharMovement;

    [Header("Time to play extra sound")]
   // [SerializeField] bool endSound;
   // private PlaySound dialogueSound;

    [Header("Other Continuations")]
    [SerializeField] private GameObject postProcessEffect;
    [SerializeField] private GameObject postProcessEffect2;
    [SerializeField] private float _countProvisorio;

    public static bool isSomeDialogueRunning;

    public delegate void PlayerDuringDialogueOn();
    public static event PlayerDuringDialogueOn playerDuringDialogueOn;
    public delegate void PlayerDuringDialogueOff();
    public static event PlayerDuringDialogueOff playerDuringDialogueOff;  // seria aqui um evento do tipo playerDuringDialogueOff?

    public static Dialogue instance;

    private void Awake()
    {
        dialogueManager = GetComponentInParent<DialogueManager2>();
       // dialogueSound = GetComponent<PlaySound>();
        instance = this;

            
        alreadyExecuted = false;

        if (nextDialogue != null) { nextDialogueScript = nextDialogue.GetComponent<Dialogue>(); }
    }

    private void Start()
    {
        
    }

    public IEnumerator Speech()
    {
       // if(!endSound) dialogueSound.PlayOneShoot();

        isSomeDialogueRunning = true;
        if(restrictCharMovement) playerDuringDialogueOn();

        dialogueManager.GetCharacterNameUI().text = characterName;

        if (!alreadyExecuted)
        {
          //  if(dialogueSound != null)
           // dialogueSound.StartSound();
            dialogueManager.GetDialogueBox().SetActive(true);
           
            if(Cellphone.instance != null) Cellphone.instance.SetInDialogue(true);

            _countProvisorio = _countProvisorio / speechs.Length;

            for (int i = 0; i < speechs.Length; i++)
            {
                dialogueManager.GetDialogueTextUI().text = speechs[i];
                yield return new WaitForSeconds(_countProvisorio);
                
            }

            if (onlyOnce) alreadyExecuted = true;
            
        }
      //  if(dialogueSound != null)
      //  dialogueSound.StopSound();

        dialogueManager.GetDialogueBox().SetActive(false);
        Cellphone.instance.SetInDialogue(false);

        // if (nextDialogueScript != null) isSomeDialogueRunning = true; // tava false antes
        // else isSomeDialogueRunning = false; // tava true antes
        isSomeDialogueRunning = false;

        if (nextDialogueScript != null) { nextDialogueScript.RunCoroutine(); }
        else if(nextDialogueScript == null && restrictCharMovement) { DelayPlayerDuringDialogueOff(); }


        if(postProcessEffect != null) { postProcessEffect.SetActive(true); }
        if (postProcessEffect2 != null) { postProcessEffect2.SetActive(true); }   

      //  if(endSound) dialogueSound.PlayOneShoot();
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
            if(letters != ' ')totalTime += 0.2f; 
        }
        if (totalTime < 1f) totalTime = 2.5f;
        return totalTime;
    }

    private void DelayPlayerDuringDialogueOff()
    {
        playerDuringDialogueOff();
    }
}
