using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelefoneMissao3 : MonoBehaviour, IInteractable, ISelectable
{
    // Ao sair das mensagens, o telefone da sala começa a tocar
    // Quando o jogador se aproxima para atender, o telefone para
    // trigger de dialogo: Incrível como eu nunca consigo atender o telefone! Mas quem seria essa hora?
    // trigger de update das mensagens celular: a mensagem é de Rafaela, sua psicóloga

    // Pós eventos no quartinho de bagunça e tals
    // Quando o jogador retorna para a casa através do quarto de visitas, ele pode escutar o telefone tocando mais uma vez
    // Ao chegar na sala e interagir com o telefone para atender -> trigger do dialogo final do jogo
    // load da próxima cena é chamado pelo postprocessing do dialogo
    [Header("distance from player")]
    [SerializeField] private Transform playerTransform;
    private float distanceFromPlayer;
    private bool podeAtender = false;

    [Header("Dialogues")]
    private DialogueManager2 objectiveManager;
    [SerializeField] private int ligacaoDesligaDialogueIndex; // mariana reclamando que nunca consegue atender
    [SerializeField] private int vouLaNoQuartinhoIndex;  // caso o alombado do jogador n aperte o botão 
    [SerializeField] private int diaFinalIndex; // ligação do bombeiro

    [Header("Cellphone")]
    private MessageTrigger triggerMensagens;


    [Header("Selectable")]
    [SerializeField] string _objectDescription;
    public string objectDescription { get => _objectDescription; set => _objectDescription = value; }

    private FMOD.Studio.EventInstance instance;

    private void Awake()
    {
        objectiveManager = FindObjectOfType<DialogueManager2>();
        triggerMensagens = GetComponent<MessageTrigger>();
        PlayerPrefs.DeleteKey("FindPote");

        instance = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/SFX_MISSAO 3/SFX_Telefone");
    }
    public void Interact()
    {
        if (podeAtender)
        {
            instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            objectiveManager.ExecuteDialogue(diaFinalIndex);
        }
        else
        {
            return;
        }
    }

    private void Start()
    {
        ReceberLigacao(false);
    }

    private void Update()
    {
        instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));

        distanceFromPlayer = Vector3.Distance(this.transform.position, playerTransform.position); // distancia entre o player e o telefone

        if(distanceFromPlayer < 1.4f && !podeAtender) // player se aproxima no inicio do game, quando ainda não pode atender a ligação
        {
            // stop telephone sound
            instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            objectiveManager.ExecuteDialogue(ligacaoDesligaDialogueIndex);
            triggerMensagens.ActivateTrigger();
            PlayerPrefs.SetInt("FindPote", 1);
        }
        else if(distanceFromPlayer > 2.8f)
        {
            if (PlayerPrefs.HasKey("FindPote"))
            {
                if(PlayerPrefs.GetInt("FindPote", 0) == 1)
                {
                    objectiveManager.ExecuteDialogue(vouLaNoQuartinhoIndex);
                    PlayerPrefs.SetInt("FindPote", 2);
                }
            }
        }

    }

    public void ReceberLigacao(bool ligaram)
    {
        // chamar aqui o código de fazer som de telefone tocando
        instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        instance.start();
        instance.release();
        podeAtender = ligaram;
    }
}
