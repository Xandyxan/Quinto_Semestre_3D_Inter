using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogueProv : MonoBehaviour
{
    [SerializeField] DialogueManager2 objectiveManager;

    [SerializeField] List<int> dialogueIndexes;

    private float delay;
    private void OnTriggerStay(Collider other)
    {
        if(delay < Time.time)
        {
            delay = Time.time + 5f;
            if (other.CompareTag("Player"))
            {
                int i = 0;
                if (dialogueIndexes.Count > 1) { i = Random.Range(0, dialogueIndexes.Count); }
                objectiveManager.ExecuteDialogue(dialogueIndexes[i]);
                print("DIALOGUE HAPPENED" + i);
            }
        }
        
    }

}
