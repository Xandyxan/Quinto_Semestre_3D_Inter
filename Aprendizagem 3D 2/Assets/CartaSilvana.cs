using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartaSilvana : Inspecao
{
    [Header("Collectable")]  
    [Tooltip("Tag that will be written on the inventory prefs")]
    [SerializeField] private string itemTag;

    private Animator cartaAnimator;

    public override void Interact()
    {
        cartaAnimator.SetTrigger("Read");
        base.Interact();
        PlayerPrefs.SetInt(itemTag, 1);
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SFX_GENERAL/SFX_Pegar_Papel", transform.position);
    }

    private void Awake()
    {
        cartaAnimator = GetComponent<Animator>();
        if (PlayerPrefs.HasKey(itemTag)) PlayerPrefs.DeleteKey(itemTag);

    }

  
}
