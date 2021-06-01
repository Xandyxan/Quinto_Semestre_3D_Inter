using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionToFadeCama : MonoBehaviour, IFade, ISelectable, IInteractable
{
    private FadeImage fadeScript;

    [Header("Selected")]

    [SerializeField] string _objectDescription;
    public string objectDescription { get => _objectDescription; set => _objectDescription = value; }

    [SerializeField] int sceneToLoadIndex = 5;
    [SerializeField] string keyPraInteracao = "Banho";

    private void Awake()
    {
        fadeScript = FindObjectOfType<FadeImage>();
        if (PlayerPrefs.HasKey(keyPraInteracao)) { PlayerPrefs.DeleteKey(keyPraInteracao); }
    }
    public void Fade()
    {
        fadeScript.SetFadeIn(true);
        fadeScript.SetHasNextFade(false);
        fadeScript.SetHasSceneLoad(true);
        fadeScript.SetSceneIndex(sceneToLoadIndex);

        fadeScript.StartCoroutine(fadeScript.Fade( 3f));
    }

    public void Interact()
    {
        if (PlayerPrefs.HasKey(keyPraInteracao))
        {
            if (PlayerPrefs.GetInt(keyPraInteracao, 0) == 1)
            {
                Fade();
            }
        }
        else
        {
            return;
        }
    }

   
}
