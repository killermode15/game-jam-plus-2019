using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensionHelper
{
    public static void SetPosition(this GameObject gameObject, Vector3 position)
    {
        gameObject.transform.position = position;
    }
}
