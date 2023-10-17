using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorCtrl : MonoBehaviour
{
    readonly float areaSizeZ = 3.42f * 8;

    Vector3 basepos;

    [SerializeField] 
    private StageCtrl stage;

    // Start is called before the first frame update
    void Start()
    {
        basepos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        var posz = Mathf.Round((stage.transform.position.z - basepos.z)/areaSizeZ);

        var nowpos = transform.position;
        nowpos.z = areaSizeZ * posz + basepos.z;
        transform.position = nowpos;
    }
}
