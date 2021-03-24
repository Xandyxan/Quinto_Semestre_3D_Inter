using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitchPanel : MonoBehaviour
{
    [SerializeField] private List<GameObject> shadowZones;
    [SerializeField] private List<GameObject> lightsRealTime;
 

    public void PowerOn()
    {
        DialogueManager.instance.StartCoroutine(2);


        foreach (GameObject shadow in shadowZones)
        {
            shadow.SetActive(false); // fazer as sombras sumirem um pouco suavemente.
        }
        foreach(GameObject light in lightsRealTime)
        {
            light.SetActive(true);
        }
    }
}
