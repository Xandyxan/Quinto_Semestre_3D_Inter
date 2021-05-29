using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialMessagesSetup : MonoBehaviour
{
    private MessageTrigger initialMessagesTrigger;
    [SerializeField] private float delayMessages = 5;

    // Start is called before the first frame update
    void Start()
    {
        initialMessagesTrigger = GetComponent<MessageTrigger>();
        Invoke("StartSetup", delayMessages);                                    //tempo até terminar o monólogo inicial
    }

    private void StartSetup()
    {
        initialMessagesTrigger.ActivateTrigger();
        this.enabled = false;
    }
}
