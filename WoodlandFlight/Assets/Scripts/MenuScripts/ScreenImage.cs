using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.UIElements;

public class ScreenImage : MonoBehaviour
{
    public static ScreenImage Instance;

    [SerializeField] private UIDocument _document;
    private VisualElement image;

    void Awake()
    {
        // if (Instance != null)
        // {
        //     Destroy(gameObject);
        //     return;
        // }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        image = _document.rootVisualElement.Q<VisualElement>("Image");
    }

    public void ShowForAmountOfTime(float holdTime, Texture2D img)
    {
        image.style.backgroundImage = new StyleBackground(img);
        StartCoroutine(Sequence(holdTime));
    }

    IEnumerator Sequence(float hold)
    {
        image.style.display = DisplayStyle.Flex;
        yield return new WaitForSeconds(hold);
        image.style.display = DisplayStyle.None;
    }
}