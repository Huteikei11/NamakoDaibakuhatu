using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ObjectController2D : MonoBehaviour
{
    // Rigidbody2D のパラメータをInspectorから設定可能にする
    [Header("Rigidbody2D Settings")]
    public float linearDrag = 1f;
    public float gravityScale = 1f;

    // Y座標の制限範囲
    [Header("Y Position Limits")]
    public float minY = -5f;
    public float maxY = 5f;

    // Rigidbody2Dコンポーネントへの参照
    private Rigidbody2D rb;

    void Awake()
    {
        // Rigidbody2Dコンポーネントを取得
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // 初期パラメータをRigidbody2Dに反映
        ApplyRigidbodySettings();
    }

    void Update()
    {
        if (!GameManager.Instance.IsGameStarted || GameManager.Instance.IsPaused) return; // ポーズ中は動かない
        // Inspectorで値を変更した際にリアルタイムで反映
        ApplyRigidbodySettings();

        // Y座標の範囲制限
        LimitYPosition();
    }

    // Rigidbody2D のパラメータを反映するメソッド
    private void ApplyRigidbodySettings()
    {
        if (rb != null)
        {
            rb.drag = linearDrag;
            rb.gravityScale = gravityScale;
        }
    }

    // Y座標を範囲内に制限するメソッド
    private void LimitYPosition()
    {
        if (rb == null) return;

        // Y座標が範囲外の場合の処理
        if (rb.position.y < minY && rb.velocity.y < 0)
        {
            // 下方向の速度をリセット
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.position = new Vector2(rb.position.x, minY);
        }
        else if (rb.position.y > maxY && rb.velocity.y > 0)
        {
            // 上方向の速度をリセット
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.position = new Vector2(rb.position.x, maxY);
        }
    }

    // Y方向に力を加えるメソッド
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
            // Y座標の範囲制限
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
