using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayPosY : MonoBehaviour
{
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        rectTransform.transform.position = new Vector3(rectTransform.transform.position.x, 200.54f, rectTransform.transform.position.z);
    }
}
