using System;

[Serializable]
public class Records
{
    public Record[] records; // ������Record���i�[����z��
}

[Serializable]
public class Record
{
    // Gaman�֘A�f�[�^
    public float maxStamina;
    public float staminaSpeed;
    public float gamanPower;
    public float gamanCoolTime;
    public float staminaRecoveryRate;
    public float staminaNotGamanRecoveryRate;
    public float staminaRecoveryInterval;

    // OppaiManager�֘A�f�[�^
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

    // KeikiManager�֘A�f�[�^
    public float totrans;
    public float canceltrans;
    public float changePaimin;
    public float changePaiMax;

    // ScoreManager�֘A�f�[�^
    public float scoreAdjust;
    public float gamanratioList0;
    public float gamanratioList1;
    public float gamanratioList2;
    public float gamanratioList3;
    public float gamanratioList4;
    public float wRationAdjust;

    // GameManager�֘A�f�[�^
    public int gametime; // �Q�[������
}
