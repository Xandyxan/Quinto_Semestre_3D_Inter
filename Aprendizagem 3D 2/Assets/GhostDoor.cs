using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostDoor : MonoBehaviour, IInteractable
{
    private bool conditionCleared = false;
    [SerializeField] private List<Doors> portas;

    [SerializeField] private List<ParticleSystem> particulasLama; // fazer lama esguichar dos moveis enquanto esse evento está rolando

    [SerializeField] private GameObject ursinho;

    [SerializeField] private GameObject Orbe;
    private IEnumerator RemoteOpenDoor()
    {
        // para cada porta, chamar método que faz a porta abrir, esperar alguns segundos e chamar o método novamente, para que a porta feche.
        // Deixar isso num loop até o jogador concluir o objetivo necessário.
        while (conditionCleared == false)
        {
            foreach( Doors door in portas)
            {
                door.SetRandomRotationSpeed(150f, 200f);
                door.Interact(); // abrir porta
            }
            yield return new WaitForSeconds(Random.Range(.2f, 1.5f));
        }
        StopCoroutine(RemoteOpenDoor());
        yield return null;
    }

    // métodos para inscrever em eventos
    public void StartGhostDoors() // evento para começar
    {
        conditionCleared = false;
        StartCoroutine(RemoteOpenDoor());
    }

    public void SetConditionClearedTrue() // evento para terminar
    {
        conditionCleared = true;
    }

    public void Interact()
    {
        StartGhostDoors();
        Invoke("StartNextEvent", 4f);
    }

    private void StartNextEvent()
    {
        ursinho.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        Orbe.gameObject.SetActive(false);
    }
}
