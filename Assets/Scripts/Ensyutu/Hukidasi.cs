using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening; // DOTween���g�p���邽�߂ɕK�v

public class SpeechBubbleManager_SpriteRenderer : MonoBehaviour
{
    [Header("�����o���ݒ�")]
    [SerializeField] private SpriteRenderer speechBubbleSpriteRenderer; // �����o����\������Sprite Renderer�R���|�[�l���g
    [SerializeField] private List<Sprite> speechBubbleSprites; // �����o���摜�̃��X�g
    [SerializeField] private float initialDelay = 0.0f; // �ŏ��̐����o�����\�������܂ł̏����x���i�b�j
    [SerializeField] private float displayInterval = 3.0f; // ���̐����o�����\�������܂ł̊Ԋu�i�b�j
    [SerializeField] private float fadeOutDuration = 1.0f; // �����o����������܂ł̎��ԁi�b�j

    private int currentBubbleIndex = 0;
    private Coroutine displayCoroutine;
    private bool isDisplayingSpecialBubble = false;
    private bool isDisplayingEnabled = true; // �����o���\�����L�����ǂ������Ǘ�����t���O

    public Sprite Failed;
    public Sprite GYU;
    public Sprite StageClear;
    public Sprite SpeedUP;
    public Sprite SpeedDOWN;

    void Start()
    {
        if (speechBubbleSpriteRenderer == null)
        {
            Debug.LogError("SpeechBubbleSpriteRenderer���ݒ肳��Ă��܂���I");
            enabled = false;
            return;
        }
        // ������Ԃł͓����ɂ��Ă����iSprite Renderer��color�v���p�e�B��Color�^�j
        speechBubbleSpriteRenderer.color = new Color(1, 1, 1, 0);
        StartAutoDisplay();
    }

    /// <summary>
    /// �����ł̐����o���\�����J�n���܂��B
    /// </summary>
    public void StartAutoDisplay()
    {
        if (displayCoroutine != null)
        {
            StopCoroutine(displayCoroutine);
        }
        if (isDisplayingEnabled) // �\�����L���ȏꍇ�̂݊J�n
        {
            displayCoroutine = StartCoroutine(AutoDisplayBubblesRoutine());
        }
    }

    /// <summary>
    /// �����o���������ŏ��Ԃɕ\������R���[�`���ł��B
    /// </summary>
    private IEnumerator AutoDisplayBubblesRoutine()
    {
        // �ŏ��̐����o�����\�������܂ł̏����x��
        if (initialDelay > 0)
        {
            yield return new WaitForSecondsRealtime(initialDelay); // Time.timeScale�̉e�����󂯂Ȃ��悤��
        }

        while (isDisplayingEnabled) // �\�����L���ȊԂ̂݃��[�v
        {
            if (!isDisplayingSpecialBubble && speechBubbleSprites.Count > 0)
            {
                DisplayBubble(speechBubbleSprites[currentBubbleIndex]);
                currentBubbleIndex = (currentBubbleIndex + 1) % speechBubbleSprites.Count;
            }
            yield return new WaitForSecondsRealtime(displayInterval); // Time.timeScale�̉e�����󂯂Ȃ��悤��
        }
    }

    /// <summary>
    /// �w�肳�ꂽ�X�v���C�g�̐����o����\�����AfadeOutDuration��Ƀt�F�[�h�A�E�g�����܂��B
    /// </summary>
    /// <param name="spriteToDisplay">�\������X�v���C�g</param>
    private void DisplayBubble(Sprite spriteToDisplay ,bool displaytime = false,float displaytimevalue = 5)
    {
        if (!isDisplayingEnabled) return; // �\���������Ȃ牽�����Ȃ�

        // ���ݕ\�����̐����o���������DOTween�̃A�j���[�V�������~
        speechBubbleSpriteRenderer.DOKill(true);

        speechBubbleSpriteRenderer.sprite = spriteToDisplay;
        speechBubbleSpriteRenderer.color = new Color(1, 1, 1, 1); // �s�����ɂ���

        // fadeOutDuration�b��Ƀt�F�[�h�A�E�g���J�n
        if (displaytime) { //�ς������Ƃ��͂�����
            speechBubbleSpriteRenderer.DOFade(0, displaytimevalue)
    .SetDelay(1.0f)
    .SetUpdate(true) // Time.timeScale�̉e�����󂯂Ȃ��悤��
    .OnComplete(() =>
    {
        // �t�F�[�h�A�E�g�������ɓ��ʂȐ����o���t���O������
        if (isDisplayingSpecialBubble) // �f�t�H���g�ł�
        {
            isDisplayingSpecialBubble = false;
        }
    });
        }

        // ����displaytime��false�Ȃ�A�ʏ�̃t�F�[�h�A�E�g��K�p
        else
        {
            speechBubbleSpriteRenderer.DOFade(0, fadeOutDuration)
    .SetDelay(1.0f)
    .SetUpdate(true) // Time.timeScale�̉e�����󂯂Ȃ��悤��
    .OnComplete(() =>
    {
        // �t�F�[�h�A�E�g�������ɓ��ʂȐ����o���t���O������
        if (isDisplayingSpecialBubble)
        {
            isDisplayingSpecialBubble = false;
        }
    });
        }

    }

