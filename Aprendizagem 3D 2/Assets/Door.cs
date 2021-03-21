using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    // coordena o funcionamento das portas, checando se precisam ser abertas, qual chave, animação, etc
    [Header("Default Infos")]

    [Tooltip("If the door starts closed or not")]
    [SerializeField] private bool isClosed = true; // se a porta está aberta ou fechada na animação
    private Animator doorAnim;
    
    [Space]

    [Header("Locked Door Stuff")]

    [Tooltip("If the door is locked by a key")]
    [SerializeField] private bool isLocked = true;
    // da pra usar uma string pra quando o jogador coletar uma chave, nós criarmos um playerPref com o nome da chave e depois checar se bate com esse.
    [Tooltip("Name of the key used to open this door")]
    [SerializeField] private string keyName; 
   

    // Start is called before the first frame update
    void Start()
    {
        doorAnim = GetComponent<Animator>();
    }

    public void OpenDoor()
    {
        if (isLocked) CheckKey();
        else
        {

            isClosed = !isClosed;
            doorAnim.SetBool("IsClosed", isClosed);
            doorAnim.SetBool("IsIdle", false);
        }

    }
    private void CheckKey()
    {
        if (PlayerPrefs.HasKey(keyName))
        {
            // destranca a porta
            print("Open the door!");
            isLocked = false;
         
        }
        else
        {
            doorAnim.SetTrigger("Locked");
            print("jogador não possui a chave!");
            return;
        }
    }

}
