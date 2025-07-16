using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool IsGameStarted { get; private set; } = false;
    public bool IsPaused { get; private set; } = true; // 最初はポーズ状態（ゲーム開始前）

    public GameObject pauseMenu;  // ポーズメニューUI
    public  List<Rigidbody2D> allRigidbodies;
    public List<Animator> allAnimators;

    public int difficulty;
    public int gametime;
    [SerializeField] private CountdownTimer countdownTimer;
    [SerializeField] private ScoreManager scoreManager; // スコアマネージャーの参照]
    [SerializeField] private PaiSE paiSE; // 効果音の参照

    private bool isWaitingStart = true; // 3秒待機中かどうか

    private void Awake()
    {
        difficulty = DifficultyManager.Instance != null ? DifficultyManager.Instance.GetDifficulty() : 0;
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        allRigidbodies = new List<Rigidbody2D>(FindObjectsOfType<Rigidbody2D>());
        allAnimators = new List<Animator>(FindObjectsOfType<Animator>());
    }

    private void Start()
    {


        //Time.timeScale = 0f; // 最初はゲーム停止
        pauseMenu.SetActive(false); // ゲーム開始前はポーズパネルを非表示

        StartCoroutine(WaitAndStartGame());
    }

    private IEnumerator WaitAndStartGame()
    {
        Debug.Log("Waiting for game start...");
        TogglePauseLoad(true); // （ポーズ）
        isWaitingStart = true;

        yield return new WaitForSecondsRealtime(3f); // 3秒間リアルタイムで待機
        isWaitingStart = false;
        IsGameStarted = true;

        countdownTimer.StartTimer(gametime); // カウントダウン開始
        scoreManager.ReStartLoop(); // スコアマネージャーのループを再開
        Debug.Log("Game started!");
        TogglePauseLoad(false); // ゲーム開始（ポーズ解除）
    }

    private void Update()
    {
        // 3秒待機中は何もしない
        if (isWaitingStart) return;

        // ゲーム開始後のポーズ切り替え
        if (IsGameStarted && Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSecondsRealtime(0.5f); // フェード後に待機

        countdownTimer.StartTimer(gametime); // カウントダウン開始

        TogglePause(); // ゲーム開始（ポーズ解除）
    }

    public void TogglePause(bool panel = true)
    {
        IsPaused = !IsPaused;
        if (panel)
        {
            pauseMenu.SetActive(IsPaused); // ポーズメニューの表示切替
        }

        Time.timeScale = IsPaused ? 0f : 1f; // ポーズ時にゲーム停止

        
        // Rigidbody2D の動きを止める・再開
        foreach (var rb in allRigidbodies)
        {
            if (rb != null)
            {
                rb.isKinematic = IsPaused;
                if (!IsPaused) rb.velocity = Vector2.zero;
                if (!IsPaused) rb.gravityScale = 0;
            }
        }

        // Animator の動きを止める・再開
        foreach (var animator in allAnimators)
        {
                animator.enabled = !IsPaused;
        }

        paiSE.SetPlaying(!IsPaused); // 効果音のポーズ切り替え

        if (IsPaused)
        {
            countdownTimer.StopTimer();
        }
        else
        {
            countdownTimer.ResumeTimer();
        }
    }


    public void TogglePauseLoad(bool hoge = true)//画面遷移の時だけ使う
    {
        IsPaused = hoge;

        //Time.timeScale = IsPaused ? 0f : 1f; // ポーズ時にゲーム停止


        // Rigidbody2D の動きを止める・再開
        foreach (var rb in allRigidbodies)
        {
            if (rb != null)
            {
                rb.isKinematic = IsPaused;
                if (!IsPaused) rb.velocity = Vector2.zero;
                if (!IsPaused) rb.gravityScale = 0;
            }
        }

        // Animator の動きを止める・再開
        foreach (var animator in allAnimators)
        {
            if (animator != null)
            {
                animator.enabled = !IsPaused;
            }
        }


        paiSE.SetPlaying(!IsPaused); // 効果音のポーズ切り替え

        if (IsPaused)
        {
            countdownTimer.StopTimer();
        }
        else
        {
            countdownTimer.ResumeTimer();
        }
    }
    IEnumerator FadeIn()//多分使わない
    {
        float duration = 1.5f;
        float time = 0f;
        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            //fadePanel.alpha = 1f - (time / duration);
            yield return null;
        }
        //fadePanel.alpha = 0f;
    }
}
