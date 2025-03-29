using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ObjectController2D : MonoBehaviour
{
    // Rigidbody2D �̃p�����[�^��Inspector����ݒ�\�ɂ���
    [Header("Rigidbody2D Settings")]
    public float linearDrag = 1f;
    public float gravityScale = 1f;

    // Y���W�̐����͈�
    [Header("Y Position Limits")]
    public float minY = -5f;
    public float maxY = 5f;

    // Rigidbody2D�R���|�[�l���g�ւ̎Q��
    private Rigidbody2D rb;

    void Awake()
    {
        // Rigidbody2D�R���|�[�l���g���擾
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // �����p�����[�^��Rigidbody2D�ɔ��f
        ApplyRigidbodySettings();
    }

    void Update()
    {
        if (!GameManager.Instance.IsGameStarted || GameManager.Instance.IsPaused) return; // �|�[�Y���͓����Ȃ�
        // Inspector�Œl��ύX�����ۂɃ��A���^�C���Ŕ��f
        ApplyRigidbodySettings();

        // Y���W�͈̔͐���
        LimitYPosition();
    }

    // Rigidbody2D �̃p�����[�^�𔽉f���郁�\�b�h
    private void ApplyRigidbodySettings()
    {
        if (rb != null)
        {
            rb.drag = linearDrag;
            rb.gravityScale = gravityScale;
        }
    }

    // Y���W��͈͓��ɐ������郁�\�b�h
    private void LimitYPosition()
    {
        if (rb == null) return;

        // Y���W���͈͊O�̏ꍇ�̏���
        if (rb.position.y < minY && rb.velocity.y < 0)
        {
            // �������̑��x�����Z�b�g
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.position = new Vector2(rb.position.x, minY);
        }
        else if (rb.position.y > maxY && rb.velocity.y > 0)
        {
            // ������̑��x�����Z�b�g
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.position = new Vector2(rb.position.x, maxY);
        }
    }

    // Y�����ɗ͂������郁�\�b�h
    public void AddPositionY(float force)
    {
        if (rb != null)
        {
            rb.AddForce(new Vector2(0, force), ForceMode2D.Impulse);
        }
    }

    public void AddpositionY(float position)
    {
        if (rb != null)
        {
            rb.position += new Vector2(0, position);
            // Y���W�͈̔͐���
            LimitYPosition();
        }
    }

    public void AddvelocityY(float velocity)
    {
        if (rb != null)
        {
            rb.velocity += new Vector2(0, velocity);
        }
    }
}
