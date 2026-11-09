using UnityEngine;
using UnityEngine.XR;

public class ShootManager : MonoBehaviour
{
    [SerializeField] private Transform controller;
    [SerializeField] private float angleThreshold = 10f;
    [SerializeField] private GameObject hitParticlesPrefab;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private float pointerLength = 10f;

    private GameObject[] targets;
    private InputDevice rightController;
    private bool previousTriggerPressed;
    private LineRenderer pointer;

    private void Start()
    {
        targets = GameObject.FindGameObjectsWithTag("Target");
        rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        pointer = gameObject.AddComponent<LineRenderer>();
        pointer.positionCount = 2;
        pointer.startWidth = 0.01f;
        pointer.endWidth = 0.01f;
        pointer.material = new Material(Shader.Find("Sprites/Default"));
        pointer.material.color = Color.white;
    }

    private void Update()
    {
        if (controller == null)
        {
            return;
        }

        pointer.SetPosition(0, controller.position);
        pointer.SetPosition(1, controller.position + controller.forward * pointerLength);

        if (!rightController.isValid)
        {
            rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        }

        if (rightController.TryGetFeatureValue(
                CommonUsages.trigger, out float triggerValue))
        {
            bool triggerPressed = triggerValue > 0.1f;
            if (triggerPressed && !previousTriggerPressed)
            {
                Shoot();
            }
            previousTriggerPressed = triggerPressed;
        }
    }

    private void Shoot()
    {
        foreach (GameObject target in targets)
        {
            if (Vector3.Angle(
                    controller.forward,
                    target.transform.position - controller.position) > angleThreshold)
            {
                continue;
            }

            if (hitParticlesPrefab != null)
            {
                GameObject particles = Instantiate(hitParticlesPrefab, target.transform.position,
                    Quaternion.identity);
                ParticleSystem particleSystem = particles.GetComponent<ParticleSystem>();
                if (particleSystem != null)
                {
                    particleSystem.Play();
                    Destroy(particles, particleSystem.main.duration +
                        particleSystem.main.startLifetime.constantMax);
                }
            }

            AudioSource audioSource = target.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.PlayOneShot(hitSound != null ? hitSound : audioSource.clip);
            }

            break;
        }
    }
}
