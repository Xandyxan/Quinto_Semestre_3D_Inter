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
    
    SelectionManager sManager;
    InspectorHolder iHolder;
    public float escala;
    public float rotVel;

    bool chegando;
    
    void Start()
    {
        chegando = false;
        sManager = FindObjectOfType<SelectionManager>();
        origemPos = transform.position;
        origemRot = transform.rotation;
      
    }

    void Update()
    {
        posicaoAtual = transform.position;
        rotacaoAtual = transform.rotation;

        if(Input.GetKeyDown(KeyCode.E))
        {
            sManager.inspecionando = false;
            

        }

        if(sManager.inspecionando)
        {
            
            posicaoAtual = Vector3.MoveTowards(posicaoAtual, Camera.main.transform.position, - Input.mouseScrollDelta.y * escala * Time.deltaTime);
            //Colocar limite para Zoom de Inspeção
          
            

            if(Input.GetMouseButton(0))
            {
                this.transform.Rotate((Input.GetAxis("Mouse Y") * rotVel * Time.deltaTime), (Input.GetAxis("Mouse X") * -rotVel * Time.deltaTime), 0, Space.World);
            }
            else
            {
               
                this.transform.rotation = Quaternion.Slerp(transform.rotation, pontoInspecao.rotation, Time.deltaTime * 2);
            }
            
            transform.position = posicaoAtual;
        }
        
        if(posicaoAtual != origemPos && rotacaoAtual != origemRot && !sManager.inspecionando)
        {
            transform.position = Vector3.Slerp(posicaoAtual,origemPos, Time.deltaTime * 2);
            transform.rotation = Quaternion.Slerp(rotacaoAtual, origemRot, Time.deltaTime * 2);
        }

        else if(posicaoAtual != pontoInspecao.position && rotacaoAtual != pontoInspecao.rotation && sManager.inspecionando && chegando)
        {
            transform.position = Vector3.Slerp(posicaoAtual, pontoInspecao.position, Time.deltaTime * 8);
            transform.rotation =  Quaternion.Slerp(rotacaoAtual, pontoInspecao.rotation, Time.deltaTime * 8);

            if((int)posicaoAtual.x == (int)pontoInspecao.position.x && (int)posicaoAtual.z == (int)pontoInspecao.position.z && (int)posicaoAtual.y == (int)pontoInspecao.position.y && 
            (int)rotacaoAtual.x == (int)pontoInspecao.rotation.x && (int)rotacaoAtual.y == (int)pontoInspecao.rotation.y && (int)rotacaoAtual.z == (int)pontoInspecao.rotation.z)
            
            {
                chegando = false;
                
            }
            
        }

    }

    public void Interagindo()
    {
        chegando = true;
        sManager.inspecionando = true;
    }
    
}
