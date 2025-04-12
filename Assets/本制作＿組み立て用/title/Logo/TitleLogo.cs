using UnityEngine;
using DG.Tweening;

public class TitleLogo : MonoBehaviour
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

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        // 初期位置にセット
        transform.position = originalPosition;

        // 指定した座標へ移動
        transform.DOMove(targetPosition, moveDuration)
            .SetEase(moveEase)
            .SetDelay(moveDelay);
    }

    public void Shift()
    {
        Vector3 shiftedPosition = transform.position + new Vector3(0f, shiftAmount, 0f);

        transform.DOMove(shiftedPosition, shiftDuration).SetEase(Ease.OutCubic);
    }

    public void ResetPosition()
    {
        transform.DOMove(originalPosition, shiftDuration).SetEase(Ease.OutCubic);
    }
}
