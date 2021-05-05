using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager2 : MonoBehaviour
{
    //That have to be in order, it's the main dialogues from objectives in order
    [Header("Dialogues References")]
    [SerializeField] public Dialogue[] dialogues;
    
    [Header("Initial dialogue")]
    [Tooltip("The scene has initial dialogue?")]
    [SerializeField] private bool hasInitialDialogue;
    
    [Tooltip("The dialogue index")]
    [SerializeField] private int initialDialogue;

    [Header("Delay initial dialogue")]
    [Tooltip("Have a delay to start initial dialogue?")]
    [SerializeField] private bool hasDelayInitialDialogue;
    [SerializeField] private float delayTime;


    private void Awake()
    {
        
    }

    void Start()
    {
        if (hasInitialDialogue) Invoke("StartInitialDialogue", delayTime);
    }

    private void StartInitialDialogue()
    {
        ExecuteDialogue(this.initialDialogue);
    }

    public void ExecuteDialogue(int index)
    {
        dialogues[index].RunCoroutine();
    }
}
