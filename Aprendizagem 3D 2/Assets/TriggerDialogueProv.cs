using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogueProv : MonoBehaviour
{
    [SerializeField] private DialogueManager2 objectiveManager;
    [SerializeField] private int dialogueIndex;

    private float delay;

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            objectiveManager.ExecuteDialogue(dialogueIndex);
        }
    }

}
