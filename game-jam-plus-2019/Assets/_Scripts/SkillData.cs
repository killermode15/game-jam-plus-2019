using UnityEngine;
using UnityEngine.Events;
using VRTK;

[System.Serializable]
public class SkillData
{
    public float Cooldown;
    public bool Used;
    public GameObject skillHoldObject;
    public GameObject skillUseObject;
    public float skillDuration;

    //private void Update()
    //{
    //    if (Used)
    //    {
    //        cooldown = Time.deltaTime;
    //        if (cooldown >= Cooldown)
    //        {
    //            Used = false;
    //        }
    //    }


    //}

    //private void OnTriggerEnter(Collider col)
    //{
    //    switch (col.tag)
    //    {
    //        case "PlayerHand":
    //            if (!Used)
    //            {
    //                grabControl = col.GetComponent<VRTK_InteractGrab>();
    //                if (grabControl.IsGrabButtonPressed())
    //                {
    //                    OnSkillGrab.Invoke();
    //                }
    //            }

    //            break;

    //        case "Terrain":
    //            ActivateSkill(skillObj);
    //            break;
    //    }
    //}

    //public virtual void PrepareSkill() // call in OnSkillGrab
    //{
    //    transform.parent = grabControl.transform;
    //}

    //public virtual void ActivateSkill(GameObject skillPrefab)
    //{
    //    GameObject go = Instantiate(skillPrefab, 
    //        new Vector3(grabControl.transform.position.x, transform.position.y, grabControl.transform.position.z), 
    //        Quaternion.identity);

    //    Used = true;
    //    Destroy(go, skillDuration);
    //}
}
