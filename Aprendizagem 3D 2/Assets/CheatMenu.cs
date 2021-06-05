using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheatMenu : MonoBehaviour
{
    [Header("SetSceneLoad")]
    [SerializeField] int sceneIndex; // usado pra setar a cena pra ser carregada nos botões numericos do menu de confugurações / cheats

    [Header("SetColetáveis")]
    [SerializeField] string keycollectablePref;
    [SerializeField] int prefValue;

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex); // setado pelo inspector 
    }

    public void SetItemKey(string itemKey)
    {  // coloca a string do item
        keycollectablePref = itemKey;
    }

    public void SetKeyValue(int itemValue)
    { // seta o valor do pref, se for 1, o jogador coletou o item
        prefValue = itemValue;
    }

    public void UpdateNewPref()
    { // Chamar após setar os valores

        PlayerPrefs.SetInt(keycollectablePref, prefValue);
    }
}
