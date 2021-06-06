using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveDesactive : MonoBehaviour
{
    [SerializeField] bool isMirror = false;
    private void Start()
    {
        if(!isMirror)
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
    }
}