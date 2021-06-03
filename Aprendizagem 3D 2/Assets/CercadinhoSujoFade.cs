using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CercadinhoSujoFade : MonoBehaviour,IFade
{
    // Após alguns poucos segundos, o berço começa a borbulhar lama, até jorrar na cara de Mariana e a tela escurecer
    // fade out seguido de load da missão 2
    // Mover esse final pra um script fade próprio do cercadinho sujo, que é invocado após alguns segundos desde que o objeto foi ativado.

    private FadeImage fadeScript;
    [SerializeField] private ParticleSystem mudBubbles, mudShot;

    private void Awake()
    {
        fadeScript = FindObjectOfType<FadeImage>();
    }
    // Start is called before the first frame update
    void Start()
    {
        // bolhas de lamaParticles.Play();
        StartCoroutine(TriggerMudShot());
    }

    
    public void Fade()
    {
        GameManager.instance.removePlayerControlEvent?.Invoke();
        fadeScript.SetFadeIn(true);
        fadeScript.SetHasNextFade(false);
        fadeScript.SetHasSceneLoad(true);
        fadeScript.SetSceneIndex(5);

        fadeScript.StartCoroutine(fadeScript.Fade(1.2f));
    }

    private IEnumerator TriggerMudShot()
    {
        yield return new WaitForSeconds(5);
      //  print("Borbulhando");
        mudBubbles.Play();
        yield return new WaitForSeconds(2);
       // print("Jato de lama");
        mudShot.Play();
        yield return new WaitForSeconds(1f);
        Fade();
        yield return null;
    }
}
