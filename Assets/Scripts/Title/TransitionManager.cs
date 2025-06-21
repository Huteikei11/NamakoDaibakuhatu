using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using System;

[System.Serializable]
public class TransitionTarget
{
    public string name;
    public Animator animator;
    public List<float> destinationXPositions = new List<float>(); // �ړI�n�̍��W��6�w��
    [HideInInspector] public float initialXPosition; // �N������X���W��ۑ�
}

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance;

    [Header("�o�^����A�j���[�^�[����")]
    [SerializeField] private List<TransitionTarget> targets = new List<TransitionTarget>();

    [Header("�ǉ��̃A�j���[�^�[����")]
    [SerializeField] private List<Animator> additionalAnimators = new List<Animator>(); // �ǉ���6��Animator

    [Header("�g�����W�V�����̑ҋ@����")]
    [SerializeField] private float fadeOutDuration = 1.0f;
    [SerializeField] private float fadeInDelay = 0.5f;
    [SerializeField] private float logoDelay = 1.0f;

    private string nextSceneName;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // �N�����Ɋe�^�[�Q�b�g�̏���X���W��ۑ�
        foreach (var target in targets)
        {
            if (target.animator != null)
            {
                target.initialXPosition = target.animator.transform.position.x;
            }
        }
        // �ǉ��̃A�j���[�^�[���\���ɐݒ�
        foreach (var animator in additionalAnimators)
        {
            var spriteRenderer = animator.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                var color = spriteRenderer.color;
                color.a = 0f; // ��\��
                spriteRenderer.color = color;
            }
        }
    }

    public void TransitionToScene(string scene)
    {
        nextSceneName = scene;

        // �g�����W�V�����������J�n
        StartCoroutine(TransitionRoutine());
    }

    private IEnumerator TransitionRoutine()
    {

        // 1. FadeOut ��x�点�� 3 �񂸂K�p
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < targets.Count; j += 2) // 2������
            {
                for (int k = j; k < j + 2 && k < targets.Count; k++) // j�Ԗڂ���2�̃^�[�Q�b�g������
                {
                    var target = targets[k];
                    if (target.destinationXPositions.Count > i) // �ړI�n���w�肳��Ă���ꍇ�̂�
                    {
                        float destinationX = target.destinationXPositions[i]; // i�Ԗڂ̖ړI�n���擾
                        target.animator.transform.DOMoveX(destinationX, 0.2f).SetEase(Ease.InOutQuad);
                    }
                }
                yield return new WaitForSeconds(0.1f); // �x��
            }
        }
        // �ǉ��̃A�j���[�^�[��\��
        for (int n = 0; n < additionalAnimators.Count; n++)
        {
            var spriteRenderer = additionalAnimators[n].GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                var color = spriteRenderer.color;
                color.a = 1f; // �\��
                spriteRenderer.color = color;
            }
            additionalAnimators[n].SetTrigger("Start");
            yield return new WaitForSeconds(0.06f); // �x��
        }

        // 2. ���ׂẴI�u�W�F�N�g�� SetTrigger("Blue") �𑗐M
        foreach (var target in targets)
        {
            target.animator.SetTrigger("Blue");
        }

        yield return new WaitForSeconds(0.1f); // �K�؂Ȓx����}��

        // 3. �A�j���[�V�������t�Đ������Ԃɂ��� "BlueOut" �𑗐M
        yield return new WaitForSeconds(1.0f); // �K�؂Ȓx����}��

        // 5. �V�[���J��
        SceneManager.LoadScene(nextSceneName);
        SceneManager.sceneLoaded += OnSceneLoadedCoroutine;
        yield return new WaitForSeconds(0.2f); // �K�؂Ȓx����}��

        // ��Ԃ����ɖ߂�
        foreach (var target in targets)
        {
            target.animator.SetTrigger("BlueOut"); // �g���K�[�𑗐M
        }

        // �ǉ��̃A�j���[�^�[���\���ɐݒ�
        for (int n = additionalAnimators.Count-1; n >=0; n--)
        {
            additionalAnimators[n].SetTrigger("End");
            yield return new WaitForSeconds(0.06f); // �x��
            var spriteRenderer = additionalAnimators[n].GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                var color = spriteRenderer.color;
                color.a = 0f; // ��\��
                spriteRenderer.color = color;
            }

        }

        // 4. �ŏ��̈ʒu�ɖ߂� (���� target ����2���ړ�)
        for (int i = 0; i < targets.Count; i += 2)
        {
            for (int j = Mathf.Min(targets.Count - 1, targets.Count - 1 - i); j >= Mathf.Max(0, targets.Count - 2 - i); j--)
            {
                var target = targets[j];
                target.animator.transform.DOMoveX(target.initialXPosition, 0.2f).SetEase(Ease.InOutQuad); // �����ʒu�ɖ߂�
            }
            yield return new WaitForSeconds(0.1f); // �x��
        }


    }
    private void OnSceneLoadedCoroutine(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoadedCoroutine;
        StartCoroutine(PlaySceneEnterEffects());
    }

    private IEnumerator PlaySceneEnterEffects()
    {
        // �����҂��Ă���FadeIn�J�n
        yield return new WaitForSeconds(fadeInDelay);

        foreach (var target in targets)
        {
            if (target.name == "FadePanel")
            {
                target.animator.SetTrigger("FadeIn");
            }
        }

        // ���S�Ȃǂ̉��o������ɒx�点��
        yield return new WaitForSeconds(logoDelay);

        foreach (var target in targets)
        {
            if (target.name == "LogoPanel")
            {
                target.animator.SetTrigger("PlayLogo");
            }
        }
    }
}
