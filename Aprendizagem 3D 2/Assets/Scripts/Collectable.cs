using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [Tooltip("Tag that will be written on the inventory prefs")]
    [SerializeField] private string itemTag;

    private void Awake()
    {
        if (PlayerPrefs.HasKey(itemTag)) PlayerPrefs.DeleteKey(itemTag);
    }
    public void CollectItem()
    {
        PlayerPrefs.SetInt(itemTag, 1);
        gameObject.SetActive(false);
    }
}
