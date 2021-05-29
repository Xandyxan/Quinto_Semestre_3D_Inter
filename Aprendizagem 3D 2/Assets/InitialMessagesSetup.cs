using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialMessagesSetup : MonoBehaviour
{
    private MessageTrigger initialMessagesTrigger;

    // Start is called before the first frame update
    void Start()
    {
        initialMessagesTrigger = GetComponent<MessageTrigger>();
        Invoke("StartSetup", 5f);                                    //tempo até terminar o monólogo inicial
    }

    private void StartSetup()
    {
        initialMessagesTrigger.ActivateTrigger();
        this.enabled = false;
    }
}
