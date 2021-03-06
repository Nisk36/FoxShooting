using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMoving : MonoBehaviour
{
    public float speed;
    PoolContent content;
    // Start is called before the first frame update
    void Start()
    {
        content = transform.GetComponent<PoolContent>();
    }

    // Update is called once per frame
    void Update()
    {
        //直線移動
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        //画面外に出たら消失
        if(transform.localPosition.z > 10 || transform.localPosition.z < -10 || transform.localPosition.x > 10 || transform.localPosition.x < -10)
        {
            content.HideFromStage();
        }
    }
}
