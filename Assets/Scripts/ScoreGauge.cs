using UnityEngine;

public class ScoreGauge : MonoBehaviour
{
    public Transform maskTransform;  // マスクオブジェクトのTransform
    [SerializeField] ScoreManager scoreManager;
    public float[] sectionScores = { 0, 20, 40, 60, 80, 100, 120 }; // E, D, C, B, A, S, SSのスコア
    public float[] sectionXPositions = { -2f, -1.5f, -1f, -0.5f, 0f, 0.5f, 1f }; // 各スコアのx座標
    public float currentScore = 0; // 現在のスコア
    public int rank = 0; // ランク


    void Update()
    {
        UpdateGauge();
        rank = CalRank();
    }

    void UpdateGauge()
    {
        currentScore = scoreManager.score;
        float targetX = GetGaugeXPosition(currentScore);
        maskTransform.position = new Vector3(targetX, maskTransform.position.y, maskTransform.position.z);
    }

    float GetGaugeXPosition(float score)
    {
        for (int i = 0; i < sectionScores.Length - 1; i++)
        {
            if (score >= sectionScores[i] && score < sectionScores[i + 1])
            {
                float t = (score - sectionScores[i]) / (sectionScores[i + 1] - sectionScores[i]);
                return Mathf.Lerp(sectionXPositions[i], sectionXPositions[i + 1], t);
            }
        }
        return sectionXPositions[sectionXPositions.Length - 1]; // SSの最大位置
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