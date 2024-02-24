using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineRenderer;

    [SerializeField]
    private bool activated;

    [SerializeField] 
    private float laserThickness;

    [SerializeField]
    private Gradient laserColor;

    [SerializeField]
    private float laserLifeTime = 0.5f;

    private Interactable interactable;

    private EyeManager eyeManager;

    private void Start()
    {
        interactable = GetComponent<Interactable>();
        eyeManager = EyeManager.Instance;

        lineRenderer.enabled = false;
        lineRenderer.widthMultiplier = laserThickness;
        lineRenderer.colorGradient = laserColor;
    }

    private void FixedUpdate()
    {
        if (activated)
        {
            lineRenderer.SetPosition(1, transform.InverseTransformPoint(eyeManager.hitPosition));
        }
    }

    public void ToogleActivation()
    {
        activated = !activated;
        lineRenderer.enabled = activated;

        interactable.selected = false;
    }

    public void Shoot()
    {
        StartCoroutine(ShootDelay());
    }

    public void EnableLaser()
    {
        lineRenderer.enabled = true;
        activated = true;
    }

    public void DisableLaser()
    {
        lineRenderer.enabled = false;
        activated = false;
    }

    private IEnumerator ShootDelay()
    {
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(laserLifeTime);

        lineRenderer.enabled = false;
    }
}
