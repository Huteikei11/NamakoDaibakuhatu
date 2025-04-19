using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private const string ProgressKey = "Progress";
    private const string RankKey = "Rank";
    private const string PlayRecordsKey = "PlayRecords";
    public const int InitialRankValue = -1; // まだクリアしていないときの初期値

    [System.Serializable]
    public class PlayRecord
    {
        public int difficulty;
        public int rank;
        public string resultPhraseNo; // 型をstringに変更
        public float score; // scoreフィールドを追加
        public bool isCleared; // クリア状態を記録する変数を追加
    }

    public int progress;
    public int[] ranks = new int[3] { InitialRankValue, InitialRankValue, InitialRankValue };
    public List<PlayRecord> playRecords = new List<PlayRecord>();

    void Start()
    {
        LoadGameData();
        Debug.Log("SaveManager: Start called. Progress: " + progress);
    }

    void Update()
    {
        // ゲームの進行状況やプレイ記録の更新処理をここに追加
    }

    public void SaveGameData()
    {
        ES3.Save(ProgressKey, progress);
        ES3.Save(RankKey, ranks);
        ES3.Save(PlayRecordsKey, playRecords);
    }

    public void LoadGameData()
    {
        if (ES3.KeyExists(ProgressKey))
        {
            progress = ES3.Load<int>(ProgressKey);
        }

        if (ES3.KeyExists(RankKey))
        {
            ranks = ES3.Load<int[]>(RankKey);
        }

        if (ES3.KeyExists(PlayRecordsKey))
        {
            playRecords = ES3.Load<List<PlayRecord>>(PlayRecordsKey);
        }

        Debug.Log("SaveManager: LoadGameData called. Progress: " + progress + ", Ranks: " + string.Join(", ", ranks));
    }

    public void AddPlayRecord(int difficulty, int rank, string resultPhraseNo, float score, bool isCleared) // 引数にisClearedを追加
    {
        PlayRecord newRecord = new PlayRecord
        {
            difficulty = difficulty,
            rank = rank,
            resultPhraseNo = resultPhraseNo,
            score = score, // scoreを設定
            isCleared = isCleared // クリア状態を設定
        };
        playRecords.Add(newRecord);
        if (isCleared)
        {
            // クリアした場合のみ進行状況を更新
            UpdateProgressAndRanks(difficulty, rank);
        }
        SaveGameData();
    }

    private void UpdateProgressAndRanks(int difficulty, int rank)
    {
        if (difficulty > progress)
        {
            progress = difficulty;
        }

        if (rank > ranks[difficulty])
        {
            ranks[difficulty] = rank;
        }
    }

    public int GetRank(int difficulty)
    {
        if (difficulty >= 0 && difficulty < ranks.Length)
        {
            return ranks[difficulty];
        }
        return InitialRankValue; // 無効な難易度の場合
    }
}
