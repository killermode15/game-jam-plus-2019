using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VRTK;
using VRTK.Examples.Archery;
using VRTK.UnityEventHelper;

public class LightningStrike : MonoBehaviour
{
    [SerializeField] private VRTK_ControllerEvents_UnityEvents leftControllerEvents;
    [SerializeField] private VRTK_InteractGrab_UnityEvents rightControllerEvents;

    private VRTK_InteractGrab grabControl;
    private VRTK_InteractGrab_UnityEvents grabControlEvents;

    private bool isUsable;
    private bool isLightningGrabbed;

    private void Start()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(!other.CompareTag("PlayerHand")) return;
            
        if(!isUsable) return;

        grabControl = other.GetComponentInParent<VRTK_InteractGrab>();

    }

    

    //#region old code
    ////public SkillData SkillData;

    //public Transform skillHidingDock;

    //private VRTK_InteractGrab grabControl;
    //[SerializeField] private Transform clouds;
    //[SerializeField] private GameObject skillGrabbed;
    //[SerializeField] private GameObject skillUsed;

    //public UnityEvent OnSkillGrab;
    //public UnityEvent OnSkillUngrab;

    //private bool isSkillGrabbed;

    //private void Update()
    //{
    //    if (grabControl != null)
    //    {
    //        if (grabControl.IsGrabButtonPressed())
    //        {
    //            if (isSkillGrabbed) return;

    //            isSkillGrabbed = true;
    //            OnSkillGrab.Invoke();
    //        }
    //        else
    //        {
    //            if (!isSkillGrabbed) return;

    //            OnSkillUngrab.Invoke();
    //            isSkillGrabbed = false;
    //        }
    //    }
    //}

    //public void PrepareSkill(VRTK_InteractGrab control)
    //{
    //    Debug.Log(control);

    //    grabControl = control;

    //    //skillGrabbed = SkillData.skillUseObject;
    //    skillGrabbed.transform.parent = grabControl.transform;
    //    skillGrabbed.transform.localPosition = Vector3.zero;
    //    skillGrabbed.GetComponent<ParticleSystem>().Play();
    //}

    //// Activate skill 
    //public void UseSkill()
    //{
    //    Vector3 pos = new Vector3(grabControl.transform.position.x, clouds.position.y - 15f, grabControl.transform.position.z);

    //    //skillUsed = SkillData.skillUseObject;

    //    skillUsed.transform.position = pos;
    //    skillUsed.GetComponent<ParticleSystem>().Play();
    //}

    //// Hide skill objects
    //public void DockSkill(Transform obj)
    //{
    //    obj.position = skillHidingDock.position;
    //    obj.GetComponent<ParticleSystem>().Stop();
    //}
    //#endregion
}
