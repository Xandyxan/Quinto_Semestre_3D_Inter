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
    private Quaternion rotacaoAtual, rotacaoInspecao;

    [SerializeField] private SelectionManager selectionManager;
    [SerializeField] private Vector3 inspecaoRotation;

    public float zoomMultiplier;
    public float rotVel;
   // public float inspectionZoom;

    bool chegando, voltando;

    bool estaSelecionado = false;

    [Header("Trigger Messages")]
    [SerializeField] private bool triggerMessage;
    private MessageTrigger messageTrigger;

    [Header("Selected")]

    [SerializeField] string _objectDescription;
    [SerializeField] bool _triggerDialogue;
    [SerializeField] int _dialogueIndex;

    [Header("Distancia da camera")]
    [SerializeField] private float objectOffset;

    // [Header("Pan")]
    //private Vector3 objMovement;
    private Vector3 initialPos;
    // float panSpeed = .00001f;

    [Header("Text Inspection")]
    [SerializeField] bool isText;
    [SerializeField] private GameObject pressToReadText;
    [SerializeField] GameObject textUIObj;
    private bool textIsOn;

    public string objectDescription { get => _objectDescription; set => _objectDescription = value; }
    public bool triggerDialogue { get => _triggerDialogue; set => _triggerDialogue = value; }
    public int dialogueIndex { get => _dialogueIndex; set => _dialogueIndex = value; }

    private void Awake()
    {
        jogador = FindObjectOfType<PlayerController>();

    }
    void Start()
    {
        origemPos = transform.position;
        origemRot = transform.rotation;
        chegando = false;
        if (triggerMessage) { messageTrigger = GetComponent<MessageTrigger>(); } // se o objeto for ser trigger de novas mensagens no zap, ele pega a ref do trigger.
        rotacaoInspecao = Quaternion.Euler(inspecaoRotation);
    }

    private void Update()
    {
        posicaoAtual = transform.position;
        rotacaoAtual = transform.rotation;

        //Encerra o processo de inspeção
        if (Input.GetKeyDown(KeyCode.E) && estaSelecionado)
        {
            ConcludeInspection();
        }

        //O Objeto esta sendo inspecionado
        if (estaSelecionado)
        {
            // mover objeto pro ponto de inspecao
            if (chegando)
            {
                Vector3 targetPos = pontoInspecao.position;
                targetPos += pontoInspecao.forward * objectOffset;
                if (posicaoAtual != targetPos && rotacaoAtual != pontoInspecao.rotation)
                {
                    transform.position = Vector3.Slerp(posicaoAtual, targetPos, Time.deltaTime * 8);
                    transform.rotation = Quaternion.Slerp(rotacaoAtual, rotacaoInspecao, Time.deltaTime * 8);

                    //Verifica a posição do objeto em relação ao ponto de inspeção e desliga o Slerp acima. 
                    if (Vector3.Distance(transform.position, pontoInspecao.position) <= 0)
                    {
                        chegando = false;
                        initialPos = transform.position;
                        posicaoAtual = pontoInspecao.transform.position;
                    }
                }

                if (isText)
                {
                    if (Input.GetKeyDown(KeyCode.T))
                    {
                        textIsOn = !textIsOn;
                        if (textIsOn)
                        {
                            pressToReadText.SetActive(false);
                            textUIObj.SetActive(true);
                        }

                        else
                        {
                            textUIObj.SetActive(false);
                            pressToReadText.SetActive(true);
                        }
                           
                    }
                }
            }

           // inspectionZoom += Input.mouseScrollDelta.y * Time.deltaTime;
            //inspectionZoom = Mathf.Clamp(inspectionZoom, -0.03f, -0.02f);


            //Rotaciona o objeto em inspeção quando o jogador aperta e segura o botão esquerdo do mouse e o move.
            if (Input.GetMouseButton(0))
            {

                transform.RotateAround(transform.position, jogador.transform.right, (Input.GetAxis("Mouse Y") * rotVel * Time.deltaTime));
                transform.RotateAround(transform.position, -jogador.transform.up, (Input.GetAxis("Mouse X") * rotVel * Time.deltaTime));

            }
        }

        //Retorna o objeto para origem.
        if (posicaoAtual != origemPos && rotacaoAtual != origemRot && voltando)
        {
            transform.position = Vector3.Slerp(posicaoAtual, origemPos, Time.deltaTime * 2);
            transform.rotation = Quaternion.Slerp(rotacaoAtual, origemRot, Time.deltaTime * 2);
        }

    }
    private void FixedUpdate()
    {
       // if (estaSelecionado && Input.GetAxis("Vertical") != 0) { HandleObjectPan(); }
    }

    /*private void HandleObjectPan() // Para que o jogador possa mover o objeto durante a inspeção, facilitando a visualização dos objetos
    {
        // move pra cima e para os lados a partir do referencial do ponto de inspecao
        objMovement *= 0;
        objMovement = transform.position;

        // objMovement += Input.GetAxis("Horizontal") * pontoInspecao.transform.right * panSpeed * Time.deltaTime;

        objMovement += Input.GetAxis("Vertical") * pontoInspecao.transform.up * panSpeed * Time.deltaTime;

         objMovement.x = Mathf.Clamp(objMovement.x, initialPos.x - 0.03f, initialPos.x + 0.03f);

         objMovement.z = Mathf.Clamp(objMovement.z, initialPos.z - 0.03f, initialPos.z + 0.03f);
        objMovement.y = Mathf.Clamp(objMovement.y, initialPos.y - 0.03f, initialPos.y + 0.03f);

        transform.position = objMovement;
    }*/
    protected virtual void ConcludeInspection()
    {
        selectionManager.inspecionando = false;
        chegando = false;
        voltando = true;
        estaSelecionado = false;
        if (isText) { pressToReadText.SetActive(false); }

        Cellphone.instance.SetInspecting(false);

        GameManager.instance.returnPlayerControlEvent?.Invoke();

    }

    public virtual void Interact()
    {
        voltando = false;
        origemPos = transform.position;
        origemRot = transform.rotation;
        chegando = true;
        selectionManager.inspecionando = true;
        estaSelecionado = true;
        if (isText) { pressToReadText.SetActive(true); }

        Cellphone.instance.SetInspecting(true);

        GameManager.instance.removePlayerControlEvent?.Invoke();

        if (triggerMessage) { messageTrigger.ActivateTrigger(); }
    }
}
