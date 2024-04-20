using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class FeedbackScale : MonoBehaviour
{
    [SerializeField] [Range(0f, 1f)] float scaleLerpSpeed = 0.15f;

    private const float scaleOffset = 1.2f;

    private Vector3 originalScale;

    public UnityEvent FBFinish;

    public bool IsScaling => transform.localScale != originalScale;

    private bool active = true;
    public bool Active
    {
        get { return active; }
        set
        {
            if (!value)
            {
                ForceStop();
            }

            active = value;
        }
    }

    public void SetActive(bool value)
    {
        Active = value;
    }

    private void OnEnable()
    {
        originalScale = transform.localScale;
    }

    private void OnDisable()
    {
        Debug.LogWarning("do not desable please use the Active property instead");

        ForceStop();
    }

    private IEnumerator ScaleTimer(Vector3 targetScale)
    {
        while (transform.localScale != targetScale)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, scaleLerpSpeed);
            yield return null;
        }

        FBFinish?.Invoke();
    }

    public void ScaleIn()
    {
        StopAllCoroutines();

        Vector3 targetScale = originalScale * scaleOffset;
        if (gameObject.activeInHierarchy)
            StartCoroutine(ScaleTimer(targetScale));
    }

    public void ScaleOut()
    {
        StopAllCoroutines();

        if (gameObject.activeInHierarchy)
            StartCoroutine(ScaleTimer(originalScale));
    }

    public void ForceStop()
    {
        StopAllCoroutines();
        transform.localScale = originalScale;
    }
}
