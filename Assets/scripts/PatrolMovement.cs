using UnityEngine;

public class PatrolMovement : MonoBehaviour
{
    public enum MovementType
    {
        MoveTowards,
        Lerp,
        Smooth
    }

    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private MovementType movementType = MovementType.MoveTowards;
    [SerializeField] private float speed = 1f;

    private Transform target;
    private Vector3 smoothVelocity;

    private void Start()
    {
        target = pointB;
    }

    private void Update()
    {
        if (target == null)
        {
            return;
        }

        switch (movementType)
        {
            case MovementType.MoveTowards:
                MoveTowards();
                break;
            case MovementType.Lerp:
                MoveWithLerp();
                break;
            case MovementType.Smooth:
                MoveWithSmoothDamp();
                break;
        }

        if (Vector3.Distance(transform.position, target.position) < 0.05f)
        {
            target = target == pointA ? pointB : pointA;
            smoothVelocity = Vector3.zero;
        }
    }

    private void MoveTowards()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime);
    }

    private void MoveWithLerp()
    {
        transform.position = Vector3.Lerp(
            transform.position,
            target.position,
            speed * Time.deltaTime);
    }

    private void MoveWithSmoothDamp()
    {
        float smoothTime = 1f / Mathf.Max(speed, 0.01f);

        transform.position = Vector3.SmoothDamp(
            transform.position,
            target.position,
            ref smoothVelocity,
            smoothTime);
    }
}
