using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogueProv : MonoBehaviour
{
    public Dialogue dialogue;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            dialogue.RunCoroutine();
        }
    }
}
