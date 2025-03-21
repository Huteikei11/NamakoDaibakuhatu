﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OppaiManager : MonoBehaviour
{
    [SerializeField] private GameObject suii;
    private ObjectController2D suiiscript;
    //[SerializeField] private MayumiManager mayumi;


    public float adjustPaizuriPower;//パイズリパワー調節用の値
    public float gPaizuriPower;//パイズリパワー計算結果
    public float wValue;//Wキーの入力結果
    public float wValueIncrease; //Wキーの増加度
    public int paiMode;//パイズリのモーション
    public float[] paiModevalues = new float[3];//モーションごとのパイズリパワー

    // Start is called before the first frame update
    void Start()
    {
        suiiscript = suii.GetComponent<ObjectController2D>();
        ScheduleNextNoise(); // 初回のノイズスケジュールを設定
    }

    // Update is called once per frame
    void Update()
    {
        // Wキーが押されたときの処理
        if (Input.GetKeyDown(KeyCode.W))
        { 
           wValue = W_Calculate(wValue, wValueIncrease);
        }

        //パイズリパワーを射精水位に適用
        gPaizuriPower = -1 * G_PaizuriPowerCalulate(paiModevalues[paiMode], wValue, adjustPaizuriPower);
        suiiscript.gravityScale = gPaizuriPower;

        //射精（ミス）の検知
        if (suii.transform.position.y >= suiiscript.maxY)
        {
            Debug.Log("射精!!");
        }
    }

    private float W_Calculate(float wValue, float wValueIncrease)
    {
        return wValue+wValueIncrease;
    }


    private float G_PaizuriPowerCalulate(float paivalue, float wValue,float adjust)
    {
        return paivalue*wValue*adjust;
    }

    //ノイズを加える時間
    private void ScheduleNextNoise()
    {
        float nextTime = Random.Range(1f, 2f); // 1〜5秒のランダムな時間を設定
        Invoke("CheckAndExecuteNoise", nextTime);
    }


    private void CheckAndExecuteNoise()
    {
        if (paiMode == 2)
        {
            if (Random.value > 0.4f) // 確率で実行(Radom.Valueは0~1.0)
            {
                float noise = G_PaizuriNoise();
                Debug.Log("ビクッ（急増)" + noise);

                // 必要に応じて、gPaizuriPowerや他の変数にノイズを適用
                suiiscript.AddPositionY(noise);
            }
        }
        ScheduleNextNoise(); // 次のノイズスケジュールを設定
    }

    //パイズリモード=2のときに数秒ごとにランダムで上に上がる
    private float G_PaizuriNoise()
    {
        // 適当なノイズ値を返す
        return Random.Range(0.2f, 0.5f); // 0.1〜1.0のランダムな値
    }


    public void SetPaiMode(int mode)
    {
        paiMode = mode;
    }

}
