using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTelefone : MonoBehaviour
{
    [SerializeField] private TelefoneMissao3 telefone;
    // Start is called before the first frame update
    void Start()
    {
        telefone.enabled = true;   
    }

}
