using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagCapture : MonoBehaviour
{
    [SerializeField] private List<Transform> checkPoints;
    [SerializeField] private Material swapTextureMat;

    [SerializeField] private bool updateDebug;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnValidate()
    {
        if (updateDebug)
            Update();
    }
    

    // Update is called once per frame
    void Update()
    {
        List<Vector4> positions = new List<Vector4>(50);
        
        for(int i = 0; i < 50; i ++) positions.Add(new Vector4());

        for (int i = 50 - checkPoints.Count - 1; i >= 0; i--)
        {
            positions[i] = new Vector4(-9999, -9999, -9999);
        }

        //swapTextureMat.SetVectorArray("_Position", positions);

        for (int i = 0; i < checkPoints.Count; i++)
        {
            positions[i] = checkPoints[i].position;
        }

        swapTextureMat.SetVectorArray("_Position", positions);
    }
}
