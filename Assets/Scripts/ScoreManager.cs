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

    public float[] gamanratioList = new float[5];//倍率の表
    
    public float[] gamanAreas = new float[5];//倍率の表
    public TextMeshProUGUI scoretext;
    public TextMeshProUGUI gamanBonustext;
    public TextMeshProUGUI wBonustext;
    public TextMeshProUGUI goukeitext;
    public TypewriterByWord typewriterAddscore;
    private float addscore;
    


    [SerializeField] private GameObject suii;
    private ObjectController2D suiiscript;
    [SerializeField] private GameObject Oppai;
    private OppaiManager oppai;
    // Start is called before the first frame update
    void Start()
    {
        suiiscript = suii.GetComponent<ObjectController2D>();
        oppai = Oppai.GetComponent<OppaiManager>();
        InvokeRepeating("ScoreDisplay", 0.0f, roopInterval);
    }

    void Update()
    {
        //ボーナスの計算
        wRatio = wRatioCalulate(oppai.wValue); //1.02 -> 2 のように大きさを変える
        gamanArea = GamanAreaCalcurate(suii.transform.position.y, gamanAreas);//ガマンのどのエリアなのか計算
        gamanRatio = GamanRatio(gamanArea, gamanratioList);
        //ボーナスの表示
        gamanBonustext.text = "×"+gamanRatio.ToString();
        wBonustext.text = "×"+wRatio.ToString();
        goukeitext.text = "×"+(gamanRatio+wRatio).ToString();
    }

    // Update is called once per frame

    void ScoreDisplay()//ループ処理。任意の間隔にしたいのでこの形に
    {
        //スコアの表示
        addscore = scoreAdjust*ScoreAdd(wRatio * wRationAdjust, gamanRatio); //快楽享受*エリアごとの倍率 を現在のスコアに足す
        score += addscore;
        score = (int)score;
        scoretext.text = score.ToString();
        typewriterAddscore.ShowText("+" + addscore.ToString());


        //ランクの表示処理
    }

    void StopLoop()//ループを止める
    {
        // "LoopAction"のInvokeRepeatingを停止
        CancelInvoke("ScoreDisplay");
    }

    private float ScoreAdd(float wration,float gaman)
    {
        return wration + gaman;
    }

    private int GamanAreaCalcurate(float y ,float[] area)
    {
        if (y > area[0])//一番上
        {
            return 0;
        }
        else if (y > area[1])
        {
            return 1;
        }
        else if (y > area[2])
        {
            return 2;
        }
        else if (y > area[3])
        {
            return 3;
        }
        else if (y > area[4])
        {
            return 4;
        }
        return 0;
    }
    private float GamanRatio(int gamanratio, float[] list2)
    {
        if (gamanratio == 0)//一番上
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
    private int wRatioCalulate(float w)
    {
        return (int)((w-1f) * 100+0.1f);
    }
}
