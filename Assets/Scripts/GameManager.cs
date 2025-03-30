using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool IsGameStarted { get; private set; } = false;
    public bool IsPaused { get; private set; } = true; // 最初はポーズ状態（ゲーム開始前）

    //public CanvasGroup fadePanel; // フェード用UI
    public GameObject pauseMenu;  // ポーズメニューUI
    private List<Rigidbody2D> allRigidbodies;
    private List<Animator> allAnimators;

    public int difficulty;
    [SerializeField] private CountdownTimer countdownTimer;

    private void Awake()
    {
        difficulty = DifficultyManager.Instance != null ? DifficultyManager.Instance.GetDifficulty() : 0;
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // シーン内の Rigidbody2D と Animator を取得
        allRigidbodies = new List<Rigidbody2D>(FindObjectsOfType<Rigidbody2D>());
        allAnimators = new List<Animator>(FindObjectsOfType<Animator>());

        Time.timeScale = 0f; // 最初はゲーム停止
        pauseMenu.SetActive(false); // ゲーム開始前はポーズパネルを非表示

    }

    private void Update()
    {
        if (!IsGameStarted && Input.GetKeyDown(KeyCode.Return)) // Enter でゲーム開始
        {
            StartCoroutine(StartGame());
        }
        else if (IsGameStarted && Input.GetKeyDown(KeyCode.Escape)) // Escape でポーズ切り替え
        {
            TogglePause();
        }
    }

    IEnumerator StartGame()
    {
        //yield return FadeIn();
        yield return new WaitForSecondsRealtime(0.5f); // フェード後に待機

        IsGameStarted = true;
        countdownTimer.StartTimer(60f); // カウントダウン開始

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
