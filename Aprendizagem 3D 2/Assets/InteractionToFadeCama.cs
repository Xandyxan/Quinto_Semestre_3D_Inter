using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionToFadeCama : MonoBehaviour, IFade, ISelectable, IInteractable
{
    private FadeImage fadeScript;

    [Header("Selected")]

    [SerializeField] string _objectDescription;
    public string objectDescription { get => _objectDescription; set => _objectDescription = value; }

    private void Awake()
    {
        fadeScript = FindObjectOfType<FadeImage>();
        if (PlayerPrefs.HasKey("Banho")) { PlayerPrefs.DeleteKey("Banho"); }
    }
    public void Fade()
    {
        fadeScript.SetFadeIn(true);
        fadeScript.SetHasNextFade(false);
        fadeScript.SetHasSceneLoad(true);
        fadeScript.SetSceneIndex(5);

        fadeScript.StartCoroutine(fadeScript.Fade( 3f));
    }

    public void Interact()
    {
        if (PlayerPrefs.HasKey("Banho"))
        {
            if (PlayerPrefs.GetInt("Banho", 0) == 1)
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
