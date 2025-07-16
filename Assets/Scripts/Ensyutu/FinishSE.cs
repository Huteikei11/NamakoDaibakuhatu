using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishSE : MonoBehaviour
{
    // クリア時効果音リスト
    [SerializeField]
    private List<AudioClip> clearSEList;

    // 失敗時効果音リスト
    [SerializeField]
    private List<AudioClip> failedSEList;

    // 効果音再生用AudioSource
    [SerializeField]
    private AudioSource audioSource;

    // クリアSE再生間隔
    [SerializeField]
    private float clearSEInterval = 2.0f;
    // 失敗SE再生間隔
    [SerializeField]
    private float failedSEInterval = 2.0f;

    private Coroutine clearSECoroutine;
    private Coroutine failedSECoroutine;

    // Start is called before the first frame update
    void Start()
    {
        // 必要に応じて初期化
    }

    // Update is called once per frame
    void Update()
    {
        // ...existing code...
    }

    // クリア時リストからランダムに効果音を再生（単発）
    public void PlayRandomClearSE()
    {
        if (clearSEList != null && clearSEList.Count > 0 && audioSource != null)
        {
            int index = Random.Range(0, clearSEList.Count);
            audioSource.PlayOneShot(clearSEList[index]);
        }
    }

    // 失敗時リストからランダムに効果音を再生（単発）
    public void PlayRandomFailedSE()
    {
        if (failedSEList != null && failedSEList.Count > 0 && audioSource != null)
        {
            int index = Random.Range(0, failedSEList.Count);
            audioSource.PlayOneShot(failedSEList[index]);
        }
    }

    // クリアSEの連続再生開始
    public void StartClearSE()
    {
        if (clearSECoroutine == null)
        {
            clearSECoroutine = StartCoroutine(PlayClearSERoutine());
        }
    }

    // クリアSEの連続再生停止
    public void StopClearSE()
    {
        if (clearSECoroutine != null)
        {
            StopCoroutine(clearSECoroutine);
            clearSECoroutine = null;
        }
    }

    // 失敗SEの連続再生開始
    public void StartFailedSE()
    {
        if (failedSECoroutine == null)
        {
            failedSECoroutine = StartCoroutine(PlayFailedSERoutine());
        }
    }

    // 失敗SEの連続再生停止
    public void StopFailedSE()
    {
        if (failedSECoroutine != null)
        {
            StopCoroutine(failedSECoroutine);
            failedSECoroutine = null;
        }
    }

    // クリアSE再生コルーチン
    private IEnumerator PlayClearSERoutine()
    {
        while (true)
        {
            PlayRandomClearSE();
            yield return new WaitForSeconds(clearSEInterval);
        }
    }

    // 失敗SE再生コルーチン
    private IEnumerator PlayFailedSERoutine()
    {
        while (true)
        {
            PlayRandomFailedSE();
            yield return new WaitForSeconds(failedSEInterval);
        }
    }
}
