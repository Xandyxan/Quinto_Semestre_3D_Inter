using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RectTransformTestScript : MonoBehaviour
{
    private RectTransform thisRecTransform;

    [SerializeField] RectTransform targetRecTransform;

    // Start is called before the first frame update
    void Start()
    {
        thisRecTransform = GetComponent<RectTransform>();
        Vector3 position = thisRecTransform.anchoredPosition; // valor especificado no editor, com uma casa de aproximação
        print("rectTransform" + position);
        //thisRecTransform.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
