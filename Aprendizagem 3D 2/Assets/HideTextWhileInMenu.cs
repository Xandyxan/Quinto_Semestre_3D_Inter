using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideTextWhileInMenu : MonoBehaviour
{

    private void Start()
    {
        //We remove the methods from the delegate at the beggining to prevent it to run multiple times.
        #region Set Delegates
        GameManager.instance.removePlayerControlEvent -= HideUI;
        GameManager.instance.returnPlayerControlEvent -= ShowUI;
        GameManager.instance.removePlayerControlEvent += HideUI;
        GameManager.instance.returnPlayerControlEvent += ShowUI;
        #endregion
    }

    private void HideUI()
    {
        gameObject.SetActive(false);
    }

    private void ShowUI()
    {
        gameObject.SetActive(true);
    }
}
