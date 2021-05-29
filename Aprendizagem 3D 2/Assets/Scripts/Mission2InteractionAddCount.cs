using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission2InteractionAddCount : MonoBehaviour
{

    private Mission2InteractionCounter interactionCounter;

    private void Awake()
    {
        interactionCounter = FindObjectOfType<Mission2InteractionCounter>();
    }

    public void AddCount()
    {
        print("ADICIONOU");
        interactionCounter.AddInteractionCount();
        this.enabled = false;
    }

    
}
