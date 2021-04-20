using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveManager : MonoBehaviour
{
    //That have to be in order, it's the main dialogues from objectives in order
    [SerializeField] public Dialogues[] dialogues;
    
    [Tooltip("The scene has initial dialogue?")]
    [SerializeField] private bool hasInitialDialogue;
    [SerializeField] private int initialDialogue;

    // Start is called before the first frame update
    void Start()
    {
        if(hasInitialDialogue)
        {
            ExecuteDialogue(initialDialogue);
        }
    }

    public void ExecuteDialogue(int index)
    {
        dialogues[index].RunCoroutine();
    }
}
