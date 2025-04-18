using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MayumiManager : MonoBehaviour
{
    [SerializeField] private ObjectController2D suii;
    private Animator anim = null;
    public int mode = 0; // 0:通常, 1:射精, 2:余韻

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (mode == 0)
        {
            float persent = suii.GetYPositionRatio();

            // persent を 0.0～1.0 の範囲で 8 段階に分ける
            int state = Mathf.FloorToInt(persent * 8); // 0～7 の整数値を取得

            // 念のため範囲を制限
            state = Mathf.Clamp(state, 0, 7);

            // Animator のステートを設定
            anim.SetInteger("state", state);
        }
        else if (mode == 1) 
        {
            // Animator のステートを設定
            anim.SetInteger("state", 8);
        }
        else
        {
            // Animator のステートを設定
            anim.SetInteger("state", 9);
        }
    }
}
