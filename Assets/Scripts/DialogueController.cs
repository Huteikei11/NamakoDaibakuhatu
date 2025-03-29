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

    // �w��̕b���ҋ@���郁�\�b�h
    public IEnumerator WaitForSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    public IEnumerator ShowDialogue()
    {
        Debug.Log("�Z���t1: ����ɂ��́I");
        yield return StartCoroutine(WaitForEnterPress());

        Debug.Log("�Z���t2: �����҂��Ă�������...");
        yield return StartCoroutine(WaitForSeconds(2.0f)); // 2�b�ҋ@

        Debug.Log("�Z���t3: ���悤�Ȃ�I");
        yield return StartCoroutine(WaitForEnterPress());

        Debug.Log("�_�C�A���O�I��");
    }

    public IEnumerator FinishDialog()//�ː����I������Ƃ��̉��o
    {
        
        //�^�C�}�[�ƃX�R�A�v�Z���~�߂�
        countdownTimer.StopTimer();
        scoreManager.StopLoop();
        //���͂��Ƃ߂�
        gaman.isOperable = false;

        //���o�Ƃ�
        Debug.Log("�Z���t1: ����ɂ��́I");
        karioki.SetActive(true);
        yield return StartCoroutine(WaitForEnterPress());
        karioki.SetActive(false);
        //���U���g���
        SceneManager.LoadScene("Result");
    }
}


/* �Ăяo���Ƃ��̃T���v���v���O����
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
        Debug.Log("�C�x���g�J�n");

        yield return StartCoroutine(DialogueController.Instance.ShowDialogue());

        Debug.Log("�C�x���g�I��");
    }
}

 */
