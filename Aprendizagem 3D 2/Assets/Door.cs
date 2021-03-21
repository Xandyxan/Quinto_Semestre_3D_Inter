using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    // coordena o funcionamento das portas, checando se precisam ser abertas, qual chave, animação, etc
    [Header("Default Infos")]

    [Tooltip("If the door starts closed or not")]
    [SerializeField] private bool isClosed = true; // se a porta está aberta ou fechada na animação
    private Animator doorAnimator;
    
    [Space]

    [Header("Locked Door Stuff")]

    [Tooltip("If the door is locked by a key")]
    [SerializeField] private bool isLocked = true;
    // da pra usar uma string pra quando o jogador coletar uma chave, nós criarmos um playerPref com o nome da chave e depois checar se bate com esse.
    [Tooltip("Name of the key used to open this door")]
    [SerializeField] private string keyName; 
   


    // Será que vai dar conflito? ai ai ai
    // test for conflicts 17:31
    //Xandy> Testando merge conflict nos scripts! 16:51 - 21/03
    //Xandy> Testando merge conflict nos scripts! 17:24 - 21/03
    //Xandy> Segundo teste de merge conflict nos scripts! 17:13 - 21/03

    //Xandy> Segundo teste de merge conflict nos scripts! 17:13 - 21/03
    //Xandy> Terceiro teste de merge conflict nos scripts! 17:32 - 21/03

    // Start is called before the first frame update
    void Start()          // AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA 17:32
    {
        doorAnimator = GetComponent<Animator>(); // test comment, delete later
    }

    public void OpenDoor()
    {
        if (isLocked) CheckKey(); // glubglub
        else
        {

            isClosed = !isClosed;
            doorAnimator.SetBool("IsClosed", isClosed);
            doorAnimator.SetBool("IsIdle", false);
        }

    }
    private void CheckKey()   // checa se o player tem um pref com a mesma string do nome da chave
    {
        if (PlayerPrefs.HasKey(keyName))   // abre a porta
        {
            // destranca a porta
            print("Open the door!");
            isLocked = false;
         
        }
        else                    // roda a animação de porta trancada
        {
            doorAnimator.SetTrigger("Locked");
            print("jogador não possui a chave!");
            return;
        }
    }

}
