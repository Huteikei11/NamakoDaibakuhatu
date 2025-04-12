using UnityEngine;
using DG.Tweening;

public class TitleLogo : MonoBehaviour
{
    [Header("�ړ��ݒ�")]
    public Vector3 originalPosition = Vector3.zero; // �ړ����iInspector����w��j
    public Vector3 targetPosition = new Vector3(2f, 0f, 0f); // �ړ���
    public float moveDelay = 1f;
    public float moveDuration = 1f;
    public Ease moveEase = Ease.InOutQuad;

    [Header("�E�ɂ���铮��ݒ�")]
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

        // �����ʒu�ɃZ�b�g
        transform.position = originalPosition;

        // �w�肵�����W�ֈړ�
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
