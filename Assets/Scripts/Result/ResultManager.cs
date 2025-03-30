using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ResultManager : MonoBehaviour
{
    public static ResultManager Instance { get; private set; }
    private bool isCleared;
    [SerializeField] private ResultMenuController menucontroller;
    [SerializeField] private GameObject hukidasi;


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

    private void Start()
    {
        isCleared = DifficultyManager.Instance != null && DifficultyManager.Instance.IsGameCleared();
        StartCoroutine(ShowDialogue());
    }

    public IEnumerator WaitForEnterPress()//エンターを押すまで待機
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
        yield return StartCoroutine(WaitForSeconds(2f)); // 2秒待機

        hukidasi.SetActive(true);//吹き出し出力
        yield return StartCoroutine(WaitForEnterPress());

        //メニュー表示
        menucontroller.ShowDynamicMenu(isCleared);
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
