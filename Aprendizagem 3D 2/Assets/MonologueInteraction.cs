using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonologueInteraction : MonoBehaviour, IInteractable, IObjectiveObj, ISelectable
{
    [SerializeField] string _objectDescription;
    [SerializeField] bool _triggerDialogue;
    [SerializeField] int _dialogueIndex;

    [SerializeField] float zoomTime;
    public string objectDescription { get => _objectDescription; set => _objectDescription = value; }
    public bool triggerDialogue { get => _triggerDialogue; set => _triggerDialogue = value; }
    public int dialogueIndex { get => _dialogueIndex; set => _dialogueIndex = value; }

    private PlayerView playerView;

    private void Awake()
    {
        playerView = FindObjectOfType<PlayerView>();
    }
    public void Interact()
    {
        playerView.ForceZoom(zoomTime);
    }

    
}
