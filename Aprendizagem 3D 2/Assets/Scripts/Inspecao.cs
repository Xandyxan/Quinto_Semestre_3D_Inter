using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inspecao : MonoBehaviour, IInteractable, ISelectable, IObjectiveObj
{
    [Header("pontoDeInspecao")]

    [Tooltip("Tranform position for inspection")]

    public Transform pontoInspecao;
    public PlayerController jogador;

    private Vector3 origemPos;
    private Quaternion origemRot;
    private Vector3 posicaoAtual;
    private Quaternion rotacaoAtual;
    Transform obTrans;
    
    private SelectionManager sManager;

    public float zoomMultiplier;
    public float rotVel;
    public float inspectionZoom;

    bool chegando;

    bool estaSelecionado = false;

    [Header("Trigger Messages")]
    [SerializeField] private bool triggerMessage;
    private MessageTrigger messageTrigger;

    [Header("Selected")]

    [SerializeField] string _objectDescription;
    [SerializeField] bool _triggerDialogue;
    [SerializeField] int _dialogueIndex;

    public string objectDescription { get => _objectDescription; set => _objectDescription = value; }
    public bool triggerDialogue { get => _triggerDialogue; set => _triggerDialogue = value; }
    public int dialogueIndex { get => _dialogueIndex; set => _dialogueIndex = value; }

    void Start()
    {
        jogador = FindObjectOfType<PlayerController>();
        chegando = false;
        obTrans = transform;
        sManager = FindObjectOfType<SelectionManager>();
        origemPos = transform.position;
        origemRot = transform.rotation;
        if(triggerMessage) { messageTrigger = GetComponent<MessageTrigger>(); } // se o objeto for ser trigger de novas mensagens no zap, ele pega a ref do trigger.
    }

    private void Update()
    {
        posicaoAtual = transform.position;
        rotacaoAtual = transform.rotation;

        //Encerra o processo de inspeção
        if(Input.GetKeyDown(KeyCode.E) && estaSelecionado)
        {
            ConcludeInspection();
        }

        //O Objeto esta sendo inspecionado
        if(estaSelecionado)
        {
                                 
            inspectionZoom += Input.mouseScrollDelta.y * Time.deltaTime;
            inspectionZoom = Mathf.Clamp(inspectionZoom, -0.03f, -0.02f);
            posicaoAtual = pontoInspecao.transform.position + inspectionZoom * pontoInspecao.transform.forward * zoomMultiplier ;

            //Colocar limite para Zoom de Inspeção**
          
            //Rotaciona o objeto em inspeção quando o joghador aperta e segura o botão esquerdo do mouse e o move.
            if(Input.GetMouseButton(0))
            {
               
                transform.RotateAround(transform.position, jogador.transform.right,(Input.GetAxis("Mouse Y") * rotVel * Time.deltaTime));
                transform.RotateAround(transform.position, -jogador.transform.up,(Input.GetAxis("Mouse X") * rotVel * Time.deltaTime));
                 
            }
            //Rotaciona o objeto para a rotação inicial de inspeção quando o jogado não está mais pressionando o botão esquerdo do mouse
            else
            {
               
                this.transform.rotation = Quaternion.Slerp(transform.rotation, pontoInspecao.rotation, Time.deltaTime * 2);
            }
            
            this.transform.position = posicaoAtual;
            
        }

        //Retorna o objeto para origem.
        if(posicaoAtual != origemPos && rotacaoAtual != origemRot && !sManager.inspecionando) 
        {
            transform.position = Vector3.Slerp(posicaoAtual,origemPos, Time.deltaTime * 2);  
            transform.rotation = Quaternion.Slerp(rotacaoAtual, origemRot, Time.deltaTime * 2);
        }

        //Leva o Objeto até o ponto de inspeção.
        else if(posicaoAtual != pontoInspecao.position && rotacaoAtual != pontoInspecao.rotation && sManager.inspecionando && chegando) 
        { 
            transform.position = Vector3.Slerp(posicaoAtual, pontoInspecao.position, Time.deltaTime * 8); 
            transform.rotation =  Quaternion.Slerp(rotacaoAtual, pontoInspecao.rotation, Time.deltaTime * 8);

            //Verifica a posição do objeto em relação ao ponto de inspeção e desliga o Slerp acima. 
            if((int)posicaoAtual.x == (int)pontoInspecao.position.x && (int)posicaoAtual.z == (int)pontoInspecao.position.z && (int)posicaoAtual.y == (int)pontoInspecao.position.y && 
            (int)rotacaoAtual.x == (int)pontoInspecao.rotation.x && (int)rotacaoAtual.y == (int)pontoInspecao.rotation.y && (int)rotacaoAtual.z == (int)pontoInspecao.rotation.z)
            
            {
                chegando = false;
                
            }
            
        }

    }
    private void LateUpdate()
    {
        if (estaSelecionado) { HandleObjectPan(); }
        
    }
    //Começa o processo de inspeção.    
   // public virtual void Interagindo()                     // é chamado no Interactive
   // {
       
   // }
    
    private void HandleObjectPan() // Para que o jogador possa mover o objeto durante a inspeção, facilitando a visualização dos objetos
    {
        Vector3 objMovement = Vector3.zero;
        float panSpeed = 2f;
     
        objMovement += Input.GetAxis("Horizontal") * pontoInspecao.transform.right * panSpeed * Time.deltaTime;

        objMovement += Input.GetAxis("Vertical") * pontoInspecao.transform.up * panSpeed * Time.deltaTime;

        // objMovement = new Vector3();
        transform.position += objMovement;

    }
    protected virtual void ConcludeInspection()
    {
        sManager.inspecionando = false;
        chegando = false; // testando essa bool, ainda n sei oq ela faz
        estaSelecionado = false;

        // Cellphone.instance.SetCanUseCellphone(true); // Após o término do processo de inspeção, o player se torna novamente capaz de ativar o menu de celular.
        Cellphone.instance.SetInspecting(false);

        GameManager.instance.returnPlayerControlEvent?.Invoke();
    }

    public virtual void Interact()
    {
        chegando = true;
        sManager.inspecionando = true;
        estaSelecionado = true;

        // Cellphone.instance.SetCanUseCellphone(false); // impede o jogador de ativar o menu de celular enquanto está inspecionando um objeto.
        Cellphone.instance.SetInspecting(true);

        GameManager.instance.removePlayerControlEvent?.Invoke();

        if (triggerMessage) { messageTrigger.ActivateTrigger(); }
    }
}
