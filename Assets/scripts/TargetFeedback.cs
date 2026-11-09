using UnityEngine;

public class TargetFeedback : MonoBehaviour
{
    private const string ProjectileTag = "Projectile";

    [SerializeField] private Color hitColor = Color.red;
    [SerializeField] private float feedbackDuration = 0.15f;
    [SerializeField] private AudioClip hitClip;

    private Renderer targetRenderer;
    private Color originalColor;
    private Coroutine colorFeedbackCoroutine;

    private void Awake()
    {
        targetRenderer = GetComponent<Renderer>();

        if (targetRenderer != null)
        {
            originalColor = targetRenderer.material.color;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!IsProjectile(collision.collider))
        {
            return;
        }

        PlayVisualFeedback();
        PlayAudioFeedback();
    }

    private static bool IsProjectile(Collider collider)
    {
        if (collider.CompareTag(ProjectileTag))
        {
            return true;
        }

        if (collider.attachedRigidbody != null &&
            collider.attachedRigidbody.CompareTag(ProjectileTag))
        {
            return true;
        }

        return collider.transform.root.CompareTag(ProjectileTag);
    }

    private void PlayVisualFeedback()
    {
        if (targetRenderer == null)
        {
            return;
        }

        if (colorFeedbackCoroutine != null)
        {
            StopCoroutine(colorFeedbackCoroutine);
        }

        colorFeedbackCoroutine = StartCoroutine(FlashColor());
    }

    private System.Collections.IEnumerator FlashColor()
    {
        targetRenderer.material.color = hitColor;
        yield return new WaitForSeconds(feedbackDuration);
        targetRenderer.material.color = originalColor;
        colorFeedbackCoroutine = null;
    }

    private void PlayAudioFeedback()
    {
        if (hitClip == null)
        {
            Debug.LogWarning("TargetFeedback requires a Hit Clip.", this);
            return;
        }

        AudioSource.PlayClipAtPoint(hitClip, transform.position);
    }
}
