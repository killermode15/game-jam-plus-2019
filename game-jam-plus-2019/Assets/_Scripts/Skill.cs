using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;
using VRTK;

public class Skill : MonoBehaviour
{
    public float Cooldown;
    public bool Used;
    public GameObject skillObj;
    public float skillDuration;
    public UnityEvent OnSkillGrab;

    private float cooldown;
    private VRTK_InteractGrab grabControl;

    private void Update()
    {
        if (Used)
        {
            cooldown = Time.deltaTime;
            if (cooldown >= Cooldown)
            {
                Used = false;
            }
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        switch (col.tag)
        {
            case "PlayerHand":
                if (!Used)
                {
                    grabControl = col.GetComponent<VRTK_InteractGrab>();
                    if (grabControl.IsGrabButtonPressed())
                    {
                        OnSkillGrab.Invoke();
                    }
                }
                
                break;

            case "Terrain":
                ActivateSkill(skillObj);
                break;
        }
    }

    public virtual void PrepareSkill()
    {
        //when player grabs
    }

    public virtual void ActivateSkill(GameObject skillPrefab)
    {
        GameObject go = Instantiate(skillPrefab, transform.parent = null);
        Used = true;
        Destroy(go, skillDuration);
    }

    private void SkillOnHand(VRTK_InteractGrab control)
    {
        this.transform.parent = control.transform;
    }
}
