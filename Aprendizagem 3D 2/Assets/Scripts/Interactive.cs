using System.Collections;
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

    [Space]

    [Header("Functionality")]

    [Tooltip("What kind of interaction this object have?")]
    [SerializeField] private string interactionKind;     // checamos em um switchCase, pra ver qual tipo de interação é, dai mandamos sinal pro script específico

    [Header("It's a objective object?")]
    [SerializeField] private bool isObjectiveObj;
    [SerializeField] ObjectiveManager objectiveManager;
    [SerializeField] private int dialogueIndex;


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
                if (isObjectiveObj) objectiveManager.ExecuteDialogue(dialogueIndex);

                switch (interactionKind)
                {
                    case "Furniture door":
                        Doors doorScript2 = transform.GetComponent<Doors>();
                        doorScript2.OpenCloseDoors();
                        break;

                    case "Interagivel":
                        Inspecao insp = transform.GetComponent<Inspecao>();
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

                    case "Fade":
                        Fade fade = transform.GetComponent<Fade>();
                       // Inspecao inspec  = transform.GetComponent<Inspecao>();
                       // inspec.Interagindo();
                        fade.FadeImage();
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
