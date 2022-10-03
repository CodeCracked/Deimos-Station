using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeathScreenManager : MonoBehaviour
{
    public static DeathScreenManager Instance;
    public void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    [Header("Fade In")]
    public float FadeInDelay = 2.0f;
    public float FadeInTime = 1.0f;

    [Header("Components")]
    public Image Background;
    public TMP_Text DeathLabel;
    public GameObject Menu;

    public static void Show()
    {
        Instance.gameObject.SetActive(true);
        Instance.StartCoroutine(Instance.Show_Coroutine());
    }

    private IEnumerator Show_Coroutine()
    {
        yield return new WaitForSeconds(FadeInDelay);

        float timer = 0.0f;
        while (timer < FadeInTime)
        {
            timer = Mathf.Clamp(timer + Time.deltaTime, 0, FadeInTime);
            float alpha = timer / FadeInTime;

            Background.color = new Color(Background.color.r, Background.color.g, Background.color.b, alpha);
            DeathLabel.color = new Color(DeathLabel.color.r, DeathLabel.color.g, DeathLabel.color.b, alpha);
            yield return null;
        }

        if (Menu) Menu.SetActive(true);
    }
}