﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool IsCaptured;

    public void Capture()
    {
        IsCaptured = true;
    }
}
