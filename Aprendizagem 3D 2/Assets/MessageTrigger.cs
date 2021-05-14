using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageTrigger : MonoBehaviour
{
    [SerializeField] private List<string> contactNameMessage;        // key do pref do contato
    [SerializeField] private List<int> desiredNumberOfMessages;      // número total de mensagens na conversa com esse contato

    [SerializeField] private bool triggeredByCollider;               // Se esse evento ocorre quando o jogador entra em um trigger de collider. Caso não, ainda
                                                                     // podemos chamar esse método através de uma referencia em outro script, como ao coletar um item,
                                                                     // rolar um dialogo especifico e etc

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (triggeredByCollider) { ActivateTrigger(); }
        }
       
    }
    public void ActivateTrigger()   // esse método seta o número de mensagens desejado pra um contato determinado e então atualiza os chats de mensagens
    {
        for(int i = 0; i < desiredNumberOfMessages.Count; i++)
        {
            PlayerPrefs.SetInt(contactNameMessage[i], desiredNumberOfMessages[i]);
        }
       
        Cellphone.instance.UpdateAllMessages();
        this.enabled = false; // desativamos o componente do script, para que ele só rode uma vez
    }
}
