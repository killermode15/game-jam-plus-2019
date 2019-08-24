using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagCapture : MonoBehaviour
{
    [SerializeField] private List<Transform> checkPoints;
    [SerializeField] private Material swapTextureMat;
    [SerializeField] private Shader swapTexShader;

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
        swapTextureMat.SetVectorArray("_Position", new Vector4[50]);
        Vector4[] positions = swapTextureMat.GetVectorArray("_Position");
        

        for (int i = 50 - checkPoints.Count - 1; i >= 0; i--)
        {
            positions[i] = new Vector4(-9999, -9999, -9999);
        }

        //swapTextureMat.SetVectorArray("_Position", positions);

        for (int i = 0; i < checkPoints.Count; i++)
        {
            positions[i] = checkPoints[i].position;
        }

        positions[0] = transform.position;
        
        swapTextureMat.SetVectorArray("_Position", positions);
    }
}
