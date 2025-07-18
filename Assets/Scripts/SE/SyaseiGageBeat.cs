using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyaseiGageBeat : MonoBehaviour
{
    public ObjectController2D objectController2D;
    public AudioSource audioSource;
    public AudioClip audioClip;

    public float audioSpeed = 1.0f;

    private bool isPaused = false; // 外部からの一時停止状態

    // 外部から鼓動を一時停止
    public void PauseBeat()
    {
        isPaused = true;
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
        }
    }

    // 外部から鼓動を再開
    public void ResumeBeat()
    {
        isPaused = false;
        if (audioSource.clip == audioClip && !audioSource.isPlaying)
        {
            audioSource.UnPause();
        }
    }

    void Update()
    {
        if (isPaused)
        {
            return;
        }

        float persent = objectController2D.GetYPositionRatio();
        if (persent > 0.8f)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = audioClip;
                audioSource.loop = true;
                audioSource.Play();
            }
            float pitch = Mathf.Clamp(0.5f + (persent - 0.8f) * 6.5f, 0.5f, 1.5f);
            audioSource.pitch = pitch;
            audioSource.volume = Mathf.Clamp(0.1f + (persent - 0.8f) * 5f, 0.1f, 1f);
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
}
