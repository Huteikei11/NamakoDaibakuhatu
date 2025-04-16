using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankAnimator : MonoBehaviour
{
    public SaveManager saveManager;
    public Animator animator;
    public int difficulty;

    void Start()
    {
        if (saveManager != null && animator != null)
        {
            // SaveManager�̃f�[�^�����[�h�����̂�҂����ɒ��ڒl���擾
            saveManager.LoadGameData();
            int rank = saveManager.GetRank(difficulty);
            animator.SetInteger("rank", rank);
            Debug.Log("RankAnimator: Start called. Difficulty: " + difficulty + ", Rank: " + rank);
        }
    }
}
