using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefExample : MonoBehaviour
{
    int valor = 1;

    // Start is called before the first frame update
    void Start()
    {
        Example(valor);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Example(int soma)
    {
        soma++;
        print("O valor da variável 'valor método' é " + soma);
        print("O valor da variável 'valor' é " + valor);
    }
}
