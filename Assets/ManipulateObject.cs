using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class ManipulateObject : StaticInstance<ManipulateObject>
{
    private MLInput.Controller controller;
    public GameObject selectedGameObject;
    public GameObject attachPoint;
    //public GameObject controllerObject;
    private bool trigger;
    void Start()
    {
        controller = MLInput.GetController(MLInput.Hand.Left);
    }

    void UpdateTriggerInfo()
    {
        if (controller.TriggerValue > 0.8f)
        {
            if (trigger)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit))
                {
                    if (hit.transform.gameObject.CompareTag("Interactable"))
                    {
                        selectedGameObject = hit.transform.gameObject;
                        selectedGameObject.GetComponent<Rigidbody>().useGravity = false;
                        attachPoint.transform.position = hit.transform.position;
                    }
                }
                trigger = false;
            }
        }

        if (controller.TriggerValue < 0.2f)
        {
            trigger = true;
            if (selectedGameObject != null)
            {
                selectedGameObject.GetComponent<Rigidbody>().useGravity = true;
                selectedGameObject = null;
            }
        }
    }

    public float rotationMultiply = 10f;
    void UpdateTouchPad()
    {
        if (controller.Touch1Active)
        {
            float x = controller.Touch1PosAndForce.x;
            float y = controller.Touch1PosAndForce.y;
            float force = controller.Touch1PosAndForce.z;

            if (force > 0)
            {
                if (x > 0.3 || x < -0.3)
                {
                    ChangeRotation(x);
                }

                if (y > 0.3 || y < -0.3)
                {
                    ChangeScale(y);
                }
            }
        }
    }

    public void ChangeScale(float value)
    {
        selectedGameObject.transform.localScale +=
            selectedGameObject.transform.localScale * (value * Time.deltaTime);
    }
    public void ChangeRotation(float value)
    {
        selectedGameObject.transform.Rotate(new Vector3(0, value * rotationMultiply, 0) * Time.deltaTime);
    }
    
    public void ChangeHeight(float value)
    {
        selectedGameObject.transform.Translate(new Vector3(0, value, 0) * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (selectedGameObject)
        {
            selectedGameObject.transform.position = attachPoint.transform.position;
            selectedGameObject.transform.rotation = gameObject.transform.rotation;
        }
        */
        //UpdateTriggerInfo();
        UpdateTouchPad();

    }
}
