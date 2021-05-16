using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitchPanel : MonoBehaviour, IInteractable, ISelectable
{
    [SerializeField] private List<GameObject> shadowZones;
    [SerializeField] private List<GameObject> lightsRealTime;

    [SerializeField] private List<GameObject> disjuntores;

    private bool lightsOn = true;

    [Header("Selected")]

    [SerializeField] string _objectDescription;

    [SerializeField] bool _isObjectiveObj;
    [SerializeField] int _dialogueIndex;

    public string objectDescription { get => _objectDescription; set => _objectDescription = value; }
    public bool isObjectiveObj { get => _isObjectiveObj; set => _isObjectiveObj = value; }
    public int dialogueIndex { get => _dialogueIndex; set => _dialogueIndex = value; }

    //private Vector3 currentSwitchAngle;

    private void Start()
    {
        //currentSwitchAngle = transform.rotation.eulerAngles;
       Interact();
    }
   

    public void Interact()
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
