using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inspecao : MonoBehaviour
{
    [Header("pontoDeInspecao")]

    [Tooltip("Tranform position for inspection")]

    public Transform pontoInspecao;

    private Vector3 origemPos;
    private Vector3 origemRot;
    private Vector3 posicaoAtual;
    private Vector3 rotacaoAtual;
    
    public bool inspecionando;
    public float escala;
    public float rotVel;
    
    void Start()
    {
         origemPos = transform.position;
         origemRot = transform.localEulerAngles;
         inspecionando = false;
    }

    
    void Update()
    {
        posicaoAtual = transform.position;
        rotacaoAtual = transform.localEulerAngles;
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            inspecionando = false;
            transform.position = origemPos;
            transform.localEulerAngles = origemRot;

        }

        if(inspecionando == true)
        {
            posicaoAtual += Input.mouseScrollDelta.y * transform.forward * escala * Time.deltaTime;

            if(Input.GetMouseButton(0))
            {
                this.transform.Rotate((Input.GetAxis("Mouse Y") * rotVel * Time.deltaTime), (Input.GetAxis("Mouse X") * -rotVel * Time.deltaTime), 0, Space.World);
            }
            //posicaoAtual.z = Mathf.Clamp(posicaoAtual.z, pontoInspecao.position.z, pontoInspecao.position.z + 1);
            transform.position = posicaoAtual;
        }
    }

    public void Interagindo()
    {
        this.transform.position = pontoInspecao.position;
        this.transform.rotation = pontoInspecao.rotation;
        inspecionando = true;
        
    }
    
}
