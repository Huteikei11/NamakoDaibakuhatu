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

    void Start()
    {
        currentMenu = mainMenuItems;
        ShowMenu(mainMenuContainer);
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
        newPosition.y = currentMenu[selectedIndex].transform.position.y;
        cursor.transform.position = newPosition;
    }

    void SelectOption()
    {
        if (currentMenu == mainMenuItems)
        {
            if (selectedIndex == 0) // �����J�n
            {
                OpenMenu(testMenuItems, testMenuContainer);
            }
            else if (selectedIndex == 1) // ���f
            {
                OpenMenu(exitMenuItems, exitMenuContainer);
            }
        }
        else if (currentMenu == testMenuItems)
        {
            if (selectedIndex == 3)//�����L�^
            {

            }
            else if (selectedIndex == 4) // ����ϒ��~
            {
                GoBack();
            }

            else
            {
                //�Q�[���J�n��Փx�I��
                difficulty = selectedIndex; //��Փx�B�ǂݍ��ނƂ���TitleMenuController.difficulty
                //A=0,B=1,C=2
                Debug.Log("���� " + ( difficulty.ToString()) + " �J�n");
                DifficultyManager.Instance.StartGame("Main", difficulty);


            }
        }
        else if (currentMenu == exitMenuItems)
        {
            if (selectedIndex == 0) // �͂�
            {
                Debug.Log("�Q�[���I��");
                Application.Quit();
            }
            else if (selectedIndex == 1) // ������
            {
                GoBack();
            }
        }
    }

    void OpenMenu(GameObject[] newMenu, GameObject menuContainer)
    {
        menuHistory.Push(currentMenu);
        currentMenu = newMenu;
        selectedIndex = 0;
        ShowMenu(menuContainer);
        UpdateCursor();
    }

    void GoBack()
    {
        if (menuHistory.Count > 0)
        {
            currentMenu = menuHistory.Pop();
            selectedIndex = 0;
            ShowMenu(GetContainerForMenu(currentMenu));
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
