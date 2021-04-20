using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceScroll : MonoBehaviour
{
    [Header("Mask Image Values")]
    [SerializeField] RectTransform maskRect;
    private float maskPos, maskHeight;

    [Header("Scrolling Bounds")]
    [SerializeField] float newestMessagePos;
    [SerializeField] private float offsetTop = 485;
    [SerializeField] private float offsetBottom = 637;

    [SerializeField] private float scrollLimitMin; // rolar pras mensagens mais antigas
    [SerializeField] private float scrollLimitMax; // rolar pras mensagens mais recentes

    [SerializeField] private float scrollSpeed;  // sets the speed of the scrolling

    private RectTransform containerRectTransform, lastMessageRectTransform;
    Vector2 pos;

    private void Awake()
    {
        containerRectTransform = GetComponent<RectTransform>();
       // maskPos = maskRect.anchoredPosition;
    }
    private void FixedUpdate()  // gets input value from mouse scroll and changes the messages container transform acordingly
    {
        pos = containerRectTransform.anchoredPosition;
        pos.y -= Input.mouseScrollDelta.y * scrollSpeed; // passou no teste
        pos.y = Mathf.Clamp(pos.y, scrollLimitMin, scrollLimitMax); // ***rever lógica daqui
        containerRectTransform.anchoredPosition = pos;
    }

    private void OnEnable() // Position the most recent message on the bottom of the screen.
    {
        CalculateOffset(newestMessagePos);
        containerRectTransform.anchoredPosition = new Vector2(containerRectTransform.anchoredPosition.x, 0);
        pos.y = newestMessagePos * -1+ offsetBottom - 30f; // sem o offset, a posição deverá ser o centro da tela. Adicionamos o offset para a mensagem ir para baixo.
        containerRectTransform.anchoredPosition = new Vector2(containerRectTransform.anchoredPosition.x, pos.y);
        scrollLimitMax = pos.y;

        scrollLimitMin = -offsetBottom + 30;
        if (scrollLimitMin > scrollLimitMax) { scrollLimitMin = scrollLimitMax; }
    }

    private void CalculateOffset(float messagePos)
    {
        // offset = distancia do centro da tela + (heightMensagem/2) + sangria;
        float messageBoxHeight = lastMessageRectTransform.rect.height;
        // offsetBottom = -137 + 2 * messagePos;
        offsetBottom = -165.8f + Mathf.Abs(messageBoxHeight / 2);

    }

    // Setters       
    // these values will be set by the calculus made by the messages scrit to determine the distance between the newest message position and the oldest one.
    public void SetScrollLimitMin(float minPos) { scrollLimitMin = minPos; }

    public void SetScrollLimitMax(float maxPos) { scrollLimitMax = maxPos; }

    public void SetNewestMessagePos(float newMessagePos) { newestMessagePos = newMessagePos; }

    public void SetLastMessageRectTransform(RectTransform messageTransform)
    {
        lastMessageRectTransform = messageTransform;
    }
   


}
