using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitchPanel : MonoBehaviour
{
    [SerializeField] private List<GameObject> shadowZones;
    [SerializeField] private List<GameObject> lightsRealTime;

    [SerializeField] private List<GameObject> disjuntores;

    private bool lightsOn = true;

    //private Vector3 currentSwitchAngle;

    private void Start()
    {
        //currentSwitchAngle = transform.rotation.eulerAngles;
        PowerOn();
    }
    public void PowerOn()
    {
        if (!lightsOn)
        {
            foreach (GameObject shadow in shadowZones)
            {
                shadow.SetActive(false); // fazer as sombras sumirem um pouco suavemente.
            }
            foreach (GameObject light in lightsRealTime)
            {
                light.SetActive(true);
            }

            foreach (GameObject disjuntor in disjuntores)
            {
                // disjuntor roda pra direita (-40 no y)
                
                disjuntor.transform.rotation = Quaternion.AngleAxis(50, Vector3.up);
            }
        }
        else
        {
            foreach (GameObject shadow in shadowZones)
            {
                shadow.SetActive(true); // fazer as sombras sumirem um pouco suavemente.
            }
            foreach (GameObject light in lightsRealTime)
            {
                light.SetActive(false);
            }

            foreach (GameObject disjuntor in disjuntores)
            {
                // disjuntor roda pra esquerda (40 no Y)

                disjuntor.transform.rotation = Quaternion.AngleAxis(130, Vector3.up);
            }
        }
       

        lightsOn = !lightsOn;
        //currentSwitchAngle = transform.rotation.eulerAngles;
    }
}
