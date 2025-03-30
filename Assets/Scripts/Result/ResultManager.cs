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

    public IEnumerator WaitForEnterPress()//�G���^�[�������܂őҋ@
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
        yield return StartCoroutine(WaitForSeconds(2f)); // 2�b�ҋ@

        hukidasi.SetActive(true);//�����o���o��
        yield return StartCoroutine(WaitForEnterPress());

        //���j���[�\��
        menucontroller.ShowDynamicMenu(isCleared);
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
