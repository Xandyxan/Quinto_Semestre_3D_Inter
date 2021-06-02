using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogoPosMensagem : MonoBehaviour
{
    [Header("Condicao dialogo")]
    [Tooltip("Qual condição deve estar concluida pro dialogo ser acionado?")]
    [SerializeField] private string keyToDialogueTrigger;
    [Header("Dialogues")]
    private DialogueManager2 objectiveManager;
    [SerializeField] int triggeredDialogueIndex;

    private void Awake()
    {
        objectiveManager = FindObjectOfType<DialogueManager2>();
    }

    public void RodarDialogo()
    {
        if (PlayerPrefs.HasKey(keyToDialogueTrigger))
        {
            if(PlayerPrefs.GetInt(keyToDialogueTrigger, 0) == 1)
            {
                Invoke("RunDialogue", 3f);
            }
            else
            {
                return;
            }
        }
    }

    private void RunDialogue()
    {
        objectiveManager.ExecuteDialogue(triggeredDialogueIndex);
    }
}
