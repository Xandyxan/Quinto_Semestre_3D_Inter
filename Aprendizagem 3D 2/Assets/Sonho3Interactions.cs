using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sonho3Interactions : MonoBehaviour
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

    [Header("Portas Single Call")]
    [SerializeField] private Doors portaBanheiroVo, portaQuartoVisitas, portaFundos;

    [Header("Portas GhostDoor")]
    [SerializeField] private List<Doors> portasCorredor;
    [SerializeField] private List<Doors> portasMoveisVisitas;

    [Header("Flutuar")]
    bool floatUp = true;
    [SerializeField] float minX, maxX, minZ, maxZ;
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





    private void OnTriggerEnter(Collider other)
    {
        objectiveManager.ExecuteDialogue(antoniaMinhaFiaDialogueIndex); // dialogo roda apenas uma vez, então n importa o jogador entrar no trigger denovo (acho)

        if (PlayerPrefs.HasKey("Corredor")) { objectiveManager.ExecuteDialogue(masTonhaDialogueIndex); } // talvez eu tenha que dividir em mais de um objeto / script pra isso funcionar

    }
    private void FixedUpdate() // objetos flutuantes do quarto de visitas
    {
        foreach(Transform floatTransform in flutuantes)
        {
            floatTransform.position = new Vector3(Mathf.Clamp(floatTransform.position.x, minX, maxX), floatTransform.position.y, Mathf.Clamp(floatTransform.position.z, minZ, maxZ));

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
        tempPos.x += 0.01f * Time.deltaTime;
        tempPos.z += 0.02f * Time.deltaTime;
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
        tempPos.x -= 0.01f * Time.deltaTime;
        tempPos.z -= 0.02f * Time.deltaTime;
        tempRot.x += 6 * Time.deltaTime;
        tempRot.z -= 6.3f * Time.deltaTime;
        tempRot.y -= 6 * Time.deltaTime;
        flutuante.position = tempPos;
        flutuante.rotation = Quaternion.Euler(tempRot);
        yield return new WaitForSeconds(1.7f);
        floatUp = true;
    }
}
