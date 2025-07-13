using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening; // DOTweenを使用するために必要

public class SpeechBubbleManager_SpriteRenderer : MonoBehaviour
{
    [Header("吹き出し設定")]
    [SerializeField] private SpriteRenderer speechBubbleSpriteRenderer; // 吹き出しを表示するSprite Rendererコンポーネント
    [SerializeField] private List<Sprite> speechBubbleSprites; // 吹き出し画像のリスト
    [SerializeField] private float initialDelay = 0.0f; // 最初の吹き出しが表示されるまでの初期遅延（秒）
    [SerializeField] private float displayInterval = 3.0f; // 次の吹き出しが表示されるまでの間隔（秒）
    [SerializeField] private float fadeOutDuration = 1.0f; // 吹き出しが消えるまでの時間（秒）

    private int currentBubbleIndex = 0;
    private Coroutine displayCoroutine;
    private bool isDisplayingSpecialBubble = false;
    private bool isDisplayingEnabled = true; // 吹き出し表示が有効かどうかを管理するフラグ

    public Sprite Failed;
    public Sprite GYU;
    public Sprite StageClear;
    public Sprite SpeedUP;
    public Sprite SpeedDOWN;

    void Start()
    {
        if (speechBubbleSpriteRenderer == null)
        {
            Debug.LogError("SpeechBubbleSpriteRendererが設定されていません！");
            enabled = false;
            return;
        }
        // 初期状態では透明にしておく（Sprite RendererのcolorプロパティはColor型）
        speechBubbleSpriteRenderer.color = new Color(1, 1, 1, 0);
        StartAutoDisplay();
    }

    /// <summary>
    /// 自動での吹き出し表示を開始します。
    /// </summary>
    public void StartAutoDisplay()
    {
        if (displayCoroutine != null)
        {
            StopCoroutine(displayCoroutine);
        }
        if (isDisplayingEnabled) // 表示が有効な場合のみ開始
        {
            displayCoroutine = StartCoroutine(AutoDisplayBubblesRoutine());
        }
    }

    /// <summary>
    /// 吹き出しを自動で順番に表示するコルーチンです。
    /// </summary>
    private IEnumerator AutoDisplayBubblesRoutine()
    {
        // 最初の吹き出しが表示されるまでの初期遅延
        if (initialDelay > 0)
        {
            yield return new WaitForSecondsRealtime(initialDelay); // Time.timeScaleの影響を受けないように
        }

        while (isDisplayingEnabled) // 表示が有効な間のみループ
        {
            if (!isDisplayingSpecialBubble && speechBubbleSprites.Count > 0)
            {
                DisplayBubble(speechBubbleSprites[currentBubbleIndex]);
                currentBubbleIndex = (currentBubbleIndex + 1) % speechBubbleSprites.Count;
            }
            yield return new WaitForSecondsRealtime(displayInterval); // Time.timeScaleの影響を受けないように
        }
    }

    /// <summary>
    /// 指定されたスプライトの吹き出しを表示し、fadeOutDuration後にフェードアウトさせます。
    /// </summary>
    /// <param name="spriteToDisplay">表示するスプライト</param>
    private void DisplayBubble(Sprite spriteToDisplay ,bool displaytime = false,float displaytimevalue = 5)
    {
        if (!isDisplayingEnabled) return; // 表示が無効なら何もしない

        // 現在表示中の吹き出しがあればDOTweenのアニメーションを停止
        speechBubbleSpriteRenderer.DOKill(true);

        speechBubbleSpriteRenderer.sprite = spriteToDisplay;
        speechBubbleSpriteRenderer.color = new Color(1, 1, 1, 1); // 不透明にする

        // fadeOutDuration秒後にフェードアウトを開始
        if (displaytime) { //変えたいときはこっち
            speechBubbleSpriteRenderer.DOFade(0, displaytimevalue)
    .SetDelay(1.0f)
    .SetUpdate(true) // Time.timeScaleの影響を受けないように
    .OnComplete(() =>
    {
        // フェードアウト完了時に特別な吹き出しフラグを解除
        if (isDisplayingSpecialBubble) // デフォルトでは
        {
            isDisplayingSpecialBubble = false;
        }
    });
        }

        // もしdisplaytimeがfalseなら、通常のフェードアウトを適用
        else
        {
            speechBubbleSpriteRenderer.DOFade(0, fadeOutDuration)
    .SetDelay(1.0f)
    .SetUpdate(true) // Time.timeScaleの影響を受けないように
    .OnComplete(() =>
    {
        // フェードアウト完了時に特別な吹き出しフラグを解除
        if (isDisplayingSpecialBubble)
        {
            isDisplayingSpecialBubble = false;
        }
    });
        }

    }

