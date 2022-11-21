using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���[�U�[����̓��͑S��
/// </summary>
public class Inputter : MonoBehaviour
{
    //�e��Unity�I�u�W�F�N�g
    [SerializeField] GameObject mainObj;
    private Main main;//���C���ւ̌Ăяo���p

    /// <summary>
    /// �N��������
    /// </summary>
    private void Start()
    {
        main = mainObj.GetComponent<Main>();//���C���ւ̌Ăяo���p�֌Ăяo�����ȗ���
    }

    /// <summary>
    /// ���b�Z�[�W�E�B���h�E�̃N���b�N
    /// </summary>
    public void ClickText()
    {
        switch (main.GetPhase())//�t�F�[�Y�ɂ�镪��
        {
            case Params.PHASE_BATTLE_START:
                main.ActiveSelectButtons(true);//�퓬�{�^���̃A�N�e�B�u��
                break;
            case Params.PHASE_ATTACK:
                main.EnemyAttack(false);//�G�̍U��
                main.ActiveSelectButtons(false);//�퓬�{�^���̔�A�N�e�B�u��
                break;
            case Params.PHASE_ENEMY_ACTION:
                if (main.CheckBattleEnd())
                    break;//�퓬�p���E�I���̃`�F�b�N
                main.ResetSelect();//���̏�����
                break;
            case Params.PHASE_GUARD:
                main.EnemyAttack(true);//�G�̍U���i�h�䂠��j
                break;
            case Params.PHASE_HEAL://�񕜂�I��
                main.EnemyAttack(false);//�G�̍U���ցi�h��Ȃ��j
                break;
            case Params.PHASE_BATTLE_WIN://�퓬�ɏ���
                main.GetBattleEndMessage();//�������b�Z�[�W�̎擾��
                break;
            case Params.PHASE_TALK://��b�t�F�[�Y
            case Params.PHASE_BATTLE_LOSE://�퓬�ɔs�k
                main.ScenarioNext();//���e�L�X�g�̊m�F
                break;
            case Params.PHASE_LEVELUP://���x���A�b�v�t�F�[�Y
                main.LevelUp();//���x���A�b�v����
                break;
            case Params.PHASE_BATTLE_END://�퓬�I���t�F�[�Y
                main.EndBattle();
                break;
        }
    }

    /// <summary>
    /// �s���{�^���̃N���b�N�i0�`3�j
    /// </summary>
    /// <param name="i"></param>
    public void ClickSelectButton(int i)
    {
        if(main.GetPhase() == Params.PHASE_SELECT)//�s���I����
            main.SelectAction(i);//�{�^���ԍ��̎󂯓n��
        if (main.GetPhase() == Params.PHASE_ANSER)//�𓚑I����
            main.SelfAttack(i);//�{�^���ԍ��̎󂯓n��
    }

    /// <summary>
    /// �邩��o��{�^���̃N���b�N
    /// </summary>
    public void ClickExit()
    {
        main.ExitTown();
    }

    /// <summary>
    /// ��{�^���̃N���b�N
    /// </summary>
    public void ClickTown()
    {
        main.EnterTown();
    }

    /// <summary>
    /// �A�C�e���̍w���i1�`8�j
    /// </summary>
    /// <param name="no"></param>
    public void BuyItem(int no)
    {
        main.BuyItem(no);
        main.SetStatus();
    }

    /// <summary>
    /// �x���{�^���̃N���b�N
    /// </summary>
    public void RestInn()
    {
        main.RestInn();//�x������
    }

    /// <summary>
    /// �}�b�v�̃N���b�N
    /// </summary>
    public void ClickMap(int no)
    {
        main.Encount(no);
    }

    /// <summary>
    /// �A�C�e���̏�ɃJ�[�\�����̂���
    /// </summary>
    /// <param name="no"></param>
    public void OnMouseItem(int no)
    {
        main.OnMouseItem(no);
    }

}
