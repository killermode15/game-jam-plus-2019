using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Earthquake : MonoBehaviour
{
    public VRTK_VelocityEstimator leftEstimator;
    public VRTK_VelocityEstimator rightEstimator;

    public float requiredSpeed;

    private bool canShake;
    private float leftHandSpeed;
    private float rightHandSpeed;

    private void Update()
    {
        leftHandSpeed = leftEstimator.GetVelocityEstimate().magnitude;
        rightHandSpeed = rightEstimator.GetVelocityEstimate().magnitude;

        if (leftHandSpeed >= requiredSpeed && rightHandSpeed >= requiredSpeed)
        {
            canShake = true;
        }
    }

    private void OnCollsionEnter(Collision other)
    {
        if(!other.collider.CompareTag("PlayerHand")) return;
        
        this.GetComponent<Animator>().Play("Earthquake");
    }
}
