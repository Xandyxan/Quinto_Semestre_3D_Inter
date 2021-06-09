using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrsinhoInteractionSonho2 : MonoBehaviour, IInteractable, IFade
{
    // Objetos do ursinho e do berço limpo são ativados na sala
    // Um barulho de criança chorando (Silvana) surge bem forte vindo da sala, seguido de um dialogo de Mariana

    // Conforme o jogador se aproxima da sala, o som de choro vai ficando cada vez mais alto, a ponto de se tornar insuportável
    // A origem aparenta ser um berço que surge no centro da sala
    // O berço começa a tremer quando o jogador se aproxima

    // Ao olhar para dentro do berço, se encontra um ursinho de pelúcia flutuando. Ao interagir com o urso, o SOM DE CHORO se concentra apenas nele, 
    // como se ele estivesse chorando e, então, rola um fade out.

    // Após o fade out, o ursinho desaparece e o berço limpo é substituido pelo colchão sujo.

   // ----------------------------------------------------------------------------------------------------------------------------------------------------------
   

    private FadeImage fadeScript;

    [Header("Dialogues")]
    [SerializeField] DialogueManager2 objectiveManager;
    [SerializeField] int dialogueIndex;

    [Header("Triggers Distancia")]
    private Transform playerViewTransform; // usado pra pegar a posição do player e talvez forçar a direção da camera, caso seja necessário que ele olhe pra algo.
    private float distanceFromObjective;

    [Header("Tremer berço")]
    float speed = 46f; //how fast it shakes
    [SerializeField] float amount = 0.002f; //how much it shakes

    [Header("Ativaveis")]
    [SerializeField] private GameObject cercadinhoLimpo, cercadinhoSujoLama;
    [SerializeField] private List<GameObject> coisasDesativar; // talvez desativar lama, armários da cozinha e etc quando o jogador interagir com o urso.

    [Header("Flutuar")]
    bool floatUp = true;
    [SerializeField] float minX, maxX, minZ, maxZ;

    [Header("Focar no urso")]
    private Quaternion targetRotation;
    [SerializeField] float rotationMultiplier = 3f;
    void Awake()
    {
        fadeScript = FindObjectOfType<FadeImage>();
        cercadinhoLimpo.SetActive(true);
        playerViewTransform = FindObjectOfType<PlayerView>().GetComponent<Transform>();
        objectiveManager = FindObjectOfType<DialogueManager2>();
        // fazer choro da criança começando aqui!!!

    }

    // Start is called before the first frame update
    void Start()
    {
        objectiveManager.ExecuteDialogue(12);
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_SONHO 2/SFX_Silvana_Chorando", this.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        distanceFromObjective = Vector3.Distance(this.transform.position, playerViewTransform.position); // distancia entre o player e o ursinho
        Vector3 objectiveDirection = new Vector3(this.transform.position.x, 50, this.transform.position.z) - playerViewTransform.position;
        targetRotation = Quaternion.LookRotation(objectiveDirection);
        // print(distanceFromObjective + "distance ursinho");
        // quanto mais próximo do urso, mais alto o choro fica. Talvez você consiga resolver usando som 3d, então talvez isso seja desnecessário.
        float choroVolumeMultiplier = 1 / distanceFromObjective;

        float value = Mathf.InverseLerp(1, 4, distanceFromObjective);
        speed = 1 / value;
        if (distanceFromObjective < 2f)
        {
            Vector3 tempPos = cercadinhoLimpo.transform.position;
           
            tempPos.x += Mathf.Sin(Time.time * speed) * amount;
            tempPos.z += Mathf.Sin(Time.time * speed) * amount;
           
            
            cercadinhoLimpo.transform.position = tempPos;
        }
        if(distanceFromObjective < 1.8f)
        {
            playerViewTransform.rotation = Quaternion.Slerp(playerViewTransform.rotation, targetRotation, Time.deltaTime * rotationMultiplier);
        }
    }

    private void FixedUpdate() // controlar fisica da cena
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minX, maxX), transform.position.y, Mathf.Clamp(transform.position.z, minZ, maxZ));
        if (floatUp)
        {
            StartCoroutine(FloatUp());
        }
        else
        {
            StartCoroutine(FloatDown());
        }
        
    }
    public void Fade()
    {
        rotationMultiplier = 0f;
        GameManager.instance.removePlayerControlEvent?.Invoke();
        fadeScript.SetFadeIn(true);
        fadeScript.SetHasNextFade(true);
        fadeScript.SetHasSceneLoad(false);

        fadeScript.StartCoroutine(fadeScript.Fade(1.25f));

        foreach (GameObject item in coisasDesativar) { gameObject.SetActive(false); } // desativar coisas da cozinha
        cercadinhoLimpo.SetActive(false);
        cercadinhoSujoLama.SetActive(true);
        Invoke("AfterFade", 2f);

    }

    public void Interact()
    {
        rotationMultiplier = 6f;
        PlayerView pv = playerViewTransform.GetComponent<PlayerView>();
        pv.StartCoroutine(pv.ForceZoom(2f));
        // fazer choro ficar focado no ursinho aqui!!!
        Invoke("Fade", 2f);
        

    }

    private void AfterFade()
    {
        GameManager.instance.returnPlayerControlEvent?.Invoke();
       
        this.gameObject.SetActive(false);
    }

    private IEnumerator FloatUp()
    {
        Vector3 tempPos = transform.position;
        Vector3 tempRot = transform.rotation.eulerAngles;
        tempPos.y += 0.08f * Time.deltaTime;
        tempPos.x +=  0.01f * Time.deltaTime;
        tempPos.z += 0.02f * Time.deltaTime;
        tempRot.x -= 6 * Time.deltaTime;
        tempRot.z += 6 * Time.deltaTime;
        tempRot.y += 6 * Time.deltaTime;
        transform.position = tempPos;
        transform.rotation = Quaternion.Euler(tempRot);
        yield return new WaitForSeconds(1.65f);
        floatUp = false;
    }

    private IEnumerator FloatDown()
    {
        Vector3 tempPos = transform.position;
        Vector3 tempRot = transform.rotation.eulerAngles;
        tempPos.y -= 0.07f * Time.deltaTime;
        tempPos.x -= 0.01f * Time.deltaTime;
        tempPos.z -= 0.02f * Time.deltaTime;
        tempRot.x += 6 * Time.deltaTime;
        tempRot.z -= 6.3f * Time.deltaTime;
        tempRot.y -= 6 * Time.deltaTime;
        transform.position = tempPos;
        transform.rotation = Quaternion.Euler(tempRot);
        yield return new WaitForSeconds( 1.7f);
        floatUp = true;
    }
}
