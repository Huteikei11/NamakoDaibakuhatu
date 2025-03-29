using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DialogueController : MonoBehaviour
{
    public static DialogueController Instance { get; private set; }
    [SerializeField] private GameManager gameManager;
    [SerializeField] private CountdownTimer countdownTimer;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private Gaman gaman;
    public GameObject karioki;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator WaitForEnterPress()
    {
        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null;
        }
    }

    // 指定の秒数待機するメソッド
    public IEnumerator WaitForSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    public IEnumerator ShowDialogue()
    {
        Debug.Log("セリフ1: こんにちは！");
        yield return StartCoroutine(WaitForEnterPress());

        Debug.Log("セリフ2: 少し待ってください...");
        yield return StartCoroutine(WaitForSeconds(2.0f)); // 2秒待機

        Debug.Log("セリフ3: さようなら！");
        yield return StartCoroutine(WaitForEnterPress());

        Debug.Log("ダイアログ終了");
    }

    public IEnumerator FinishDialog()//射精が終わったときの演出
    {
        
        //タイマーとスコア計算を止める
        countdownTimer.StopTimer();
        scoreManager.StopLoop();
        //入力をとめる
        gaman.isOperable = false;

        //演出とか
        Debug.Log("セリフ1: こんにちは！");
        karioki.SetActive(true);
        yield return StartCoroutine(WaitForEnterPress());
        karioki.SetActive(false);
        //リザルト画面
        SceneManager.LoadScene("Result");
    }
}


/* 呼び出すときのサンプルプログラム
using System.Collections;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(StartEvent());
    }

    private IEnumerator StartEvent()
    {
        Debug.Log("イベント開始");

        yield return StartCoroutine(DialogueController.Instance.ShowDialogue());

        Debug.Log("イベント終了");
    }
}

 */
