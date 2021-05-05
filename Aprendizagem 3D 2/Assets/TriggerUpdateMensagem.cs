using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerUpdateMensagem : MonoBehaviour
{
    [SerializeField] private List<Messages> mensagens; // lista de scripts de mensagem, contendo a informação de contato.
    [SerializeField] private List<int> numberOfMessages; // lista com o numero de mensagens, que tem que ser igual a lista de messages.


    public void UpdateContactMessages() // rodar por exemplo quando o jogador entrar em um trigger especifico, ou interagir com um item especifico
    {
        for(int i = 0; i < mensagens.Count; i++)
        {
            mensagens[i].SetMessagesNumber(numberOfMessages[i]);
        }
    }
}
