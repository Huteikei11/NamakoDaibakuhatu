using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gaman : MonoBehaviour
{
    [SerializeField]�@private ObjectController2D suii;
    [SerializeField] private OppaiManager oppaiManager;
    public float stamina;            // ���݂̃X�^�~�i
    public float maxStamina;         // �X�^�~�i�̍ő�l
    public float staminaSpeed;       // �X�^�~�i����x
    public float gamanPower;         // �䖝�̃p���[
    public float gamanCoolTime;      // �N�[���^�C��
    public float staminaRecoveryRate; // �X�^�~�i�񕜗� (m)
    public float staminaNotGamanRecoveryRate; // �X�^�~�i�񕜗� (m)
    public float staminaRecoveryInterval; // �X�^�~�i�񕜊Ԋu (n)

    private float lastGamanTime;     // �O��gmanadd�����s��������
    private float lastRecoveryTime;  // �Ō�ɃX�^�~�i���񕜂�������
    private bool lastGaman; //�O��̉񕜂���K�}�����g������
    

    // Start is called before the first frame update
    void Start()
    {
        lastGamanTime = -gamanCoolTime; // ���������ɃN�[���^�C�������Z�b�g
        lastRecoveryTime = Time.time;  // ���������ɉ񕜃^�C�}�[�����Z�b�g
    }

    // Update is called once per frame
    void Update()
    {
        // S�L�[�������ꂽ�Ƃ��̏���
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (stamina > staminaSpeed && Time.time >= lastGamanTime + gamanCoolTime) // �����ύX: staminaSpeed ���傫���ꍇ
            {
                stamina -= staminaSpeed;
                stamina = Mathf.Max(0, stamina); // stamina�����̒l�ɂȂ�Ȃ��悤�ɂ���
                gmanadd();
                lastGamanTime = Time.time; // ���s���Ԃ��X�V
                lastGaman = false;
                oppaiManager.CheckGamanKey();

            }
        }

        // n�b���ƂɃX�^�~�i����
        if (Time.time >= lastRecoveryTime + staminaRecoveryInterval)
        {
            if (lastGaman)//�O��̉񕜂���K�}�����g������
            {
                stamina += staminaNotGamanRecoveryRate;//�g��Ȃ�����
            }
            else
            {
                stamina += staminaRecoveryRate;
            }
            stamina = Mathf.Min(maxStamina, stamina); // stamina��maxStamina�𒴂��Ȃ��悤�ɂ���
            lastRecoveryTime = Time.time; // �񕜎��Ԃ��X�V
            lastGaman = true;
        }

    }

    // gamanadd���\�b�h�̒�`
    void gmanadd()
    {
        //�ː��������ȂƂ�
        if (oppaiManager.isChecking)
        {
            // gamanPower��p���������������ɋL�q
            Debug.Log("�ː����O�I: " + gamanPower);
            suii.AddPositionY(gamanPower/4);
        }
        //����
        else
        {
            // gamanPower��p���������������ɋL�q
            Debug.Log("�K�}���I: " + gamanPower);
            suii.AddPositionY(gamanPower);
        }
    }
}
