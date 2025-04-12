using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleMenuController : MonoBehaviour
{
    public GameObject[] mainMenuItems;
    public GameObject[] testMenuItems;
    public GameObject[] exitMenuItems;
    public GameObject cursor;
    public GameObject mainMenuContainer;
    public GameObject testMenuContainer;
    public GameObject exitMenuContainer;

    private int selectedIndex = 0;
    private GameObject[] currentMenu;
    private Stack<GameObject[]> menuHistory = new Stack<GameObject[]>();

    public static int difficulty;
    public float yAdjust;

    void Start()
    {
        currentMenu = mainMenuItems;
        //ShowMenu(mainMenuContainer);
        UpdateCursor();
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
        selectedIndex = (selectedIndex + direction + currentMenu.Length) % currentMenu.Length;
        UpdateCursor();
    }

    void UpdateCursor()
    {
        Vector3 newPosition = cursor.transform.position;
        newPosition.y = currentMenu[selectedIndex].transform.position.y+yAdjust;
        cursor.transform.position = newPosition;
    }

    void SelectOption()
    {
        if (currentMenu == mainMenuItems)
        {
            if (selectedIndex == 0) // 試験開始
            {
                ToOriginalmainMenuItemAt(0, 0.3f);
                ToOriginalmainMenuItemAt(1, 0.4f);
                OpenMenu(testMenuItems, testMenuContainer);
                ToTargettestMenuItemAt(0, 0.3f);
                ToTargettestMenuItemAt(1, 0.4f);
                ToTargettestMenuItemAt(2, 0.5f);
                ToTargettestMenuItemAt(3, 0.6f);
            }
            else if (selectedIndex == 1) // 中断
            {
                ToOriginalmainMenuItemAt(0, 0.4f);
                ToOriginalmainMenuItemAt(1, 0.3f);
                OpenMenu(exitMenuItems, exitMenuContainer);
            }
        }
        else if (currentMenu == testMenuItems)
        {
            if (selectedIndex == 3)//もどる
            {
                ToOriginaltestMenuItemAt(0, 0.6f);
                ToOriginaltestMenuItemAt(1, 0.5f);
                ToOriginaltestMenuItemAt(2, 0.4f);
                ToOriginaltestMenuItemAt(3, 0.3f);
                GoBack();
                ToTargetmainMenuItemAt(0, 0.3f);
                ToTargetmainMenuItemAt(1, 0.4f);
            }

            else
            {
                //ゲーム開始難易度選択
                difficulty = selectedIndex; //難易度。読み込むときはTitleMenuController.difficulty
                //A=0,B=1,C=2
                Debug.Log("試験 " + ( difficulty.ToString()) + " 開始");
                DifficultyManager.Instance.StartGame("Main", difficulty);


            }
        }
        else if (currentMenu == exitMenuItems)
        {
            if (selectedIndex == 0) // はい
            {
                Debug.Log("ゲーム終了");
                Application.Quit();
            }
            else if (selectedIndex == 1) // いいえ
            {
                GoBack();
            }
        }
    }


    public void ToOriginaltestMenuItemAt(int index, float delay = 0f)
    {
        if (index >= 0 && index < testMenuItems.Length)
            testMenuItems[index]?.GetComponent<TitleButtonAnime>()?.MoveToOriginal(delay);
    }

    public void ToTargettestMenuItemAt(int index, float delay = 0f)
    {
        if (index >= 0 && index < testMenuItems.Length)
            testMenuItems[index]?.GetComponent<TitleButtonAnime>()?.MoveToTarget(delay);
    }

    public void ToOriginalmainMenuItemAt(int index, float delay = 0f)
    {
        if (index >= 0 && index < mainMenuItems.Length)
            mainMenuItems[index]?.GetComponent<TitleButtonAnime>()?.MoveToOriginal(delay);
    }

    public void ToTargetmainMenuItemAt(int index, float delay = 0f)
    {
        if (index >= 0 && index < mainMenuItems.Length)
            mainMenuItems[index]?.GetComponent<TitleButtonAnime>()?.MoveToTarget(delay);
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
