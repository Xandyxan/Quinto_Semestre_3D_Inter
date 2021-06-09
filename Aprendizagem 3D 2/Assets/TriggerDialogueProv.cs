using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogueProv : MonoBehaviour
{
    [SerializeField] private DialogueManager2 objectiveManager;
    [SerializeField] private int dialogueIndex;
    [SerializeField] bool tv;
    [SerializeField] private Renderer tvRenderer;
    private float delay;

    private PlaySound playSound;
    private bool hasAnotherSound;

    private void Awake()
    {
        playSound = GetComponent<PlaySound>();
        if (playSound != null) hasAnotherSound = true;
        else hasAnotherSound = false;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            objectiveManager.ExecuteDialogue(dialogueIndex);
            if (hasAnotherSound) playSound.PlayOneShoot2();
            if (tv)
            {
                tvRenderer.material.mainTexture = null;
                StartCoroutine(changeScreenColor());
            }
        }
    }

    private IEnumerator changeScreenColor()
    {
        tvRenderer.material.color = Color.white;
        yield return new WaitForSeconds(28f);
        tvRenderer.material.color = Color.black;
    }

}
