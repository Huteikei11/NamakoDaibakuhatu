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

    private Sequence sequence; // DOTween�̃V�[�P���X���t�B�[���h�ŕێ�
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
                // �X�L�b�v���̍ŏI��ԁi�����x0�j
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0f);
                spriteRenderer2.color = new Color(spriteRenderer2.color.r, spriteRenderer2.color.g, spriteRenderer2.color.b, 0f);
                spriteRenderer3.color = new Color(spriteRenderer3.color.r, spriteRenderer3.color.g, spriteRenderer3.color.b, 0f);
                spriteRenderer4.color = new Color(spriteRenderer4.color.r, spriteRenderer4.color.g, spriteRenderer4.color.b, 0f);
                spriteRenderer5.color = new Color(spriteRenderer5.color.r, spriteRenderer5.color.g, spriteRenderer5.color.b, 0f);
                // ������StartTitle���Ă�
                StartTitle();
            }
        }
    }

    public void StartTitle()
    {
        //�^�C�g�����S���o�������������ɒǉ�
        titleKeiki.StartOP();
        titleKuroobi.StartOP();
        titlelogo.StartOP();
        titleMenuController.StartOP();
        titleButtonAnime1.FirstMovetoTarget();
        titleButtonAnime2.FirstMovetoTarget();
        titleButtonAnime3.FirstMovetoTarget();
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
