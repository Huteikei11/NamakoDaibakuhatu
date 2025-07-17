using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TitleOPController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteRenderer spriteRenderer2;
    [SerializeField] private SpriteRenderer spriteRenderer3;
    [SerializeField] private SpriteRenderer spriteRenderer4;
    // Start is called before the first frame update
    void Start()
    {
        var sequence = DOTween.Sequence();
        // spriteRenderer, spriteRenderer2, spriteRenderer3, spriteRenderer4を同時にMoveSprite
        sequence.Append(MoveSprite(spriteRenderer, new Vector3(-1f, 0, 0), 1f));
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
        // 2つのFadeSpriteを同時に
        sequence.Append(FadeSprite(spriteRenderer, 0f,1f));
        sequence.Join(FadeSprite(spriteRenderer2, 0f, 1f));
        sequence.Join(FadeSprite(spriteRenderer3, 0f, 1f));
        sequence.Join(FadeSprite(spriteRenderer4, 0f, 1f));
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
