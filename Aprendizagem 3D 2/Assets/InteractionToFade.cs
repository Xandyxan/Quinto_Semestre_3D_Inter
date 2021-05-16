using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionToFade : MonoBehaviour, ISelectable
{
    private FadeImage fadeScript;

    [Header("Needs Item")]
    [SerializeField] private bool needsItem;
    [SerializeField] private string itemTag; // key com o qual o pref do item foi registrado

    [Header("Selected")]

    [SerializeField] string _objectDescription;
    public string objectDescription { get => _objectDescription != null ? _objectDescription : "interagir"; set => _objectDescription = value; }
    void Awake()
    {
        fadeScript = FindObjectOfType<FadeImage>();
    }


    void Update()
    {
        if(gameObject.activeInHierarchy == true)
        {
            print("A");
        }
    }

    public virtual void StartFadeInteraction() // fazer override pra cada script que tem isso
    {
        fadeScript.StartCoroutine(fadeScript.Fade(5f));

    }
}
