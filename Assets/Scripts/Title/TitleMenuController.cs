using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleMenuController : MonoBehaviour
{
    public GameObject[] mainMenuItems;
    public GameObject[] testMenuItems;
    public GameObject[] exitMenuItems;
    public GameObject[] runks;
    public GameObject cursor;
    public GameObject mainMenuContainer;
    public GameObject testMenuContainer;
    public GameObject exitMenuContainer;
    [SerializeField] private SaveManager saveManager;

    private int selectedIndex = 0;
    private GameObject[] currentMenu;
    private Stack<GameObject[]> menuHistory = new Stack<GameObject[]>();
    public float originalX;
    private float cursorDelay;

    public static int difficulty;
    public float yAdjust;
    private GameObject lastMovedItem = null; // �O�񓮂��������j���[����

    public TitleLogo titleLogo;
    public TitleKeiki titleKeiki;
    public TitleKuroobi titleKuroobi;

    private bool isAllowMove;
    void Start()
    {
        currentMenu = mainMenuItems;
        //ShowMenu(mainMenuContainer);
        lastMovedItem = mainMenuItems[0];
        isAllowMove = true;

    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))&&isAllowMove)
        {
            isAllowMove = false;
            MoveCursor(-1);
            DOVirtual.DelayedCall(0.2f, () =>
            {
                isAllowMove=true;
            });
        }
        else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))&&isAllowMove)
        {
            isAllowMove = false;
            MoveCursor(1);
            DOVirtual.DelayedCall(0.2f, () =>
            {
                isAllowMove = true;
            });
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            SelectOption();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentMenu == testMenuItems)
            {
                cursorDelay = 1f;
                ToOriginaltestMenuItemAt(0, 0.6f);
                ToOriginaltestMenuItemAt(1, 0.5f);
                ToOriginaltestMenuItemAt(2, 0.4f);
                ToOriginaltestMenuItemAt(3, 0.3f);
                ToOriginalRunkAt(0, 0.6f);
                ToOriginalRunkAt(1, 0.5f);
                ToOriginalRunkAt(2, 0.4f);
                GoBack();
                ToTargetmainMenuItemAt(0, 0.3f, 0.5f);
                ToTargetmainMenuItemAt(1, 0.4f);
            }
        }
    }

    void MoveCursor(int direction)
    {
        selectedIndex = (selectedIndex + direction + currentMenu.Length) % currentMenu.Length;
        cursorDelay = 0f;
        UpdateCursor();
    }



    void UpdateCursor()
    {
        Animator cursorAnimator = cursor.GetComponent<Animator>();
        if (cursorAnimator != null)
        {
            cursorAnimator.SetTrigger("End");
        }

        float offScreenX = originalX - 10f;
        cursor.transform.DOMoveX(offScreenX, 0.2f).OnComplete(() =>
        {
            // �ޏꊮ����A�J�[�\���ʒu��Y�X�V
            Vector3 newPosition = cursor.transform.position;
            newPosition.y = currentMenu[selectedIndex].transform.position.y + yAdjust;
            cursor.transform.position = new Vector3(offScreenX, newPosition.y, newPosition.z);

            // --- �O�񓮂��������ڂ����̈ʒu�ɖ߂� ---
            if (lastMovedItem != null)
            {
                lastMovedItem.transform.DOLocalMoveX(-3.94f, 0.1f); // X=0�����̈ʒu�Ɖ���
            }

            // --- ����̍��ڂ������E�փX���C�h ---
            GameObject currentItem = currentMenu[selectedIndex];
            currentItem.transform.DOLocalMoveX(currentItem.transform.position.x+0.5f, 0.1f); // �����E��
            lastMovedItem = currentItem; // ����̂��߂ɕۑ�

            // �J�[�\���o��A�j���[�V�����i0.1�b�҂��Ă���j
            DOVirtual.DelayedCall(cursorDelay, () =>
            {
                cursor.transform.DOMoveX(originalX, 0.2f).OnComplete(() =>
                {
                    if (cursorAnimator != null)
                    {
                        cursorAnimator.SetTrigger("Start");
                    }
                });
            });
        });
    }



    void SelectOption()
    {
        if (currentMenu == mainMenuItems)
        {
            if (selectedIndex == 0) // �����J�n
            {
                ToOriginalmainMenuItemAt(0, 0.3f);
                ToOriginalmainMenuItemAt(1, 0.4f);
                titleLogo.Shift();
                titleKeiki.Shift();
                titleKuroobi.Shift();
                cursorDelay = 1.5f;
                OpenMenu(testMenuItems, testMenuContainer);
                ToTargettestMenuItemAt(0, 0.3f,0.5f);
                ToTargettestMenuItemAt(1, 0.4f);
                ToTargettestMenuItemAt(2, 0.5f);
                ToTargettestMenuItemAt(3, 0.6f);
                ToTargetRunkAt(0, 0.3f);
                ToTargetRunkAt(1, 0.4f);
                ToTargetRunkAt(2, 0.5f);
            }
            else if (selectedIndex == 1) // ���f
            {
                ToOriginalmainMenuItemAt(0, 0.4f);
                ToOriginalmainMenuItemAt(1, 0.3f);
                StartCoroutine(Quit());
            }
        }
        else if (currentMenu == testMenuItems)
        {
            if (selectedIndex == 3)//���ǂ�
            {
                cursorDelay = 1f;
                ToOriginaltestMenuItemAt(0, 0.6f);
                ToOriginaltestMenuItemAt(1, 0.5f);
                ToOriginaltestMenuItemAt(2, 0.4f);
                ToOriginaltestMenuItemAt(3, 0.3f);
                ToOriginalRunkAt(0, 0.6f);
                ToOriginalRunkAt(1, 0.5f);
                ToOriginalRunkAt(2, 0.4f);
                titleLogo.ResetPosition();
                titleKeiki.ResetPosition();
                titleKuroobi.ResetPosition();
                GoBack();
                ToTargetmainMenuItemAt(0, 0.3f,0.5f);
                ToTargetmainMenuItemAt(1, 0.4f);
            }

            else
            {
                // �Q�[���J�n��Փx�I��
                if (selectedIndex > saveManager.progress+1)
                {
                    Debug.Log("�I��������Փx�͂܂��������Ă��܂���B");
                    return;
                }

                difficulty = selectedIndex; //��Փx�B�ǂݍ��ނƂ���TitleMenuController.difficulty
                //A=0,B=1,C=2
                Debug.Log("���� " + ( difficulty.ToString()) + " �J�n");
                for (int i = 0; i <= 3; i++)
                {
                    float delay = (i == difficulty) ? 0.1f : 0.3f;
                    ToOriginaltestMenuItemAt(i, delay);
                    ToOriginalRunkAt(i,delay);
                }

                // Animator��Trigger��Close�A�j���[�V�������Đ�
                Animator cursorAnimator = cursor.GetComponent<Animator>();
                if (cursorAnimator != null)
                {
                    cursorAnimator.SetTrigger("End");
                }

                // DOTween�őޏ�i��ʊO�ֈړ��j
                float offScreenX = originalX - 10f; // ��ʂ̊O��

                cursor.transform.DOMoveX(offScreenX, 0.2f);
                StartCoroutine(ChangeScene(difficulty));
                    
            }
        }
    }
    IEnumerator ChangeScene(int difficult)
    {
        yield return new WaitForSeconds(1f);//1�b�҂�
        DOTween.KillAll(); // �STween���~�E�j��
        DifficultyManager.Instance.StartGame("Main", difficulty);

    }

    IEnumerator Quit()
    {
        Animator cursorAnimator = cursor.GetComponent<Animator>();
        if (cursorAnimator != null)
        {
            cursorAnimator.SetTrigger("End");
        }

        float offScreenX = originalX - 10f;
        cursor.transform.DOMoveX(offScreenX, 0.2f).OnComplete(() =>
        {
            // �ޏꊮ����A�J�[�\���ʒu��Y�X�V
            Vector3 newPosition = cursor.transform.position;
            newPosition.y = currentMenu[selectedIndex].transform.position.y + yAdjust;
            cursor.transform.position = new Vector3(offScreenX, newPosition.y, newPosition.z);
        });

            yield return new WaitForSeconds(1f);//1�b�҂�
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//�Q�[���v���C�I��
#else
    Application.Quit();//�Q�[���v���C�I��
#endif
    }


    public void ToOriginaltestMenuItemAt(int index, float delay = 0f)
    {
        if (index >= 0 && index < testMenuItems.Length)
            testMenuItems[index]?.GetComponent<TitleButtonAnime>()?.MoveToOriginal(delay);
    }

    public void ToTargettestMenuItemAt(int index, float delay = 0f,float adjust=0f)
    {
        if (index >= 0 && index < testMenuItems.Length)
            testMenuItems[index]?.GetComponent<TitleButtonAnime>()?.MoveToTarget(delay,adjust);
    }

    public void ToOriginalmainMenuItemAt(int index, float delay = 0f)
    {
        if (index >= 0 && index < mainMenuItems.Length)
            mainMenuItems[index]?.GetComponent<TitleButtonAnime>()?.MoveToOriginal(delay);
    }

    public void ToTargetmainMenuItemAt(int index, float delay = 0f,float adjust = 0)
    {
        if (index >= 0 && index < mainMenuItems.Length)
            mainMenuItems[index]?.GetComponent<TitleButtonAnime>()?.MoveToTarget(delay,adjust);
    }

    public void ToOriginalexitMenuItemAt(int index, float delay = 0f)
    {
        if (index >= 0 && index < exitMenuItems.Length)
            exitMenuItems[index]?.GetComponent<TitleButtonAnime>()?.MoveToOriginal(delay);
    }
    public void ToTargetexitMenuItemAt(int index, float delay = 0f)
    {
        if (index >= 0 && index < exitMenuItems.Length)
            exitMenuItems[index]?.GetComponent<TitleButtonAnime>()?.MoveToTarget(delay);
    }

    public void ToOriginalRunkAt(int index, float delay = 0f)
    {
        if (index >= 0 && index < runks.Length)
            runks[index]?.GetComponent<TitleButtonAnime>()?.MoveToOriginal(delay);
    }
    public void ToTargetRunkAt(int index, float delay = 0f)
    {
        if (index >= 0 && index < runks.Length)
            runks[index]?.GetComponent<TitleButtonAnime>()?.MoveToTarget(delay);
    }

    void OpenMenu(GameObject[] newMenu, GameObject menuContainer)
    {
        menuHistory.Push(currentMenu);
        currentMenu = newMenu;
        selectedIndex = 0;
        //ShowMenu(menuContainer);
        UpdateCursor();
    }

    void GoBack()
    {
        if (menuHistory.Count > 0)
        {
            currentMenu = menuHistory.Pop();
            selectedIndex = 0;
            //ShowMenu(GetContainerForMenu(currentMenu));
            UpdateCursor();
        }
    }

    void ShowMenu(GameObject menuToShow)
    {
        mainMenuContainer.SetActive(false);
        testMenuContainer.SetActive(false);
        exitMenuContainer.SetActive(false);
        menuToShow.SetActive(true);
    }

    GameObject GetContainerForMenu(GameObject[] menu)
    {
        if (menu == mainMenuItems) return mainMenuContainer;
        if (menu == testMenuItems) return testMenuContainer;
        if (menu == exitMenuItems) return exitMenuContainer;
        return null;
    }
}
