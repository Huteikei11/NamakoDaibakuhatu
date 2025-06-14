using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance { get; private set; }
    public AudioSource audioSource;

    [Header("Inspector�Ŏw�肷��BGM�N���b�v")]
    public AudioClip bgmClip;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Inspector�Ŏw�肳�ꂽBGM�������Đ��������ꍇ
        if (bgmClip != null)
        {
            PlayBGM(bgmClip);
        }
    }

    public void PlayBGM(AudioClip clip, bool loop = true)
    {
        if (audioSource.clip == clip && audioSource.isPlaying)
            return;

        audioSource.clip = clip;
        audioSource.loop = loop;
        audioSource.Play();
    }

    public void PlayBGMFromInspector(bool loop = true)
    {
        if (bgmClip != null)
        {
            PlayBGM(bgmClip, loop);
        }
    }

    public void StopBGM()
    {
        audioSource.Stop();
    }

    public void DestroyBGMManager()
    {
        StopBGM();
        if (Instance == this)
        {
            Instance = null;
        }
        Destroy(gameObject);
    }

    /*
     *�Ăяo�����@
        // �V�[���J�ڌ��BGMManager���폜
        if (BGMManager.Instance != null)
        {
            BGMManager.Instance.DestroyBGMManager();
        }

     */
}
