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
    [SerializeField] private int[] shotnums;
    public GameObject karioki;
    [Header("複製したいプレハブ")]
    public GameObject prefab;
    [Header("生成間隔（秒）")]
    public float spawnInterval = 0.5f;

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

    public void debugFinish(bool success)//ボタンで呼び出す用（デバッグ)
    {
        StartCoroutine(FinishDialog(success));
    }
    public IEnumerator FinishDialog(bool success)//射精が終わったときの演出
        //とりあえず失敗時と成功時にどっちもこれがよびだされるようにしてある
    {
        
        //タイマーとスコア計算を止める
        countdownTimer.StopTimer();
        scoreManager.StopLoop();
        //入力をとめる
        gaman.isOperable = false;

        //演出とか
        Debug.Log("セリフ1: こんにちは！");
        //karioki.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        //yield return StartCoroutine(WaitForEnterPress());

        if (success)//成功時
        {
            int area = scoreManager.gamanArea;
            int num = shotnums[area];
            StartCoroutine(SpawnObjects(num,"Success"));

        }
        else//失敗時
        {
            int area = scoreManager.gamanArea;
            int num = shotnums[area];
            StartCoroutine(SpawnObjects(num, "Failed"));
        }

        //karioki.SetActive(false);
        //リザルト画面
        //SceneManager.LoadScene("Result");
    }



    IEnumerator SpawnObjects(int spawnCount, string triggerName)
    {
        for (int i = 0; i < spawnCount; i++)
        {
            GameObject instance = Instantiate(prefab, transform.position, Quaternion.identity);

            // y座標だけ -3.9f に設定（x, z は元の位置を維持）
            Vector3 pos = instance.transform.position;
            pos.y = -3.9f;
            instance.transform.position = pos;

            Animator animator = instance.GetComponent<Animator>();

            if (animator != null)
            {
                int randomValue = Random.Range(0, 4); // 0〜3のランダムな整数
                animator.SetInteger("State", randomValue);
                animator.SetTrigger(triggerName);
            }
            else
            {
                Debug.LogWarning("Animatorが見つかりませんでした: " + instance.name);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
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
