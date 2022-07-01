using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    private ObjectPool playerBulletPool;
    private float shotInterval = 0;

    public bool isDead;
    public AudioClip se;
    // Start is called before the first frame update
    void Start()
    {
        playerBulletPool = StageCtrl.Instance.PlayerBulletPool;
    }

    // Update is called once per frame
    void Update()
    {
        shotInterval -= Time.deltaTime;
    }

    public void Move(Vector3 _movevec)
    {
        transform.Translate(_movevec * 6 * Time.deltaTime);
        var nowpos = transform.localPosition;
        //à⁄ìÆîÕàÕêßå¿
        nowpos.x = Mathf.Clamp(nowpos.x, -3.3f, 3.3f);
        nowpos.z = Mathf.Clamp(nowpos.z, -5,5);
        transform.localPosition = nowpos;
    }

    public void Shot()
    {
        if(shotInterval <= 0)
        {
            var obj = playerBulletPool.Launch(transform.position + Vector3.up * 0.2f, 0);
            AudioSource.PlayClipAtPoint(se, transform.position);
            if(obj != null)
            {
                obj.GetComponent<BulletMoving>().speed = 15;
            }
            shotInterval = 0.1f;
        }
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("enemybullet"))
        {
            _other.GetComponent<PoolContent>().HideFromStage();
            isDead = true;
        }
    }

    public void SetUpForPlay()
    {
        shotInterval = 0;
        isDead = false;
        transform.localPosition = new Vector3(0, 0, -1.8f);
        transform.localEulerAngles = new Vector3(0, 0, 0);
    }
}
