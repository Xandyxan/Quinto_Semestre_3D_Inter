using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // this script contain events, wich affect the following scripts: PlayerController, HeadBobber, SelectionManager, Inspecao, Cellphone. 
    // Use it to control the game state, the actual scene or if the player can/can't move
    // Needs to be a singleton, so any script can call it's methods on the go
    #region Singleton Stuff
    private static GameManager _instance;
    public static GameManager instance { get { return _instance; } }
    #endregion

    #region delegates
    public delegate void TakePlayerControl();     // tira controle da movimentação do player. 
    public TakePlayerControl removePlayerControlEvent;            
    public delegate void ReturnPlayerControl();         // retorna o controle do player sobre sua movimentação.
    public ReturnPlayerControl returnPlayerControlEvent;

    public delegate void UpdateMessages();     // tira controle da movimentação do player. 
    public UpdateMessages updateMessagesEvent;
  
    #endregion
    //----------------------------------------------------------------------------\\
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadScene(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }

    public void BacktoMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetPrefValue(string prefKey, int number)     // -> chamar no evento para setar o número de mensagens com o contato
    {
        PlayerPrefs.SetInt(prefKey, number);
    }
}
