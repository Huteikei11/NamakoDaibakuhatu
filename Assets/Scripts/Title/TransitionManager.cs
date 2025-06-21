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
    public List<float> destinationXPositions = new List<float>(); // 目的地の座標を6つ指定
    [HideInInspector] public float initialXPosition; // 起動時のX座標を保存
}

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance;

    [Header("登録するアニメーターたち")]
    [SerializeField] private List<TransitionTarget> targets = new List<TransitionTarget>();

    [Header("追加のアニメーターたち")]
    [SerializeField] private List<Animator> additionalAnimators = new List<Animator>(); // 追加の6つのAnimator

    [Header("トランジションの待機時間")]
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

        // 起動時に各ターゲットの初期X座標を保存
        foreach (var target in targets)
        {
            if (target.animator != null)
            {
                target.initialXPosition = target.animator.transform.position.x;
            }
        }
        // 追加のアニメーターを非表示に設定
        foreach (var animator in additionalAnimators)
        {
            var spriteRenderer = animator.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                var color = spriteRenderer.color;
                color.a = 0f; // 非表示
                spriteRenderer.color = color;
            }
        }
    }

    public void TransitionToScene(string scene)
    {
        nextSceneName = scene;

        // トランジション処理を開始
        StartCoroutine(TransitionRoutine());
    }

    private IEnumerator TransitionRoutine()
    {

        // 1. FadeOut を遅らせて 3 回ずつ適用
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < targets.Count; j += 2) // 2つずつ処理
            {
                for (int k = j; k < j + 2 && k < targets.Count; k++) // j番目から2つのターゲットを処理
                {
                    var target = targets[k];
                    if (target.destinationXPositions.Count > i) // 目的地が指定されている場合のみ
                    {
                        float destinationX = target.destinationXPositions[i]; // i番目の目的地を取得
                        target.animator.transform.DOMoveX(destinationX, 0.2f).SetEase(Ease.InOutQuad);
                    }
                }
                yield return new WaitForSeconds(0.1f); // 遅延
            }
        }
        // 追加のアニメーターを表示
        for (int n = 0; n < additionalAnimators.Count; n++)
        {
            var spriteRenderer = additionalAnimators[n].GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                var color = spriteRenderer.color;
                color.a = 1f; // 表示
                spriteRenderer.color = color;
            }
            additionalAnimators[n].SetTrigger("Start");
            yield return new WaitForSeconds(0.06f); // 遅延
        }

        // 2. すべてのオブジェクトに SetTrigger("Blue") を送信
        foreach (var target in targets)
        {
            target.animator.SetTrigger("Blue");
        }

        yield return new WaitForSeconds(0.1f); // 適切な遅延を挿入

        // 3. アニメーションを逆再生する状態にして "BlueOut" を送信
        yield return new WaitForSeconds(1.0f); // 適切な遅延を挿入

        // 5. シーン遷移
        SceneManager.LoadScene(nextSceneName);
        SceneManager.sceneLoaded += OnSceneLoadedCoroutine;
        yield return new WaitForSeconds(0.2f); // 適切な遅延を挿入

        // 状態を元に戻す
        foreach (var target in targets)
        {
            target.animator.SetTrigger("BlueOut"); // トリガーを送信
        }

        // 追加のアニメーターを非表示に設定
        for (int n = additionalAnimators.Count-1; n >=0; n--)
        {
            additionalAnimators[n].SetTrigger("End");
            yield return new WaitForSeconds(0.06f); // 遅延
            var spriteRenderer = additionalAnimators[n].GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                var color = spriteRenderer.color;
                color.a = 0f; // 非表示
                spriteRenderer.color = color;
            }

        }

        // 4. 最初の位置に戻す (後ろの target から2つずつ移動)
        for (int i = 0; i < targets.Count; i += 2)
        {
            for (int j = Mathf.Min(targets.Count - 1, targets.Count - 1 - i); j >= Mathf.Max(0, targets.Count - 2 - i); j--)
            {
                var target = targets[j];
                target.animator.transform.DOMoveX(target.initialXPosition, 0.2f).SetEase(Ease.InOutQuad); // 初期位置に戻す
            }
            yield return new WaitForSeconds(0.1f); // 遅延
        }


    }
    private void OnSceneLoadedCoroutine(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoadedCoroutine;
        StartCoroutine(PlaySceneEnterEffects());
    }

    private IEnumerator PlaySceneEnterEffects()
    {
        // 少し待ってからFadeIn開始
        yield return new WaitForSeconds(fadeInDelay);

        foreach (var target in targets)
        {
            if (target.name == "FadePanel")
            {
                target.animator.SetTrigger("FadeIn");
            }
        }

        // ロゴなどの演出をさらに遅らせる
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
