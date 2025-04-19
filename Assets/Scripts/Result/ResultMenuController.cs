using System.Collections.Generic;
using DG.Tweening; // DOTween���g�p���邽�߂̖��O���
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

    public float cursorMoveDuration = 0.2f; // �J�[�\���ړ��̃A�j���[�V��������
    public float cursorOffsetX = 0.5f; // �J�[�\����X�����̃I�t�Z�b�g

    private GameObject lastMovedItem = null; // �O�񓮂��������j���[����
    private Dictionary<GameObject, float> initialXPositions = new Dictionary<GameObject, float>(); // ����X���W���L�^
    private bool isAnimating = false; // �A�j���[�V���������ǂ����𔻒肷��t���O
    public float cursorX;

    void Awake()
    {
        difficulty = DifficultyManager.Instance != null ? DifficultyManager.Instance.GetDifficulty() : 0;

        // �e�I�����̏���X���W���L�^
        foreach (var item in dynamicMenuItems)
        {
            if (item != null)
            {
                //2�R�ڂ̃I�u�W�F�N�g�̏����ʒu���
                initialXPositions[item] = dynamicMenuItems[2].transform.localPosition.x;
            }
        }
    }

    void Update()
    {
        if (isAnimating) return; // �A�j���[�V�������͓��͂𖳎�

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

        isAnimating = true; // �A�j���[�V�����J�n

        // �J�[�\���̑ޏ�A�j���[�V����
        float offScreenX = cursor.transform.position.x + 10f;
        cursor.transform.DOMoveX(offScreenX, cursorMoveDuration).OnComplete(() =>
        {
            // �J�[�\���ʒu���X�V
            Vector3 newPosition = cursor.transform.position;
            newPosition.y = currentMenu[selectedIndex].transform.position.y+0.12f;
            cursor.transform.position = new Vector3(offScreenX, newPosition.y, newPosition.z);

            // �O�񓮂��������ڂ����̈ʒu�ɖ߂�
            if (lastMovedItem != null && initialXPositions.ContainsKey(lastMovedItem))
            {
                float initialX = initialXPositions[lastMovedItem];
                lastMovedItem.transform.DOLocalMoveX(initialX, 0.1f); // �����ʒu�ɖ߂�
            }

            // ����̍��ڂ������E�փX���C�h
            GameObject currentItem = currentMenu[selectedIndex];
            float currentLocalX = cursorX; // ���݂̃��[�J��X���W���擾
            currentItem.transform.DOLocalMoveX(currentLocalX + cursorOffsetX, 0.1f); // ���݂̈ʒu�ɃI�t�Z�b�g�����Z
            lastMovedItem = currentItem;

            // �J�[�\���̓o��A�j���[�V����
            cursor.transform.DOMoveX(cursor.transform.position.x - 10f, cursorMoveDuration).OnComplete(() =>
            {
                isAnimating = false; // �A�j���[�V�����I��
            });
        });
    }

    void SelectOption()
    {
        if (currentMenu.Count == 0) return;

        if (selectedIndex == 0)
        {
            if (dynamicOptionState) // �N���A���Ă������ƍŏI�X�e�[�W�ł͂Ȃ���
            {
                int nextDifficulty = Mathf.Min(2, difficulty + 1);
                DifficultyManager.Instance.StartGame("Main", nextDifficulty);
            }
            else // ���s
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
        dynamicOptionState = optionState && difficulty != 2; // �����������ŏI�X�e�[�W�ł͂Ȃ�
        dynamicMenuItems[0].SetActive(dynamicOptionState);
        dynamicMenuItems[1].SetActive(!dynamicOptionState);
        dynamicMenuItems[2].SetActive(true); // 2�Ԗڂ̃I�u�W�F�N�g�͏�ɕ\��

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
