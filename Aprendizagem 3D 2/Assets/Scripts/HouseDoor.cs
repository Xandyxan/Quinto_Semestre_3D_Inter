using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseDoor : Doors
{
    // classe baseada na Doors, em que a diferença está em que a maçaneta tem de girar junto com a porta ao abri-la. 
    // Além disso, a porta também deve ter o estado de trancada, realizando a animação devida.

    // player da o input para abrir, maçaneta gira e porta gira. Quando a rotação finalizar, a macaneta volta para a rotação inicial, enquanto que a porta mantém.


    private Animator knobAnimator;

    [Header("Locked Door Stuff")]

    [Tooltip("If the door is locked by a key")]
    [SerializeField] private bool isLocked = true; // seta se a porta esta trancada no inspector

    // da pra usar uma string pra quando o jogador coletar uma chave, nós criarmos um playerPref com o nome da chave e depois checar se bate com esse.
    [Tooltip("Name of the key used to open this door")]
    [SerializeField] private string keyName;
    [SerializeField] private int KeyValue = 1;

    [Header("Trigger dialogue?")]  // valores específicos pra quando for uma porta trancada
    [Tooltip("valores específicos pra interação trancada / destrancou")]
    [SerializeField] private bool hasDoorDialogue;
    [Tooltip("Index do diálogo")]
    [SerializeField] private int doorLocked, doorUnlocked;
    private DialogueManager2 objectiveManager;

    [SerializeField] private GameObject collectedItemHud;

    protected override void Awake()
    {
        base.Awake();
        knobAnimator = GetComponent<Animator>(); // talvez mudar pra um get child na macaneta
        if(hasDoorDialogue) { objectiveManager = FindObjectOfType<DialogueManager2>(); }
    }

    public override void Interact()
    {
        if (isLocked) CheckKey();
        else
        {
            base.Interact();
            knobAnimator.SetTrigger("Open");
            if(collectedItemHud!= null) collectedItemHud.SetActive(false);
        }
    }




    private void CheckKey()   // checa se o player tem um pref com a mesma string do nome da chave e se o pref está com o valor 1. (Valor1 = true / Valor0 = false)
    {

        if (PlayerPrefs.HasKey(keyName))   // abre a porta
        {
            if (PlayerPrefs.GetInt(keyName, 0) == KeyValue)
                // destranca a porta
                print("Open the door!");
            if (hasDoorDialogue) objectiveManager.ExecuteDialogue(doorUnlocked);
            isLocked = false;
            // depois que isLocked fica false, quando o jogador tentar rodar o código de abrir a porta, ela irá abrir normalmente

        }
        else                    // roda a animação de porta trancada, roda a parte do código de abrir a porta em que a porta abre e fecha rápido.
        {
            // bool isLocked já começa como true
            knobAnimator.SetTrigger("Locked");
            print("jogador não possui a chave!");
            //DialogueManager.UpdateObjective();
            if (hasDoorDialogue) objectiveManager.ExecuteDialogue(doorLocked);
            return;
        }

    }

}
