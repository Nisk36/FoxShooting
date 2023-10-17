using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    [SerializeField] private GameObject bgm = default;
    [SerializeField] private GameObject bossBgm = default;

    public void InitializeBGM()
    {
        bgm.SetActive(false);
        bossBgm.SetActive(false);
    }
    
    public void PlayBossBGM()
    {
        bgm.SetActive(false);
        bossBgm.SetActive(true);
    }

    public void PlayNormalBGM()
    {
        bgm.SetActive(true);
        bossBgm.SetActive(false);
    }
}
