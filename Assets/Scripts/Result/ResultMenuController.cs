using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    void Start()
    {
        difficulty = DifficultyManager.Instance != null ? DifficultyManager.Instance.GetDifficulty() : 0;
    }

    void Update()
    {
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

        Vector3 newPosition = cursor.transform.position;
        newPosition.y = currentMenu[selectedIndex].transform.position.y;
        cursor.transform.position = newPosition;
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
        dynamicOptionState = optionState && difficulty != 2;//成功したかつ最終ステージではない
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
