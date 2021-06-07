using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // this script contain events, wich affect the following scripts: PlayerController, HeadBobber, SelectionManager, Inspecao, Cellphone. 
    // Use it to control the game state, the actual scene or if the player can/can't move
    // Needs to be a singleton, so any script can call it's methods on the go

    #region ---Singleton Stuff---
    private static GameManager _instance;
    public static GameManager instance { get { return _instance; } }
    #endregion

    #region ---Delegates---
    public delegate void TakePlayerControl();               // tira controle da movimentação do player. 
    public TakePlayerControl removePlayerControlEvent;
    public delegate void ReturnPlayerControl();             // retorna o controle do player sobre sua movimentação.
    public ReturnPlayerControl returnPlayerControlEvent;

    public delegate void PauseGameTrue();                       // pausa o jogo
    public PauseGameTrue pauseGameTrue;
    public delegate void PauseGameFalse();
    public PauseGameFalse pauseGameFalse;

    public delegate void UpdateMessages();     // tira controle da movimentação do player. 
    public UpdateMessages updateMessagesEvent;
    #endregion
   
    [Header("Pause Menu Screens")]
    [SerializeField] private GameObject pauseMenuObject;
    [HideInInspector] public bool isPausedGame;
    [SerializeField] private GameObject homePauseMenu;
    [SerializeField] private GameObject[] secondaryPauseMenus;

    [Header("It's main screen?")]
    [SerializeField] private bool mainMenuScreen;

    [Header("Tasks")]
    [SerializeField] private Text taskText;

    private bool playerWasNotFree = false;
    private bool usingCellphone, playerInScene;
    [SerializeField] private PlayerController playerMovement;

    //----------------------------------------------------------------------------\\

    private void Awake()
    {
        if (_instance != null && _instance != this) Destroy(this.gameObject);
        else _instance = this;
        
        if(!mainMenuScreen)SetLockCursor(true);
        if (playerMovement == null) playerInScene = false;
    }
    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetButtonDown("Pause") && !mainMenuScreen)
        {
            if (!isPausedGame) SetPauseGame(true);
            else SetPauseGame(false);
        }
    }

    public void SetPauseGame(bool on)
    {
        if (on)
        {
            if (playerInScene)
            {
                if (!playerMovement.GetCanMove()) playerWasNotFree = true;
                usingCellphone = playerMovement.GetUsingCellphone();
            }
            Time.timeScale = 0;
            isPausedGame = true;
            pauseMenuObject.SetActive(true);
            homePauseMenu.SetActive(true);
            SetLockCursor(false);

            instance.pauseGameTrue();
            instance.removePlayerControlEvent();
           if(Cellphone.instance!= null) Cellphone.instance.SetIsPausedGame(true);
        }
        else
        {
            Time.timeScale = 1;
            isPausedGame = false;

            for (int i = 0; i < secondaryPauseMenus.Length; i++) secondaryPauseMenus[i].SetActive(false);
            pauseMenuObject.SetActive(false);
           
            instance.pauseGameFalse();
            if (!playerWasNotFree) 
            {
                instance.returnPlayerControlEvent();
            }
            if (!usingCellphone)
            {
                SetLockCursor(true);
            }

            if (Cellphone.instance!= null) Cellphone.instance.SetIsPausedGame(false);
            playerWasNotFree = false;
        }
    }

    #region ---Scene Management---
    public void PlayGame() { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); }
    public void LoadScene(int sceneNumber) { SceneManager.LoadScene(sceneNumber); }
    public void BacktoMenu() { SceneManager.LoadScene(0); }
    public void QuitGame() { Application.Quit(); }
    #endregion

    public void SetLockCursor(bool on)
    {
        if (on)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    public void SetPrefValue(string prefKey, int number)     // -> chamar no evento para setar o número de mensagens com o contato
    {
        PlayerPrefs.SetInt(prefKey, number);
    }

    public void ClearProgress()
    {
        PlayerPrefs.SetInt("levelAt", 2);
    }
    
    public void SetTaskText(string actualTask)  // scripts vão chamar pra colocar um "Pegue uma toalha" aparecendo no canto da tela
    {
        taskText.text = actualTask;
        taskText.gameObject.SetActive(true);
    }

    public void ConcludeCurrentTask()    // quando uma condição de task acaba, o script da task chama esse método
    {
        taskText.text = "";
        taskText.gameObject.SetActive(false);
    }
}
