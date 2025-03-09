using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class debugPaizurimode : MonoBehaviour
{
    //Dropdown���i�[����ϐ�
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private OppaiManager oppai;

    // Update is called once per frame
    void Update()
    {
        //Dropdown��Value��0�̂Ƃ��i�Ԃ��I������Ă���Ƃ��j
        if (dropdown.value == 0)
        {
            oppai.paiMode = 0;
        }
        //Dropdown��Value��1�̂Ƃ��i���I������Ă���Ƃ��j
        else if (dropdown.value == 1)
        {
            oppai.paiMode = 1;
        }
        //Dropdown��Value��2�̂Ƃ��i�����I������Ă���Ƃ��j
        else if (dropdown.value == 2)
        {
            oppai.paiMode = 2;
        }
    }
}