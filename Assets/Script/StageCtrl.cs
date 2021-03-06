using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCtrl : MonoBehaviour
{
    [SerializeField] public PlayerCtrl playerObj = default;
    [SerializeField] public ObjectPool PlayerBulletPool = default;
    [SerializeField] public ObjectPool enemyBulletPool = default;
    [SerializeField] public Transform enemypool = default;
    [SerializeField] public ObjectPool explosionPool = default;

    [SerializeField] public StageSequencer sequencer = default;

    public float stagespeed = 1;
    private float stageProgressTime = 0;
    //インスタンス化する
    private static StageCtrl instance;
    public static StageCtrl Instance { get => instance; }

    public bool isPlaying;
    public bool isStageBossDead;
    public bool isBossAppear;

    int score = 0;
    [SerializeField] UnityEngine.UI.Text ScoreValue = default;

    public enum PlayStopCodeDef
    {
        PlayerDead,
        BossDefeat,
    }
    public PlayStopCodeDef playStopCode;

    private void Awake()
    {
        instance = this.GetComponent<StageCtrl>();
    }
    // Start is called before the first frame update
    void Start()
    {
        sequencer.Load();
        sequencer.Reset();
        stageProgressTime = 0;
        isPlaying = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerObj.isDead)
        {
            playStopCode = PlayStopCodeDef.PlayerDead;
            isPlaying = false;
        }
        if (isStageBossDead)
        {
            playStopCode = PlayStopCodeDef.BossDefeat;
            isPlaying = false;
        }
        if (!isPlaying) return;
        sequencer.Step(stageProgressTime);
        stageProgressTime += Time.deltaTime;

        transform.Translate(Vector3.forward * Time.deltaTime * stagespeed);

        var xaxis = Input.GetAxisRaw("Horizontal");
        var yaxis = Input.GetAxisRaw("Vertical");
        playerObj.Move(new Vector3(xaxis, 0, yaxis));

        if (Input.GetButton("Fire1"))
        {
            playerObj.Shot();
        }
    }
    public void StageStart()
    {
        isPlaying = true;
        stageProgressTime = 0;
        stagespeed = 1;
        sequencer.Reset();
        isStageBossDead = false;
        playerObj.SetUpForPlay();
        SetScore(0);
    }
    public void ResetStage()
    {
        BroadcastMessage("HideFromStage", SendMessageOptions.DontRequireReceiver);
        isBossAppear = false;
        transform.position = Vector3.zero;
        playerObj.SetUpForPlay();
    }

    public void AddScore(int _val)
    {
        SetScore(score + _val);
    }

    public void SetScore(int _val)
    {
        score = _val;
        ScoreValue.text = $"{score:000000}";
    }

    public int GetScore()
    {
        return score;
    }
}
