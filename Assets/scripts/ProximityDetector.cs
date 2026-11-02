using UnityEngine;
using TMPro;

public class ProximityDetector : MonoBehaviour
{
    [SerializeField] private Transform controller;
    [SerializeField] private GameObject[] cubes;
    [SerializeField] private Color proximityColor = Color.green;
    [SerializeField] private float triggerDistance = 0.2f;
    [SerializeField] private float scaleCoefficient = 1.2f;
    [SerializeField] private float angleThreshold = 20f;

    private Renderer[] renderers;
    private Color[] originalColors;
    private Vector3[] originalScales;
    private LineRenderer line;
    private TextMeshPro text;

    private void Start()
    {
        renderers = new Renderer[cubes.Length];
        originalColors = new Color[cubes.Length];
        originalScales = new Vector3[cubes.Length];
        line = gameObject.AddComponent<LineRenderer>();
        line.positionCount = 2;
        line.startWidth = 0.01f;
        line.endWidth = 0.01f;
        line.material = new Material(Shader.Find("Sprites/Default"));
        line.material.color = Color.white;

        GameObject textObject = new GameObject("Distance and Angle");
        textObject.transform.SetParent(controller);
        textObject.transform.localPosition = new Vector3(0f, 0.15f, 0f);
        text = textObject.AddComponent<TextMeshPro>();
        text.fontSize = 0.08f;
        text.alignment = TextAlignmentOptions.Center;
        text.color = Color.white;

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

        float angle = Vector3.Angle(
            controller.forward,
            cubes[closestCube].transform.position - controller.position);
        bool isPointed = angle <= angleThreshold;

        for (int i = 0; i < cubes.Length; i++)
        {
            renderers[i].material.color = originalColors[i];
            cubes[i].transform.localScale = originalScales[i];
        }

        if (shortestDistance <= triggerDistance)
        {
            renderers[closestCube].material.color = proximityColor;
        }

        if (shortestDistance <= triggerDistance && isPointed)
        {
            cubes[closestCube].transform.localScale =
                originalScales[closestCube] * scaleCoefficient;
        }

        text.text = "Distance : " + shortestDistance.ToString("F2") +
            " m\nAngle : " + angle.ToString("F1") + "°";

        line.enabled = isPointed;

        if (isPointed)
        {
            line.SetPosition(0, controller.position);
            line.SetPosition(1, cubes[closestCube].transform.position);
        }
    }
}
