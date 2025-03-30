using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TinpoManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        CheckStageToRender(gameManager.difficulty);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void CheckStageToRender(int difficulty)
    {
        if(difficulty >= 3)
        {
            //特定ステージ以上なら消す
            spriteRenderer.enabled = false;
        }
        else
        {
            //特定ステージ以下なら表示
            spriteRenderer.enabled = true;
        }
    }
}
