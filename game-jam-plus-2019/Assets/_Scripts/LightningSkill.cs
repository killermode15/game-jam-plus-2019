using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using VRTK;
using VRTK.Examples.Archery;

public class LightningSkill : MonoBehaviour
{
    public AudioManager audioManager;
    public LayerMask lightningMask;
    public GameObject lightningPrefab;
    public GameObject lightningFXPrefab;
    public GameObject lightningHitFXtPrefab;
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

        lastGrabController.GetGrabbedObject().transform.localPosition = Vector3.zero;

        if (!lastGrabController.GetComponent<VRTK_ControllerEvents>()
            .IsButtonPressed(VRTK_ControllerEvents.ButtonAlias.GripPress))
        {
            lastGrabController.ForceRelease();
            isLightningGrabbed = false;

            OnGrabRelease.Invoke();

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

        Ray ray = new Ray(pos + new Vector3(0, 20, 0), Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, lightningMask, QueryTriggerInteraction.Ignore))
        {

            AudioSource source = audioManager.GetAudio("Lightning").PlayAudio(randomPitch: true);

            Destroy(source, source.clip.length+1);

            areaHit = hit.point;
            Collider[] detected = Physics.OverlapSphere(areaHit, range);//, lightningMask);

            GameObject hitFx = Instantiate(lightningHitFXtPrefab, areaHit, Quaternion.identity);

            Destroy(hitFx, 2);

            foreach (Collider col in detected)
            {
                Rigidbody rb = col.GetComponent<Rigidbody>();
                NavMeshAgent agent = col.GetComponent<NavMeshAgent>();
                Animator anim = col.GetComponent<Animator>();

                if (col.CompareTag("Enemy"))
                {
                    #region Rigidbody version
                    /*
                    if (col.GetComponent<AIBehaviour>() != null)
                    {
                        col.GetComponent<AIBehaviour>().DestroyEnemy(2f);
                        col.GetComponent<AIBehaviour>().enabled = false; //Destroy(col.GetComponent<AIBehaviour>());
                    }

                    if (agent != null)
                    {   
                        agent.speed = 0;
                        
                        agent.isStopped = true;
                        agent.enabled = false;//Destroy(col.GetComponent<NavMeshAgent>());
                    }
                    
                    rb.constraints = RigidbodyConstraints.None;
                    rb.AddExplosionForce(100, areaHit, range, 100, ForceMode.VelocityChange);
                    */
                    #endregion

                    #region Animation Version

                    anim.SetBool("Kill", true);
                    col.GetComponent<AIBehaviour>().DestroyEnemy(2f);

                    #endregion

                }

                if (col.CompareTag("EnemyBoat"))
                {
                    #region Rigidbody Versoin

                    //if(col.GetComponent<BoatController>())
                    //    Destroy(col.GetComponent<BoatController>());

                    //rb.constraints = RigidbodyConstraints.None;
                    //rb.AddExplosionForce(100, areaHit, range, 100, ForceMode.VelocityChange);


                    //Destroy(col.gameObject, 2f);

                    #endregion

                    #region Animation Version

                    anim.SetBool("Kill", true);
                    Destroy(col.gameObject, 2f);

                    #endregion

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
