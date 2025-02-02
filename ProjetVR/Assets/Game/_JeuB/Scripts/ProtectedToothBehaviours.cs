using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


public class ProtectedToothBehaviours : MonoBehaviour
{
    [Header("Tooth Characteristics")]
    [SerializeField] int Maxhealth;
    [SerializeField] int receivedDamagedOnHit;
    [SerializeField] UnityEvent onDamaged = new UnityEvent();
    [SerializeField] Material material2HP; //the material the tooth has at 2 hp
    [SerializeField] Material material1HP; //the material the tooth has at 1 hp

    [SerializeField] float shakeMagnitude = 0.1f;
    [SerializeField] float shakeSpeed = 50f;
    [SerializeField] float shakeDuration = 3f;

    private Vector3 originalPosition;

    public UnityEvent onDeath = new UnityEvent();
    public GameObject toothExplosion;
    private OutlineScale _outlineScaleEffect;
    private Outline _outlineEffect;
    private float _originalWidth = 5f;
    private float _finalWidth = 10f;
    private float _currentWidth;
    private float _speed = 0.5f;

    private int health;

    //variables for thomas trying to make outline work
    public float minValueOutline = 6f;
    public float maxValueOutline = 12f;
    public float duration = 1f;

    private float outlineValue; //the variable that constantly changes up and down
    private bool increasing = true;
    private float timer = 0f;

    void OnEnable()
    {
        originalPosition = transform.position;
        _outlineScaleEffect = GetComponent<OutlineScale>();
        _outlineEffect = GetComponent<Outline>();
        _outlineEffect.OutlineWidth = 0;

        health = Maxhealth;
    }

    public void Damaged()
    {
        if (health == 0)
            return;

        health = Mathf.Clamp(health - receivedDamagedOnHit, 0, Maxhealth);
        onDamaged.Invoke();

        if (health == 0)
        {
            onDeath.Invoke();

            Invoke(nameof(Explode), 2.05f);
            GameManager.Instance.ReloadGameMode(3);
            StartCoroutine(ShakeAndDie());
        }
    }

    public void OutlinePulsating()
    {
        timer += Time.deltaTime;

        if (timer >= duration)
        {
            timer = 0f;
            increasing = !increasing;
        }

        if (increasing)
        {
            outlineValue = Mathf.Lerp(minValueOutline, maxValueOutline, timer / duration);
        }
        else
        {
            outlineValue = Mathf.Lerp(maxValueOutline, minValueOutline, timer / duration);
        }

        _outlineEffect.OutlineWidth = outlineValue;
    }

    void Update()
    {
        if (health == 1)
        {
            OutlinePulsating();
        }
    }

    IEnumerator ShakeAndDie()
    {
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            Vector3 shakeOffset = Random.insideUnitSphere * shakeMagnitude;
            transform.position = originalPosition + shakeOffset;
            elapsedTime += Time.deltaTime * shakeSpeed;
            yield return null;
        }

        yield return null;
    }

    private void Explode()
    {
        Instantiate(toothExplosion, transform.parent); //the tooth explodes into many pieces
        gameObject.SetActive(false);
    }
}
