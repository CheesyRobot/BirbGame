using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFade : MonoBehaviour
{
    public static ScreenFade Instance;

    [SerializeField] private CanvasGroup canvasGroup;

    Coroutine currentFade;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void FadeIn(float duration)
    {
        StartCoroutine(Fade(1f, 0f, duration));
    }

    public void FadeOut(float duration)
    {
        StartCoroutine(Fade(0f, 1f, duration));
    }

    public void FadeOutHoldFadeIn(float fadeOutTime, float holdTime, float fadeInTime)
    {
        StartCoroutine(FadeSequence(fadeOutTime, holdTime, fadeInTime));
    }

    IEnumerator Fade(float from, float to, float duration)
    {
        float t = 0f;
        canvasGroup.alpha = from;

        while (t < duration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, t / duration);
            yield return null;
        }

        canvasGroup.alpha = to;
    }

    IEnumerator FadeSequence(float fadeOut, float hold, float fadeIn)
    {
        yield return Fade(0f, 1f, fadeOut);
        yield return new WaitForSeconds(hold);
        yield return Fade(1f, 0f, fadeIn);
    }
}
