using UnityEngine.Networking;
using UnityEngine;
using System.Collections;
using System;

public class Sample : MonoBehaviour
{
    [SerializeField] private string accessKey;

    private void Start()
    {
        StartCoroutine(GetData());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            StartCoroutine(GetData());
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            // �f�[�^���M�̗�
            StartCoroutine(PostData());
        }
    }

    private IEnumerator GetData()
    {
        Debug.Log("�f�[�^��M�J�n�E�E�E");
        var request = UnityWebRequest.Get("https://script.google.com/macros/s/" + accessKey + "/exec");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            if (request.responseCode == 200)
            {
                Debug.Log("��M����JSON�f�[�^: " + request.downloadHandler.text);

                try
                {
                    // JSON�f�[�^��Records�N���X�Ƀf�V���A���C�Y
                    var records = JsonUtility.FromJson<Records>(request.downloadHandler.text);

                    // �ŏ���Record���擾�i��Ƃ���1�ڂ̃f�[�^���g�p�j
                    if (records.records.Length > 0)
                    {
                        var record = records.records[0];

                        // Gaman�X�N���v�g�Ƀf�[�^����
                        Gaman gaman = FindObjectOfType<Gaman>();
                        gaman.maxStamina = record.maxStamina;
                        gaman.staminaSpeed = record.staminaSpeed;
                        gaman.gamanPower = record.gamanPower;
                        gaman.gamanCoolTime = record.gamanCoolTime;
                        gaman.staminaRecoveryRate = record.staminaRecoveryRate;
                        gaman.staminaNotGamanRecoveryRate = record.staminaNotGamanRecoveryRate;
                        gaman.staminaRecoveryInterval = record.staminaRecoveryInterval;

                        // OppaiManager�X�N���v�g�Ƀf�[�^����
                        OppaiManager oppaiManager = FindObjectOfType<OppaiManager>();
                        oppaiManager.adjustPaizuriPower = record.adjustPaizuriPower;
                        oppaiManager.wValuePaizuriPowerAdjust = record.wValuePaizuriPowerAdjust;
                        oppaiManager.wValueIncreaseRate = record.wValueIncreaseRate;
                        oppaiManager.wValueDecreaseRate = record.wValueDecreaseRate;
                        oppaiManager.CNoisemin = record.CNoisemin;
                        oppaiManager.CNoiseMax = record.CNoisemax;
                        oppaiManager.CNoiseRandom = record.CNoiseRandom;
                        oppaiManager.paiModevalues = new float[]
                        {
                            record.paiModevalues0,
                            record.paiModevalues1,
                            record.paiModevalues2,
                            record.paiModevalues3,
                            record.paiModevalues4,
                            record.paiModevalues5,
                            record.paiModevalues6,
                            record.paiModevalues7
                        };

                        // KeikiManager�X�N���v�g�Ƀf�[�^����
                        KeikiManager keikiManager = FindObjectOfType<KeikiManager>();
                        keikiManager.totrans = record.totrans;
                        keikiManager.canceltrans = record.canceltrans;
                        keikiManager.changePaiMin = record.changePaimin;
                        keikiManager.changePaiMax = record.changePaiMax;

                        // ScoreManager�X�N���v�g�Ƀf�[�^����
                        ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
                        scoreManager.scoreAdjust = record.scoreAdjust;
                        scoreManager.gamanratioList = new float[]
                        {
                            record.gamanratioList0,
                            record.gamanratioList1,
                            record.gamanratioList2,
                            record.gamanratioList3,
                            record.gamanratioList4
                        };
                        scoreManager.wRationAdjust = record.wRationAdjust;

                        // GameManager�X�N���v�g�Ƀf�[�^����
                        GameManager gameManager = FindObjectOfType<GameManager>();
                        gameManager.gametime = record.gametime;

                        Debug.Log("�f�[�^��M�Ƒ�������I");
                    }
                    else
                    {
                        Debug.LogError("��M�����f�[�^�Ƀ��R�[�h���܂܂�Ă��܂���B");
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError("�f�[�^����G���[: " + ex.Message);
                }
            }
            else
            {
                Debug.LogError("�f�[�^��M���s�F" + request.responseCode);
            }
        }
        else
        {
            Debug.LogError("�f�[�^��M���s: " + request.error);
        }
    }

    private IEnumerator PostData()
    {
        Debug.Log("�f�[�^���M�J�n�E�E�E");

        // �t�H�[���f�[�^���쐬
        var form = new WWWForm();
        form.AddField("maxStamina", 100.0f.ToString());
        form.AddField("staminaSpeed", 1.5f.ToString());
        form.AddField("gamanPower", 50.0f.ToString());
        form.AddField("gamanCoolTime", 10.0f.ToString());
        form.AddField("staminaRecoveryRate", 0.5f.ToString());
        form.AddField("staminaNotGamanRecoveryRate", 0.2f.ToString());
        form.AddField("staminaRecoveryInterval", 1.0f.ToString());
        form.AddField("adjustPaizuriPower", 1.2f.ToString());
        form.AddField("wValuePaizuriPowerAdjust", 0.8f.ToString());
        form.AddField("wValueIncreaseRate", 0.05f.ToString());
        form.AddField("wValueDecreaseRate", 0.03f.ToString());
        form.AddField("CNoisemin", 0.1f.ToString());
        form.AddField("CNoisemax", 0.5f.ToString());
        form.AddField("CNoiseRandom", 0.2f.ToString());
        form.AddField("paiModevalues0", 1.0f.ToString());
        form.AddField("paiModevalues1", 1.1f.ToString());
        form.AddField("paiModevalues2", 1.2f.ToString());
        form.AddField("paiModevalues3", 1.3f.ToString());
        form.AddField("paiModevalues4", 1.4f.ToString());
        form.AddField("paiModevalues5", 1.5f.ToString());
        form.AddField("paiModevalues6", 1.6f.ToString());
        form.AddField("paiModevalues7", 1.7f.ToString());
        form.AddField("totrans", 0.5f.ToString());
        form.AddField("canceltrans", 0.3f.ToString());
        form.AddField("changePaimin", 0.2f.ToString());
        form.AddField("changePaiMax", 0.8f.ToString());
        form.AddField("scoreAdjust", 1.0f.ToString());
        form.AddField("gamanratioList0", 0.1f.ToString());
        form.AddField("gamanratioList1", 0.2f.ToString());
        form.AddField("gamanratioList2", 0.3f.ToString());
        form.AddField("gamanratioList3", 0.4f.ToString());
        form.AddField("gamanratioList4", 0.5f.ToString());
        form.AddField("wRationAdjust", 0.8f.ToString());
        form.AddField("gametime", 300.ToString()); // GameManager��gametime�𑗐M

        // ���N�G�X�g�𑗐M
        var request = UnityWebRequest.Post("https://script.google.com/macros/s/" + accessKey + "/exec", form);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            if (request.responseCode == 200)
            {
                Debug.Log("�f�[�^���M�����I");
            }
            else
            {
                Debug.LogError("�f�[�^���M���s�F" + request.responseCode);
            }
        }
        else
        {
            Debug.LogError("�f�[�^���M���s: " + request.error);
        }
    }
}
