using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [Header("Dialogue Stuff")]
    [SerializeField] Text dialogueText;

    [TextArea(2, 3)]
    [SerializeField] private List<string> sentences;

    [Header("Objective Texts")]

    [SerializeField] Text objectiveText;
    [TextArea(1, 2)]
    [SerializeField] List<string> objectives;

    private int dialogueIndex, objectiveIndex;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(InitialD());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) NextSentence();
        if (Input.GetKeyDown(KeyCode.T)) UpdateObjective();
    }

    private void NextSentence()
    {
        dialogueText.text = sentences[dialogueIndex];
        dialogueText.gameObject.SetActive(true);
        dialogueIndex++;
    }

    public void UpdateObjective()
    {
        objectiveText.text = objectives[objectiveIndex];
        objectiveText.gameObject.SetActive(true);
        objectiveIndex++;
    }

    private IEnumerator InitialD() // dialogo inicial
    {
        yield return new WaitForSeconds(0.5f);
        NextSentence();
        yield return new WaitForSeconds(2.5f);
        dialogueText.gameObject.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        NextSentence();
        yield return new WaitForSeconds(2.5f);
        dialogueText.gameObject.SetActive(false);
        yield return new WaitForSeconds(.2f);
        UpdateObjective();
        yield return new WaitUntil(PressedF);
        objectiveText.gameObject.SetActive(false);
        objectiveIndex++;
    }

    private bool PressedF()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            return true;
        }
        else return false;
    }

    public IEnumerator DoorInteraction() // dialogo da porta
    {
        if (PlayerPrefs.GetInt("EntranceKey", 0) == 0)
        {
            yield return new WaitForEndOfFrame();
            dialogueText.text = sentences[2];
            dialogueText.gameObject.SetActive(true);
            yield return new WaitForSeconds(2.5f);
            dialogueText.text = sentences[3];
            yield return new WaitForSeconds(2.5f);
            dialogueText.gameObject.SetActive(false);
        }
        else if (PlayerPrefs.GetInt("EntranceKey", 0) == 1)
        {
            yield return new WaitForEndOfFrame();
            dialogueText.text = sentences[4];
            dialogueText.gameObject.SetActive(true);
            yield return new WaitForSeconds(3.5f);
            dialogueText.text = sentences[5];
            yield return new WaitForSeconds(3.5f);
            dialogueText.gameObject.SetActive(false);
            UpdateObjective();
        }
    }

    public IEnumerator LigthInteraction()
    {
        objectiveText.gameObject.SetActive(false);
        yield return new WaitForEndOfFrame();
        dialogueText.text = sentences[6];
        dialogueText.gameObject.SetActive(true);
        yield return new WaitForSeconds(4.5f);
        dialogueText.text = sentences[7];
        yield return new WaitForSeconds(5.5f);
        dialogueText.gameObject.SetActive(false);
        UpdateObjective();
    }

    public void StartCoroutine(int number)
    {
        if (number == 1) StartCoroutine(DoorInteraction());
        if (number == 2) StartCoroutine(LigthInteraction());
    }
}
