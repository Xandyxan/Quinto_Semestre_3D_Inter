using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitchPanel : MonoBehaviour
{
    AudioSource audioS;
    public AudioClip ligou;
    [SerializeField] private List<GameObject> shadowZones;
    [SerializeField] private List<GameObject> lightsRealTime;

    void Start()
    {
        audioS = GetComponent<AudioSource>();
    }

    public void PowerOn()
    {
        audioS.PlayOneShot(ligou);
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
