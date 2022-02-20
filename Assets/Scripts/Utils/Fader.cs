using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

public class Fader : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup = null;

    private Coroutine currentActiveFade = null;

    private void Awake()
    {
        if (this.canvasGroup == null)
            this.canvasGroup = GetComponent<CanvasGroup>();
    }

    public void FadeOutImmediate()
    {
        this.canvasGroup.alpha = 1.0f;
    }

    public IEnumerator FadeOut(float time)
    {
        yield return Fade(1, time);
    }

    public IEnumerator FadeIn(float time)
    {
        yield return Fade(0, time);
    }

    public IEnumerator Fade(float target, float time)
    {
        if (this.currentActiveFade != null)
            StopCoroutine(this.currentActiveFade);

        this.currentActiveFade = StartCoroutine(FadeRoutine(target, time));
        yield return this.currentActiveFade;
    }

    public IEnumerator FadeRoutine(float target, float time)
    {
        while (!Mathf.Approximately(this.canvasGroup.alpha, target))
        {
            this.canvasGroup.alpha = Mathf.MoveTowards(this.canvasGroup.alpha, target, Time.deltaTime / time);
            yield return null;
        }
    }
}