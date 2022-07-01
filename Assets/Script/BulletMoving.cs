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
        //’¼üˆÚ“®
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        //‰æ–ÊŠO‚Éo‚½‚çÁŽ¸
        if(transform.localPosition.z > 10 || transform.localPosition.z < -10 || transform.localPosition.x > 10 || transform.localPosition.x < -10)
        {
            content.HideFromStage();
        }
    }
}
