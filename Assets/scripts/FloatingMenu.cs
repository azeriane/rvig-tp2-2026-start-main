using UnityEngine;
using TMPro;

public class FloatingMenu : MonoBehaviour
{
    private TMP_Text text;
    private Transform hand;
    private Transform quad;

    private void Start()
    {
        text = GetComponentInChildren<TMP_Text>();
        hand = transform.parent;
        quad = transform.Find("Quad");

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).localPosition = Vector3.zero;
        }

        text.transform.localPosition = new Vector3(0f, 0f, -0.01f);
        quad.localPosition = Vector3.zero;
        quad.localRotation = Quaternion.identity;
        quad.gameObject.SetActive(true);
    }

    private void Update()
    {
        transform.localPosition = new Vector3(0f, 0f, 0.15f);

        text.text = "Position locale : " + transform.localPosition +
                    "\nPosition globale : " + transform.position;
    }
}