    /// <summary>
    /// 特定の吹き出しを割り込んで表示します。
    /// </summary>
    /// <param name="specialSprite">割り込んで表示するスプライト</param>
    public void DisplaySpecialBubble(Sprite specialSprite,bool display = false ,float displayvalue = 5f)
    {
        if (!isDisplayingEnabled) return; // 表示が無効なら割り込みもできない

        if (specialSprite == null)
        {
            Debug.LogWarning("割り込み表示するスプライトが指定されていません。");
            return;
        }

        isDisplayingSpecialBubble = true;
        DisplayBubble(specialSprite,display,displayvalue);

        // 自動表示のコルーチンをリセットして、割り込み表示後に通常の表示間隔を再開
        if (displayCoroutine != null)
        {
            StopCoroutine(displayCoroutine);
            // ここでは初期遅延をスキップして、すぐに通常のループに戻るようにします
            displayCoroutine = StartCoroutine(AutoDisplayBubblesRoutineAfterInterruption());
        }
    }

    /// <summary>
    /// 割り込み表示後に通常の吹き出し表示ループを再開するためのコルーチンです。
    /// 初期遅延は適用しません。
    /// </summary>
    private IEnumerator AutoDisplayBubblesRoutineAfterInterruption()
    {
        // 割り込み表示が完了するのを待つ（DOTweenのOnCompleteでisDisplayingSpecialBubbleがfalseになるのを待つ）
        // ただし、即座に次の吹き出しが表示されないように、displayIntervalの待機は必要
        yield return new WaitForSecondsRealtime(displayInterval); // 割り込み表示後の最初の待機

        while (isDisplayingEnabled) // 表示が有効な間のみループ
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
    /// 現在表示中の吹き出しをすぐに消します。
    /// </summary>
    public void HideBubbleImmediately()
    {
        speechBubbleSpriteRenderer.DOKill(true); // DOTweenアニメーションを強制終了
        speechBubbleSpriteRenderer.color = new Color(1, 1, 1, 0); // 即座に透明にする
        isDisplayingSpecialBubble = false; // 特殊吹き出しフラグも解除
    }

    // --- 新規追加メソッド ---

    /// <summary>
    /// 吹き出しの自動表示を停止し、現在表示中の吹き出しもすぐに消します。
    /// </summary>
    public void StopDisplay()
    {
        isDisplayingEnabled = false; // 表示を無効にする
        if (displayCoroutine != null)
        {
            StopCoroutine(displayCoroutine); // 自動表示コルーチンを停止
        }
        //HideBubbleImmediately(); // 現在表示中の吹き出しをすぐに消す
        Debug.Log("吹き出しの表示を停止しました。");
    }

    /// <summary>
    /// 停止していた吹き出しの自動表示を再開します。
    /// </summary>
    public void ResumeDisplay()
    {
        if (!isDisplayingEnabled) // 現在停止している場合のみ再開
        {
            isDisplayingEnabled = true; // 表示を有効にする
            StartAutoDisplay(); // 自動表示を再開
            Debug.Log("吹き出しの表示を再開しました。");
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