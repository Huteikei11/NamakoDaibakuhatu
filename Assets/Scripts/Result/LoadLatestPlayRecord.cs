using TMPro; // TextMeshPro���g�p���邽�߂̖��O���
using UnityEngine;
using System.Collections; // �R���[�`�����g�p���邽�߂̖��O���
using System.Collections.Generic; // List���g�p���邽�߂̖��O���

public class LoadLatestPlayRecord : MonoBehaviour
{
    public SaveManager saveManager; // SaveManager�̎Q�Ƃ�ݒ�
    public Animator rankAnimator; // RankAnimator�̎Q�Ƃ�ݒ�
    public TextMeshProUGUI scoreText; // �X�R�A��\������TextMeshPro
    public TextMeshProUGUI resultPhraseText; // ���ʃt���[�Y��\������TextMeshPro
    public SpriteRenderer resultSpriteRenderer; // isCleared�Ɋ�Â��ăX�v���C�g��؂�ւ���SpriteRenderer
    public Sprite clearedSprite; // �N���A���̃X�v���C�g
    public Sprite failedSprite; // ���s���̃X�v���C�g
    public SpriteRenderer darkenSpriteRenderer; // ��ʂ��Â����邽�߂�SpriteRenderer

    public SpriteRenderer hukidasi;

    // Keiki�p��SpriteRenderer�ƃX�v���C�g���X�g
    public SpriteRenderer KeikiSpriteRenderer; // SpriteRenderer�̎Q�Ƃ�ݒ�
    public List<Sprite> KeikiSprites; // �����N���Ƃ̃X�v���C�g���Ǘ����郊�X�g

    // White�p��SpriteRenderer�ƃX�v���C�g���X�g
    public SpriteRenderer whiteSpriteRenderer; // SpriteRenderer�̎Q�Ƃ�ݒ�
    public List<Sprite> whiteSprites; // �����N���Ƃ̃X�v���C�g���Ǘ����郊�X�g

    [SerializeField] private ResultMenuController menucontroller;

    private bool isCleared; // �N���A��Ԃ��Ǘ�����ϐ�

    void Start()
    {
        // �Z�[�u�f�[�^�����[�h
        saveManager.LoadGameData();




        darkenSpriteRenderer.gameObject.SetActive(false);
        resultSpriteRenderer.gameObject.SetActive(false);
        hukidasi.gameObject.SetActive(false);

        // �ŐV��PlayRecord���擾
        SaveManager.PlayRecord latestRecord = GetLatestPlayRecord();

        if (latestRecord != null)
        {
            // �ŐV�̃f�[�^�����O�ɏo��
            Debug.Log($"�ŐV�̃f�[�^: Rank={latestRecord.rank}, ResultPhraseNo={latestRecord.resultPhraseNo}, Score={latestRecord.score}, IsCleared={latestRecord.isCleared}");

            // RankAnimator�ɍŐV�̃����N��ݒ�
            ViewRank(latestRecord.rank);

            // �X�R�A��\��
            ViewScore(latestRecord.score);

            // Keiki��White�̃X�v���C�g�������N�ɉ����Đ؂�ւ�
            UpdateKeikiSprite(latestRecord.rank);
            UpdateWhiteSprite(latestRecord.rank);


            isCleared = latestRecord.isCleared; // �N���A��Ԃ�ۑ�

            // resultPhraseNo���ꕶ�����\�����AEnter�L�[��ҋ@
            StartCoroutine(ViewResultPhrase(latestRecord.resultPhraseNo));
        }
        else
        {
            Debug.Log("PlayRecord�����݂��܂���B");
        }
    }

    private SaveManager.PlayRecord GetLatestPlayRecord()
    {
        // SaveManager��playRecords���X�g����łȂ��ꍇ�A�ŐV�̃f�[�^���擾
        if (saveManager.playRecords != null && saveManager.playRecords.Count > 0)
        {
            return saveManager.playRecords[saveManager.playRecords.Count - 1]; // �Ō�̗v�f���擾
        }

        return null; // �f�[�^�����݂��Ȃ��ꍇ��null��Ԃ�
    }

    private void ViewRank(int rank)
    {
        rankAnimator.SetInteger("rank", rank);
    }

    private void ViewScore(float score)
    {
        if (scoreText != null)
        {
            scoreText.text = $"{score:F0}"; // �X�R�A�������_�ȉ�0���ŕ\��
        }
        else
        {
            Debug.LogError("ScoreText���ݒ肳��Ă��܂���B");
        }
    }

    private void UpdateKeikiSprite(int rank)
    {
        if (rank >= 0 && rank < KeikiSprites.Count)
        {
            // �����N�ɑΉ�����X�v���C�g��ݒ�
            KeikiSpriteRenderer.sprite = KeikiSprites[rank];
        }
        else
        {
            Debug.LogError($"Rank {rank} �ɑΉ�����Keiki�X�v���C�g�����݂��܂���BKeikiSprites�͈̔͂��m�F���Ă��������B");
        }
    }

    private void UpdateWhiteSprite(int rank)
    {
        if (rank >= 0 && rank < whiteSprites.Count)
        {
            // �����N�ɑΉ�����X�v���C�g��ݒ�
            whiteSpriteRenderer.sprite = whiteSprites[rank];
        }
        else
        {
            Debug.LogError($"Rank {rank} �ɑΉ�����White�X�v���C�g�����݂��܂���BWhiteSprites�͈̔͂��m�F���Ă��������B");
        }
    }

    private void UpdateResultSprite(bool isCleared)
    {
        if (resultSpriteRenderer != null)
        {
            // �N���A��Ԃɉ����ăX�v���C�g��؂�ւ�
            resultSpriteRenderer.sprite = isCleared ? clearedSprite : failedSprite;
        }
        else
        {
            Debug.LogError("ResultSpriteRenderer���ݒ肳��Ă��܂���B");
        }
    }

    private IEnumerator ViewResultPhrase(string phrase)
    {
        // Enter�L�[���������܂őҋ@
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));

        hukidasi.gameObject.SetActive(true);
        resultPhraseText.text = ""; // �e�L�X�g��������

        // �ꕶ�����\��
        foreach (char c in phrase)
        {
            resultPhraseText.text += c;
            yield return new WaitForSeconds(0.08f); // 0.05�b�ҋ@
        }

        // Enter�L�[���������܂őҋ@
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));

        // ��ʂ��Â�����
        if (darkenSpriteRenderer != null)
        {
            darkenSpriteRenderer.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("DarkenSpriteRenderer���ݒ肳��Ă��܂���B");
        }

        // resultSpriteRenderer��L���ɂ��ĕ\��
        if (resultSpriteRenderer != null)
        {
            resultSpriteRenderer.gameObject.SetActive(true);
            // isCleared�Ɋ�Â��ăX�v���C�g��؂�ւ�
            UpdateResultSprite(isCleared);
        }

        //���j���[�\��
        menucontroller.ShowDynamicMenu(isCleared);
    }
}


