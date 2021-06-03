using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    public Button[] lvlButtons;

    // Start is called before the first frame update
    void Awake()
    {
        int level = PlayerPrefs.GetInt("levelAt", 2); //o O valor do número é equivalente ao primeiro level em build settings

        for(int i = 0; i < lvlButtons.Length; i++)
        {
            if(i + 2 > level)
            {
                //Mostra todos os outros níveis bloqueados
                lvlButtons[i].interactable = false;

            }
            else
            {
                lvlButtons[i].interactable = true;

            }
        }
    }

    public void ResetSave()
    {
        PlayerPrefs.SetInt("levelAt", 2);
    }
}
