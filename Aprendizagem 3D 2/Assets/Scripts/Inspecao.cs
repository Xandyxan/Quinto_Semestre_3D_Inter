using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inspecao : MonoBehaviour
{
    [Header("pontoDeInspecao")]

    [Tooltip("Tranform position for inspection")]

    public Transform pontoInspecao;

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

    bool chegando;
    
    void Start()
    {
        chegando = false;
        obTrans = transform;
        sManager = FindObjectOfType<SelectionManager>();
        origemPos = transform.position;
        origemRot = transform.rotation;
      
    }

    void Update()
    {
        
        
        posicaoAtual = transform.position;
        rotacaoAtual = transform.rotation;

        //Encerra o processo de inspeção
        if(Input.GetKeyDown(KeyCode.E))
        {
            sManager.inspecionando = false;
            sManagerObject = null;

        }

        //O Obejto esta sendo inspecionado
        if(sManager.inspecionando && sManagerObject == transform)
        {
            
            posicaoAtual = Vector3.MoveTowards(posicaoAtual, Camera.main.transform.position, - Input.mouseScrollDelta.y * escala * Time.deltaTime);
            //Colocar limite para Zoom de Inspeção**
          
            //Rotaciona o obejto em inspeção quando o joghador aperta e segura o botão esquerdo do mouse e o move.
            if(Input.GetMouseButton(0))
            {
               
                this.transform.Rotate((Input.GetAxis("Mouse Y") * rotVel * Time.deltaTime), (-Input.GetAxis("Mouse X") * rotVel * Time.deltaTime), 0);
             
               
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

    //Começa o processo de inspeção.    
    public void Interagindo()
    {
        sManagerObject = sManager.selectionTransform;
        chegando = true;
        sManager.inspecionando = true;
    }
    
}
