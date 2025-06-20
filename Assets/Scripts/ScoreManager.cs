using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using Febucci.UI;
public class ScoreManager : MonoBehaviour
{

    public float score;
    public float scoreAdjust;
    public float gamanRatio;
    public int gamanArea;
    public float wRatio;
    public float wRationAdjust;
    public float roopInterval;

    public float[] gamanratioList = new float[5];//�{���̕\
    
    public float[] gamanAreas = new float[5];//�{���̕\
    public TextMeshProUGUI scoretext;
    public TextMeshProUGUI gamanBonustext;
    public TextMeshProUGUI wBonustext;
    public TextMeshProUGUI goukeitext;
    public TypewriterByWord typewriterAddscore;
    private float addscore;
    private bool isLoop = true;
    
    


    [SerializeField] private GameObject suii;
    private ObjectController2D suiiscript;
    [SerializeField] private GameObject Oppai;
    private OppaiManager oppai;
    // Start is called before the first frame update
    void Start()
    {
        suiiscript = suii.GetComponent<ObjectController2D>();
        oppai = Oppai.GetComponent<OppaiManager>();
        // InvokeRepeating("ScoreDisplay", 0.0f, roopInterval);
        // GameManager����J�n���w��
    }

    void Update()
    {
        if (isLoop)
        {
            //�{�[�i�X�̌v�Z
            wRatio = wRatioCalulate(oppai.wValue); //1.02 -> 2 �̂悤�ɑ傫����ς���
            gamanArea = GamanAreaCalcurate(suii.transform.position.y, gamanAreas);//�K�}���̂ǂ̃G���A�Ȃ̂��v�Z
            gamanRatio = GamanRatio(gamanArea, gamanratioList);
            //�{�[�i�X�̕\��
            gamanBonustext.text = "�~" + gamanRatio.ToString();
            wBonustext.text = "�~" + wRatio.ToString();
            goukeitext.text = "�~" + (gamanRatio + wRatio).ToString();
        }

    }

    void ScoreDisplay()//���[�v�����B�C�ӂ̊Ԋu�ɂ������̂ł��̌`��
    {
        //�X�R�A�̕\��
        addscore = scoreAdjust*ScoreAdd(wRatio * wRationAdjust, gamanRatio); //���y����*�G���A���Ƃ̔{�� �����݂̃X�R�A�ɑ���
        score += addscore;
        score = (int)score;
        scoretext.text = score.ToString();
        typewriterAddscore.ShowText("+" + addscore.ToString());


        //�����N�̕\������
    }

    public void StopLoop()//���[�v���~�߂�
    {
        // "LoopAction"��InvokeRepeating���~
        CancelInvoke("ScoreDisplay");
        isLoop = false;
    }

    public void ReStartLoop()
    {
        InvokeRepeating("ScoreDisplay", 0.0f, roopInterval);
        isLoop = true;
    }

    private float ScoreAdd(float wration,float gaman)
    {
        return wration + gaman;
    }

    private int GamanAreaCalcurate(float y ,float[] area)
    {
        if (y > area[0])//��ԏ�
        {
            return 4;
        }
        else if (y > area[1])
        {
            return 3;
        }
        else if (y > area[2])
        {
            return 2;
        }
        else if (y > area[3])
        {
            return 1;
        }
        else if (y > area[4])
        {
            return 0;
        }
        return 0;
    }
    private float GamanRatio(int gamanratio, float[] list2)
    {
        if (gamanratio == 0)//��ԏ�
        {
            return list2[0];
        }
        else if (gamanratio == 1)
        {
            return list2[1];
        }
        else if (gamanratio == 2)
        {
            return list2[2];
        }
        else if (gamanratio == 3)
        {
            return list2[3];
        }
        else if (gamanratio == 4)
        {
            return list2[4];
        }
        return 0;
    }
    private int wRatioCalulate(float w )//�ʏ펞�ƍ��̃p�C�Y���p���[�̔������������
    {
        return (int)(w*10)+1;
    }
}
