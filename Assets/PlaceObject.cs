using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.MagicLeap;

public class PlaceObject : MonoBehaviour
{
    public GameObject objectToPlace;
    public GameObject marker;

    private MLInputDevice controller;
    public GameObject Mesh;
    
    // Start is called before the first frame update
    void Start()
    {
        MLInput.OnControllerButtonDown += OnButtonDown;
        MLInput.OnControllerButtonUp += OnButtonUp;
    }

    private void OnButtonUp(byte controllerid, MLInput.Controller.Button button)
    {
        if (button == MLInput.Controller.Button.Bumper)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                //GameObject placeObject = Instantiate(ObjectToPlace, hit.point, Quaternion.Euler(hit.normal));
                objectToPlace.transform.position = hit.point;
            }
            Mesh.SetActive(false);
        }
    }

    private void OnButtonDown(byte controllerid, MLInput.Controller.Button button)
    {
        if (button == MLInput.Controller.Button.Bumper)
        {
            Mesh.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        MLInput.OnControllerButtonDown -= OnButtonDown;
        MLInput.OnControllerButtonUp -= OnButtonUp;
    }


    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            marker.transform.position = hit.point;
        }
    }
}
