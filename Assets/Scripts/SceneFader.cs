using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    #region Singleton

    static SceneFader _instance;
    public static SceneFader Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("Scene Fader does not exist!");

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    #endregion

    [SerializeField]
    Image _img;

    public AnimationCurve curve;


    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    // FadeTo after starting FadeIn
    public void FadeTo(string scene)
    {
        StartCoroutine(FadeOut(scene));
    }

    public void FadeTo(int scene)
    {
        StartCoroutine(FadeOut(scene));
    }

    IEnumerator FadeIn()
    {
        float t = 1f;

        while(t > 0f)
        {
            t -= Time.deltaTime;
            float a = curve.Evaluate(t);
            _img.color = new Color(0f, 0f, 0f, a);

            yield return 0;
        }
    }

    IEnumerator FadeOut(string scene)
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime;
            float a = curve.Evaluate(t);
            _img.color = new Color(0f, 0f, 0f, a);

            yield return 0;
        }

        SceneManager.LoadScene(scene);
    }

    IEnumerator FadeOut(int scene)
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime;
            float a = curve.Evaluate(t);
            _img.color = new Color(0f, 0f, 0f, a);

            yield return 0;
        }

        SceneManager.LoadScene(scene);
    }
}
