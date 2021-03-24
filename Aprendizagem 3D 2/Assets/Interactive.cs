﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactive : MonoBehaviour
{
    [Header("MouseOver")]

    [Tooltip("Reference to the text component from the HUD Canvas")]
    public Text selectionText;

    [Tooltip("Text displayed when object is selected")]
    [SerializeField] private string objectDescription;

    [Tooltip("Material of the object when selected")]
    public Material selectionMaterial;

    [Space]

    [Header("Functionality")]

    [Tooltip("What kind of interaction this object have?")]
    [SerializeField] private string interactionKind;     // checamos em um switchCase, pra ver qual tipo de interação é, dai mandamos sinal pro script específico


    private bool isSelected = false;  // script do SeletionManager vai tornar isso true

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isSelected)
        {
            selectionText.text = objectDescription;
            selectionText.gameObject.SetActive(true);

            if (Input.GetMouseButtonDown(0))
            {
                switch (interactionKind)
                {
                    case "Door":
                        Transform doorRoot = transform.root;
                        Door doorScript = doorRoot.GetComponent<Door>();
                        doorScript.OpenDoor();
                        break;

                    case "Furniture door":
                        Doors doorScript2 = transform.GetComponent<Doors>();
                        doorScript2.OpenCloseDoors();
                        break;
                        
                    case "Interagivel":
                        Transform objectRoot = transform.root;
                        Inspecao insp = objectRoot.GetComponent<Inspecao>();
                        insp.Interagindo();
                        break;
                    case "Collectable":
                        Collectable collectable = transform.GetComponent<Collectable>();
                        collectable.CollectItem();
                        break;
                    case "LightsOn":
                        LightSwitchPanel lightPanel = transform.GetComponent<LightSwitchPanel>();
                        lightPanel.PowerOn();
                        break;
                }
                    
            }
        }
    }

    public void SetSelectedTrue()
    {
        isSelected = true;
    }

    public void SetSelectedFalse()
    {
        isSelected = false;

        selectionText.gameObject.SetActive(false);
    }
}
