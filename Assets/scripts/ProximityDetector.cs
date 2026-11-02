using UnityEngine;

public class ProximityDetector : MonoBehaviour
{
    [SerializeField] private Transform controller;
    [SerializeField] private GameObject[] cubes;
    [SerializeField] private Color proximityColor = Color.green;
    [SerializeField] private float triggerDistance = 0.2f;
    [SerializeField] private float scaleCoefficient = 1.2f;

    private Renderer[] renderers;
    private Color[] originalColors;
    private Vector3[] originalScales;

    private void Start()
    {
        renderers = new Renderer[cubes.Length];
        originalColors = new Color[cubes.Length];
        originalScales = new Vector3[cubes.Length];

        for (int i = 0; i < cubes.Length; i++)
        {
            renderers[i] = cubes[i].GetComponent<Renderer>();
            originalColors[i] = renderers[i].material.color;
            originalScales[i] = cubes[i].transform.localScale;
        }
    }

    private void Update()
    {
        int closestCube = -1;
        float shortestDistance = Mathf.Infinity;

        for (int i = 0; i < cubes.Length; i++)
        {
            float distance = Vector3.Distance(controller.position, cubes[i].transform.position);

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestCube = i;
            }
        }

        for (int i = 0; i < cubes.Length; i++)
        {
            renderers[i].material.color = originalColors[i];
            cubes[i].transform.localScale = originalScales[i];
        }

        if (shortestDistance <= triggerDistance)
        {
            renderers[closestCube].material.color = proximityColor;
            cubes[closestCube].transform.localScale =
                originalScales[closestCube] * scaleCoefficient;
        }
    }
}
