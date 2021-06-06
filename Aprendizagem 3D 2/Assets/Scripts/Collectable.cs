using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collectable : Inspecao
{
    [Header("Collectable")]  // 😎
    [Tooltip("Tag that will be written on the inventory prefs")]
    [SerializeField] private string itemTag;

    private bool CanBeCollected = false;
    [SerializeField] private GameObject PressToCollectText;

    [SerializeField] private GameObject CollectedObjHud;

    private void Awake()
    {
        if (PlayerPrefs.HasKey(itemTag)) PlayerPrefs.DeleteKey(itemTag);  // reseta o pref no começo da fase, como se o jogador não tivesse o item.
    }

    protected override void ConcludeInspection() // por algum motivo quando chamado do mainkey, esse método roda no grandmaKey (╯‵□′)╯︵┻━┻
    {
        //Cellphone.instance.gameObject.SetActive(true);
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
            if(CollectedObjHud!= null)CollectedObjHud.SetActive(true);
            print(this.name);
            GameManager.instance.ConcludeCurrentTask();
        }
    }
   
    public override void Interact()
    {
       // Cellphone.instance.gameObject.SetActive(false); // eu sei que é ruim chamar duas vezes, mas tava bugado 🤠
        this.CanBeCollected = true;
        PressToCollectText.SetActive(true);
        base.Interact();
        
    }

    
}
