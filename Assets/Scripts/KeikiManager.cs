﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeikiManager : MonoBehaviour
{
    [SerializeField] private OppaiManager oppaiManager;
    public int paiMode;
    // Start is called before the first frame update
    void Start()
    {
        ScheduleNextPaiMode();
    }

    // Update is called once per frame
    void Update()
    {
        oppaiManager.SetPaiMode(paiMode);
    }

    private void ScheduleNextPaiMode()
    {
        float nextTime = Random.Range(3f, 6f); // 3〜10秒のランダムな時間を設定
        Invoke("IsChangePaiMode", nextTime);
    }


    private void IsChangePaiMode()
    {
            if (Random.value > 0.3f) // 確率で実行(Radom.Valueは0~1.0)
        {
                int PaiMode = ChangePaiMode();
                Debug.Log("パイズリモード変更:" + PaiMode);

                // 必要に応じて、gPaizuriPowerや他の変数にノイズを適用
            }
        ScheduleNextPaiMode(); // 次のノイズスケジュールを設定
    }

    //パイズリモードを変える中身
    private int ChangePaiMode()
    {
        // パイズリのモードを変える
        int result = Random.Range(0, 3);//とりあえずランダムで
        oppaiManager.SetPaiMode(result);
        return result;
    }
}
