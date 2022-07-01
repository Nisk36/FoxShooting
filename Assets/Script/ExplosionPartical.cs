using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionPartical : MonoBehaviour
{
    private ParticleSystem p;
    private PoolContent pool;
    private void Awake()
    {
        p = transform.GetComponent<ParticleSystem>();
        pool = transform.GetComponent<PoolContent>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayParticle()
    {
        p.Play();
        StartCoroutine(TimeWait());
    }
    IEnumerator TimeWait()
    {
        yield return new WaitForSeconds(0.3f);
        p.Stop();
        pool.HideFromStage();
    }
}
