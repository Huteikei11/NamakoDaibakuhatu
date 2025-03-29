using UnityEngine;
using TMPro; // TextMeshPro���g�����߂ɒǉ�
using System.Collections;
using System;

public class CountdownTimer : MonoBehaviour
{
    public float duration; // �^�C�}�[�̏������ԁi�b�j
    private float remainingTime; // �c�莞��
    private bool isRunning = false;
    private Coroutine timerCoroutine;

    public TMP_Text timerText; // **TextMeshPro�̎Q��**
    public event Action OnTimerEnd; // �^�C�}�[�I�����̃C�x���g

    void Start()
    {
        remainingTime = duration; // ������
        UpdateTimerText(); // �����l��\��
    }

    public void StartTimer(float time)
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
        UpdateTimerText(); // UI���X�V
    }

    private IEnumerator RunTimer()
    {
        isRunning = true;
        while (remainingTime > 0)
        {
            yield return new WaitForSeconds(1f);
            remainingTime--;
            UpdateTimerText(); // **UI���X�V**
        }
        isRunning = false;
        TimerEnded();
    }

    private void TimerEnded()
    {
        Debug.Log("�^�C�}�[�I���I");
        OnTimerEnd?.Invoke(); // **�C�x���g����**
        UpdateTimerText(); // **�^�C�}�[�I�����̕\���X�V**
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

    // **TextMeshPro�̃e�L�X�g���X�V**
    private void UpdateTimerText()
    {
        if (timerText != null)
        {
            timerText.text = GetFormattedTime();
        }
    }
}
