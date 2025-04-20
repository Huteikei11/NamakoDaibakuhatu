using System;

[Serializable]
public class Records
{
    public Record[] records; // •¡”‚ÌRecord‚ğŠi”[‚·‚é”z—ñ
}

[Serializable]
public class Record
{
    public float maxStamina;
    public float staminaSpeed;
    public float gamanPower;
    public float gamanCoolTime;
    public float staminaRecoveryRate;
    public float staminaNotGamanRecoveryRate;
    public float staminaRecoveryInterval;
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
    public float totrans;
    public float canceltrans;
    public float changePaimin;
    public float changePaiMax;
}
