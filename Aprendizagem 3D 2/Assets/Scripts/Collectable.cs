﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collectable : Inspecao
{
    [Tooltip("Tag that will be written on the inventory prefs")]
    [SerializeField] private string itemTag;

    private bool CanBeCollected = false;
    [SerializeField] private GameObject PressToCollectText;
    private void Awake()
    {
        if (PlayerPrefs.HasKey(itemTag)) PlayerPrefs.DeleteKey(itemTag);  // reseta o pref no começo da fase, como se o jogador não tivesse o item.
    }

    protected override void ConcludeInspection() // por algum motivo quando chamado do mainkey, esse método roda no grandmaKey (╯‵□′)╯︵┻━┻
    {
        print(CanBeCollected + this.name);
        base.ConcludeInspection();
        CollectItem();
    }

    private void CollectItem()
    {
        if (this.CanBeCollected)
        {
            PlayerPrefs.SetInt(this.itemTag, 1);
            this.gameObject.SetActive(false);
            PressToCollectText.SetActive(false);
            print(this.name);
        }
    }
   
    public override void Interagindo()
    {
        this.CanBeCollected = true;
        base.Interagindo();
        print(this.name + "está interagindo");
        PressToCollectText.SetActive(true);
        print(CanBeCollected + this.name);
    }

    
}
