using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueController : MonoBehaviour
{
    public static DialogueController Instance { get; private set; }
    [SerializeField] private GameManager gameManager;
    [SerializeField] private CountdownTimer countdownTimer;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private OppaiManager oppaiManager;
    [SerializeField] private Gaman gaman;
    [SerializeField] private SaveManager saveManager;
    [SerializeField] private ScoreGauge scoreGauge;
    [SerializeField] private MayumiManager mayumiManager;
    public Animator keikianim;
    [SerializeField] private int[] shotnums;
    public GameObject karioki;
    [Header("複製したいプレハブ")]
    public GameObject prefab;
    [Header("生成間隔（秒）")]
    public float spawnInterval = 0.5f;
    [Header("射精バー止める")]
    public Rigidbody2D rigbar;
    public ObjectController2D controller2D;
    [Header("ランクごとのセリフを記録するリスト")]
    public List<RankPhrases> rankPhrases = new List<RankPhrases>();

    private int difficulty;
    public float ejaculationScore;

    [System.Serializable]
    public class RankPhrases
    {
        public List<string> phrases;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        difficulty = DifficultyManager.Instance != null ? DifficultyManager.Instance.GetDifficulty() : 0;
    }

    public IEnumerator WaitForEnterPress()
    {
        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null;
        }
    }

    public IEnumerator WaitForAnyKeyPress()
    {
        while (!Input.anyKeyDown)
        {
            yield return null;
        }
    }

    // 指定の秒数待機するメソッド
    public IEnumerator WaitForSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    public void debugFinish(bool success)//ボタンで呼び出す用（デバッグ)
    {
        StartCoroutine(FinishDialog(success));
    }

    public IEnumerator FinishDialog(bool success)//射精が終わったときの演出
    //とりあえず失敗時と成功時にどっちもこれがよびだされるようにしてある
    {
        oppaiManager.isFinish = true;//失敗の処理をとめる
        //タイマーとスコア計算を止める
        countdownTimer.StopTimer();
        scoreManager.StopLoop();
        //入力をとめる
        gaman.isOperable = false;
        StopObject();

        //演出とか
        yield return new WaitForSeconds(0.5f);
        //yield return StartCoroutine(WaitForEnterPress());


        float score = scoreManager.score; // スコアを取得
        SaveRank(difficulty, scoreGauge.GetRank(), score, success); // セリフを保存する

        if (success) // 成功時
        {
            int area = Mathf.Min(scoreManager.gamanArea, 3);
            int num = shotnums[area];
            StartCoroutine(SpawnObjects(num, "Success"));
        }
        else // 失敗時
        {
            StartCoroutine(SpawnFailed("Failed"));
        }

    }

    private void SaveRank(int difficulty, int rank, float score,bool clear)
    {
        if (rank < 0 || rank >= rankPhrases.Count)
        {
            Debug.LogError("Invalid rank value");
            return;
        }

        List<string> phrases = rankPhrases[rank].phrases;
        if (phrases == null || phrases.Count == 0)
        {
            Debug.LogError("No phrases available for the given rank");
            return;
        }

        // ランダムにセリフを選択
        int randomIndex = Random.Range(0, phrases.Count);
        string selectedPhrase = phrases[randomIndex];

        // SaveManagerのAddPlayRecordを呼び出してセーブ
        saveManager.AddPlayRecord(difficulty, rank, selectedPhrase, score,clear);
    }


    IEnumerator SpawnObjects(int spawnCount, string triggerName)//成功
    {
        // 時間満了時の演出
        // まゆみちゃんのアニメーションを止める
        keikianim.speed = 0;
        // 少し待つ
        yield return new WaitForSeconds(2f);

        // たまってる方 けいきのアニメーションを変える
        keikianim.speed = 1;
        keikianim.SetTrigger(triggerName);

        //キー入力を待つ
        yield return StartCoroutine(WaitForAnyKeyPress());

        // まゆみちゃん射精表情
        mayumiManager.mode = 1;

        for (int i = 0; i < spawnCount; i++)
        {
            GameObject instance = Instantiate(prefab, transform.position, Quaternion.identity);

            // y座標だけ -3.9f に設定（x, z は元の位置を維持）
            Vector3 pos = instance.transform.position;
            pos.y = -3.9f;
            instance.transform.position = pos;

            Animator animator = instance.GetComponent<Animator>();

            if (animator != null)
            {
                int randomValue = Random.Range(0, 4); // 0〜3のランダムな整数
                animator.SetInteger("State", randomValue);
                animator.SetTrigger(triggerName);
            }
            else
            {
                Debug.LogWarning("Animatorが見つかりませんでした: " + instance.name);
            }
            scoreManager.score += ejaculationScore; // スコアを加算

            yield return new WaitForSeconds(spawnInterval);
        }
        // まゆみちゃん余韻表情
        mayumiManager.mode = 2;
        keikianim.SetTrigger("AnyKey");//射精モーション止める

        yield return new WaitForSeconds(3f);
        TransitionManager.Instance.TransitionToScene("Result");
    }

    IEnumerator SpawnFailed(string triggerName)
    {
        //たまってる方 けいきのアニメーションを変える
        keikianim.SetTrigger(triggerName);
        yield return new WaitForSeconds(spawnInterval);

        // まゆみちゃん射精表情
        mayumiManager.mode = 1;

        //発射する方　精液のアニメーションを出す
        int spawnCount = Random.Range(1, 5);//ランダムな回数射精
        for (int i = 0; i < spawnCount; i++)
        {
            GameObject instance = Instantiate(prefab, transform.position, Quaternion.identity);

            // y座標だけ -3.9f に設定（x, z は元の位置を維持）
            Vector3 pos = instance.transform.position;
            pos.y = -3.9f;
            instance.transform.position = pos;

            Animator animator = instance.GetComponent<Animator>();

            if (animator != null)
            {
                int randomValue = Random.Range(0, 4); // 0〜3のランダムな整数
                animator.SetInteger("State", randomValue);
                animator.SetTrigger(triggerName);
            }
            else
            {
                Debug.LogWarning("Animatorが見つかりませんでした: " + instance.name);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
        // まゆみちゃん余韻表情
        mayumiManager.mode = 2;

        yield return new WaitForSeconds(3f);
        TransitionManager.Instance.TransitionToScene("Result");
    }

    private void StopObject()
    {
        rigbar.isKinematic = true;
        rigbar.gravityScale = 0;
        controller2D.enabled = false;
        rigbar.velocity = Vector3.zero;
    }
}   