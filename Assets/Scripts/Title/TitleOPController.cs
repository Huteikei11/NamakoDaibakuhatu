using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEditor.ShaderKeywordFilter;

public class TitleOPController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteRenderer spriteRenderer2;
    [SerializeField] private SpriteRenderer spriteRenderer3;
    [SerializeField] private SpriteRenderer spriteRenderer4;
    [SerializeField] private SpriteRenderer spriteRenderer5;

    [SerializeField] private TitleKeiki titleKeiki;
    [SerializeField] private TitleLogo titlelogo;
    [SerializeField] private TitleKuroobi titleKuroobi;
    [SerializeField] private TitleMenuController titleMenuController;
    [SerializeField] private TitleButtonAnime titleButtonAnime1;
    [SerializeField] private TitleButtonAnime titleButtonAnime2;
    [SerializeField]private TitleButtonAnime titleButtonAnime3;

    private Sequence sequence; // DOTweenのシーケンスをフィールドで保持
    private bool isSkipped = false;

    // Start is called before the first frame update
    void Start()
    {
        sequence = DOTween.Sequence();
        sequence.Append(MoveSprite(spriteRenderer, new Vector3(-1f, 0, 0), 1f));
        sequence.Join(MoveSprite(spriteRenderer5, new Vector3(0f, 0, 0), 0f));
        sequence.Append(Wait(0.6f));
        sequence.Append(FadeSprite(spriteRenderer, 0.5f, 0.5f));
        sequence.Append(MoveSprite(spriteRenderer2, new Vector3(2f, 0f, 0), 1f));
        sequence.Append(Wait(0.6f));
        sequence.Append(FadeSprite(spriteRenderer2, 0.5f, 0.5f));
        sequence.Append(Wait(0.6f));
        sequence.Append(MoveSprite(spriteRenderer3, new Vector3(0f, -3f, 0), 1f));
        sequence.Append(Wait(0.4f));
        sequence.Append(MoveSprite(spriteRenderer4, new Vector3(0, 0f, 0), 0.6f));
        sequence.Append(Wait(2f));
        sequence.Append(FadeSprite(spriteRenderer, 0f,1f));
        sequence.Join(FadeSprite(spriteRenderer2, 0f, 1f));
        sequence.Join(FadeSprite(spriteRenderer3, 0f, 1f));
        sequence.Join(FadeSprite(spriteRenderer4, 0f, 1f));
        sequence.Join(FadeSprite(spriteRenderer5, 0f, 1f));
        sequence.Append(Wait(0.5f));
        sequence.OnComplete(() => StartTitle());
    }

    void Update()
    {
        if (isSkipped) return;
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            isSkipped = true;
            if (sequence != null && sequence.IsActive())
            {
                sequence.Kill();
                // スキップ時の最終状態（透明度0）
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0f);
                spriteRenderer2.color = new Color(spriteRenderer2.color.r, spriteRenderer2.color.g, spriteRenderer2.color.b, 0f);
                spriteRenderer3.color = new Color(spriteRenderer3.color.r, spriteRenderer3.color.g, spriteRenderer3.color.b, 0f);
                spriteRenderer4.color = new Color(spriteRenderer4.color.r, spriteRenderer4.color.g, spriteRenderer4.color.b, 0f);
                spriteRenderer5.color = new Color(spriteRenderer5.color.r, spriteRenderer5.color.g, spriteRenderer5.color.b, 0f);
                // ここでStartTitleを呼ぶ
                StartTitle();
            }
        }
    }

    public void StartTitle()
    {
        //タイトルロゴを出す処理をここに追加
        titleKeiki.StartOP();
        titleKuroobi.StartOP();
        titlelogo.StartOP();
        titleMenuController.StartOP();
        titleButtonAnime1.FirstMovetoTarget();
        titleButtonAnime2.FirstMovetoTarget();
        titleButtonAnime3.FirstMovetoTarget();
    }

    // 移動量を引いた座標から現在座標へ移動しつつ透明度0→1に変化（Tween返却）
    public Tween MoveSprite(SpriteRenderer sprite, Vector3 moveAmount, float duration)
    {
        // 目的地（現在座標）
        Vector3 destination = sprite.transform.position;
        // スタート地点（目的地 - 移動量）
        Vector3 startPosition = destination - moveAmount;
        sprite.transform.position = startPosition;

        // まず透明度を0に設定
        var color = sprite.color;
        color.a = 0f;
        sprite.color = color;

        // Sequenceで移動と透明度変更を同時に実行し、Tweenを返す
        return DOTween.Sequence()
            .Join(sprite.transform.DOMove(destination, duration))
            .Join(sprite.DOFade(1f, duration));
    }

    // Spriteの透明度を変える（Tween返却）
    public Tween FadeSprite(SpriteRenderer sprite, float targetAlpha, float duration)
    {
        return sprite.DOFade(targetAlpha, duration);
    }

    // 指定秒数待つ（コールバック無し、Tweenを返す）
    public Tween Wait(float seconds)
    {
        return DOTween.Sequence().AppendInterval(seconds);
    }
}
