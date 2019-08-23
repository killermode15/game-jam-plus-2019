using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VRTK;

public class LightningStrike : MonoBehaviour
{
    public VRTK_InteractGrab leftGrabControl, rightGrabControl;
    public SkillData SkillData;

    public UnityEvent OnSkillGrab;
    public UnityEvent OnSkillUnGrab;

    private VRTK_InteractGrab grabControl;
    private Transform clouds;
    private GameObject skillGrabbed;

    private void Update()
    {
        if (grabControl.IsGrabButtonPressed())
        {
            OnSkillGrab.Invoke();
        }
        else
        {
            OnSkillUnGrab.Invoke();
        }
    }

    public void PrepareSkill(GameObject _clouds, VRTK_InteractGrab control)
    {
        grabControl = control;
        clouds = _clouds.transform;
        skillGrabbed = SkillData.skillUseObject;
        skillGrabbed.SetActive(true);
        skillGrabbed.transform.parent = grabControl.transform;
        skillGrabbed.transform.position = Vector3.zero;
    }

    public void UseSkill()
    {
        Vector3 pos = new Vector3(grabControl.transform.position.x, clouds.position.y, grabControl.transform.position.z);
        GameObject go = Instantiate(SkillData.skillUseObject, pos, Quaternion.identity);
        Destroy(go, SkillData.skillDuration);
    }
}
