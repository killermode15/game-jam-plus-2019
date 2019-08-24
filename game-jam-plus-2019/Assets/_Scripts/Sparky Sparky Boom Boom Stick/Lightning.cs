using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Lightning : Skill
{
    [SerializeField] private GameObject lightningModel;

    [SerializeField] private VRTK_InteractGrab leftGrabControl;
    [SerializeField] private VRTK_InteractGrab rightGrabControl;

    private bool isUseable = true;
    private bool isLightningGrabbed = false;

    private bool isLeftActive, isRightActive;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isLightningGrabbed) return;

        if (isLeftActive)
        {
            if (leftGrabControl.IsGrabButtonPressed())
            {
                AttachLightningModel();
            }
        }

        if (isRightActive)
        {
            if (rightGrabControl.IsGrabButtonPressed())
            {
                AttachLightningModel();
            }
        }
    }

    public void AttachLightningModel()//object obj, ControllerInteractionEventArgs args)
    {
        Debug.Log("Attached");
        lightningModel.transform.parent = leftGrabControl.transform;
        lightningModel.SetPosition(Vector3.zero);
    }

    public void DetachLightningModel()//object obj, ControllerInteractionEventArgs args)
    {
        Debug.Log("Detached");
        lightningModel.transform.parent = null;
        lightningModel.SetPosition(new Vector3(0, -50, 0));
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("PlayerHand")) return;

        if (!isUseable) return;

        isLeftActive = VRTK_DeviceFinder.IsControllerLeftHand(other.gameObject);
        isRightActive = VRTK_DeviceFinder.IsControllerRightHand(other.gameObject);

        //if (leftGrabControl.IsGrabButtonPressed())
        //{

        //}
    }


}
