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

    // クリア時リストからランダムに効果音を再生
    public void PlayRandomClearSE()
    {
        if (clearSEList != null && clearSEList.Count > 0 && audioSource != null)
        {
            int index = Random.Range(0, clearSEList.Count);
            audioSource.PlayOneShot(clearSEList[index]);
        }
    }

    // 失敗時リストからランダムに効果音を再生
    public void PlayRandomFailedSE()
    {
        if (failedSEList != null && failedSEList.Count > 0 && audioSource != null)
        {
            int index = Random.Range(0, failedSEList.Count);
            audioSource.PlayOneShot(failedSEList[index]);
        }
    }
}
