using System.Collections.Generic;
using DG.Tweening; // DOTweenを使用するための名前空間
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultMenuController : MonoBehaviour
{
    public GameObject[] dynamicMenuItems;
    public GameObject cursor;
    public GameObject dynamicMenuContainer;

    private int selectedIndex = 0;
    private List<GameObject> currentMenu = new List<GameObject>();
    private Stack<List<GameObject>> menuHistory = new Stack<List<GameObject>>();
    private bool dynamicOptionState = false;
    public int difficulty;

    public float cursorMoveDuration = 0.2f; // カーソル移動のアニメーション時間
    public float cursorOffsetX = 0.5f; // カーソルのX方向のオフセット

    private GameObject lastMovedItem = null; // 前回動かしたメニュー項目
    private Dictionary<GameObject, float> initialXPositions = new Dictionary<GameObject, float>(); // 初期X座標を記録
    private bool isAnimating = false; // アニメーション中かどうかを判定するフラグ
    public float cursorX;

    void Awake()
    {
        difficulty = DifficultyManager.Instance != null ? DifficultyManager.Instance.GetDifficulty() : 0;

        // 各選択肢の初期X座標を記録
        foreach (var item in dynamicMenuItems)
        {
            if (item != null)
            {
                //2コ目のオブジェクトの初期位置が基準
                initialXPositions[item] = dynamicMenuItems[2].transform.localPosition.x;
            }
        }
    }

    void Update()
    {
        if (isAnimating) return; // アニメーション中は入力を無視

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveCursor(-1);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveCursor(1);
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            SelectOption();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            GoBack();
        }
    }

    void MoveCursor(int direction)
    {
        if (currentMenu.Count == 0) return;

        selectedIndex = (selectedIndex + direction + currentMenu.Count) % currentMenu.Count;
        UpdateCursor();
    }

    void UpdateCursor()
    {
        if (currentMenu.Count == 0) return;

        isAnimating = true; // アニメーション開始

        // カーソルの退場アニメーション
        float offScreenX = cursor.transform.position.x + 10f;
        cursor.transform.DOMoveX(offScreenX, cursorMoveDuration).OnComplete(() =>
        {
            // カーソル位置を更新
            Vector3 newPosition = cursor.transform.position;
            newPosition.y = currentMenu[selectedIndex].transform.position.y+0.12f;
            cursor.transform.position = new Vector3(offScreenX, newPosition.y, newPosition.z);

            // 前回動かした項目を元の位置に戻す
            if (lastMovedItem != null && initialXPositions.ContainsKey(lastMovedItem))
            {
                float initialX = initialXPositions[lastMovedItem];
                lastMovedItem.transform.DOLocalMoveX(initialX, 0.1f); // 初期位置に戻す
            }

            // 今回の項目を少し右へスライド
            GameObject currentItem = currentMenu[selectedIndex];
            float currentLocalX = cursorX; // 現在のローカルX座標を取得
            currentItem.transform.DOLocalMoveX(currentLocalX + cursorOffsetX, 0.1f); // 現在の位置にオフセットを加算
            lastMovedItem = currentItem;

            // カーソルの登場アニメーション
            cursor.transform.DOMoveX(cursor.transform.position.x - 10f, cursorMoveDuration).OnComplete(() =>
            {
                isAnimating = false; // アニメーション終了
            });
        });
    }

    void SelectOption()
    {
        if (currentMenu.Count == 0) return;

        if (selectedIndex == 0)
        {
            if (dynamicOptionState) // クリアしていた時と最終ステージではない時
            {
                int nextDifficulty = Mathf.Min(2, difficulty + 1);
                DifficultyManager.Instance.StartGame("Main", nextDifficulty);
            }
            else // 失敗
            {
                DifficultyManager.Instance.StartGame("Main", difficulty);
            }
        }
        else if (selectedIndex == 1)
        {
            SceneManager.LoadScene("Title");
        }
    }

    public void ShowDynamicMenu(bool optionState)
    {
        cursor.SetActive(true);
        dynamicOptionState = optionState && difficulty != 2; // 成功したかつ最終ステージではない
        dynamicMenuItems[0].SetActive(dynamicOptionState);
        dynamicMenuItems[1].SetActive(!dynamicOptionState);
        dynamicMenuItems[2].SetActive(true); // 2番目のオブジェクトは常に表示

        currentMenu.Clear();
        if (dynamicOptionState) currentMenu.Add(dynamicMenuItems[0]);
        else currentMenu.Add(dynamicMenuItems[1]);
        currentMenu.Add(dynamicMenuItems[2]);

        if (currentMenu.Count == 0)
        {
            cursor.SetActive(false);
            return;
        }

        selectedIndex = 0;
        ShowMenu(dynamicMenuContainer);
        UpdateCursor();
    }

    void GoBack()
    {
        if (menuHistory.Count > 0)
        {
            currentMenu = menuHistory.Pop();
            selectedIndex = 0;
            ShowMenu(GetContainerForMenu(currentMenu));

            if (currentMenu.Count > 0)
            {
                UpdateCursor();
            }
            else
            {
                cursor.SetActive(false);
            }
        }
    }

    void ShowMenu(GameObject menuToShow)
    {
        dynamicMenuContainer.SetActive(false);
        menuToShow.SetActive(true);
    }

    GameObject GetContainerForMenu(List<GameObject> menu)
    {
        if (menu.Contains(dynamicMenuItems[0]) || menu.Contains(dynamicMenuItems[1])) return dynamicMenuContainer;
        return null;
    }
}
