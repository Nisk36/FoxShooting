using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    [SerializeField] int enemyHp = default;
    [SerializeField] bool isBoss = false;
    [SerializeField] private int scorePoint = 0;

    ObjectPool enemyBulletPool;
    ObjectPool explosionPool;

    /// <summary> �}�e���A���̉��Z�F�p�����[�^��ID </summary>
    private static readonly int PROPERTY_ADDITIVE_COLOR = Shader.PropertyToID("_AdditiveColor");

    /// <summary> ���f����Renderer </summary>
    [SerializeField]
    private Renderer _renderer;

    /// <summary> ���f���̃}�e���A���̕��� </summary>
    private Material _material;

    private Sequence _seq;

    public AudioClip dieSE;
    public AudioClip damagedSE;

    private void Awake()
    {
        // material�ɃA�N�Z�X���Ď������������}�e���A����ێ�
        _material = _renderer.material;
    }

    void Start()
    {
        enemyBulletPool = StageCtrl.Instance.enemyBulletPool;
        explosionPool = StageCtrl.Instance.explosionPool;
    }

    // Update is called once per frame
    void Update()
    {
        StageCtrl.Instance.isBossAppear = isBoss;
    }
    
    public void HideFromStage()
    {
        Destroy(gameObject);
    }

    public void OnTriggerEnter(Collider _other)
    {
        if(_other.tag == "playerbullet")
        {
            //���������e�̏���
            var poolObj =_other.transform.GetComponent<PoolContent>();
            AudioSource.PlayClipAtPoint(damagedSE, transform.position);
            poolObj.HideFromStage();

            enemyHp -= 1;
            if(enemyHp <= 0)
            {
                AudioSource.PlayClipAtPoint(dieSE, transform.position);
                StageCtrl.Instance.isStageBossDead = isBoss;
                StageCtrl.Instance.AddScore(scorePoint);
                explosionPool.Launch(transform.position, 0).GetComponent<ExplosionPartical>().PlayParticle();
                HideFromStage();
            }
            else
            {
                //StartCoroutine(FlashTimeWait());
                HitFadeBlink(Color.white);
            }
        }
    }


    public void Shot(EnemyBulletPattern _o)
    {
        var angleOffset = (_o.Count - 1) / 2.0f;
        float baseDirection = 0;
        if (_o.IsAimPlayer)
        {
            baseDirection = Vector3.SignedAngle(
                Vector3.forward,
                StageCtrl.Instance.playerObj.transform.localPosition - transform.localPosition,
                Vector3.up);
        }
        else
        {
            baseDirection = _o.Direction;
        }
        for(int i = 0; i < _o.Count; i++)
        {
            var d = ((i - angleOffset) * _o.OpenAngle);
            var obj = enemyBulletPool.Launch(transform.position + Vector3.up * 0.2f, d + baseDirection);
            if(obj != null)
            {
                obj.GetComponent<BulletMoving>().speed = _o.Speed;
            }
        }
    }

    private void HitFadeBlink(Color color)
    {
        _seq?.Kill();
        _seq = DOTween.Sequence();
        _seq.Append(DOTween.To(() => Color.black, c => _material.SetColor(PROPERTY_ADDITIVE_COLOR, c), color, 0.1f));
        _seq.Append(DOTween.To(() => color, c => _material.SetColor(PROPERTY_ADDITIVE_COLOR, c), Color.black, 0.1f));
        _seq.Play();
    }
}
