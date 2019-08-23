using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class DetectPlayerHand : MonoBehaviour
{
    public LightningStrike LightningStrike;

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("PlayerHand"))
        {
            LightningStrike.PrepareSkill(gameObject, col.GetComponent<VRTK_InteractGrab>());
        }
    }
}
