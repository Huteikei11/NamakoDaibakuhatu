using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance { get; private set; }

    private int selectedDifficulty = 0; // デフォルトは Easy (0)

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーン遷移しても破棄されない
        }
        else
        {
            Destroy(gameObject); // すでに存在する場合は削除
        }
    }

    // 難易度を設定（0: Easy, 1: Normal, 2: Hard）
    public void SetDifficulty(int difficulty)
    {
        selectedDifficulty = Mathf.Clamp(difficulty, 0, 2);
        Debug.Log("難易度設定: " + selectedDifficulty);
    }

    // 現在の難易度を取得
    public int GetDifficulty()
    {
        return Instance != null ? Instance.selectedDifficulty : 0;
    }


    // 難易度を設定し、ゲームシーンへ移動
    public void StartGame(string sceneName, int difficulty)
    {
        SetDifficulty(difficulty);
        SceneManager.LoadScene(sceneName);
    }
}
