using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "StageSequncer")]
public class StageSequencer : ScriptableObject
{
    [SerializeField] private string filename = "";
    [SerializeField] private StageCtrl stage = null;

    public enum CommandType
    {
        SETSPEED,
        PUTENEMY
    }
    static readonly Dictionary<string, CommandType> commandlist = new Dictionary<string, CommandType>() {
        {"SETSPEED",CommandType.SETSPEED },
        {"PUTENEMY",CommandType.PUTENEMY }
    };

    public struct StageData
    {
        public readonly float eventPos;
        public readonly CommandType command;
        public readonly float arg1, arg2;
        public readonly uint arg3;
        public StageData(float _eventpos, string _command, float _x, float _y, uint _type)
        {
            eventPos = _eventpos;//時間軸
            command = commandlist[_command];//コマンド
            arg1 = _x;//x座標
            arg2 = _y;//y座標
            arg3 = _type;//敵の種類を符号なし整数で
        }
    }

    StageData[] stageDatas;

    private int stagedataindex = 0;

    [SerializeField] Enemy[] enemyPrefabs = default;

    public void Load()
    {
        var revarr = new Dictionary<string, uint>();
        for(uint i = 0; i < enemyPrefabs.Length; i++)
        {
            revarr.Add(enemyPrefabs[i].name, i);
        }

        var stagecsvdata = new List<StageData>();
        var csvdata = Resources.Load<TextAsset>(filename).text;
        StringReader sr = new StringReader(csvdata);
        while (sr.Peek() != -1)
        {
            var line = sr.ReadLine();
            var cols = line.Split(',');
            if(cols.Length != 5)
            {
                continue;
            }
            stagecsvdata.Add(
                new StageData(
                  float.Parse(cols[0]),
                  cols[1],
                  float.Parse(cols[2]),
                  float.Parse(cols[3]),
                  revarr.ContainsKey(cols[4]) ? revarr[cols[4]] : 0)
            );
        }
        stageDatas = stagecsvdata.OrderBy(item => item.eventPos).ToArray();
    }

    public void Reset()
    {
        stagedataindex = 0;
    }

    public void Step(float _stageProgressTime)
    {
        while(stagedataindex < stageDatas.Length && stageDatas[stagedataindex].eventPos <= _stageProgressTime)
        {
            switch (stageDatas[stagedataindex].command)
            {
                case CommandType.SETSPEED:
                    StageCtrl.Instance.stagespeed = stageDatas[stagedataindex].arg1;
                    break;
                case CommandType.PUTENEMY:
                    var enmtmp = Instantiate(enemyPrefabs[stageDatas[stagedataindex].arg3]);
                    enmtmp.transform.parent = StageCtrl.Instance.enemypool;
                    enmtmp.transform.localPosition = new Vector3(stageDatas[stagedataindex].arg1, 0, stageDatas[stagedataindex].arg2);
                    break;
            }
            ++stagedataindex;
        }
    }
}
