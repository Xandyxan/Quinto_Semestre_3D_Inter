using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostDoor : MonoBehaviour
{
    private bool conditionCleared = false;
    [SerializeField] private List<Doors> portas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator RemoteOpenDoor()
    {
        // para cada porta, chamar método que faz a porta abrir, esperar alguns segundos e chamar o método novamente, para que a porta feche.
        // Deixar isso num loop até o jogador concluir o objetivo necessário.
        while (conditionCleared == false)
        {
            foreach( Doors door in portas)
            {
                yield return new WaitForSeconds(Random.Range(2,4));
                door.OpenCloseDoors(); // abrir porta
                yield return new WaitForSeconds(Random.Range(2, 4));
                door.OpenCloseDoors(); // fechar porta
                yield return new WaitForSeconds(Random.Range(2, 4));
            }
        }
        StopCoroutine(RemoteOpenDoor());
        yield return null;
    }

    // métodos para inscrever em eventos
    public void StartGhostDoors() // evento para começar
    {
        StartCoroutine(RemoteOpenDoor());
    }

    public void SetConditionClearedTrue() // evento para terminar
    {
        conditionCleared = true;
    }
}
