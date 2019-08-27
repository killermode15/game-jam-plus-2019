using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class StartGame : MonoBehaviour
{
    public GameSequence GameSequence;

    [SerializeField] private VRTK_InteractGrab grabControl;

    private void OnTriggerStay(Collider other)
    {
        if(!other.CompareTag("PlayerHand")) return;

        grabControl = other.transform.parent.parent.parent.GetComponent<VRTK_InteractGrab>();

        if (grabControl.IsGrabButtonPressed())
        {

            GameSequence.Begin = true;
        }
    }
}
