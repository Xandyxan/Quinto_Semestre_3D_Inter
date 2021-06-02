using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    //quando o objeto for selecionado, o crosshair fica size 100
    public static SelectionManager instance; //singleton

    [SerializeField] private string selectableTag = "Selectable";

    public Transform selectionTransform;

    [SerializeField] private float maxDistance = 20f; 

    [Header("Crosshair Change")]
    public Image crosshair;
    private float chRaioSelected = 100f;
    private float zoomSpeed = 8f;
    public bool inspecionando;

    private bool usingCellphone;

    [SerializeField] private bool mission2;
    //private Color selectionColor = new Color(140, 87, 49);
    //Renderer selectionRenderer = null;

    [Tooltip("Reference to the text component from the HUD Canvas")]  // se a seleção tiver um component do tipo ISelectable, seta esse texto e ativa ele
    public Text selectionText;
    private DialogueManager2 objectiveManager; // caso consiga colocar o trigger de dialogo em uma interface, o dialogue manager fica centralizado aqui
    private void Awake()
    {
        instance = this;

        objectiveManager = FindObjectOfType<DialogueManager2>();

      /*  // essas linhas abaixo são pra normalizar a intensidade da cor HDR, deixando o valor dela como 1.
        float intensity = (selectionColor.r + selectionColor.g + selectionColor.b) / 3f;
        float factor = 1f / intensity;
        factor *= .05f;
        selectionColor = new Color(selectionColor.r * factor, selectionColor.g * factor, selectionColor.b * factor);*/
    }

    private void Start() 
    {
        inspecionando = false;

        GameManager.instance.removePlayerControlEvent -= SetUsingCellphoneTrue; // we remove the methods from the delegate at the beggining to prevent it to run multiple times.
        GameManager.instance.returnPlayerControlEvent -= SetUsingCellphoneFalse;
        GameManager.instance.removePlayerControlEvent += SetUsingCellphoneTrue;
        GameManager.instance.returnPlayerControlEvent += SetUsingCellphoneFalse;
      
    }
    private void Update()
    {
        selectionText.gameObject.SetActive(false);
        if (selectionTransform != null)
        {
           // Renderer selectionRenderer = selectionTransform.GetComponent<Renderer>();
            
           // if(selectionRenderer != null)
           // selectionRenderer.material.DisableKeyword("_EMISSION");
            selectionTransform = null;
        }
        else
        {
            crosshair.rectTransform.sizeDelta = Vector2.Lerp(crosshair.rectTransform.sizeDelta, new Vector2(chRaioSelected / 2, chRaioSelected / 2), zoomSpeed * Time.deltaTime);
        }

        if (!usingCellphone && Dialogue.isSomeDialogueRunning == false)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxDistance))
            {
                Transform selection = hit.transform;

                if (selection.gameObject.CompareTag(selectableTag) && !inspecionando)
                {
                   // selectionRenderer = selection.GetComponent<Renderer>();
                    var selectable = selection.GetComponent<ISelectable>();

                   /* if (selectionRenderer != null)
                    {
                        selectionRenderer.material.SetColor("_EmissionColor", selectionColor);
                        selectionRenderer.material.EnableKeyword("_EMISSION");
                    }*/
                    if(selectable != null)
                    {
                        selectionText.text = selectable.objectDescription;
                        selectionText.gameObject.SetActive(true);
                    }
                    crosshair.rectTransform.sizeDelta = Vector2.Lerp(crosshair.rectTransform.sizeDelta, new Vector2(chRaioSelected, chRaioSelected), (zoomSpeed - 2) * Time.deltaTime);

                    if (Input.GetMouseButtonDown(0))
                    {
                        var interactable = selection.GetComponent<IInteractable>();
                        if (interactable == null) return;
                        interactable.Interact();

                        var objectiveObj = selection.GetComponent<IObjectiveObj>(); 
                        if (objectiveObj == null) return;
                        if(objectiveObj.triggerDialogue) objectiveManager.ExecuteDialogue(objectiveObj.dialogueIndex);
                        if (mission2) 
                        { 
                            var addIntCount = selection.GetComponent<Mission2InteractionAddCount>();
                            if (addIntCount == null) return;
                            addIntCount.AddCount();
                        }
                    }

                    selectionTransform = selection;

                }
            }
        }
        
    }
    // functions called on the usingCellphone event. This code stops updating while the phone is being used, to avoid letting the player interact whith doors, for ex.
    private void SetUsingCellphoneTrue()
    {
        usingCellphone = true;
    }

    private void SetUsingCellphoneFalse()
    {
        usingCellphone = false;
    }
}
