using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial2 : MonoBehaviour
{
    [Header("Tasks")]
    private string _taskHUDText;
    public string taskHUDText { get => _taskHUDText; set => _taskHUDText = value; }

    [SerializeField] private PlayerController mariana;
    public void UpdateTaskHUD()
    {
        if (taskHUDText != "")
        {
            GameManager.instance.SetTaskText(taskHUDText);
        }
    }

    private void SetTaskText(string text)
    {
        taskHUDText = text;
    }

    private void Start()
    {
        StartCoroutine(TutCoroutine());
    }

    private IEnumerator TutCoroutine()
    {
        yield return new WaitForSeconds(3f);
        SetTaskText("・Pressione F para ligar a lanterna");
        UpdateTaskHUD();
        yield return waitForKeyPress(KeyCode.F);
        GameManager.instance.ConcludeCurrentTask();
        yield return new WaitForSeconds(2.5f);
        SetTaskText("・Utilize espaço para acessar o celular");
        UpdateTaskHUD();
        yield return waitForKeyPress(KeyCode.Space);
        GameManager.instance.ConcludeCurrentTask();
        yield return waitForCellphoneOff();
        yield return new WaitForSeconds(1.5f);
        SetTaskText("・Encontre a chave");
        UpdateTaskHUD();
    }

    private IEnumerator waitForKeyPress(KeyCode key)
    {
        bool done = false;
        while (!done) // essentially a "while true", but with a bool to break out naturally
        {
            if (Input.GetKeyDown(key))
            {
                done = true; // breaks the loop
            }
            yield return null; // wait until next frame, then continue execution from here (loop continues)
        }
    }

    private IEnumerator waitForCellphoneOff()
    {
        bool done = false;
        while (!done) // essentially a "while true", but with a bool to break out naturally
        {
            if (mariana.GetUsingCellphone() == false)
            {
                done = true; // breaks the loop
            }
            yield return null; // wait until next frame, then continue execution from here (loop continues)
        }
    }

}
