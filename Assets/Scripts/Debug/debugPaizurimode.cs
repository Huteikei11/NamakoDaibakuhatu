using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class debugPaizurimode : MonoBehaviour
{
    //Dropdownを格納する変数
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private OppaiManager oppai;

    // Update is called once per frame
    void Update()
    {
        //DropdownのValueが0のとき（赤が選択されているとき）
        if (dropdown.value == 0)
        {
            oppai.paiMode = 0;
        }
        //DropdownのValueが1のとき（青が選択されているとき）
        else if (dropdown.value == 1)
        {
            oppai.paiMode = 1;
        }
        //DropdownのValueが2のとき（黄が選択されているとき）
        else if (dropdown.value == 2)
        {
            oppai.paiMode = 2;
        }
    }
}