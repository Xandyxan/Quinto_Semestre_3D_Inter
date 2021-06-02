using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEventosMissao3 : MonoBehaviour
{
    // Ao sair das  mensagens, o telefone da sala começa a tocar -> trigger de acionar objeto do telefone

    [Header("Dialogues")]
    DialogueManager2 objectiveManager;
    [SerializeField] int pressaoBaixaDialogueIndex;

    [SerializeField] TelefoneMissao3 telefone;

    private void Awake()
    {
        objectiveManager = FindObjectOfType<DialogueManager2>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (PlayerPrefs.HasKey("Cartinha"))
        {
            if(PlayerPrefs.GetInt("Cartinha", 0) == 1)
            objectiveManager.ExecuteDialogue(pressaoBaixaDialogueIndex);
            telefone.ReceberLigacao(true);
        }
    }

}
