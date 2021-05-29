using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission2InteractionAddCount : MonoBehaviour
{

    private Mission2InteractionCounter interactionCounter;

    [SerializeField] bool isDoorLockedObj;

    private void Awake()
    {
        interactionCounter = FindObjectOfType<Mission2InteractionCounter>();
    }

    public void AddCount()
    {
        if(isDoorLockedObj) { interactionCounter.AddDoorLockedInteractionCount(); } 
        else
        {
            print("ADICIONOU");
            interactionCounter.AddInteractionCount();
           
        }
        this.enabled = false;
    }

    
}
