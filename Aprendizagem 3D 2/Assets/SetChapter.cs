using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetChapter : MonoBehaviour
{
    public int keyIndex;

    private void Start()
    {
        if(PlayerPrefs.HasKey("levelAt")) PlayerPrefs.SetInt("levelAt", keyIndex);
    }
}
