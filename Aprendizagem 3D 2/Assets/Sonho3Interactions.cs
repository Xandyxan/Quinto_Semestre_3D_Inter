using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sonho3Interactions : MonoBehaviour, IFade
{
    // script que serve de base para os trigger de dialogo que ocorrem durante o sonho3
    // trigger de diálogos, abrir portas uma única vez(chamar a ref da porta e dar.Interact), ghostDoor de múltiplas portas(pegar ref de uma lista de portas e ghost door nelas)
    // botar o script de flutuar em vários brinquedos e outros objetos no quarto de visitas
    // ghostDoor portas do quarto de visitas, com mais calma e suavidade que as portas do corredor
    // trigger de dialogo que rola do lado de fora da casa
    // abrir porta dos fundos uma única vez, de maneira remota
    // porta do quartinho de bagunças está destrancada, uma luz muito forte está dentro dela
    // fade out pra próxima cena quando o jogador entra no quartinho

    [Header("Triggers")]
    [SerializeField] private BoxCollider triggerBanheiro; // desativar após a porta abrir
    [SerializeField] private BoxCollider triggerCorredor; // desativar após a mariana chegar no corredor
    [SerializeField] private BoxCollider triggerQuartoVisitas; // talvez n precise desativar
    private int triggerCounter = 0;

    [Header("Portas Single Call")]
    [SerializeField] private Doors portaBanheiroVo, portaQuartoVisitas, portaFundos;

    [Header("Portas GhostDoor")]
    [SerializeField] private List<Doors> portasCorredor;
    [SerializeField] private List<Doors> portasMoveisVisitas;

    [Header("Flutuar")]
    bool floatUp = true;
    [SerializeField] private List<Transform> flutuantes;

    [Header("Dialogues")]
    private DialogueManager2 objectiveManager;
    [SerializeField] private int antoniaMinhaFiaDialogueIndex; // o som dessa conversa vem do quarto da avó
    [SerializeField] private int masTonhaDialogueIndex;  // o som dessa conversa vem do quarto de visitas
    [SerializeField] private int claroQueNaoDialogueIndex;    // o som dessa conversa vem da área externa, próximo ao quarto de bagunças

    [Header("Ativaveis")]
    [SerializeField] private GameObject luzFilhaFundos; //deixar isso ativo quando o jogador acionar o trigger dentro do quarto de bagunça do sonho

    [Header("Fade")]
    private FadeImage fadeScript;
    [SerializeField] private int sceneToLoadIndex;

    private void Awake()
    {
        fadeScript = FindObjectOfType<FadeImage>();
        objectiveManager = FindObjectOfType<DialogueManager2>();
        PlayerPrefs.DeleteKey("Sonho3");
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (triggerCounter)
        {
            case 0:
                objectiveManager.ExecuteDialogue(antoniaMinhaFiaDialogueIndex);
                triggerBanheiro.enabled = false;
                triggerCounter++;
                Invoke("OpenBathroomDoor", 24f);
                break;
            case 1:
                objectiveManager.ExecuteDialogue(masTonhaDialogueIndex);
                StartCoroutine(RemoteOpenDoor(portasCorredor, 150f, 200f, 30f));
                Invoke("OpenPortaVisitas", 15f);
                triggerCorredor.enabled = false;
                triggerCounter++;
                break;
            case 2:
                objectiveManager.ExecuteDialogue(claroQueNaoDialogueIndex);
                StartCoroutine(RemoteOpenDoor(portasMoveisVisitas, 60f, 100f, 15f));
                Invoke("OpenPortaFundos", 6f);
                triggerQuartoVisitas.enabled = false;
                triggerCounter++;
                break;
            case 3:
                luzFilhaFundos.SetActive(true); // quando o jogador entra no quartinho de entulhos, liga uma luz forte e chama o fade pra próxima fase
                Invoke("Fade", 2f);
                break;
        }
    }

    private void OpenBathroomDoor()
    {
        PlayerPrefs.SetInt("Sonho3", 1);
        portaBanheiroVo.Interact();
        portaBanheiroVo.Interact();
    }

    private void OpenPortaVisitas()
    {
        PlayerPrefs.SetInt("Sonho3", 2);
        portaQuartoVisitas.Interact();
        portaQuartoVisitas.Interact();
    }

    private void OpenPortaFundos()
    {
        PlayerPrefs.SetInt("Sonho3", 3);
        portaFundos.Interact();
        portaFundos.Interact();
    }

    private void FixedUpdate() // objetos flutuantes do quarto de visitas
    {
        foreach (Transform floatTransform in flutuantes)
        {
           
            if (floatUp)
            {
                StartCoroutine(FloatUp(floatTransform));
            }
            else
            {
                StartCoroutine(FloatDown(floatTransform));
            }
        }
    }

    private IEnumerator FloatUp(Transform flutuante)
    {
        Vector3 tempPos = flutuante.position;
        Vector3 tempRot = flutuante.rotation.eulerAngles;
        tempPos.y += 0.08f * Time.deltaTime;
        //tempPos.x += 0.01f * Time.deltaTime;
        //tempPos.z += 0.02f * Time.deltaTime;
        tempRot.x -= 6 * Time.deltaTime;
        tempRot.z += 6 * Time.deltaTime;
        tempRot.y += 6 * Time.deltaTime;
        flutuante.position = tempPos;
        flutuante.rotation = Quaternion.Euler(tempRot);
        yield return new WaitForSeconds(1.65f);
        floatUp = false;
    }

    private IEnumerator FloatDown(Transform flutuante)
    {
        Vector3 tempPos = flutuante.position;
        Vector3 tempRot = flutuante.rotation.eulerAngles;
        tempPos.y -= 0.07f * Time.deltaTime;
        //tempPos.x -= 0.01f * Time.deltaTime;
        //tempPos.z -= 0.02f * Time.deltaTime;
        tempRot.x += 6 * Time.deltaTime;
        tempRot.z -= 6.3f * Time.deltaTime;
        tempRot.y -= 6 * Time.deltaTime;
        flutuante.position = tempPos;
        flutuante.rotation = Quaternion.Euler(tempRot);
        yield return new WaitForSeconds(1.7f);
        floatUp = true;
    }

    private IEnumerator RemoteOpenDoor(List<Doors> doorList,float randomRotationMin, float RandomRotationMax, float duration)
    {
        float timer = Time.time + duration;
        print("Chubiscado");
        while (timer > Time.time)
        {
            foreach (Doors door in doorList)
            {
                door.SetRandomRotationSpeed(randomRotationMin, RandomRotationMax);
                door.Interact(); // abrir porta
            }
            yield return new WaitForSeconds(Random.Range(.2f, 1.5f));
        }
        StopCoroutine(RemoteOpenDoor(doorList, randomRotationMin, RandomRotationMax, duration));
        yield return null;
    }

    public void Fade()
    {
        fadeScript.SetFadeIn(true);
        fadeScript.SetHasNextFade(false);
        fadeScript.SetHasSceneLoad(true);
        fadeScript.SetSceneIndex(sceneToLoadIndex);

        fadeScript.StartCoroutine(fadeScript.Fade(3f));
    }
}
