using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaixaBagunca : MonoBehaviour, ISelectable, IInteractable
{
    [Header("Selectable")]
    [SerializeField] string _objectDescription;
    public string objectDescription { get => _objectDescription; set => _objectDescription = value; }

    private Animator boxAnimator;

    private void Awake()
    {
        boxAnimator = GetComponent<Animator>();
    }

    public void Interact()
    {
        boxAnimator.SetTrigger("Open");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
