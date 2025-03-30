using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool IsGameStarted { get; private set; } = false;
    public bool IsPaused { get; private set; } = true; // �ŏ��̓|�[�Y��ԁi�Q�[���J�n�O�j

    //public CanvasGroup fadePanel; // �t�F�[�h�pUI
    public GameObject pauseMenu;  // �|�[�Y���j���[UI
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
        // �V�[������ Rigidbody2D �� Animator ���擾
        allRigidbodies = new List<Rigidbody2D>(FindObjectsOfType<Rigidbody2D>());
        allAnimators = new List<Animator>(FindObjectsOfType<Animator>());

        Time.timeScale = 0f; // �ŏ��̓Q�[����~
        pauseMenu.SetActive(false); // �Q�[���J�n�O�̓|�[�Y�p�l�����\��

    }

    private void Update()
    {
        if (!IsGameStarted && Input.GetKeyDown(KeyCode.Return)) // Enter �ŃQ�[���J�n
        {
            StartCoroutine(StartGame());
        }
        else if (IsGameStarted && Input.GetKeyDown(KeyCode.Escape)) // Escape �Ń|�[�Y�؂�ւ�
        {
            TogglePause();
        }
    }

    IEnumerator StartGame()
    {
        //yield return FadeIn();
        yield return new WaitForSecondsRealtime(0.5f); // �t�F�[�h��ɑҋ@

        IsGameStarted = true;
        countdownTimer.StartTimer(60f); // �J�E���g�_�E���J�n

        TogglePause(); // �Q�[���J�n�i�|�[�Y�����j
    }

    public void TogglePause(bool panel = true)
    {
        IsPaused = !IsPaused;
        if (panel)
        {
            pauseMenu.SetActive(IsPaused); // �|�[�Y���j���[�̕\���ؑ�
        }

        Time.timeScale = IsPaused ? 0f : 1f; // �|�[�Y���ɃQ�[����~

        // Rigidbody2D �̓������~�߂�E�ĊJ
        foreach (var rb in allRigidbodies)
        {
            if (rb != null)
            {
                rb.isKinematic = IsPaused;
                if (!IsPaused) rb.velocity = Vector2.zero;
            }
        }

        // Animator �̓������~�߂�E�ĊJ
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


    IEnumerator FadeIn()//�����g��Ȃ�
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
