using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using VRTK.Examples.Archery;

public class DetectPlayerHand : MonoBehaviour
{
    public LightningStrike LightningStrike;

    public GameObject arrowPrefab;
    public float spawnDelay = 1f;

    private float spawnDelayTimer = 0f;
    private BowAim bow;

    private void Start()
    {
        spawnDelayTimer = 0f;
    }

    private void OnTriggerStay(Collider collider)
    {
        VRTK_InteractGrab grabbingController = (collider.gameObject.GetComponent<VRTK_InteractGrab>() ? collider.gameObject.GetComponent<VRTK_InteractGrab>() : collider.gameObject.GetComponentInParent<VRTK_InteractGrab>());
        if (CanGrab(grabbingController) && NoLightningAttached(grabbingController.gameObject) && Time.time >= spawnDelayTimer)
        {
            GameObject newArrow = Instantiate(arrowPrefab);
            newArrow.name = "ArrowClone";
            grabbingController.GetComponent<VRTK_InteractTouch>().ForceTouch(newArrow);
            grabbingController.AttemptGrab();
            spawnDelayTimer = Time.time + spawnDelay;
        }
    }

    private bool CanGrab(VRTK_InteractGrab grabbingController)
    {
        return (grabbingController && grabbingController.GetGrabbedObject() == null && grabbingController.IsGrabButtonPressed());
    }

    private bool NoLightningAttached(GameObject controller)
    {
        if (VRTK_DeviceFinder.IsControllerLeftHand(controller))
        {
            GameObject controllerRightHand = VRTK_DeviceFinder.GetControllerRightHand(true);
            controllerRightHand.transform.Find("LightningStrike");
        }
        else if (VRTK_DeviceFinder.IsControllerRightHand(controller))
        {
            GameObject controllerLeftHand = VRTK_DeviceFinder.GetControllerLeftHand(true);
            controllerLeftHand.transform.Find("LightningStrike");
        }

        return (bow == null || !bow.HasArrow());
    }
}
