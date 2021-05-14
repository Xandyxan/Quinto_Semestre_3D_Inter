using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cellphone : MonoBehaviour
{
    // this script contain events, wich affect the following scripts: PlayerController, HeadBobber, SelectionManager, Inspecao. 
    #region Singleton Stuff
    private static Cellphone _instance;
    public static Cellphone instance { get { return _instance; } }
    #endregion

    private Animator cellAnim;
    public bool cellOn;
    #region delegates
    public delegate void UsingCellMenu();     // apenas métodos com parametros bool podem ser inscritos nesse delegate. 
    public UsingCellMenu usingCellphoneEvent;             // Talvez mudar para um delegate sem parametros.
    public delegate void CloseCellMenu();
    public CloseCellMenu closeCellMenuEvent;
    #endregion

    // says if the player can open the cellphone menu, or not. In cases like an object being inspecionated, we dont want to open it.
    private bool inDialogue = false;
    private bool inspecting = false;

    [SerializeField] private List<Messages> MessagesContacts;
    private void Awake()
    {
        if(_instance!= null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        cellAnim = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!inspecting && !inDialogue)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                cellOn = !cellOn;
                if (cellOn)
                {
                    //start usingCellphoneEvent -> takes control out of the player, they cannot move nor control the camera. The mouse lock is disabled.
                    // The ? before the invoke checks if the delegate is not null.
                   // usingCellphoneEvent?.Invoke();
                    GameManager.instance.removePlayerControlEvent?.Invoke();
                }
                else
                {
                    //start closeCellMenu event -> gives control back to the player, they can now control the camera and move around the map. The mouse lock is activated.
                    //closeCellMenuEvent?.Invoke();
                    GameManager.instance.returnPlayerControlEvent?.Invoke();
                }
            }


            cellAnim.SetBool("CellOn", cellOn);
        }
       
    }

    // Setters
    public void SetInDialogue(bool value)
    {
        inDialogue = value;
    }

    public void SetInspecting(bool value)
    {
        inspecting = value;
    }

    public void RemoteCloseCellphone()  // used for the home button on the cellphone main screen, maybe we will change this option.
    {
        cellOn = false;
       // closeCellMenuEvent?.Invoke();
        GameManager.instance.returnPlayerControlEvent?.Invoke();
    }

    public void UpdateAllMessages()
    {
        foreach(Messages message in MessagesContacts){
            message.UpdateMessages();
        }
    }
}

// lookCursor fica falso, usingCellphone fica true. PlayerController becomes disabled.
// Quando o playerController é desabilitado, seria melhor fazer uma transição para a animação idle.
//Quando o celular é ativado, talvez seja interessante manter o headbobbing ativo, mas em idle. Caso fique estranho, transicionar para idle e desativar.
// Quando o celular é ativado, preciso desabilitar os scripts de seleção (selectionManager), headbobbing, playerController e etc.
// *Nota para o futuro: talvez também fazer condição de que as falas pausam quando o jogador abre o menu do celular, mas dai temos que ver como fica com o áudio.