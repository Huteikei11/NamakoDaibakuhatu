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
        // spriteRenderer, spriteRenderer2, spriteRenderer3, spriteRenderer4�𓯎���MoveSprite
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
        // 2��FadeSprite�𓯎���
        sequence.Append(FadeSprite(spriteRenderer, 0f,1f));
        sequence.Join(FadeSprite(spriteRenderer2, 0f, 1f));
        sequence.Join(FadeSprite(spriteRenderer3, 0f, 1f));
        sequence.Join(FadeSprite(spriteRenderer4, 0f, 1f));
    }

    // �ړ��ʂ����������W���猻�ݍ��W�ֈړ��������x0��1�ɕω��iTween�ԋp�j
    public Tween MoveSprite(SpriteRenderer sprite, Vector3 moveAmount, float duration)
    {
        // �ړI�n�i���ݍ��W�j
        Vector3 destination = sprite.transform.position;
        // �X�^�[�g�n�_�i�ړI�n - �ړ��ʁj
        Vector3 startPosition = destination - moveAmount;
        sprite.transform.position = startPosition;

        // �܂������x��0�ɐݒ�
        var color = sprite.color;
        color.a = 0f;
        sprite.color = color;

        // Sequence�ňړ��Ɠ����x�ύX�𓯎��Ɏ��s���ATween��Ԃ�
        return DOTween.Sequence()
            .Join(sprite.transform.DOMove(destination, duration))
            .Join(sprite.DOFade(1f, duration));
    }

    // Sprite�̓����x��ς���iTween�ԋp�j
    public Tween FadeSprite(SpriteRenderer sprite, float targetAlpha, float duration)
    {
        return sprite.DOFade(targetAlpha, duration);
    }

    // �w��b���҂i�R�[���o�b�N�����ATween��Ԃ��j
    public Tween Wait(float seconds)
    {
        return DOTween.Sequence().AppendInterval(seconds);
    }
}
