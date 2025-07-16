using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool IsGameStarted { get; private set; } = false;
    public bool IsPaused { get; private set; } = true; // �ŏ��̓|�[�Y��ԁi�Q�[���J�n�O�j

    public GameObject pauseMenu;  // �|�[�Y���j���[UI
    public  List<Rigidbody2D> allRigidbodies;
    public List<Animator> allAnimators;

    public int difficulty;
    public int gametime;
    [SerializeField] private CountdownTimer countdownTimer;
    [SerializeField] private ScoreManager scoreManager; // �X�R�A�}�l�[�W���[�̎Q��]
    [SerializeField] private PaiSE paiSE; // ���ʉ��̎Q��

    private bool isWaitingStart = true; // 3�b�ҋ@�����ǂ���

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


        //Time.timeScale = 0f; // �ŏ��̓Q�[����~
        pauseMenu.SetActive(false); // �Q�[���J�n�O�̓|�[�Y�p�l�����\��

        StartCoroutine(WaitAndStartGame());
    }

    private IEnumerator WaitAndStartGame()
    {
        Debug.Log("Waiting for game start...");
        TogglePauseLoad(true); // �i�|�[�Y�j
        isWaitingStart = true;

        yield return new WaitForSecondsRealtime(3f); // 3�b�ԃ��A���^�C���őҋ@
        isWaitingStart = false;
        IsGameStarted = true;

        countdownTimer.StartTimer(gametime); // �J�E���g�_�E���J�n
        scoreManager.ReStartLoop(); // �X�R�A�}�l�[�W���[�̃��[�v���ĊJ
        Debug.Log("Game started!");
        TogglePauseLoad(false); // �Q�[���J�n�i�|�[�Y�����j
    }

    private void Update()
    {
        // 3�b�ҋ@���͉������Ȃ�
        if (isWaitingStart) return;

        // �Q�[���J�n��̃|�[�Y�؂�ւ�
        if (IsGameStarted && Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSecondsRealtime(0.5f); // �t�F�[�h��ɑҋ@

        countdownTimer.StartTimer(gametime); // �J�E���g�_�E���J�n

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
                if (!IsPaused) rb.gravityScale = 0;
            }
        }

        // Animator �̓������~�߂�E�ĊJ
        foreach (var animator in allAnimators)
        {
                animator.enabled = !IsPaused;
        }

        paiSE.SetPlaying(!IsPaused); // ���ʉ��̃|�[�Y�؂�ւ�

        if (IsPaused)
        {
            countdownTimer.StopTimer();
        }
        else
        {
            countdownTimer.ResumeTimer();
        }
    }


    public void TogglePauseLoad(bool hoge = true)//��ʑJ�ڂ̎������g��
    {
        IsPaused = hoge;

        //Time.timeScale = IsPaused ? 0f : 1f; // �|�[�Y���ɃQ�[����~


        // Rigidbody2D �̓������~�߂�E�ĊJ
        foreach (var rb in allRigidbodies)
        {
            if (rb != null)
            {
                rb.isKinematic = IsPaused;
                if (!IsPaused) rb.velocity = Vector2.zero;
                if (!IsPaused) rb.gravityScale = 0;
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


        paiSE.SetPlaying(!IsPaused); // ���ʉ��̃|�[�Y�؂�ւ�

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
