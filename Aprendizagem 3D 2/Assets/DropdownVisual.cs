using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownVisual : MonoBehaviour
{
    private Dropdown dropdown;

    private void Awake()
    {
        dropdown = GetComponent<Dropdown>();
    }

    private void Start()
    {
        dropdown.value = PlayerPrefs.GetInt("qualityIndex");
    }
}
