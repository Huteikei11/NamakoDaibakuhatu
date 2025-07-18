using UnityEngine;
using TMPro; // TextMeshProを使うために追加
using System.Collections;
using System;

public class CountdownTimer : MonoBehaviour
{
    private float duration = 30; // タイマーの初期時間（秒）
    private float remainingTime; // 残り時間
    private bool isRunning = false;
    private Coroutine timerCoroutine;

    public TMP_Text timerText; // **TextMeshProの参照**

    void Start()
    {
        remainingTime = duration; // 初期化
        UpdateTimerText(); // 初期値を表示
    }

    public void StartTimer(float time)//GameManagerから呼び出す
    {
        duration = time;
        remainingTime = duration;
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }
        timerCoroutine = StartCoroutine(RunTimer());
    }

    public void StopTimer()
    {
        if (isRunning && timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            isRunning = false;
        }
    }

    public void ResumeTimer()
    {
        if (!isRunning && remainingTime > 0)
        {
            timerCoroutine = StartCoroutine(RunTimer());
        }
    }

    public void ResetTimer()
    {
        StopTimer();
        remainingTime = duration;
        UpdateTimerText(); // UIを更新
    }

    private IEnumerator RunTimer()
    {
        isRunning = true;
        while (remainingTime > 0)
        {
            yield return new WaitForSeconds(1f);
            remainingTime--;
            UpdateTimerText(); // **UIを更新**
        }
        isRunning = false;
        TimerEnded();
    }

    private void TimerEnded()
    {
        Debug.Log("タイマー終了！");
        if (DifficultyManager.Instance != null)//クリアのフラグ
        {
            DifficultyManager.Instance.SetGameCleared(true);
        }
        StartCoroutine(FinishEvent());
        UpdateTimerText(); // **タイマー終了時の表示更新**
    }

    private IEnumerator FinishEvent()//呼び出す関数をとりあえず失敗時と同じにしている
    {
        yield return StartCoroutine(DialogueController.Instance.FinishDialog(true));
    }

    public float GetRemainingTime()
    {
        return remainingTime;
    }

    public string GetFormattedTime()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        return string.Format("{0}:{1:D2}", minutes, seconds);
    }

    // **TextMeshProのテキストを更新**
    private void UpdateTimerText()
    {
        if (timerText != null)
        {
            timerText.text = GetFormattedTime();
        }
    }
}
