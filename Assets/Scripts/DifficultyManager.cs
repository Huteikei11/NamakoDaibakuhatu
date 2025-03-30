using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance { get; private set; }

    private int selectedDifficulty = 0; // �f�t�H���g�̓�Փx (0: Easy)
    private bool isGameCleared = false; // �Q�[���N���A�t���O�i�f�t�H���g�͖��N���A�j

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �V�[���J�ڎ��ɍ폜����Ȃ�
        }
        else
        {
            Destroy(gameObject); // ���łɑ��݂���ꍇ�͍폜
        }
    }

    // ��Փx��ݒ�i0: Easy, 1: Normal, 2: Hard�j
    public void SetDifficulty(int difficulty)
    {
        selectedDifficulty = Mathf.Clamp(difficulty, 0, 2);
        Debug.Log("��Փx�ݒ�: " + selectedDifficulty);
    }

    // ��Փx���擾�iInstance��null�̏ꍇ�̓f�t�H���g�l0��Ԃ��j
    public int GetDifficulty()
    {
        return Instance != null ? Instance.selectedDifficulty : 0;
    }

    // �Q�[�����N���A��Ԃɂ���
    public void SetGameCleared(bool cleared)
    {
        isGameCleared = cleared;
        Debug.Log("�Q�[���N���A���: " + isGameCleared);
    }

    // �Q�[�����N���A���ꂽ�����擾
    public bool IsGameCleared()
    {
        return Instance != null ? Instance.isGameCleared : false;
    }

    // ��Փx��ݒ肵�A�Q�[���V�[���ֈړ�
    public void StartGame(string sceneName, int difficulty)
    {
        SetDifficulty(difficulty);
        isGameCleared = false; // �Q�[���J�n���͖��N���A�Ƀ��Z�b�g
        SceneManager.LoadScene(sceneName);
    }
}
