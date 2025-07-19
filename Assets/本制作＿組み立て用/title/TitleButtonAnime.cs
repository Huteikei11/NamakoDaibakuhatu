using UnityEngine;
using DG.Tweening;

public class TitleButtonAnime : MonoBehaviour
{
    [Header("移動設定")]
    public Vector3 originalPosition = Vector3.zero; // 移動元（Inspectorから指定）
    public Vector3 targetPosition = new Vector3(2f, 0f, 0f); // 移動先
    public float moveDelay = 1f;
    public float moveDuration = 1f;
    public Ease moveEase = Ease.InOutQuad;

    [Header("右にずれる動作設定")]
    public float shiftAmount = 0.5f;
    public float shiftDuration = 0.5f;
    public Color darkenColor = Color.gray;
    public float colorChangeDuration = 0.5f;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    public bool isfitst;
    public bool top;

    private float adjust;
   
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;


        // 初期位置にセット
        transform.position = originalPosition;
        // 指定時間待ってから移動（delay = 1秒）
        adjust = 0f;
        if (top)
        {
            adjust = 0.5f;
        }
        if (isfitst)
        {

            // MoveToTarget(moveDelay,adjust);
        }
        // adjust = 0f;
    }


    public void FirstMovetoTarget()
    {
        MoveToTarget(moveDelay, adjust);
    }
    // 指定した座標（targetPosition）へ delay秒待ってから移動
    public void MoveToTarget(float delay, float adjust =0f)
    {
        targetPosition.x += adjust;
        transform.DOMove(targetPosition, moveDuration)
            .SetEase(moveEase)
            .SetDelay(delay);
        targetPosition.x -= adjust;
    }

    // 初期位置（originalPosition）へ delay秒待ってから戻る
    public void MoveToOriginal(float delay)
    {
        transform.DOMove(originalPosition, moveDuration)
            .SetEase(moveEase)
            .SetDelay(delay);
    }

    // 右にずれて暗くする（Shift）
    public void Shift()
    {
        Vector3 shiftedPosition = transform.position + new Vector3(shiftAmount, 0f, 0f);
        transform.DOMove(shiftedPosition, shiftDuration).SetEase(Ease.OutCubic);
    }

    // 元の位置に戻す（Reset）
    public void ResetPosition()
    {
        transform.DOMove(originalPosition, shiftDuration).SetEase(Ease.OutCubic);
    }
}
