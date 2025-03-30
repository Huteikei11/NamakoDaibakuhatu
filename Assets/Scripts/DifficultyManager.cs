using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance { get; private set; }

    private int selectedDifficulty = 0; // デフォルトの難易度 (0: Easy)
    private bool isGameCleared = false; // ゲームクリアフラグ（デフォルトは未クリア）

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーン遷移時に削除されない
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

    // 難易度を取得（Instanceがnullの場合はデフォルト値0を返す）
    public int GetDifficulty()
    {
        return Instance != null ? Instance.selectedDifficulty : 0;
    }

    // ゲームをクリア状態にする
    public void SetGameCleared(bool cleared)
    {
        isGameCleared = cleared;
        Debug.Log("ゲームクリア状態: " + isGameCleared);
    }

    // ゲームがクリアされたかを取得
    public bool IsGameCleared()
    {
        return Instance != null ? Instance.isGameCleared : false;
    }

    // 難易度を設定し、ゲームシーンへ移動
    public void StartGame(string sceneName, int difficulty)
    {
        SetDifficulty(difficulty);
        isGameCleared = false; // ゲーム開始時は未クリアにリセット
        SceneManager.LoadScene(sceneName);
    }
}
