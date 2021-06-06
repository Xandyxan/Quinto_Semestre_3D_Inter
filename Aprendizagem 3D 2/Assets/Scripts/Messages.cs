using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Messages : MonoBehaviour
{
    [Tooltip("Com quem a mariana está conversando")]
    [SerializeField] string contactName;

    [SerializeField] InterfaceScroll scrollMessages;

    [SerializeField] List<RectTransform> messages; // We chose a list of Rectransforms because we'll need to get and set the UI elements position.

    [Header("New messages Arrived")]

    private bool newMessagesArrived; // guardamos o último valor do contador de mensagens em uma variavel e batemos com o novo valor durante o update.
    private int numberOfMessages;

    [Space]

    [SerializeField] private GameObject feedbackAnimation;

  

    public void SetMessagesNumber(int number)     // -> chamar no evento para setar o número de mensagens com o contato
    {
        PlayerPrefs.SetInt(contactName, number);
        UpdateMessages();
    }

    public void UpdateMessages()
    {
       // print("aaaaaaaaaaaa");
        int updatedChatMessagesCounter = 0; // vamos setar quantas das mensagens presentes na lista estarão ativas (enviadas) no momento em que o update foi chamado.

        if (PlayerPrefs.HasKey(contactName)) // pega a quantidade de mensagens presente na conversa atual através do valor do Pref.
        {
            updatedChatMessagesCounter = PlayerPrefs.GetInt(contactName, 0);

            // evitar um ArgumentOutOfRangeException né.

            if (updatedChatMessagesCounter > messages.Count - 1) { updatedChatMessagesCounter = messages.Count; }

            if(updatedChatMessagesCounter < 0) { updatedChatMessagesCounter = 0; }
        }
        int counter = 0;
        foreach(RectTransform message in messages) // primeiro desativa todos os gameobjects das mensagens e posiciona eles com -90 de dustância entre si.
        {
            message.anchoredPosition = new Vector2(message.anchoredPosition.x, counter * -90);
            message.gameObject.SetActive(false);
            counter++;
        }  

        for (int i = 0; i < updatedChatMessagesCounter; i++) // dai pega apenas a quantidade de mensagens atual e ativa os objetos delas.
        {
            messages[i].gameObject.SetActive(true);
           // print((i+ 1) + " messages are active");
        }

        if(updatedChatMessagesCounter > numberOfMessages)  // ativar feedback de mensagem nova aqui. //q(≧▽≦q)
        {
            feedbackAnimation.SetActive(true);
            //print("NEW MESSAGES ARRIVED"); 
        } 

        // after adding the messages, set the newest's position as the start.
        if(updatedChatMessagesCounter > 0) 
        {
          scrollMessages.SetNewestMessagePos(messages[updatedChatMessagesCounter - 1].anchoredPosition.y); // vamos usar pra posicionar a mensagem no centro da tela
          scrollMessages.SetLastMessageRectTransform(messages[updatedChatMessagesCounter - 1]); // vamos usar pra posicionar a mensagem no final da tela
          //print(messages[updatedChatMessagesCounter - 1].name + " é a mensagem mais recente" + "pos =" + (messages[updatedChatMessagesCounter - 1].anchoredPosition.y));
        } 
        else // there are no messages bro
        { 
          scrollMessages.SetNewestMessagePos(0);
          //print("there are no messages in " + contactName + " yet");
        }
        
        numberOfMessages = updatedChatMessagesCounter;
    }



}
