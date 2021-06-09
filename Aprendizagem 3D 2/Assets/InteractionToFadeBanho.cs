using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionToFadeBanho : MonoBehaviour, IInteractable, IFade, ISelectable
{
    private FadeImage fadeScript;

    [Header("Needs Item")]
    [SerializeField] private bool needsItem;
    [SerializeField] private string itemTag; // key com o qual o pref do item foi registrado

    [SerializeField] private List<GameObject> roupasAtivar;
    [SerializeField] private List<GameObject> roupasDesativar;

    [SerializeField] private DialogueManager2 objectiveManager;

    [SerializeField] private GameObject toalhaHud;
    [SerializeField] string _objectDescription;

    public string objectDescription { get => _objectDescription; set => _objectDescription = value; }

    void Awake()
    {
        fadeScript = FindObjectOfType<FadeImage>();
        if (PlayerPrefs.HasKey(itemTag)) { PlayerPrefs.DeleteKey(itemTag); }
    }

    public void Interact()
    {
        if (PlayerPrefs.HasKey(itemTag))
        {
            Fade();
            foreach(GameObject roupaNova in roupasAtivar) { roupaNova.SetActive(true); }
            foreach(GameObject roupaSuja in roupasDesativar) { roupaSuja.SetActive(false); }
            if(toalhaHud!= null)toalhaHud.SetActive(false);
            Invoke("ReturnPlayer", 4f);
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_MISSAO 1/SFX_Chuveiro", this.transform.position);
        }
        else
        {
            return;
        }
    }

    public void Fade()
    {
        GameManager.instance.removePlayerControlEvent?.Invoke();
        fadeScript.SetFadeIn(true);
        fadeScript.SetHasNextFade(true);
        fadeScript.SetHasSceneLoad(false);

        fadeScript.StartCoroutine(fadeScript.Fade(2.25f));
        PlayerPrefs.SetInt("Banho", 1);
    }

    private void ReturnPlayer()
    {
        GameManager.instance.returnPlayerControlEvent?.Invoke();
        objectiveManager.ExecuteDialogue(10);
        PlayerPrefs.DeleteKey(itemTag);
    }
}
