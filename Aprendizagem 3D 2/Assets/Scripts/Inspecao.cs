using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inspecao : MonoBehaviour
{
    [Header("pontoDeInspecao")]

    [Tooltip("Tranform position for inspection")]

    public Transform pontoInspecao;
    public PlayerController jogador;

    private Vector3 origemPos;
    private Quaternion origemRot;
    private Vector3 posicaoAtual;
    private Quaternion rotacaoAtual;
    Transform sManagerObject;
    Transform obTrans;
    
    SelectionManager sManager;
    InspectorHolder iHolder;
    public float escala;
    public float rotVel;
    public float limite;

    bool chegando;

    private Interactive interactiveScript;
    void Start()
    {
        jogador = FindObjectOfType<PlayerController>();
        chegando = false;
        obTrans = transform;
        sManager = FindObjectOfType<SelectionManager>();
        origemPos = transform.position;
        origemRot = transform.rotation;
        interactiveScript = GetComponent<Interactive>();
    }

    void Update()
    {
        
        
        posicaoAtual = transform.position;
        rotacaoAtual = transform.rotation;

        //Encerra o processo de inspeção
        if(Input.GetKeyDown(KeyCode.E))
        {
            sManager.inspecionando = false;
            chegando = false; // testando essa bool, ainda n sei oq ela faz
            interactiveScript.SetSelectedFalse();// adicionei essa linha pq o objeto continuava selecionado msm após terminar o processo de inspeção!
            sManagerObject = null;

            Cellphone.instance.SetCanUseCellphone(true); // Após o término do processo de inspeção, o player se torna novamente capaz de ativar o menu de celular.
        }

        //O Obejto esta sendo inspecionado
        if(sManager.inspecionando && sManagerObject == transform)
        {
            
            
            limite += Input.mouseScrollDelta.y * Time.deltaTime;
            limite = Mathf.Clamp(limite, -0.02f, 0.01f);
            posicaoAtual = pontoInspecao.transform.position + limite * pontoInspecao.transform.forward * escala ;

            //Colocar limite para Zoom de Inspeção**
          
            //Rotaciona o obejto em inspeção quando o joghador aperta e segura o botão esquerdo do mouse e o move.
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
            transform.position = Vector3.Slerp(posicaoAtual,origemPos, Time.deltaTime * 2);  // passou
            transform.rotation = Quaternion.Slerp(rotacaoAtual, origemRot, Time.deltaTime * 2);
        }

        //Leva o Objeto até o ponto de inspeção.
        else if(posicaoAtual != pontoInspecao.position && rotacaoAtual != pontoInspecao.rotation && sManager.inspecionando && chegando) 
        { 
            transform.position = Vector3.Slerp(posicaoAtual, pontoInspecao.position, Time.deltaTime * 8); // passou
            transform.rotation =  Quaternion.Slerp(rotacaoAtual, pontoInspecao.rotation, Time.deltaTime * 8);

            //Verifica a posição do objeto em relação ao ponto de inspeção e desliga o Slerp acima. 
            if((int)posicaoAtual.x == (int)pontoInspecao.position.x && (int)posicaoAtual.z == (int)pontoInspecao.position.z && (int)posicaoAtual.y == (int)pontoInspecao.position.y && 
            (int)rotacaoAtual.x == (int)pontoInspecao.rotation.x && (int)rotacaoAtual.y == (int)pontoInspecao.rotation.y && (int)rotacaoAtual.z == (int)pontoInspecao.rotation.z)
            
            {
                chegando = false;
                
            }
            
        }

    }

    //Começa o processo de inspeção.    
    public void Interagindo()
    {
        sManagerObject = sManager.selectionTransform;
        chegando = true;
        sManager.inspecionando = true;

        Cellphone.instance.SetCanUseCellphone(false); // impede o jogador de ativar o menu de celular enquanto está inspecionando um objeto.
    }
    
}
