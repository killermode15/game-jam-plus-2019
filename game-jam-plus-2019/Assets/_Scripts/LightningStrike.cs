using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VRTK;
using VRTK.Examples.Archery;

public class LightningStrike : MonoBehaviour
{



    #region old code
    //public SkillData SkillData;

    public Transform skillHidingDock;

    private VRTK_InteractGrab grabControl;
    [SerializeField] private Transform clouds;
    [SerializeField] private GameObject skillGrabbed;
    [SerializeField] private GameObject skillUsed;

    public UnityEvent OnSkillGrab;
    public UnityEvent OnSkillUngrab;

    private bool isSkillGrabbed;

    private void Update()
    {
        if (grabControl != null)
        {
            if (grabControl.IsGrabButtonPressed())
            {
                if (isSkillGrabbed) return;

                isSkillGrabbed = true;
                OnSkillGrab.Invoke();
            }
            else
            {
                if (!isSkillGrabbed) return;

                OnSkillUngrab.Invoke();
                isSkillGrabbed = false;
            }
        }
    }

    public void PrepareSkill(GameObject _clouds, VRTK_InteractGrab control)
    {
        Debug.Log(control);

        grabControl = control;
        clouds = _clouds.transform;

        //skillGrabbed = SkillData.skillUseObject;
        skillGrabbed.transform.parent = grabControl.transform;
        skillGrabbed.transform.localPosition = Vector3.zero;
        skillGrabbed.GetComponent<ParticleSystem>().Play();
    }

    // Activate skill 
    public void UseSkill()
    {
        Vector3 pos = new Vector3(grabControl.transform.position.x, clouds.position.y - 15f, grabControl.transform.position.z);

        //skillUsed = SkillData.skillUseObject;

        skillUsed.transform.position = pos;
        skillUsed.GetComponent<ParticleSystem>().Play();
    }

    // Hide skill objects
    public void DockSkill(Transform obj)
    {
        obj.position = skillHidingDock.position;
        obj.GetComponent<ParticleSystem>().Stop();
    }
    #endregion
}
