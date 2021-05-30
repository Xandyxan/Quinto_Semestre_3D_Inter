using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission2InteractionCounter : MonoBehaviour
{
    // script pra parte da missão 2 em que a mariana precisa procurar pelo ursinho dela nos cômodos
    // Após interagir com 2 / 3 dos móveis, marina diz: "Não é possível! Será que tá no quartinho do lado de fora? Ou no quarto da vó?"

    #region Singleton Stuff
    private static Mission2InteractionCounter _instance;
    public static Mission2InteractionCounter instance { get { return _instance; } }
    #endregion

    private int interactions, doorLokedInteractions; // contador de interações

    [Header("Condition")]
    [SerializeField] private int requiredInteractions;
    [Header("Dialogue")]
    [SerializeField] private int startSearchDialogueIndex;
    [SerializeField] private int bubuNotFoundDialogueIndex;
    [SerializeField] private int cadeAChaveDialogueIndex;
    [SerializeField] private int dialogueTaSujoIndex;
    [SerializeField] private int dialogueAulasIndex;

    private DialogueManager2 objectiveManager;
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        objectiveManager = FindObjectOfType<DialogueManager2>();
    }


    public void AddInteractionCount()
    {
        interactions++;

        if (interactions >= requiredInteractions)
        {
            print("CADE ELE?");
            objectiveManager.ExecuteDialogue(bubuNotFoundDialogueIndex);
        }
    }

    public void AddDoorLockedInteractionCount()
    {
        doorLokedInteractions++;

        if (doorLokedInteractions >= 2)
        {
            print("CADE ELE?");
            Invoke("ExecuteDialogueChave", 3f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        objectiveManager.ExecuteDialogue(startSearchDialogueIndex);
        if (PlayerPrefs.HasKey("Bubu")) { objectiveManager.ExecuteDialogue(dialogueTaSujoIndex); }
        if(PlayerPrefs.HasKey("Aula")) 
        { 
            objectiveManager.ExecuteDialogue(dialogueAulasIndex);
            PlayerPrefs.DeleteKey("Aula");
        }
    }

    private void ExecuteDialogueChave()
    {
        objectiveManager.ExecuteDialogue(cadeAChaveDialogueIndex);
    }

}
