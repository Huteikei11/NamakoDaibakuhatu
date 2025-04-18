using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MayumiManager : MonoBehaviour
{
    [SerializeField] private ObjectController2D suii;
    private Animator anim = null;
    public int mode = 0; // 0:�ʏ�, 1:�ː�, 2:�]�C

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

            // persent �� 0.0�`1.0 �͈̔͂� 8 �i�K�ɕ�����
            int state = Mathf.FloorToInt(persent * 8); // 0�`7 �̐����l���擾

            // �O�̂��ߔ͈͂𐧌�
            state = Mathf.Clamp(state, 0, 7);

            // Animator �̃X�e�[�g��ݒ�
            anim.SetInteger("state", state);
        }
        else if (mode == 1) 
        {
            // Animator �̃X�e�[�g��ݒ�
            anim.SetInteger("state", 8);
        }
        else
        {
            // Animator �̃X�e�[�g��ݒ�
            anim.SetInteger("state", 9);
        }
    }
}
