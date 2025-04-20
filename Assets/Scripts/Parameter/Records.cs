using System;

[Serializable]
public class Records
{
    public Record[] records; // 複数のRecordを格納する配列
}

[Serializable]
public class Record
{
    // Gaman関連データ
    public float maxStamina;
    public float staminaSpeed;
    public float gamanPower;
    public float gamanCoolTime;
    public float staminaRecoveryRate;
    public float staminaNotGamanRecoveryRate;
    public float staminaRecoveryInterval;

    // OppaiManager関連データ
    public float adjustPaizuriPower;
    public float wValuePaizuriPowerAdjust;
    public float wValueIncreaseRate;
    public float wValueDecreaseRate;
    public float CNoisemin;
    public float CNoisemax;
    public float CNoiseRandom;
    public float paiModevalues0;
    public float paiModevalues1;
    public float paiModevalues2;
    public float paiModevalues3;
    public float paiModevalues4;
    public float paiModevalues5;
    public float paiModevalues6;
    public float paiModevalues7;

    // KeikiManager関連データ
    public float totrans;
    public float canceltrans;
    public float changePaimin;
    public float changePaiMax;

    // ScoreManager関連データ
    public float scoreAdjust;
    public float gamanratioList0;
    public float gamanratioList1;
    public float gamanratioList2;
    public float gamanratioList3;
    public float gamanratioList4;
    public float wRationAdjust;

    // GameManager関連データ
    public int gametime; // ゲーム時間
}
