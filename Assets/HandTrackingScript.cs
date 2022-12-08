using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.MagicLeap;

public class HandTrackingScript : MonoBehaviour
{
    public enum HandPoses
    {
        Ok,
        Finger,
        Thumb,
        OpenHand,
        Pinch,
        NoPose
    };

    public HandPoses leftHandPose = HandPoses.NoPose;
    public HandPoses rightHandPose = HandPoses.NoPose;
    public Vector3[] leftPos;
    public Vector3[] rightPos;

    public GameObject sphereThumbLeft;
    public GameObject sphereIndexLeft;
    public GameObject sphereWristLeft;

    public GameObject sphereThumbRight;
    public GameObject sphereIndexRight;
    public GameObject sphereWristRight;

    [FormerlySerializedAs("leftPointHand")]
    public Transform leftHandTransform;

    [FormerlySerializedAs("rightPointHand")]
    public Transform rightHandTransform;

    private float oldDistance;

    private MLHandTracking.HandKeyPose[] _gestures;

    private void Start()
    {
        _gestures = new MLHandTracking.HandKeyPose[5];
        _gestures[0] = MLHandTracking.HandKeyPose.Ok;
        _gestures[1] = MLHandTracking.HandKeyPose.Finger;
        _gestures[2] = MLHandTracking.HandKeyPose.OpenHand;
        _gestures[3] = MLHandTracking.HandKeyPose.Pinch;
        _gestures[4] = MLHandTracking.HandKeyPose.Thumb;
        MLHandTracking.KeyPoseManager.EnableKeyPoses(_gestures, true, false);
        leftPos = new Vector3[3];
        rightPos = new Vector3[3];
    }

    private void Update()
    {
        leftHandTransform.position = leftPos[1];
        rightHandTransform.position = rightPos[1];

        UpdateHandPose();
        ShowPoints();

        if (leftHandPose == HandPoses.Pinch)
        {
            sphereThumbLeft.gameObject.GetComponent<Renderer>().material.color = Color.red;
            sphereIndexLeft.gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
        else if (leftHandPose == HandPoses.NoPose)
        {
            sphereThumbLeft.gameObject.GetComponent<Renderer>().material.color = Color.white;
            sphereIndexLeft.gameObject.GetComponent<Renderer>().material.color = Color.white;
        }

        if (rightHandPose == HandPoses.Pinch)
        {
            sphereThumbRight.gameObject.GetComponent<Renderer>().material.color = Color.red;
            sphereIndexRight.gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
        else if (rightHandPose == HandPoses.NoPose)
        {
            sphereThumbRight.gameObject.GetComponent<Renderer>().material.color = Color.white;
            sphereIndexRight.gameObject.GetComponent<Renderer>().material.color = Color.white;
        }

        if (leftHandPose == HandPoses.Pinch)
            if (rightHandPose == HandPoses.Pinch)
            {
                var newDistance = leftHandTransform.position - rightHandTransform.position;

                if (oldDistance > newDistance.magnitude)
                    ManipulateObject.Instance.ChangeScale(-1);
                else
                    ManipulateObject.Instance.ChangeScale(1);

                oldDistance = newDistance.magnitude;
            }
    }

    private void UpdateHandPose()
    {
        if (GetGesture(MLHandTracking.Left, MLHandTracking.HandKeyPose.Ok))
        {
            leftHandPose = HandPoses.Ok;
        }
        else if (GetGesture(MLHandTracking.Left, MLHandTracking.HandKeyPose.Finger))
        {
            leftHandPose = HandPoses.Finger;
        }
        else if (GetGesture(MLHandTracking.Left, MLHandTracking.HandKeyPose.OpenHand))
        {
            leftHandPose = HandPoses.OpenHand;
        }
        else if (GetGesture(MLHandTracking.Left, MLHandTracking.HandKeyPose.Pinch))
        {
            leftHandPose = HandPoses.Pinch;
        }
        else if (GetGesture(MLHandTracking.Left, MLHandTracking.HandKeyPose.Thumb))
        {
            leftHandPose = HandPoses.Thumb;
        }
        else
        {
            leftHandPose = HandPoses.NoPose;
        }

        if (GetGesture(MLHandTracking.Right, MLHandTracking.HandKeyPose.Ok))
        {
            rightHandPose = HandPoses.Ok;
        }
        else if (GetGesture(MLHandTracking.Right, MLHandTracking.HandKeyPose.Finger))
        {
            rightHandPose = HandPoses.Finger;
        }
        else if (GetGesture(MLHandTracking.Right, MLHandTracking.HandKeyPose.OpenHand))
        {
            rightHandPose = HandPoses.OpenHand;
        }
        else if (GetGesture(MLHandTracking.Right, MLHandTracking.HandKeyPose.Pinch))
        {
            rightHandPose = HandPoses.Pinch;
        }
        else if (GetGesture(MLHandTracking.Right, MLHandTracking.HandKeyPose.Thumb))
        {
            rightHandPose = HandPoses.Thumb;
        }
        else
        {
            rightHandPose = HandPoses.NoPose;
        }
    }

    private void ShowPoints()
    {
        // Left Hand Thumb tip
        leftPos[0] = MLHandTracking.Left.Thumb.KeyPoints[2].Position;
        // Left Hand Index finger tip 
        leftPos[1] = MLHandTracking.Left.Index.KeyPoints[2].Position;
        // Left Hand Wrist 
        leftPos[2] = MLHandTracking.Left.Wrist.KeyPoints[0].Position;
        sphereThumbLeft.transform.position = leftPos[0];
        sphereIndexLeft.transform.position = leftPos[1];
        sphereWristLeft.transform.position = leftPos[2];

        rightPos[0] = MLHandTracking.Right.Thumb.KeyPoints[2].Position;
        rightPos[1] = MLHandTracking.Right.Index.KeyPoints[2].Position;
        rightPos[2] = MLHandTracking.Right.Wrist.KeyPoints[0].Position;
        sphereThumbRight.transform.position = rightPos[0];
        sphereIndexRight.transform.position = rightPos[1];
        sphereWristRight.transform.position = rightPos[2];
    }

    private bool GetGesture(MLHandTracking.Hand hand, MLHandTracking.HandKeyPose type)
    {
        if (hand != null)
        {
            if (hand.KeyPose == type)
            {
                if (hand.HandKeyPoseConfidence > 0.9f)
                {
                    return true;
                }
            }
        }

        return false;
    }
}