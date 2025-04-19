using TMPro; // TextMeshProを使用するための名前空間
using UnityEngine;
using System.Collections; // コルーチンを使用するための名前空間
using System.Collections.Generic; // Listを使用するための名前空間

public class LoadLatestPlayRecord : MonoBehaviour
{
    public SaveManager saveManager; // SaveManagerの参照を設定
    public Animator rankAnimator; // RankAnimatorの参照を設定
    public TextMeshProUGUI scoreText; // スコアを表示するTextMeshPro
    public TextMeshProUGUI resultPhraseText; // 結果フレーズを表示するTextMeshPro
    public SpriteRenderer resultSpriteRenderer; // isClearedに基づいてスプライトを切り替えるSpriteRenderer
    public Sprite clearedSprite; // クリア時のスプライト
    public Sprite failedSprite; // 失敗時のスプライト
    public SpriteRenderer darkenSpriteRenderer; // 画面を暗くするためのSpriteRenderer

    public SpriteRenderer hukidasi;

    // Keiki用のSpriteRendererとスプライトリスト
    public SpriteRenderer KeikiSpriteRenderer; // SpriteRendererの参照を設定
    public List<Sprite> KeikiSprites; // ランクごとのスプライトを管理するリスト

    // White用のSpriteRendererとスプライトリスト
    public SpriteRenderer whiteSpriteRenderer; // SpriteRendererの参照を設定
    public List<Sprite> whiteSprites; // ランクごとのスプライトを管理するリスト

    [SerializeField] private ResultMenuController menucontroller;

    private bool isCleared; // クリア状態を管理する変数

    void Start()
    {
        // セーブデータをロード
        saveManager.LoadGameData();




        darkenSpriteRenderer.gameObject.SetActive(false);
        resultSpriteRenderer.gameObject.SetActive(false);
        hukidasi.gameObject.SetActive(false);

        // 最新のPlayRecordを取得
        SaveManager.PlayRecord latestRecord = GetLatestPlayRecord();

        if (latestRecord != null)
        {
            // 最新のデータをログに出力
            Debug.Log($"最新のデータ: Rank={latestRecord.rank}, ResultPhraseNo={latestRecord.resultPhraseNo}, Score={latestRecord.score}, IsCleared={latestRecord.isCleared}");

            // RankAnimatorに最新のランクを設定
            ViewRank(latestRecord.rank);

            // スコアを表示
            ViewScore(latestRecord.score);

            // KeikiとWhiteのスプライトをランクに応じて切り替え
            UpdateKeikiSprite(latestRecord.rank);
            UpdateWhiteSprite(latestRecord.rank);


            isCleared = latestRecord.isCleared; // クリア状態を保存

            // resultPhraseNoを一文字ずつ表示し、Enterキーを待機
            StartCoroutine(ViewResultPhrase(latestRecord.resultPhraseNo));
        }
        else
        {
            Debug.Log("PlayRecordが存在しません。");
        }
    }

    private SaveManager.PlayRecord GetLatestPlayRecord()
    {
        // SaveManagerのplayRecordsリストが空でない場合、最新のデータを取得
        if (saveManager.playRecords != null && saveManager.playRecords.Count > 0)
        {
            return saveManager.playRecords[saveManager.playRecords.Count - 1]; // 最後の要素を取得
        }

        return null; // データが存在しない場合はnullを返す
    }

    private void ViewRank(int rank)
    {
        rankAnimator.SetInteger("rank", rank);
    }

    private void ViewScore(float score)
    {
        if (scoreText != null)
        {
            scoreText.text = $"{score:F0}"; // スコアを小数点以下0桁で表示
        }
        else
        {
            Debug.LogError("ScoreTextが設定されていません。");
        }
    }

    private void UpdateKeikiSprite(int rank)
    {
        if (rank >= 0 && rank < KeikiSprites.Count)
        {
            // ランクに対応するスプライトを設定
            KeikiSpriteRenderer.sprite = KeikiSprites[rank];
        }
        else
        {
            Debug.LogError($"Rank {rank} に対応するKeikiスプライトが存在しません。KeikiSpritesの範囲を確認してください。");
        }
    }

    private void UpdateWhiteSprite(int rank)
    {
        if (rank >= 0 && rank < whiteSprites.Count)
        {
            // ランクに対応するスプライトを設定
            whiteSpriteRenderer.sprite = whiteSprites[rank];
        }
        else
        {
            Debug.LogError($"Rank {rank} に対応するWhiteスプライトが存在しません。WhiteSpritesの範囲を確認してください。");
        }
    }

    private void UpdateResultSprite(bool isCleared)
    {
        if (resultSpriteRenderer != null)
        {
            // クリア状態に応じてスプライトを切り替え
            resultSpriteRenderer.sprite = isCleared ? clearedSprite : failedSprite;
        }
        else
        {
            Debug.LogError("ResultSpriteRendererが設定されていません。");
        }
    }

    private IEnumerator ViewResultPhrase(string phrase)
    {
        // Enterキーが押されるまで待機
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));

        hukidasi.gameObject.SetActive(true);
        resultPhraseText.text = ""; // テキストを初期化

        // 一文字ずつ表示
        foreach (char c in phrase)
        {
            resultPhraseText.text += c;
            yield return new WaitForSeconds(0.08f); // 0.05秒待機
        }

        // Enterキーが押されるまで待機
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));

        // 画面を暗くする
        if (darkenSpriteRenderer != null)
        {
            darkenSpriteRenderer.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("DarkenSpriteRendererが設定されていません。");
        }

        // resultSpriteRendererを有効にして表示
        if (resultSpriteRenderer != null)
        {
            resultSpriteRenderer.gameObject.SetActive(true);
            // isClearedに基づいてスプライトを切り替え
            UpdateResultSprite(isCleared);
        }

        //メニュー表示
        menucontroller.ShowDynamicMenu(isCleared);
    }
}