    /// <summary>
    /// ����̐����o�������荞��ŕ\�����܂��B
    /// </summary>
    /// <param name="specialSprite">���荞��ŕ\������X�v���C�g</param>
    public void DisplaySpecialBubble(Sprite specialSprite,bool display = false ,float displayvalue = 5f)
    {
        if (!isDisplayingEnabled) return; // �\���������Ȃ犄�荞�݂��ł��Ȃ�

        if (specialSprite == null)
        {
            Debug.LogWarning("���荞�ݕ\������X�v���C�g���w�肳��Ă��܂���B");
            return;
        }

        isDisplayingSpecialBubble = true;
        DisplayBubble(specialSprite,display,displayvalue);

        // �����\���̃R���[�`�������Z�b�g���āA���荞�ݕ\����ɒʏ�̕\���Ԋu���ĊJ
        if (displayCoroutine != null)
        {
            StopCoroutine(displayCoroutine);
            // �����ł͏����x�����X�L�b�v���āA�����ɒʏ�̃��[�v�ɖ߂�悤�ɂ��܂�
            displayCoroutine = StartCoroutine(AutoDisplayBubblesRoutineAfterInterruption());
        }
    }

    /// <summary>
    /// ���荞�ݕ\����ɒʏ�̐����o���\�����[�v���ĊJ���邽�߂̃R���[�`���ł��B
    /// �����x���͓K�p���܂���B
    /// </summary>
    private IEnumerator AutoDisplayBubblesRoutineAfterInterruption()
    {
        // ���荞�ݕ\������������̂�҂iDOTween��OnComplete��isDisplayingSpecialBubble��false�ɂȂ�̂�҂j
        // �������A�����Ɏ��̐����o�����\������Ȃ��悤�ɁAdisplayInterval�̑ҋ@�͕K�v
        yield return new WaitForSecondsRealtime(displayInterval); // ���荞�ݕ\����̍ŏ��̑ҋ@

        while (isDisplayingEnabled) // �\�����L���ȊԂ̂݃��[�v
        {
            if (!isDisplayingSpecialBubble && speechBubbleSprites.Count > 0)
            {
                DisplayBubble(speechBubbleSprites[currentBubbleIndex]);
                currentBubbleIndex = (currentBubbleIndex + 1) % speechBubbleSprites.Count;
            }
            yield return new WaitForSecondsRealtime(displayInterval);
        }
    }

    /// <summary>
    /// ���ݕ\�����̐����o���������ɏ����܂��B
    /// </summary>
    public void HideBubbleImmediately()
    {
        speechBubbleSpriteRenderer.DOKill(true); // DOTween�A�j���[�V�����������I��
        speechBubbleSpriteRenderer.color = new Color(1, 1, 1, 0); // �����ɓ����ɂ���
        isDisplayingSpecialBubble = false; // ���ꐁ���o���t���O������
    }

    // --- �V�K�ǉ����\�b�h ---

    /// <summary>
    /// �����o���̎����\�����~���A���ݕ\�����̐����o���������ɏ����܂��B
    /// </summary>
    public void StopDisplay()
    {
        isDisplayingEnabled = false; // �\���𖳌��ɂ���
        if (displayCoroutine != null)
        {
            StopCoroutine(displayCoroutine); // �����\���R���[�`�����~
        }
        //HideBubbleImmediately(); // ���ݕ\�����̐����o���������ɏ���
        Debug.Log("�����o���̕\�����~���܂����B");
    }

    /// <summary>
    /// ��~���Ă��������o���̎����\�����ĊJ���܂��B
    /// </summary>
    public void ResumeDisplay()
    {
        if (!isDisplayingEnabled) // ���ݒ�~���Ă���ꍇ�̂ݍĊJ
        {
            isDisplayingEnabled = true; // �\����L���ɂ���
            StartAutoDisplay(); // �����\�����ĊJ
            Debug.Log("�����o���̕\�����ĊJ���܂����B");
        }
    }

    public void HukidasiFailed()
    {
        DisplaySpecialBubble(Failed,true,4f);
    }

    public void HukidasiGYU()
    {
        DisplaySpecialBubble(GYU);
    }

    public void HukidasiStageClear()
    {
        DisplaySpecialBubble(StageClear,true, 4f);
    }

    public void HukidasiSpeedUP()
    {
        DisplaySpecialBubble(SpeedUP);
    }

    public void HukidasiSpeedDOWN()
    {
        DisplaySpecialBubble(SpeedDOWN);
    }
}