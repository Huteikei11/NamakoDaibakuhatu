using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MayumiManager : MonoBehaviour
{
    [SerializeField] private ScoreManager scoreManager;
    private Animator anim = null;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //まゆみちゃんの顔の制御
        //if文じゃなくてもいけるけど他の処理も入るかもなので
        if(scoreManager.gamanArea == 0)
        {
            anim.SetInteger("state", 0);
        }
        else if(scoreManager.gamanArea == 1)
        {
            anim.SetInteger("state", 1);
        }
        else if (scoreManager.gamanArea == 2)
        {
            anim.SetInteger("state", 2);
        }
        else if (scoreManager.gamanArea == 3)
        {
            anim.SetInteger("state", 3);
        }
        else if (scoreManager.gamanArea == 4)
        {
            anim.SetInteger("state", 4);
        }
    }
}
