using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSelect : MonoBehaviour
{
    private bool isInCollider = false;
    private GameObject selectedObject = null;

    //OVR input
    public OVRInput.Controller controller;
    private float triggerValue;
    private bool isSelected = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "GameBoard")
        {
            isInCollider = true;
            selectedObject = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "GameBoard")
        {
            isInCollider = false;
            selectedObject = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        triggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, controller);

        if (isInCollider) {
            if(!isSelected && triggerValue > 0.95f) {
                isSelected = true;
                selectedObject.transform.parent = transform;
                Rigidbody rb = selectedObject.GetComponent<Rigidbody>();
                rb.isKinematic = true;
                rb.useGravity = false;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            } else if (isSelected && triggerValue < 0.95f) {
                isSelected = false;
                selectedObject.transform.parent = null;
                Rigidbody rb = selectedObject.GetComponent<Rigidbody>();
                rb.isKinematic = false;
                rb.useGravity = true;
                rb.velocity = OVRInput.GetLocalControllerVelocity(controller);
                rb.angularVelocity = OVRInput.GetLocalControllerAngularVelocity(controller);
            }
        }
    }
}
