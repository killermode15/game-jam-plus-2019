using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Earthquake : MonoBehaviour
{
    public VRTK_VelocityEstimator leftEstimator;
    public VRTK_VelocityEstimator rightEstimator;

    private float leftHandSpeed;
    private float rightHandSpeed;

    private void Update()
    {
        leftHandSpeed = leftEstimator.GetVelocityEstimate().magnitude;
        rightHandSpeed = rightEstimator.GetVelocityEstimate().magnitude;
    }
}
