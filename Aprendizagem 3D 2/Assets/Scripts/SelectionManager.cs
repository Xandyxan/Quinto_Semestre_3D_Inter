﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    //quando o objeto for selecionado, o crosshair fica size 100
    public static SelectionManager instance; //singleton

    [SerializeField] private string selectableTag = "Selectable";

    Renderer selectionRenderer = null;

    public Transform selectionTransform;

    Interactive interactiveScript = null;

    [SerializeField] private float maxDistance = 20f; 

    [Header("Crosshair Change")]
    public Image crosshair;
    private float chRaioSelected = 100f;
    private float zoomSpeed = 8f;
    public bool inspecionando;

    private bool usingCellphone;

    private Color selectionColor = new Color(140, 87, 49);
    private void Awake()
    {
        instance = this;

        // essas linhas abaixo são pra normalizar a intensidade da cor HDR, deixando o valor dela como 1.
        float intensity = (selectionColor.r + selectionColor.g + selectionColor.b) / 3f;
        float factor = 1f / intensity;
        factor *= .16f;
        selectionColor = new Color(selectionColor.r * factor, selectionColor.g * factor, selectionColor.b * factor);
    }

    private void Start() 
    {
        inspecionando = false;

        GameManager.instance.removePlayerControlEvent -= SetUsingCellphoneTrue; // we remove the methods from the delegate at the beggining to prevent it to run multiple times.
        GameManager.instance.returnPlayerControlEvent -= SetUsingCellphoneFalse;
        GameManager.instance.removePlayerControlEvent += SetUsingCellphoneTrue;
        GameManager.instance.returnPlayerControlEvent += SetUsingCellphoneFalse;
       // Cellphone.instance.usingCellphoneEvent -= SetUsingCellphoneTrue; // we remove the methods from the delegate at the beggining to prevent it to run multiple times.
       // Cellphone.instance.closeCellMenuEvent -= SetUsingCellphoneFalse;
       // Cellphone.instance.usingCellphoneEvent += SetUsingCellphoneTrue;
       // Cellphone.instance.closeCellMenuEvent += SetUsingCellphoneFalse;
    }
    private void Update()
    {
        if (selectionTransform != null)
        {
            Renderer selectionRenderer = selectionTransform.GetComponent<Renderer>();
            //selectionRenderer.material = defaultMaterial;
            selectionRenderer.material.DisableKeyword("_EMISSION");
            selectionTransform = null;
        }
        else
        {
            crosshair.rectTransform.sizeDelta = Vector2.Lerp(crosshair.rectTransform.sizeDelta, new Vector2(chRaioSelected / 2, chRaioSelected / 2), zoomSpeed * Time.deltaTime);
            if (interactiveScript != null) interactiveScript.SetSelectedFalse();
        }

        if (!usingCellphone)
        {
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
                        // defaultMaterial = selectionRenderer.material;
                        // selectionRenderer.material = highlightMaterial;
                        selectionRenderer.material.SetColor("_EmissionColor", selectionColor);
                        selectionRenderer.material.EnableKeyword("_EMISSION");
                    }

                    crosshair.rectTransform.sizeDelta = Vector2.Lerp(crosshair.rectTransform.sizeDelta, new Vector2(chRaioSelected, chRaioSelected), (zoomSpeed - 2) * Time.deltaTime);

                    if (interactiveScript != null)
                    {
                        interactiveScript.SetSelectedTrue();
                        //highlightMaterial = interactiveScript.selectionMaterial;
                    }

                    selectionTransform = selection;

                }

                if (selection.gameObject.CompareTag("Doors"))
                {
                    crosshair.rectTransform.sizeDelta = Vector2.Lerp(crosshair.rectTransform.sizeDelta, new Vector2(chRaioSelected, chRaioSelected), (zoomSpeed - 2) * Time.deltaTime);
                    interactiveScript = selection.GetComponent<Interactive>();
                    if (interactiveScript != null) interactiveScript.SetSelectedTrue();
                }
            }
        }
        
    }
    // functions called on the usingCellphone event. This code stops updating while the phone is being used, to avoid letting the player interact whith doors, for ex.
    private void SetUsingCellphoneTrue()
    {
        usingCellphone = true;
       // print("Selection off");
    }

    private void SetUsingCellphoneFalse()
    {
        usingCellphone = false;
       // print("Selection true");
    }
}
