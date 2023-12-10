using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSelect : MonoBehaviour
{
    private bool isControllerInCollider = false;
    private bool isObjectSelected = false;
    private GameObject selectedObject = null;


    public OVRInput.Controller controller;
    private float triggerValue;

    // Start is called before the first frame update
    void Start()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "GameBoard")
        {
            isControllerInCollider = true;
            selectedObject = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "GameBoard")
        {
            isControllerInCollider = false;
            selectedObject = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        triggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, controller);

        if (isControllerInCollider) {
            // Select object
            if(!isObjectSelected && triggerValue > 0.95f) {
                isObjectSelected = true;

                // Make the object a child of the controller
                selectedObject.transform.parent = transform;

                // Disable physics to follow the controller
                Rigidbody rb = selectedObject.GetComponent<Rigidbody>();
                rb.isKinematic = true;
                rb.useGravity = false;

                // Reset velocity
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            } 
            // Deselect object
            else if (isObjectSelected && triggerValue < 0.95f) {
                isObjectSelected = false;

                // Remove the object from the controller
                selectedObject.transform.parent = null;

                // Enable back physics
                Rigidbody rb = selectedObject.GetComponent<Rigidbody>();
                rb.isKinematic = false;
                rb.useGravity = true;

                // The object inherits the velocity of the controller (we can throw it)
                rb.velocity = OVRInput.GetLocalControllerVelocity(controller);
                rb.angularVelocity = OVRInput.GetLocalControllerAngularVelocity(controller);
            }
        }
    }
}
