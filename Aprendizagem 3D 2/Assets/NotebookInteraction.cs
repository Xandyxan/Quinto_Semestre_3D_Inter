using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotebookInteraction : MonoBehaviour, IFade, IInteractable, ISelectable
{
    //  Ao interagir com o notebook que se encontra fechado, a tela abre e ocorre um fade out de alguns segundos (doors, invoke do IFade)
    // Um fade in acontece com Mariana dizendo:
    // desativa mariana jogavel -> ativa mariana sentada no sofá, chama diálogo
    // "Bye my cuties! Don't forget your homework for next friday! Byeeeeee!"
    // fazer mais um fade, desativar mariana sentada e pc interagivel, ativar mariana jogavel e notebook com tela default -> chamar trigger de mensagens
    // Mudar material do skybox pra um material de fim de dia
    // Um ícone de nova mensagem surge na tela, é de Antonia, sua irmã:

    private FadeImage fadeScript;
    DialogueManager2 objectiveManager;

    [Header("Ativaveis")]
    [SerializeField] private GameObject marianaSentada, marianaJogavel, pcInterativo, livroIngles, bubuSeca, bubuFakeMolhada;
    [Space]
    [SerializeField] int cutsceneDiaIndex;
    [SerializeField] string _objectDescription;
    public string objectDescription { get => _objectDescription; set => _objectDescription = value; }
    private Animator notebookAnimator;
    private bool firstTime = true;
    private bool canInteract = false;

    [SerializeField] private TriggerUpdateMensagem triggerUpdateMensagem;

    private void Awake()
    {
        fadeScript = FindObjectOfType<FadeImage>();
        objectiveManager = FindObjectOfType<DialogueManager2>();
        notebookAnimator = GetComponent<Animator>();
    }

 
    public void Fade()
    {
      
        fadeScript.SetFadeIn(true);
        fadeScript.SetHasNextFade(true);
        fadeScript.SetHasSceneLoad(false);
        fadeScript.StartCoroutine(fadeScript.Fade(2.25f));
        livroIngles.SetActive(true);
        marianaJogavel.SetActive(false);
        marianaSentada.SetActive(true);

        Invoke("ReturnPlayer", 4.5f);
    }

    public void BackToGameFade()
    {
        GameManager.instance.removePlayerControlEvent?.Invoke();
        fadeScript.SetFadeIn(true);
        fadeScript.SetHasNextFade(true);
        fadeScript.SetHasSceneLoad(false);
        marianaJogavel.SetActive(true);
        marianaSentada.SetActive(false);
        fadeScript.StartCoroutine(fadeScript.Fade(2.25f));
        Invoke("ReturnToGame", 4.5f);
    }

    public void Interact()
    {
        if (canInteract)
        {
            if (firstTime)
            {
                notebookAnimator.SetTrigger("Open"); // fade é chamado no ultimo keyframe da animacao
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_MISSAO 2/SFX_Computador_Ligando", transform.position);
                canInteract = false;
            }
            else
            {
                notebookAnimator.SetTrigger("Close");
            }
        }
        else { return; }
      
    }

    private void ReturnPlayer()
    {
        
        objectiveManager.ExecuteDialogue(cutsceneDiaIndex);
        firstTime = false;
        Invoke("SetCanInteractTrue", 7f);
       
    }

    public void SetCanInteractTrue()
    {
        canInteract = true;
    }

    private void ReturnToGame()
    {
        GameManager.instance.returnPlayerControlEvent?.Invoke();
        triggerUpdateMensagem.UpdateContactMessages();
        bubuFakeMolhada.SetActive(false);
        bubuSeca.SetActive(true);
        PlayerPrefs.SetInt("BubuSeca", 1);

        this.enabled = false;
    }

   
}
