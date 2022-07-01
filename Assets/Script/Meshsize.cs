using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meshsize : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var bounds = transform.GetComponentsInChildren<MeshFilter>()[0].mesh.bounds;
        Debug.Log($"x:{bounds.min.x},y:{bounds.min.y},z:{bounds.min.z}");
        Debug.Log($"x:{bounds.max.x},y;{bounds.max.y},z:{bounds.max.z}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
