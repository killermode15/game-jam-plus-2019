using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VRTK;
using VRTK.Examples.Archery;

public class LightningSkill : MonoBehaviour
{
    public LayerMask lightningMask;
    public GameObject lightningPrefab;
    public GameObject lightningFXPrefab;
    public float spawnDelay = 1f;
    public float range;

    private float spawnDelayTimer = 0f;
    private BowAim bow;

    private bool isLightningGrabbed;
    private VRTK_InteractGrab lastGrabController;
    private GameObject spawnedLightning;
    private Vector3 areaHit;

    public UnityEvent OnGrabRelease;

    private void Start()
    {
        spawnDelayTimer = 0f;
    }

    private void Update()
    {
        if (!isLightningGrabbed) return;

        if (!lastGrabController) return;

        Debug.Log("TEST");

        lastGrabController.GetGrabbedObject().transform.localPosition = Vector3.zero;

        if (!lastGrabController.GetComponent<VRTK_ControllerEvents>()
            .IsButtonPressed(VRTK_ControllerEvents.ButtonAlias.GripPress))
        {
            lastGrabController.ForceRelease();
            isLightningGrabbed = false;

            OnGrabRelease.Invoke();
            Debug.Log("RELEASED");
        }
    }
    
    private void OnTriggerStay(Collider collider)
    {
        VRTK_InteractGrab grabbingController = (collider.gameObject.GetComponent<VRTK_InteractGrab>() ? collider.gameObject.GetComponent<VRTK_InteractGrab>() : collider.gameObject.GetComponentInParent<VRTK_InteractGrab>());
        if (CanGrab(grabbingController) && NoLightningAttached(grabbingController.gameObject) && Time.time >= spawnDelayTimer)
        {
            lastGrabController = grabbingController;
            isLightningGrabbed = true;
            spawnedLightning = Instantiate(lightningPrefab);
            
            spawnedLightning.name = "LightningBoltClone";
            //lightningPrefab.SetPosition(grabbingController.transform.position);
            grabbingController.GetComponent<VRTK_InteractTouch>().ForceTouch(spawnedLightning);
            grabbingController.AttemptGrab();
            spawnedLightning.transform.localPosition = Vector3.zero;
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
            controllerRightHand.transform.Find("LightningBoltClone");
        }
        else if (VRTK_DeviceFinder.IsControllerRightHand(controller))
        {
            GameObject controllerLeftHand = VRTK_DeviceFinder.GetControllerLeftHand(true);
            controllerLeftHand.transform.Find("LightningBoltClone");
        }

        return (bow == null || !bow.HasArrow());
    }


    public void CastLightning() //Sparky boom stick
    {
        Destroy(spawnedLightning);

        Vector3 pos = new Vector3(lastGrabController.transform.position.x, 40f, lastGrabController.transform.position.z);

        GameObject lightningFX = Instantiate(lightningFXPrefab, pos, Quaternion.identity);
        lightningFX.GetComponent<ParticleSystem>().Play();
        Destroy(lightningFX, 1);

        Ray ray = new Ray(pos + new Vector3(0,20,0), Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, lightningMask, QueryTriggerInteraction.Ignore))
        {
            Debug.Log(hit.collider.gameObject.name);
            areaHit = hit.point;
            Collider[] detected = Physics.OverlapSphere(areaHit, range, lightningMask);

            foreach (Collider col in detected)
            {
                if (col.CompareTag("Enemy"))
                {
                    col.GetComponent<AIBehaviour>().DestroyEnemy();
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(areaHit, range);
    }
}
