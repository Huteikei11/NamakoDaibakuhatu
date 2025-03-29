using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gaman : MonoBehaviour
{
    [SerializeField]　private ObjectController2D suii;
    [SerializeField] private OppaiManager oppaiManager;
    public float stamina;            // 現在のスタミナ
    public float maxStamina;         // スタミナの最大値
    public float staminaSpeed;       // スタミナ消費速度
    public float gamanPower;         // 我慢のパワー
    public float gamanCoolTime;      // クールタイム
    public float staminaRecoveryRate; // スタミナ回復量 (m)
    public float staminaNotGamanRecoveryRate; // スタミナ回復量 (m)
    public float staminaRecoveryInterval; // スタミナ回復間隔 (n)

    private float lastGamanTime;     // 前回gmanaddを実行した時間
    private float lastRecoveryTime;  // 最後にスタミナを回復した時間
    private bool lastGaman; //前回の回復からガマンを使ったか
    public float staminaNorma = 0;//射精を我慢するときのノルマ


    // Start is called before the first frame update
    void Start()
    {
        lastGamanTime = -gamanCoolTime; // 初期化時にクールタイムをリセット
        lastRecoveryTime = Time.time;  // 初期化時に回復タイマーをリセット
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.IsGameStarted || GameManager.Instance.IsPaused) return; // ポーズ中は動かない

        //射精を我慢するときに必要なstaminaの計算
        if (oppaiManager.dangerCount <= 1)
        {
            staminaNorma = maxStamina * 0.45f;
        }
        else if (oppaiManager.dangerCount == 2)
        {
            staminaNorma = maxStamina * 0.75f;
        }
        else if (oppaiManager.dangerCount == 3)
        {
            staminaNorma = maxStamina * 0.9f;
        }
        else if (oppaiManager.dangerCount == 4)
        {
            staminaNorma = maxStamina;
        }

        // Sキーが押されたときの処理
        if (Input.GetKeyDown(KeyCode.S))
        {
            //射精しそうなとき
            if (oppaiManager.isChecking)
            {
                
                if (stamina > staminaNorma && Time.time >= lastGamanTime + gamanCoolTime) // 条件変更: staminaSpeed より大きい場合
                {
                    stamina -= staminaNorma;
                    stamina = Mathf.Max(0, stamina); // staminaが負の値にならないようにする
                    gmanadd();
                    lastGamanTime = Time.time; // 実行時間を更新
                    lastGaman = false;
                    oppaiManager.CheckGamanKey();
                }
            }
            //通常
            else
            {
                if (stamina > staminaSpeed && Time.time >= lastGamanTime + gamanCoolTime) // 条件変更: staminaSpeed より大きい場合
                {
                stamina -= staminaSpeed;
                stamina = Mathf.Max(0, stamina); // staminaが負の値にならないようにする
                gmanadd();
                lastGamanTime = Time.time; // 実行時間を更新
                lastGaman = false;

                }
            }


        }

        // n秒ごとにスタミナを回復
        if (Time.time >= lastRecoveryTime + staminaRecoveryInterval)
        {
            if (lastGaman)//前回の回復からガマンを使ったか
            {
                stamina += staminaNotGamanRecoveryRate;//使わなかった
            }
            else
            {
                stamina += staminaRecoveryRate;
            }
            stamina = Mathf.Min(maxStamina, stamina); // staminaがmaxStaminaを超えないようにする
            lastRecoveryTime = Time.time; // 回復時間を更新
            lastGaman = true;
        }

    }

    // gamanaddメソッドの定義
    void gmanadd()
    {
        // gamanPowerを用いた処理をここに記述
        Debug.Log("ガマン！: " + gamanPower);
        suii.AddPositionY(gamanPower);
    }
}
