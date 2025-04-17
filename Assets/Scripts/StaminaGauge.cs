using UnityEngine;

public class StaminaGauge : MonoBehaviour
{
    public Transform maskTransform;  // マスクオブジェクトのTransform
    [SerializeField] Gaman gaman;
    public float[] sectionScores = { 0, 20, 40, 60, 80, 100, 120 }; // E, D, C, B, A, S, SSのスコア
    public float[] sectionYPositions = { -2f, -1.5f, -1f, -0.5f, 0f, 0.5f, 1f }; // 各スコアのy座標
    public float currentScore = 0; // 現在のスコア
    public int rank = 0; // ランク

    void Update()
    {
        UpdateGauge();
        rank = CalRank();
    }

    void UpdateGauge()
    {
        currentScore = gaman.stamina;
        float targetY = GetGaugeYPosition(currentScore);
        maskTransform.position = new Vector3(maskTransform.position.x, targetY, maskTransform.position.z);
    }

    float GetGaugeYPosition(float score)
    {
        for (int i = 0; i < sectionScores.Length - 1; i++)
        {
            if (score >= sectionScores[i] && score < sectionScores[i + 1])
            {
                float t = (score - sectionScores[i]) / (sectionScores[i + 1] - sectionScores[i]);
                return Mathf.Lerp(sectionYPositions[i], sectionYPositions[i + 1], t);
            }
        }
        return sectionYPositions[sectionYPositions.Length - 1]; // SSの最大位置
    }

    public int CalRank()
    {
        for (int i = 0; i < sectionScores.Length - 1; i++)
        {
            if (currentScore >= sectionScores[i] && currentScore < sectionScores[i + 1])
            {
                return i;
            }
        }
        return sectionScores.Length - 1; // SSの最大位置
    }

    public int GetRank()
    {
        return rank;
    }
}