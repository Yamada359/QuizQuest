using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Params 
{
    //�Q�[���o�����X�e��
    public const int ANSWER_TIME = 250;//��{�񓚎���
    public const int REST_COST = 50;//�x����p�iUnity�����ύX���K�v�j

    //��b�V�X�e��
    public const int SELECT_BUTTON_LENGTH = 4;//�񓚃{�^������

    public const int FADE_IN1 = 1;//�t�F�[�h�C������A�E�g�𑦍��ɍs��
    public const int FADE_OUT1 = 2;//�t�F�[�h�A�E�g�𑦍��ɍs��
    public const int FADE_IN2 = 3;//�t�F�[�h�C������Ó]���ێ��i�s�k���̂ݎg�p�j
    public const int FADE_OUT2 = 4;//�t�F�[�h�A�E�g���������

    //�V�i���I�ԍ�
    public const int SCENARIO_PROLOGUE_1 = 0;//�v�����[�O
    public const int SCENARIO_TUTERIAL_1 = 1;//�`���[�g���A��
    public const int SCENARIO_TUTERIAL_2 = 2;
    public const int SCENARIO_TUTERIAL_3 = 3;
    public const int SCENARIO_TUTERIAL_4 = 4;
    public const int SCENARIO_TUTERIAL_5 = 5;
    public const int SCENARIO_TUTERIAL_6 = 6;
    public const int SCENARIO_TUTERIAL_END = 7;

    public const int SCENARIO_LOSE_1 = 20;//�퓬�s�k
    public const int SCENARIO_LOSE_2 = 21;
    public const int SCENARIO_LOSE_END = 22;

    public const int SCENARIO_LASTBATTLE_1 = 30;//�ŏI�퓬
    public const int SCENARIO_LASTBATTLE_END = 31;

    public const int SCENARIO_EPLOGUE_1 = 40;//�ŏI�퓬����
    public const int SCENARIO_EPLOGUE_2 = 41;
    public const int SCENARIO_EPLOGUE_3 = 42;
    public const int SCENARIO_EPLOGUE_END = 43;

    //��
    public const int SE_SUCCSESS = 0;//�񓚓����艹
    public const int SE_FALUD = 1;//�񓚂͂��ꉹ
    public const int SE_ATTACK = 2;//�U����
    public const int SE_MISS = 3;//�U���O�ꉹ
    public const int SE_DAMAGE = 4;//�_���[�W��
    public const int SE_COIN = 5;//�R�C����
    public const int BGM = 6;//BGM
    public const int SE_QUESTION = 7;//��艹
    public const int SE_ENCOUNT = 8;//�G���J�E���g��
    public const int SE_HEAL = 9;//�񕜉�
    public const int SE_LEVELUP = 10;//���x���A�b�v��

    //�t�F�[�Y�i�������j
    public const int PHASE_TALK = 0;//��b�t�F�[�Y
    public const int PHASE_CASTLE = 1;//��t�F�[�Y
    public const int PHASE_MAP = 2;//�}�b�v�s���t�F�[�Y
    public const int PHASE_BATTLE_START = 3;//�퓬�J�n�t�F�[�Y
    public const int PHASE_SELECT = 4;//�s���I���t�F�[�Y(�U��:2 �h��: ��: ����:�j
    public const int PHASE_QUIZ = 5;//�o��t�F�[�Y
    public const int PHASE_ANSER = 6;//�񓚃t�F�[�Y
    public const int PHASE_ATTACK = 7;//�U���t�F�[�Y
    public const int PHASE_ENEMY_ACTION = 8;//�G�̍s���t�F�[�Y
    public const int PHASE_GUARD = 9;//�h��t�F�[�Y
    public const int PHASE_HEAL = 10;//�񕜃t�F�[�Y
    public const int PHASE_BATTLE_WIN = 11;//�퓬�����t�F�[�Y
    public const int PHASE_LEVELUP = 12;//���x���A�b�v�t�F�[�Y
    public const int PHASE_BATTLE_END = 13;//�퓬�I���t�F�[�Y
    public const int PHASE_BATTLE_LOSE = 14;//�퓬�s�k�t�F�[�Y
    public const int PHASE_BATTLE_ESCAPE = 15;//�����t�F�[�Y

    //�A�C�e���e��
    public const int ITEM_HEAL = 0;//��
    public const int ITEM_SWORD = 1;//��
    public const int ITEM_ARMOR = 2;//�Z
    public const int ITEM_BOOTS = 3;//�C

    public const int ITEM_LENGTH = 4;//�A�C�e������

    public static string[] itemName = { "�Ȃ�", "�S�̌�", "�|�̌�", "���̌�", "�S�̊Z", "�|�̊Z", "�T���_��", "�u�[�c" };//�A�C�e������
    public static int[] itemPower = { 0, 3, 8, 15, 8, 15, 4, 10 };//�A�C�e���㏸�l
    public static int[] itemPrice = { 0, 200, 800, 2200, 700, 1900, 180, 900, 50 };//�A�C�e�����i

    
}
