using UnityEngine;
using UnityEngine.InputSystem;

public class shout : MonoBehaviour
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

    void Start()
    {
        targets = GameObject.FindGameObjectsWithTag("Target");
        //rightController = InputDevices.GetdeviceAtXRNode(XRNode.RightHand);

        //pointer = gameObject.AddCompenent<LineRenderer>();
        //pointer.posi
        



    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
