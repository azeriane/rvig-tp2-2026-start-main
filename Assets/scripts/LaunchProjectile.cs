using UnityEngine;

public class LaunchProjectile : MonoBehaviour
{
    private const string ProjectileTag = "Projectile";

    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform startPoint;
    [SerializeField] private float launchSpeed = 10f;
    [SerializeField] private float projectileLifetime = 10f;

    public void Fire()
    {
        if (projectilePrefab == null)
        {
            Debug.LogError("LaunchProjectile requires a Projectile Prefab.", this);
            return;
        }

        if (startPoint == null)
        {
            Debug.LogError("LaunchProjectile requires a Start Point.", this);
            return;
        }

        GameObject projectile = Instantiate(
            projectilePrefab,
            startPoint.position,
            startPoint.rotation);

        projectile.tag = ProjectileTag;

        if (!projectile.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
        {
            Debug.LogError(
                "The Projectile Prefab requires a Rigidbody component.",
                projectile);
            Destroy(projectile);
            return;
        }

        rigidbody.linearVelocity = startPoint.forward * launchSpeed;

        if (projectileLifetime > 0f)
        {
            Destroy(projectile, projectileLifetime);
        }
    }
}
