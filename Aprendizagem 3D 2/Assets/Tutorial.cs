using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour, ITask
{
    [Header("Tasks")]
    private string _taskHUDText;
    public string taskHUDText { get => _taskHUDText; set => _taskHUDText = value; }

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
        SetTaskText("・Utilize WASD para andar");
        yield return new WaitForSeconds(3f);
        UpdateTaskHUD();
        yield return waitForKeyPress(KeyCode.W);
        yield return new WaitForSeconds(1.5f);
        SetTaskText("・Utilize Ctrl para agachar");
        UpdateTaskHUD();
        yield return waitForKeyPress(KeyCode.LeftControl);
        yield return new WaitForSeconds(1.5f);
        SetTaskText("・Use o botão direito para zoom");
        UpdateTaskHUD();
        yield return waitForKeyPress(KeyCode.Mouse1);
        yield return new WaitForSeconds(1.5f);
        SetTaskText("・Use o clique esquerdo para interagir com os objetos");
        UpdateTaskHUD();
        yield return waitForKeyPress(KeyCode.Mouse0);
        GameManager.instance.ConcludeCurrentTask();
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

    }
