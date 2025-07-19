using UnityEngine;
using DG.Tweening;

public class TitleButtonAnime : MonoBehaviour
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

    public bool isfitst;
    public bool top;

    private float adjust;
   
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;


        // �����ʒu�ɃZ�b�g
        transform.position = originalPosition;
        // �w�莞�ԑ҂��Ă���ړ��idelay = 1�b�j
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
    // �w�肵�����W�itargetPosition�j�� delay�b�҂��Ă���ړ�
    public void MoveToTarget(float delay, float adjust =0f)
    {
        targetPosition.x += adjust;
        transform.DOMove(targetPosition, moveDuration)
            .SetEase(moveEase)
            .SetDelay(delay);
        targetPosition.x -= adjust;
    }

    // �����ʒu�ioriginalPosition�j�� delay�b�҂��Ă���߂�
    public void MoveToOriginal(float delay)
    {
        transform.DOMove(originalPosition, moveDuration)
            .SetEase(moveEase)
            .SetDelay(delay);
    }

    // �E�ɂ���ĈÂ�����iShift�j
    public void Shift()
    {
        Vector3 shiftedPosition = transform.position + new Vector3(shiftAmount, 0f, 0f);
        transform.DOMove(shiftedPosition, shiftDuration).SetEase(Ease.OutCubic);
    }

    // ���̈ʒu�ɖ߂��iReset�j
    public void ResetPosition()
    {
        transform.DOMove(originalPosition, shiftDuration).SetEase(Ease.OutCubic);
    }
}
