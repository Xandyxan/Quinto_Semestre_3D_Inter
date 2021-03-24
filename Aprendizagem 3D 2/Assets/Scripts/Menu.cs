using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    
    void Start()
    {
        
    }

    void Update()
    {

        
        
    }

    public void PlayGame()      
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void BacktoMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void QuitGame()
    {
      
        Application.Quit();
    }

}
