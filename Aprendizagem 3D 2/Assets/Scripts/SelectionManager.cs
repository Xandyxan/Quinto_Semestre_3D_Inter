using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    //quando o objeto for selecionado, o crosshair fica size 100
    public static SelectionManager instance; //singleton

    [SerializeField] private string selectableTag = "Selectable";

    Renderer selectionRenderer = null;
    [SerializeField] private Material highlightMaterial;
    private Material defaultMaterial;

    private Transform selectionTransform;

    Interactive interactiveScript = null;

    [SerializeField] private float maxDistance = 20f; 

    [Header("Crosshair Change")]
    public Image crosshair;
    private float chRaioSelected = 100f;
    private float zoomSpeed = 8f;
    public bool inspecionando;




    private void Awake()
    {
        instance = this;
    }

    private void Start() 
    {
        inspecionando = false;
    }
    private void Update()
    {
        if (selectionTransform != null)
        {
            //Renderer selectionRenderer = selectionTransform.GetComponent<Renderer>();
            selectionRenderer.material = defaultMaterial;

                selectionTransform = null;
        }
        else
        {
            crosshair.rectTransform.sizeDelta = Vector2.Lerp(crosshair.rectTransform.sizeDelta, new Vector2(chRaioSelected / 2, chRaioSelected / 2), zoomSpeed * Time.deltaTime);
            if (interactiveScript != null) interactiveScript.SetSelectedFalse();
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            Transform selection = hit.transform;

            if (selection.gameObject.CompareTag(selectableTag) && !inspecionando)
            {
                selectionRenderer = selection.GetComponent<Renderer>();
                interactiveScript = selection.GetComponent<Interactive>();
                if (selectionRenderer != null)
                {
                    defaultMaterial = selectionRenderer.material;
                    selectionRenderer.material = highlightMaterial;
                }

                crosshair.rectTransform.sizeDelta = Vector2.Lerp(crosshair.rectTransform.sizeDelta, new Vector2(chRaioSelected, chRaioSelected), (zoomSpeed - 2) * Time.deltaTime);

                 if(interactiveScript!= null)   interactiveScript.SetSelectedTrue();

                selectionTransform = selection;
               
            }

            if(selection.gameObject.CompareTag("Doors"))
            {

                interactiveScript = selection.GetComponent<Interactive>();
                if (interactiveScript != null) interactiveScript.SetSelectedTrue();
                /*
                InteractiveDoors interactiveDoors = selection.GetComponentInParent<InteractiveDoors>();
                if(Input.GetKeyDown("e"))
                interactiveDoors.OpenCloseDoors();
                //print("está olhando para uma porta");
                */
            }
        }
    }
}
