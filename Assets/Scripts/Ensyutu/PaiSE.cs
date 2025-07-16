using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaiSE : MonoBehaviour
{
    [Header("効果音リスト")]
    public List<AudioClip> seList;
    [Header("AudioSource (自動追加可)")]
    public AudioSource audioSource;
    [Header("次の再生までの最小秒数")]
    public float minInterval = 1.0f;
    [Header("次の再生までの最大秒数")]
    public float maxInterval = 3.0f;

    private bool isPlaying = true;
    private Coroutine playCoroutine;

    void Start()
    {

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.volume = 0.25f; // デフォルトの音量設定
    }

    public void StartPlaying()
    {
        if (playCoroutine == null)
        {
            isPlaying = true;
            playCoroutine = StartCoroutine(PlaySERandomLoop());
        }
    }

    public void StopPlaying()
    {
        isPlaying = false;
        if (playCoroutine != null)
        {
            StopCoroutine(playCoroutine);
            playCoroutine = null;
        }
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }

    private IEnumerator PlaySERandomLoop()
    {
        while (isPlaying)
        {
            if (seList != null && seList.Count > 0)
            {
                int idx = Random.Range(0, seList.Count);
                audioSource.clip = seList[idx];
                audioSource.Play();
            }
            float waitTime = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(waitTime);
        }
        playCoroutine = null;
    }

    public void SetPlaying(bool playing)
    {
        if (playing)
        {
            StartPlaying();
        }
        else
        {
            StopPlaying();
        }
    }   
}
