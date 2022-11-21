using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���j�b�g�̊e��f�[�^
/// </summary>
public class Unit
{
    public int no;//���ʎq
    public string name;//���O
    public int level;//���x��
    public int hp;//�̗�
    public int maxHp;//�ő�̗�
    public int attack;//�U����
    public int def;//�h���
    public int speed;//���x�i��������Ɖ𓚎��Ԃ��Z���Ȃ�j
    public int exp;//�o���l
    public int gold;//������

    /// <summary>
    /// ���x���A�b�v
    /// </summary>
    public void LevelUp()
    {
        level++;
        maxHp += 5;
        attack += 3;
        def += 3;
        speed += 2;
        exp = 0;
    }
}

/// <summary>
/// ���C��
/// </summary>
public class Main : MonoBehaviour
{
    private int phase;//�t�F�C�Y�i�Q�[���̏�ԁj
    private int scenarioNo;//�V�i���I�ԍ�
    private int[] item = new int[Params.ITEM_LENGTH];//�����i�Ƒ����i�򑐁A����A�������A�Z�����j
    private int turn;//�^�[����
    private int time;//�𓚎���
    private int quetionNo;//���̖��ԍ�
    private int fadeIn;//�t�F�[�h�C���E�A�E�g���o���t���O
    private int attackPhase;//�U���A�j��
    private int damageValue;//�_���[�W�l
    private int damagePhase;//�_���[�W���o
    private int weaponAttack;//����̍U����
    private int armorDef;//�Z�̖h���
    private int legSpeed;//�C�̑��x

    private float alpha;//�t�F�[�h�̔Z��
    private float timeLength;//���Ԃ̏����\�� �g�嗦
    Vector3 attackFirstPosition;//�U���G�t�F�N�g�̏����ʒu

    //CSV�t�@�C���֌W�i���̎�荞�݁j
    TextAsset csvFile;//csv�t�@�C��
    readonly List<string[]> csvDatas = new();

    //�e��Unity�I�u�W�F�N�g
    [SerializeField] GameObject battleObj;//�퓬�E�B���h�E
    [SerializeField] GameObject questionObj;//���E�B���h�E
    [SerializeField] GameObject messageObj;//���b�Z�[�W�E�B���h�E
    [SerializeField] GameObject selectButtons;//�s���I���E�񓚃{�^��
    [SerializeField] GameObject fadeObj;//�t�F�[�h�C���E�A�E�g
    [SerializeField] GameObject clickObj;//�N���b�N����̎�t�I�u�W�F�N�g
    [SerializeField] GameObject attackObj;//�U���I�u�W�F
    [SerializeField] GameObject sounds;//��
    [SerializeField] GameObject enemyObj;//�G
    [SerializeField] GameObject hpObj;//HP
    [SerializeField] GameObject goldObj;//����
    [SerializeField] GameObject commandObj;//��ł̍s���E�B���h�E
    [SerializeField] GameObject mapObj;//�X�e�[�W�I��
    [SerializeField] GameObject statusObj;
    [SerializeField] GameObject timeObj;

    Unit self = new();//����
    Unit enemy = new();//�G���j�b�g

    Text messageText;//���b�Z�[�W�o�͂̊ȗ����p

    /// <summary>
    /// �N��������
    /// </summary>
    void Start()
    {
        Application.targetFrameRate = 30;//30FPS�ɌŒ�
        scenarioNo = 0;//�V�i���I������
        self.gold = 300;//����������
        timeLength = timeObj.transform.localScale.x;//���Ԃ̏����\���ʒu�ۑ�
        attackFirstPosition = attackObj.transform.position;//�U���̏����ʒu�ۑ�
        messageText = messageObj.transform.GetChild(1).GetComponent<Text>();//���b�Z�[�W�e�L�X�g�̊ȗ���

        //�e���ʂ̏����\��
        commandObj.SetActive(false);
        messageObj.SetActive(true);
        battleObj.SetActive(true);
        fadeObj.SetActive(true);
        clickObj.SetActive(true);
        mapObj.SetActive(false);
        questionObj.SetActive(false);
        
        //��蕶CSV�t�@�C���̓ǂݍ���
        csvFile = Resources.Load("quiz") as TextAsset; 
        StringReader reader = new(csvFile.text);
        while (reader.Peek() != -1) // reader.Peaek��-1�ɂȂ�܂�
        {
            string line = reader.ReadLine(); // ��s���ǂݍ���
            csvDatas.Add(line.Split(',')); // , ��؂�Ń��X�g�ɒǉ�
        }

        //�����̏����X�e�[�^�X
        self.level = 1;
        self.maxHp = self.hp = 20;//99;
        self.attack = 10;//99;
        self.def = 7;
        self.speed = 5;

        SetStatus();//�X�e�[�^�X�̔��f
    }

    /// <summary>
    /// �퓬�t�F�[�Y�̎擾
    /// </summary>
    /// <returns></returns>
    public int GetPhase() { return phase; }

    /// <summary>
    /// �G���J�E���g����
    /// </summary>
    /// <param name="enemyNo"></param>
    public void Encount(int enemyNo)
    {
        enemy.no = enemyNo;
        switch (enemyNo)//�G�̐ݒ�
        {
            case 0:
                enemy.name = "���m";
                enemy.hp = 15;
                enemy.attack = 8;
                enemy.def = 8;
                enemy.speed = 5;
                break;
            case 1:
                enemy.name = "�X���C��";
                enemy.hp = 18;
                enemy.attack = 8;
                enemy.def = 4;
                enemy.speed = 5;
                enemy.exp = 3;
                enemy.gold = 25;
                enemyObj.GetComponent<SpriteRenderer>().sprite =
                    Resources.Load<Sprite>("Graph/a001-030/sraim");
                battleObj.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite =
                    Resources.Load<Sprite>("Graph/b_gr01a_m");
                break;
            case 2:
                enemy.name = "�}���C�[�^�[";
                enemy.hp = 24;
                enemy.attack = 12;
                enemy.def = 5;
                enemy.speed = 8;
                enemy.exp = 8;
                enemy.gold = 135;
                enemyObj.GetComponent<SpriteRenderer>().sprite =
                    Resources.Load<Sprite>("Graph/a001-030/seed");
                battleObj.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite =
                    Resources.Load<Sprite>("Graph/b_gr01a_m");
                break;
            case 3:
                enemy.name = "�g�����g";
                enemy.hp = 32;
                enemy.attack = 17;
                enemy.def = 6;
                enemy.speed = 10;
                enemy.exp = 15;
                enemy.gold = 255;
                enemyObj.GetComponent<SpriteRenderer>().sprite =
                    Resources.Load<Sprite>("Graph/a061-090/treant");
                battleObj.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite =
                    Resources.Load<Sprite>("Graph/b_fo04_m");
                break;
            case 4:
                enemy.name = "�T���}���_�[";
                enemy.hp = 46;
                enemy.attack = 26;
                enemy.def = 9;
                enemy.speed = 15;
                enemy.exp = 36;
                enemy.gold = 460;
                enemyObj.GetComponent<SpriteRenderer>().sprite =
                    Resources.Load<Sprite>("Graph/a061-090/salamander");
                battleObj.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite =
                    Resources.Load<Sprite>("Graph/b_de01a_m");
                break;
            case 5:
                enemy.name = "�S�[����";
                enemy.hp = 68;
                enemy.attack = 36;
                enemy.def = 14;
                enemy.speed = 16;
                enemy.exp = 57;
                enemy.gold = 946;
                enemyObj.GetComponent<SpriteRenderer>().sprite =
                    Resources.Load<Sprite>("Graph/a061-090/mini_golem");
                battleObj.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite =
                    Resources.Load<Sprite>("Graph/b_de01a_m");
                break;
            case 6:
                enemy.name = "����";
                enemy.hp = 100;
                enemy.attack = 54;
                enemy.def = 24;
                enemy.speed = 20;
                enemy.exp = 100;
                enemyObj.GetComponent<SpriteRenderer>().sprite =
                    Resources.Load<Sprite>("Graph/a001-030/skull");
                battleObj.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite =
                    Resources.Load<Sprite>("Graph/b_d_cas03a_240");
                break;
        }
        if (enemyNo > 0)//���m�ȊO��BGM�ύX
        {
            sounds.GetComponents<AudioSource>()[Params.BGM].clip = (AudioClip)Resources.Load("Sounds/fantasy11");
            sounds.GetComponents<AudioSource>()[Params.BGM].Play();
        }
        sounds.GetComponents<AudioSource>()[Params.SE_ENCOUNT].Play();//�G���J�E���g��
        hpObj.SetActive(true);//�̗͂̕\��
        fadeIn = Params.FADE_IN1;//�t�F�[�h�̊J�n
        turn = 0;//�퓬�^�[���̃��Z�b�g
        ResetSelect();//�I�����̃��Z�b�g
        phase = Params.PHASE_BATTLE_START;//�s���I���t�F�[�Y��
    }

    /// <summary>
    /// �I���{�^���̕\���E��\��
    /// </summary>
    public void ActiveSelectButtons(bool flag)
    {
        messageObj.transform.GetChild(2).gameObject.SetActive(flag);//�퓬�{�^���̃A�N�e�B�u
    }

    /// <summary>
    /// �V�i���I�ǂݐi��
    /// </summary>
    /// <returns></returns>
    public void ScenarioNext()
    {
        switch (scenarioNo)//�C�x���g�n
        {
            case Params.SCENARIO_PROLOGUE_1:
                messageText.text = "���l�u�܂��͗͂��݂��Ă��炨��\n�@�@�@�����A�w�키�x��I�Ԃ̂��v��";
                break;
            case Params.SCENARIO_TUTERIAL_1:
                Encount(0);
                messageText.text = "���m�����ꂽ�I";
                break;
            case Params.SCENARIO_TUTERIAL_2:
                messageText.text = "���l�u���Ȃ��̗́A�����Ė�������B\n�@�@�@���̉ʂĂɂ��閂����|���Ă���v��";
                break;
            case Params.SCENARIO_TUTERIAL_3:
                messageText.text = "���l�u�x�x����p�ӂ����B\n�@�@�@�܂��͂���ŁA�����𐮂���̂��v��";
                break;
            case Params.SCENARIO_TUTERIAL_4:
                goldObj.SetActive(true);//�����̕\��
                sounds.GetComponents<AudioSource>()[Params.SE_COIN].Play();//�������ʉ�
                messageText.text = "���l����300 G�������B��";
                break;
            case Params.SCENARIO_TUTERIAL_5:
                messageText.text = "���l�u����ƁA���̂��Ȃ��ł͖����ɏ��Ă�B\n�@�@�@�킢���J��Ԃ��A�����Ȃ�̂��v��";
                break;
            case Params.SCENARIO_TUTERIAL_6:
                messageText.text = "���l�u�����s���A�E�҂�I\n�@�@�@�����Ȃ�A������|���̂��I�v��";
                break;
            case Params.SCENARIO_LOSE_1:
                clickObj.SetActive(true);
                enemyObj.SetActive(false);//�G�̔�A�N�e�B�u
                battleObj.transform.GetChild(1).gameObject.SetActive(true);//���l�̃A�N�e�B�u
                battleObj.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite =
                    Resources.Load<Sprite>("Graph/b_cas03a_m_240");//��
                SetHP(99);//HP����
                self.gold /= 2;//�������������ɂȂ�
                SetGold();
                messageText.text = "���l�u�����A�E�҂�B\n�@�@�@����ł��܂��Ƃ͏�Ȃ��v��";
                break;
            case Params.SCENARIO_LOSE_2:
                messageText.text = "���l�u���x�����グ�āA�ǂ������𔃂��̂��B\n�@�@�@���҂��Ă��邼�v��";
                fadeIn = Params.FADE_OUT1;//�t�F�[�h�A�E�g
                break;
            case Params.SCENARIO_TUTERIAL_END:
                commandObj.SetActive(true);//��̃R�}���h���X�g��\��
                clickObj.SetActive(false);//�e�L�X�g�N���b�N����̏���
                messageObj.SetActive(false);//���b�Z�[�W�E�B���h�E�����
                phase = Params.PHASE_CASTLE;//���
                break;
            case Params.SCENARIO_LOSE_END:
                sounds.GetComponents<AudioSource>()[Params.BGM].clip = (AudioClip)Resources.Load("Sounds/siro06");
                sounds.GetComponents<AudioSource>()[Params.BGM].Play();
                commandObj.SetActive(true);//��̃R�}���h���X�g��\��
                clickObj.SetActive(false);//�e�L�X�g�N���b�N����̏���
                messageObj.SetActive(false);//���b�Z�[�W�E�B���h�E�����
                phase = Params.PHASE_CASTLE;//���
                break;
            case Params.SCENARIO_LASTBATTLE_1:
                messageText.text = "�����u�����A�S�Ă͖��Ӗ�\n�@�@�@�����A�łт邪�悢�v��";
                break;
            case Params.SCENARIO_LASTBATTLE_END:
                clickObj.SetActive(false);//�e�L�X�g�N���b�N����̏���
                selectButtons.SetActive(true);//�I���{�^���̃A�N�e�B�u��
                messageText.text = enemy.name + "�����ꂽ�I";
                phase = Params.PHASE_SELECT;
                break;
            case Params.SCENARIO_EPLOGUE_1:
                selectButtons.SetActive(false);//�I���{�^���̔�A�N�e�B�u��
                messageText.text = "�����u�������c�@�����A�m���͕s�ςł͂Ȃ��B\n�@�@�@�������߂�ߖY���ȁc�v��";
                break;
            case Params.SCENARIO_EPLOGUE_2:
                fadeIn = Params.FADE_IN2;
                messageText.text = "�������ĉ����ɕ��a���߂����B\n�����A���Ȃ��͗��𑱂��Ă���B��";
                break;
            case Params.SCENARIO_EPLOGUE_3:
                messageText.text = "�����̌��t���󂯁A�m�������߂�\n�V���Ȓn�ւƌ��������̂ł���B��";
                break;
            case Params.SCENARIO_EPLOGUE_END:
                messageText.text = "�`�Q�[���N���A�I�`\n�v���C���Ă��������A���肪�Ƃ��������܂����B";
                break;
        }
        scenarioNo++;//���̃V�i���I���Ăяo��
    }

    /// <summary>
    /// �퓬����
    /// </summary>
    /// <param name="selectNo"></param>
    public void SelectAction(int selectNo)
    {
        switch (selectNo)
        {
            case 0://�키
                phase = Params.PHASE_QUIZ;//���̕\���Ɖ𓚑I����
                messageText.text = "���Ȃ��̍U���I";
                break;
            case 1://�h��
                if (scenarioNo == Params.SCENARIO_TUTERIAL_2)//�`���[�g���A�����Ȃ�
                {
                    messageText.text = "���l�u����Ă��Ă͏��Ă񂼁v";
                    break;
                }
                phase = Params.PHASE_GUARD;//�h�䏈����
                selectButtons.SetActive(false);//�I���{�^���̔�\��
                clickObj.SetActive(true);//�N���b�N����̕���
                messageText.text = "���Ȃ��͐g�\���Ă��遤";
                break;
            case 2://��
                if (item[Params.ITEM_HEAL] == 0)//�򑐂������ꍇ
                {
                    messageText.text = "�򑐂������Ă��Ȃ��c";
                    break;
                }
                sounds.GetComponents<AudioSource>()[Params.SE_HEAL].Play();//�񕜉�
                selectButtons.SetActive(false);//�I���{�^���̔�\��
                clickObj.SetActive(true);//�N���b�N����̕���
                phase = Params.PHASE_HEAL;//�񕜏�����
                SetHP(30);
                item[Params.ITEM_HEAL]--;
                messageText.text = "�򑐂��g�����B\nHP��30�񕜁�";
                break;
            case 3://����
                if (scenarioNo == Params.SCENARIO_TUTERIAL_2)
                {
                    messageText.text = "���l�u�ǂ��֍s�����肾�H�v";
                    break;
                }
                enemyObj.SetActive(false);//�G�I�u�W�F�N�g�̔�\��
                selectButtons.SetActive(false);//�I���{�^���̔�\��
                clickObj.SetActive(true);//�N���b�N����̕���
                messageText.text = "���Ȃ��͓����o������";
                phase = Params.PHASE_BATTLE_END;//�����t�F�[�Y��
                break;
        }
    }

    /// <summary>
    /// �����̍U��
    /// </summary>
    /// <returns></returns>
    public void SelfAttack(int selectNo)
    {
        questionObj.SetActive(false);//�������
                                     
        if (int.Parse(csvDatas[quetionNo][6]) == selectNo)//�𓚃`�F�b�N:����
        {
            damageValue = self.attack + weaponAttack - enemy.def;//�_���[�W�v�Z
            sounds.GetComponents<AudioSource>()[Params.SE_SUCCSESS].Play();//������
        }
        else//�𓚃`�F�b�N:���s
        {
            damageValue = 0;//�^�_���[�W��0
            sounds.GetComponents<AudioSource>()[Params.SE_FALUD].Play();//���s��
        }
        attackPhase = 1;//�U���G�t�F�N�g�̕\��
        //�����E�s�����̕\��
        for (int i = 0; i < Params.SELECT_BUTTON_LENGTH; i++)
        {
            if (i == int.Parse(csvDatas[quetionNo][6]))//����
            {
                selectButtons.transform.GetChild(i).GetComponent<Image>().color = new Color(0.3f, 0.6f, 0.3f);
            }
        }
        phase = Params.PHASE_ATTACK;
        messageText.text = "���Ȃ��̍U���I";
    }

    /// <summary>
    /// �G�̍U��
    /// </summary>
    /// <returns></returns>
    public void EnemyAttack(bool def)
    {
        questionObj.SetActive(false);
        sounds.GetComponents<AudioSource>()[Params.SE_DAMAGE].Play();//�_���[�W��
        damagePhase = 1;
        int d = enemy.attack - self.def - armorDef;
        if (def) d /= 2;//�h��I�����͔���        
        if (d < 1) d = 1;//�Œ�_���[�W�͂P
        SetHP(-d);//HP�̍ĕ\��
        phase = Params.PHASE_ENEMY_ACTION;//�����
        clickObj.SetActive(true);//�N���b�N����̃A�N�e�B�u��
        if (self.hp <= 0)
        {
            fadeIn = Params.FADE_IN2;
            messageText.text = enemy.name + "�̍U���I" + d + "�̃_���[�W�I\n���Ȃ��͎���ł��܂����c��";
            sounds.GetComponents<AudioSource>()[Params.BGM].clip = (AudioClip)Resources.Load("Sounds/gameover");//�Q�[���I�[�o�[
            sounds.GetComponents<AudioSource>()[Params.BGM].Play();
            scenarioNo = Params.SCENARIO_LOSE_1;
            phase = Params.PHASE_TALK;
            return;
        }
        messageText.text = enemy.name + "�̍U���I" + d + "�̃_���[�W�I��";
    }

    /// <summary>
    /// �G�̍s���㏈��
    /// </summary>
    public void ResetSelect()
    {
        for(int i = 0; i < Params.SELECT_BUTTON_LENGTH; i++)
        {
            selectButtons.transform.GetChild(i).GetComponent<Image>().color = new Color(0.3f, 0.2f, 0.2f);
        }
        selectButtons.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "�키";
        selectButtons.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = "���";
        selectButtons.transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "��("+item[0]+")";
        selectButtons.transform.GetChild(3).transform.GetChild(0).GetComponent<Text>().text = "������";
        phase = Params.PHASE_SELECT;//�s���I��
        clickObj.SetActive(false);
    }

    /// <summary>
    /// �퓬�I������
    /// </summary>
    public bool CheckBattleEnd()
    {
        turn++;//�^�[�������Z
        if (scenarioNo == Params.SCENARIO_TUTERIAL_2 && turn == 3)//�`���[�g���A������3�^�[���ڂɐ퓬�I��
        {
            enemyObj.SetActive(false);//�G�摜�̔�A�N�e�B�u��
            messageText.text = "���l�u����܂ŁI�v��";
            phase = Params.PHASE_TALK;//�퓬�I��
            return true;
        }
        messageText.text = "";
        messageObj.transform.GetChild(2).gameObject.SetActive(true);//�퓬�{�^���̃A�N�e�B�u
        return false;//�퓬�p��
    }

    /// <summary>
    /// �퓬�������b�Z�[�W
    /// </summary>
    /// <returns></returns>
    public void GetBattleEndMessage()
    {
        selectButtons.SetActive(false);
        clickObj.SetActive(true);
        self.exp += enemy.exp;

        sounds.GetComponents<AudioSource>()[Params.BGM].clip = (AudioClip)Resources.Load("Sounds/elepijingle");
        sounds.GetComponents<AudioSource>()[Params.BGM].Play();

        //���x���A�b�v�̃`�F�b�N
        if (self.exp > self.level * self.level * 5)
        {
            phase = Params.PHASE_LEVELUP;//���x���A�b�v�t�F�[�Y��
        }
        else//���x���A�b�v���Ă��Ȃ��ꍇ�͏I����
        {
            phase = Params.PHASE_BATTLE_END;
        }
        mapObj.transform.GetChild(5 - enemy.no).gameObject.SetActive(true);//���X�e�[�W�̉��
        self.gold += enemy.gold;//�����������Z
        SetGold();//�������̍ĕ\��
        messageText.text = enemy.name +"��|�����I\n" + enemy.exp + "�̌o���l��"+enemy.gold+" G���l���I��";
    }

    /// <summary>
    /// ���x���A�b�v����
    /// </summary>
    /// <returns></returns>
    public void LevelUp()
    {
        self.LevelUp();//���x���A�b�v
        SetStatus();//�X�e�[�^�X�\���ւ̔��f
        SetHP(0);//HP�̔��f�i�񕜂͂��Ȃ��j
        phase = Params.PHASE_BATTLE_END;
        sounds.GetComponents<AudioSource>()[Params.SE_LEVELUP].Play();//���x���A�b�v
        messageText.text = "���x���A�b�v�I\n�e��\�͂��オ��A���x����" +self.level+"�ɂȂ����I";
    }

    /// <summary>
    /// �퓬�I���㏈��
    /// </summary>
    public void EndBattle()
    {
        battleObj.SetActive(false);
        mapObj.SetActive(true);
        sounds.GetComponents<AudioSource>()[Params.BGM].clip = (AudioClip)Resources.Load("Sounds/map");
        sounds.GetComponents<AudioSource>()[Params.BGM].Play();
        clickObj.SetActive(false);
        messageObj.SetActive(false);
        phase = Params.PHASE_MAP;
    }

    /// <summary>
    /// ����o��
    /// </summary>
    public void ExitTown()
    {
        fadeIn = Params.FADE_IN1;//�t�F�[�h�C���J�n
        battleObj.transform.GetChild(1).gameObject.SetActive(false);//���l�̔�A�N�e�B�u
        sounds.GetComponents<AudioSource>()[Params.BGM].clip = (AudioClip)Resources.Load("Sounds/map");
        sounds.GetComponents<AudioSource>()[Params.BGM].Play();
        phase = Params.PHASE_MAP;//�}�b�v��
    }

    /// <summary>
    /// ��ɓ���
    /// </summary>
    public void EnterTown()
    {
        fadeIn = Params.FADE_IN1;//�t�F�[�h�C���J�n
        phase = Params.PHASE_CASTLE;//���
        sounds.GetComponents<AudioSource>()[Params.BGM].clip = (AudioClip)Resources.Load("Sounds/siro06");
        sounds.GetComponents<AudioSource>()[Params.BGM].Play();
        battleObj.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite =
            Resources.Load<Sprite>("Graph/b_cas03a_m_240");
    }

    /// <summary>
    /// �X�e�[�^�X�̍ĕ\��
    /// </summary>
    public void SetStatus()
    {
        statusObj.transform.GetChild(1).GetComponent<Text>().text
            = (self.attack + weaponAttack) + "\n" + (self.def + armorDef) + "\n"
            + (self.speed + legSpeed) + "\n" + Params.itemName[item[Params.ITEM_SWORD]] 
            + "\n" + Params.itemName[item[Params.ITEM_ARMOR]]
            + "\n" + Params.itemName[item[Params.ITEM_BOOTS]] 
            + "\n" + item[Params.ITEM_HEAL];
    }

    /// <summary>
    /// HP�E���x���̍ĕ\��
    /// </summary>
    /// <param name="value"></param>
    public void SetHP(int value)
    {
        self.hp += value;
        if (self.hp > self.maxHp) self.hp = self.maxHp;//�ő�HP�͒����Ȃ�
        if (self.hp < 0) self.hp = 0;//0���Ⴍ�͂Ȃ�Ȃ�
        hpObj.transform.GetChild(1).GetComponent<Text>().text = self.hp + "/" + self.maxHp +"\n" + self.level;
    }

    /// <summary>
    /// �����̍ĕ\��
    /// </summary>
    public void SetGold()
    {
        goldObj.transform.GetChild(1).GetComponent<Text>().text = self.gold+" G" ;
    }

    /// <summary>
    /// �A�C�e���̍w��
    /// </summary>
    /// <param name="itemNo"></param>
    /// <returns></returns>
    public bool BuyItem(int itemNo)
    {
        if (self.gold < Params.itemPrice[itemNo]) return false;
        switch (itemNo)//�A�C�e����ōX�V�Ώۂ�����
        {
            case 1:
            case 2:
            case 3:
                weaponAttack = Params.itemPower[itemNo];
                item[Params.ITEM_SWORD] = itemNo;
                break;
            case 4:
            case 5:
                armorDef = Params.itemPower[itemNo]; 
                item[Params.ITEM_ARMOR] = itemNo;
                break;
            case 6:
            case 7:
                legSpeed = Params.itemPower[itemNo];
                item[Params.ITEM_BOOTS] = itemNo;
                break;
            case 8:
                item[Params.ITEM_HEAL] += 1;
                break;
        }
        sounds.GetComponents<AudioSource>()[Params.SE_COIN].Play();
        statusObj.transform.GetChild(2).gameObject.SetActive(false);
        self.gold -= Params.itemPrice[itemNo];//�������̌��Z
        SetGold();//�������̍ĕ\��
        return true;
    }

    /// <summary>
    /// �h�ł̋x��
    /// </summary>
    public void RestInn()
    {
        if (self.gold < Params.REST_COST)//�h�オ����Ȃ��ꍇ
        {
            return;
        }
        self.gold -= Params.REST_COST;
        SetGold();
        sounds.GetComponents<AudioSource>()[Params.SE_HEAL].Play();//�񕜉�
        SetHP(99);//�S��
    }

    /// <summary>
    /// �A�C�e���I�����̑����ʌv�Z
    /// </summary>
    /// <param name="no"></param>
    public void OnMouseItem(int no)
    {
        switch (no)
        {
            case 1:
            case 2:
            case 3:
                if (Params.itemPower[no] - weaponAttack >= 0)
                    statusObj.transform.GetChild(2).GetComponent<Text>().text =
                        "+" + (Params.itemPower[no] - weaponAttack).ToString();
                else
                    statusObj.transform.GetChild(2).GetComponent<Text>().text =
                        (Params.itemPower[no] - weaponAttack).ToString();
                break;
            case 4:
            case 5:
                if (Params.itemPower[no] - armorDef > 0)
                    statusObj.transform.GetChild(2).GetComponent<Text>().text =
                        "\n+" + (Params.itemPower[no] - armorDef).ToString();
                else
                    statusObj.transform.GetChild(2).GetComponent<Text>().text =
                        "\n" + (Params.itemPower[no] - armorDef).ToString();
                break;
            case 6:
            case 7:
                if (Params.itemPower[no] - legSpeed > 0)
                    statusObj.transform.GetChild(2).GetComponent<Text>().text =
                        "\n\n+" + (Params.itemPower[no] - legSpeed).ToString();
                else
                    statusObj.transform.GetChild(2).GetComponent<Text>().text =
                        "\n\n" + (Params.itemPower[no] - legSpeed).ToString();
                break;
        }
    }

    /// <summary>
    /// ���Ԍo�ߏ���
    /// </summary>
    /// <returns></returns>
    public void Update()
    {
        //�t�F�[�h�C������
        switch (fadeIn)
        {
            case Params.FADE_IN1:
                fadeObj.SetActive(true);
                alpha += 0.1f;
                fadeObj.GetComponent<Image>().color = new Color(0, 0, 0, alpha);
                if (alpha > 1.4f)
                {
                    fadeIn = Params.FADE_OUT1;
                    switch (phase)//�t�F�[�Y�ŕ���
                    {
                        case Params.PHASE_CASTLE:
                            battleObj.SetActive(true);
                            commandObj.SetActive(true);
                            mapObj.SetActive(false);
                            battleObj.transform.GetChild(1).gameObject.SetActive(true);//���l�̃A�N�e�B�u
                            break;
                        case Params.PHASE_MAP:
                            battleObj.SetActive(false);
                            commandObj.SetActive(false);
                            mapObj.SetActive(true);
                            break;
                        case Params.PHASE_BATTLE_START:
                            mapObj.SetActive(false);//�}�b�v�̔�\��
                            battleObj.SetActive(true);//�퓬�I�u�W�F�N�g�̕\��
                            battleObj.transform.GetChild(3).gameObject.SetActive(true);//�G�摜�̃A�N�e�B�u��
                            messageObj.SetActive(true);//���b�Z�[�W�̕\��
                            if (enemy.no == 6)//������
                            {
                                scenarioNo = Params.SCENARIO_LASTBATTLE_1;
                                turn = 0;//�퓬�^�[���̃��Z�b�g
                                fadeIn = Params.FADE_IN1;//�t�F�[�h�̊J�n
                                phase = Params.PHASE_TALK;//������͉�b����J�n
                                clickObj.SetActive(true);
                                messageText.text = "�����u�悭�������܂ŒH�蒅�����v��";
                                break;
                            }
                            selectButtons.SetActive(true);//�I���{�^���̃A�N�e�B�u��
                            messageText.text = enemy.name + "�����ꂽ�I";
                            phase = Params.PHASE_SELECT;
                            break;
                    }
                }
                break;
            case Params.FADE_OUT1:
                alpha -= 0.1f;
                fadeObj.GetComponent<Image>().color = new Color(0, 0, 0, alpha);
                if (alpha < 0f) fadeIn = 0;
                fadeObj.SetActive(false);
                break;
            case Params.FADE_IN2://�s�k���t�F�[�h�C��
                fadeObj.SetActive(true);
                alpha += 0.1f;
                fadeObj.GetComponent<Image>().color = new Color(0, 0, 0, alpha);
                break;
            case Params.FADE_OUT2://�s�k���t�F�[�h�C��
                fadeObj.SetActive(true);
                alpha -= 0.05f;
                fadeObj.GetComponent<Image>().color = new Color(0, 0, 0, alpha);
                break;
        }

        //�U���G�t�F�N�g
        if (attackPhase == 1)
        {
            attackObj.SetActive(true);
            attackObj.transform.position = attackFirstPosition;
            attackPhase = 2;
        }
        else if (attackPhase == 2)
        {
            attackObj.transform.position -= new Vector3(0.3f, 0.3f, 0f);//�U���G�t�F�N�g�̈ړ�
            if (Vector3.Distance(attackObj.transform.position, attackFirstPosition) > 3.3f)
            {
                if (damageValue == 0)
                {
                    sounds.GetComponents<AudioSource>()[Params.SE_MISS].Play();
                    enemyObj.SetActive(true);//�G�摜�̃A�N�e�B�u��
                    attackPhase = 0;
                    attackObj.SetActive(false);
                    clickObj.SetActive(true);
                    messageText.text = "�s�����I�@�U���͔�����ꂽ�c��";
                }
                else
                {
                    sounds.GetComponents<AudioSource>()[Params.SE_ATTACK].Play();
                    enemyObj.SetActive(true);//�G�摜�̃A�N�e�B�u��
                    attackPhase = 0;
                    attackObj.SetActive(false);
                    clickObj.SetActive(true);
                    enemy.hp -= damageValue;
                    if(enemy.hp <= 0)
                    {
                        phase = Params.PHASE_BATTLE_WIN;//�퓬�����㏈����
                        if (enemy.no == 6)//������̏ꍇ�̓I�u�W�F�N�g����������b��
                        {
                            scenarioNo = Params.SCENARIO_EPLOGUE_1;//�G�s���[�O�̊J�n
                            phase = Params.PHASE_TALK;//��b�t�F�[�Y��
                        }
                        else
                        {
                            enemyObj.SetActive(false);//�G�I�u�W�F�N�g�̏���
                        }
                    }
                    messageText.text = "�����I�@" +enemy.name + "��" + damageValue + "�̃_���[�W�I��";
                }
            }
            else if (Vector3.Distance(attackObj.transform.position, attackFirstPosition) > 2.5f)
            {
                enemyObj.SetActive(false);//�G�摜�̔�A�N�e�B�u��
            }
        }

        //�_���[�W���o
        if (damagePhase == 1)
        {
            Camera.main.transform.position += new Vector3(0.03f, 0.03f, -10);
            if (Camera.main.transform.position.y > 0.1f) damagePhase = 2;
        }
        else if(damagePhase == 2)//�_���[�W���o�I��
        {
            Camera.main.transform.position -= new Vector3(0.03f, 0.03f, -10);
            if(Camera.main.transform.position.y < 0f){
                damagePhase = 0;
                Camera.main.transform.position = new Vector3(0, 0, -10);
            }
        }

        //�퓬�t�F�[�Y
        if (phase == Params.PHASE_QUIZ)//�o��t�F�[�Y�Ȃ�
        {
            sounds.GetComponents<AudioSource>()[Params.SE_QUESTION].Play();//�o�艹
            quetionNo = Random.Range(0,csvDatas.Count);//�����_���Ŗ���I��
            questionObj.SetActive(true);//���I�u�W�F�N�g�̕\��
            questionObj.transform.GetChild(1).gameObject.GetComponent<Text>().text = csvDatas[quetionNo][1];
            selectButtons.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = csvDatas[quetionNo][2];//��ŕύX
            selectButtons.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = csvDatas[quetionNo][3];//��ŕύX
            selectButtons.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = csvDatas[quetionNo][4];//��ŕύX
            selectButtons.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = csvDatas[quetionNo][5];//��ŕύX
            time = Params.ANSWER_TIME;//�񓚎��Ԃ̏�����
            phase = Params.PHASE_ANSER;//�񓚃t�F�[�Y��
        }
        else if(phase == Params.PHASE_ANSER)//�񓚃t�F�[�Y�Ȃ�
        {
            if (time <= 0) SelfAttack(4);//���Ԑ؂�͍U���Ɏ��s
            else if (enemy.speed - self.speed <= 0) time--;//���x�������肷����ꍇ�͈��
            else time -= enemy.speed - self.speed;//���Ԍo��
            timeObj.transform.localScale = new Vector3(time * 1.0f/Params.ANSWER_TIME * timeLength, 1,1);
        }
    }

}
