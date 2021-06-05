using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TanqueInteraction : MonoBehaviour, IInteractable, IFade
{
    private FadeImage fadeScript;

    [Header("Needs Item")]
    [SerializeField] private bool needsItem;
    [SerializeField] private string itemTag; // key com o qual o pref do item foi registrado
    [SerializeField] private GameObject itemHUD;

    [SerializeField] private List<GameObject> coisasAtivar;
    [SerializeField] private List<GameObject> coisasDesativar;

    private DialogueManager2 objectiveManager;

    [SerializeField] private TriggerUpdateMensagem triggerUpdateMensagem;
    [SerializeField] private Material skyTarde;
    void Awake()
    {
        fadeScript = FindObjectOfType<FadeImage>();
        if (PlayerPrefs.HasKey(itemTag)) { PlayerPrefs.DeleteKey(itemTag); }
        objectiveManager = FindObjectOfType<DialogueManager2>();
    }

    public void Interact()
    {
        if (PlayerPrefs.HasKey(itemTag))
        {
            Fade();
            foreach (GameObject roupaNova in coisasAtivar) { roupaNova.SetActive(true); }
            foreach (GameObject roupaSuja in coisasDesativar) { roupaSuja.SetActive(false); }
            itemHUD.SetActive(false);
            Invoke("ReturnPlayer", 4f);
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
       
    }

    private void ReturnPlayer()
    {
        GameManager.instance.returnPlayerControlEvent?.Invoke();
        objectiveManager.ExecuteDialogue(14);
        PlayerPrefs.DeleteKey(itemTag);
        triggerUpdateMensagem.UpdateContactMessages();
        PlayerPrefs.SetInt("Aula", 1);
        RenderSettings.skybox = skyTarde;
    }

   
    

}
