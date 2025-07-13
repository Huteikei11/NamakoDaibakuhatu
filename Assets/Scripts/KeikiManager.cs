using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeikiManager : MonoBehaviour
{
    [SerializeField] private OppaiManager oppaiManager;
    public int paiMode;
    public int beforepaiMode;
    public Animator oppaianime;

    [SerializeField] private List<SpriteRenderer> sprites; // Inspectorから設定するスプライトリスト

    private int difficulty;
    public bool transparent = false;
    public float totrans = 0.2f; // 見えなくなる確率
    public float canceltrans = 0.5f; // 透明を解除する確率
    public float changePaiMin = 2f; // パイズリモード変更の最小時間
    public float changePaiMax = 4f; // パイズリモード変更の最大時間

        [SerializeField] SpeechBubbleManager_SpriteRenderer speechBubble;

    void Start()
    {
        ScheduleNextPaiMode();
        difficulty = DifficultyManager.Instance != null ? DifficultyManager.Instance.GetDifficulty() : 0;
    }

    void Update()
    {
        oppaiManager.SetPaiMode(paiMode);
    }

    private void ScheduleNextPaiMode()
    {
        float nextTime = Random.Range(changePaiMin, changePaiMax); // ランダムな時間を設定
        Invoke("IsChangePaiMode", nextTime);
    }

    private void IsChangePaiMode()
    {
        if (Random.value > 0.2f) // 確率で実行(Radom.Valueは0~1.0)
        {
            paiMode = ChangePaiMode();
            oppaianime.SetInteger("State", paiMode);
            Debug.Log("パイズリモード変更:" + paiMode);
        }
        ScheduleNextPaiMode(); // 次のノイズスケジュールを設定
    }

    private int ChangePaiMode()
    {
        int result = Random.Range(0, 7); //とりあえず全パターン分

        if (difficulty == 0 || result == 4) //難易度AのときはパイズリCを除外
        {
            result = Random.Range(5, 7);
        }

        if (difficulty == 2)
        {
            if (transparent)//透明を解除
            {
                if (Random.value > canceltrans)
                {
                    transparent = false;
                    FadeInSprites(1f); // 透明度を徐々に下げる
                }

            }
            else if (Random.value > totrans) // 見えなくなる確率
            {
                transparent = true;
                FadeOutSprites(1f); // 透明度を徐々に上げる
            }
        }

        // 吹き出しの処理
        if (result == 2 || result == 3)
        {
            // ぎゅ～
            speechBubble.HukidasiGYU();
        }
        else if ((beforepaiMode == 0 || beforepaiMode == 2 || beforepaiMode == 5) && (result == 1 || result == 3 || result == 6))
        {
            // 早くなる
            speechBubble.HukidasiSpeedUP();
        }
        else if ((result == 0 || result == 2 || result == 5) && (beforepaiMode == 1 || beforepaiMode == 3 || beforepaiMode == 6))
        {
            // 遅くなる
            speechBubble.HukidasiSpeedDOWN();
        }


        beforepaiMode = result;
        oppaiManager.SetPaiMode(result);
        return result;
    }

    // スプライトを徐々に暗くして見えなくするメソッド
    public void FadeOutSprites(float duration)
    {
        StartCoroutine(FadeSpritesCoroutine(0f, duration));
    }

    // スプライトを元に戻すメソッド
    public void FadeInSprites(float duration)
    {
        StartCoroutine(FadeSpritesCoroutine(1f, duration));
    }

    // スプライトの透明度を変更するコルーチン
    private IEnumerator FadeSpritesCoroutine(float targetAlpha, float duration)
    {
        float elapsedTime = 0f;
        List<Color> initialColors = new List<Color>();

        // 初期カラーを保存
        foreach (var sprite in sprites)
        {
            if (sprite != null)
            {
                initialColors.Add(sprite.color);
            }
        }

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            for (int i = 0; i < sprites.Count; i++)
            {
                if (sprites[i] != null)
                {
                    Color color = initialColors[i];
                    color.a = Mathf.Lerp(initialColors[i].a, targetAlpha, t);
                    sprites[i].color = color;
                }
            }

            yield return null;
        }

        // 最終的なアルファ値を設定
        for (int i = 0; i < sprites.Count; i++)
        {
            if (sprites[i] != null)
            {
                Color color = sprites[i].color;
                color.a = targetAlpha;
                sprites[i].color = color;
            }
        }
    }
}
