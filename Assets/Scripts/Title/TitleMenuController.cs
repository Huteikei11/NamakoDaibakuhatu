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
    private GameObject lastMovedItem = null; // 前回動かしたメニュー項目

    public TitleLogo titleLogo;
    public TitleKeiki titleKeiki;
    public TitleKuroobi titleKuroobi;

    private bool isAllowMove;
    public bool isAnimating = false; // アニメーション中かどうかを判定するフラグ

    void Start()
    {
        currentMenu = mainMenuItems;
        lastMovedItem = mainMenuItems[0];
        isAllowMove = true;

        // ゲーム開始時にアニメーション中であることを設定
        isAnimating = true;

        // 必要に応じて初期アニメーションを実行
        PlayInitialAnimation();
    }

    void PlayInitialAnimation()
    {
        // 初期アニメーション終了後に isAnimating を false にリセット
        DOVirtual.DelayedCall(2.5f, () =>
        {
            isAnimating = false;
        });
    }
    void Update()
    {
        // アニメーション中は操作を無効化
        if (isAnimating) return;

        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && isAllowMove)
        {
            isAllowMove = false;
            MoveCursor(-1);
            DOVirtual.DelayedCall(0.2f, () =>
            {
                isAllowMove = true;
            });
        }
        else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && isAllowMove)
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
            //cursorAnimator.SetTrigger("End");
        }

        float offScreenX = originalX - 10f;
        isAnimating = false;

        // アニメーション開始
        isAnimating = true;

        cursor.transform.DOMoveX(offScreenX, 0f).OnComplete(() =>
        {
            Vector3 newPosition = cursor.transform.position;
            newPosition.y = currentMenu[selectedIndex].transform.position.y + yAdjust;
            cursor.transform.position = new Vector3(offScreenX, newPosition.y, newPosition.z);

            if (lastMovedItem != null)
            {
                lastMovedItem.transform.DOLocalMoveX(-3.94f, 0.1f);
            }

            GameObject currentItem = currentMenu[selectedIndex];
            currentItem.transform.DOLocalMoveX(currentItem.transform.position.x + 0.5f, 0.1f);
            lastMovedItem = currentItem;

            DOVirtual.DelayedCall(cursorDelay, () =>
            {
                cursor.transform.DOMoveX(originalX, 0f).OnComplete(() =>
                {
                    if (cursorAnimator != null)
                    {
                        cursorAnimator.SetTrigger("Start");
                    }

                    // アニメーション終了
                    isAnimating = false;
                });
            });
        });
    }

    void SelectOption()
    {
        if (currentMenu == mainMenuItems)
        {
            if (selectedIndex == 0) // 試験開始
            {
                isAnimating = true; // アニメーション開始
                ToOriginalmainMenuItemAt(0, 0.3f);
                ToOriginalmainMenuItemAt(1, 0.4f);
                titleLogo.Shift();
                titleKeiki.Shift();
                titleKuroobi.Shift();
                cursorDelay = 1.5f;
                OpenMenu(testMenuItems, testMenuContainer);
                ToTargettestMenuItemAt(0, 0.3f, 0.5f);
                ToTargettestMenuItemAt(1, 0.4f);
                ToTargettestMenuItemAt(2, 0.5f);
                ToTargettestMenuItemAt(3, 0.6f);
                ToTargetRunkAt(0, 0.3f);
                ToTargetRunkAt(1, 0.4f);
                ToTargetRunkAt(2, 0.5f);

                // アニメーション終了を遅延で設定
                DOVirtual.DelayedCall(1.5f, () =>
                {
                    isAnimating = false;
                });
            }
            else if (selectedIndex == 1) // 中断
            {
                isAnimating = true; // アニメーション開始
                ToOriginalmainMenuItemAt(0, 0.4f);
                ToOriginalmainMenuItemAt(1, 0.3f);
                StartCoroutine(Quit());
            }
        }
        else if (currentMenu == testMenuItems)
        {
            if (selectedIndex == 3) // もどる
            {
                isAnimating = true; // アニメーション開始
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
                ToTargetmainMenuItemAt(0, 0.3f, 0.5f);
                ToTargetmainMenuItemAt(1, 0.4f);

                // アニメーション終了を遅延で設定
                DOVirtual.DelayedCall(1f, () =>
                {
                    isAnimating = false;
                });
            }
            else
            {
                // ゲーム開始難易度選択
                if (selectedIndex > saveManager.progress + 1)
                {
                    Debug.Log("選択した難易度はまだ解放されていません。");
                    return;
                }

                difficulty = selectedIndex; //難易度。読み込むときはTitleMenuController.difficulty
                //A=0,B=1,C=2
                Debug.Log("試験 " + (difficulty.ToString()) + " 開始");
                for (int i = 0; i <= 3; i++)
                {
                    float delay = (i == difficulty) ? 0.1f : 0.3f;
                    ToOriginaltestMenuItemAt(i, delay);
                    ToOriginalRunkAt(i, delay);
                }

                // AnimatorのTriggerでCloseアニメーションを再生
                Animator cursorAnimator = cursor.GetComponent<Animator>();
                if (cursorAnimator != null)
                {
                    cursorAnimator.SetTrigger("End");
                }

                // DOTweenで退場（画面外へ移動）
                float offScreenX = originalX - 10f; // 画面の外へ

                cursor.transform.DOMoveX(offScreenX, 0.2f);
                StartCoroutine(ChangeScene(difficulty));

            }
        }
    }

    IEnumerator ChangeScene(int difficult)
    {
        yield return new WaitForSeconds(1f); // 1秒待つ
        DOTween.KillAll(); // 全Tweenを停止・破棄
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
            Vector3 newPosition = cursor.transform.position;
            newPosition.y = currentMenu[selectedIndex].transform.position.y + yAdjust;
            cursor.transform.position = new Vector3(offScreenX, newPosition.y, newPosition.z);
        });

        yield return new WaitForSeconds(1f); // 1秒待つ
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // ゲームプレイ終了
#else
        Application.Quit(); // ゲームプレイ終了
#endif
    }

    public void ToOriginaltestMenuItemAt(int index, float delay = 0f)
    {
        if (index >= 0 && index < testMenuItems.Length)
            testMenuItems[index]?.GetComponent<TitleButtonAnime>()?.MoveToOriginal(delay);
    }

    public void ToTargettestMenuItemAt(int index, float delay = 0f, float adjust = 0f)
    {
        if (index >= 0 && index < testMenuItems.Length)
            testMenuItems[index]?.GetComponent<TitleButtonAnime>()?.MoveToTarget(delay, adjust);
    }

    public void ToOriginalmainMenuItemAt(int index, float delay = 0f)
    {
        if (index >= 0 && index < mainMenuItems.Length)
            mainMenuItems[index]?.GetComponent<TitleButtonAnime>()?.MoveToOriginal(delay);
    }

    public void ToTargetmainMenuItemAt(int index, float delay = 0f, float adjust = 0)
    {
        if (index >= 0 && index < mainMenuItems.Length)
            mainMenuItems[index]?.GetComponent<TitleButtonAnime>()?.MoveToTarget(delay, adjust);
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
        UpdateCursor();
    }

    void GoBack()
    {
        if (menuHistory.Count > 0)
        {
            currentMenu = menuHistory.Pop();
            selectedIndex = 0;
            UpdateCursor();
        }
    }
}
