using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ユニットの各種データ
/// </summary>
public class Unit
{
    public int no;//識別子
    public string name;//名前
    public int level;//レベル
    public int hp;//体力
    public int maxHp;//最大体力
    public int attack;//攻撃力
    public int def;//防御力
    public int speed;//速度（差があると解答時間が短くなる）
    public int exp;//経験値
    public int gold;//所持金

    /// <summary>
    /// レベルアップ
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
/// メイン
/// </summary>
public class Main : MonoBehaviour
{
    private int phase;//フェイズ（ゲームの状態）
    private int scenarioNo;//シナリオ番号
    private int[] item = new int[Params.ITEM_LENGTH];//所持品と装備（薬草、武器、頭装備、鎧装備）
    private int turn;//ターン数
    private int time;//解答時間
    private int quetionNo;//今の問題番号
    private int fadeIn;//フェードイン・アウト演出中フラグ
    private int attackPhase;//攻撃アニメ
    private int damageValue;//ダメージ値
    private int damagePhase;//ダメージ演出
    private int weaponAttack;//武器の攻撃力
    private int armorDef;//鎧の防御力
    private int legSpeed;//靴の速度

    private float alpha;//フェードの濃さ
    private float timeLength;//時間の初期表示 拡大率
    Vector3 attackFirstPosition;//攻撃エフェクトの初期位置

    //CSVファイル関係（問題の取り込み）
    TextAsset csvFile;//csvファイル
    readonly List<string[]> csvDatas = new();

    //各種Unityオブジェクト
    [SerializeField] GameObject battleObj;//戦闘ウィンドウ
    [SerializeField] GameObject questionObj;//問題ウィンドウ
    [SerializeField] GameObject messageObj;//メッセージウィンドウ
    [SerializeField] GameObject selectButtons;//行動選択・回答ボタン
    [SerializeField] GameObject fadeObj;//フェードイン・アウト
    [SerializeField] GameObject clickObj;//クリック判定の受付オブジェクト
    [SerializeField] GameObject attackObj;//攻撃オブジェ
    [SerializeField] GameObject sounds;//音
    [SerializeField] GameObject enemyObj;//敵
    [SerializeField] GameObject hpObj;//HP
    [SerializeField] GameObject goldObj;//お金
    [SerializeField] GameObject commandObj;//城での行動ウィンドウ
    [SerializeField] GameObject mapObj;//ステージ選択
    [SerializeField] GameObject statusObj;
    [SerializeField] GameObject timeObj;

    Unit self = new();//自分
    Unit enemy = new();//敵ユニット

    Text messageText;//メッセージ出力の簡略化用

    /// <summary>
    /// 起動時処理
    /// </summary>
    void Start()
    {
        Application.targetFrameRate = 30;//30FPSに固定
        scenarioNo = 0;//シナリオ初期化
        self.gold = 300;//お金初期化
        timeLength = timeObj.transform.localScale.x;//時間の初期表示位置保存
        attackFirstPosition = attackObj.transform.position;//攻撃の初期位置保存
        messageText = messageObj.transform.GetChild(1).GetComponent<Text>();//メッセージテキストの簡略化

        //各種画面の初期表示
        commandObj.SetActive(false);
        messageObj.SetActive(true);
        battleObj.SetActive(true);
        fadeObj.SetActive(true);
        clickObj.SetActive(true);
        mapObj.SetActive(false);
        questionObj.SetActive(false);
        
        //問題文CSVファイルの読み込み
        csvFile = Resources.Load("quiz") as TextAsset; 
        StringReader reader = new(csvFile.text);
        while (reader.Peek() != -1) // reader.Peaekが-1になるまで
        {
            string line = reader.ReadLine(); // 一行ずつ読み込み
            csvDatas.Add(line.Split(',')); // , 区切りでリストに追加
        }

        //自分の初期ステータス
        self.level = 1;
        self.maxHp = self.hp = 20;//99;
        self.attack = 10;//99;
        self.def = 7;
        self.speed = 5;

        SetStatus();//ステータスの反映
    }

    /// <summary>
    /// 戦闘フェーズの取得
    /// </summary>
    /// <returns></returns>
    public int GetPhase() { return phase; }

    /// <summary>
    /// エンカウント処理
    /// </summary>
    /// <param name="enemyNo"></param>
    public void Encount(int enemyNo)
    {
        enemy.no = enemyNo;
        switch (enemyNo)//敵の設定
        {
            case 0:
                enemy.name = "兵士";
                enemy.hp = 15;
                enemy.attack = 8;
                enemy.def = 8;
                enemy.speed = 5;
                break;
            case 1:
                enemy.name = "スライム";
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
                enemy.name = "マンイーター";
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
                enemy.name = "トレント";
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
                enemy.name = "サラマンダー";
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
                enemy.name = "ゴーレム";
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
                enemy.name = "魔王";
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
        if (enemyNo > 0)//兵士以外はBGM変更
        {
            sounds.GetComponents<AudioSource>()[Params.BGM].clip = (AudioClip)Resources.Load("Sounds/fantasy11");
            sounds.GetComponents<AudioSource>()[Params.BGM].Play();
        }
        sounds.GetComponents<AudioSource>()[Params.SE_ENCOUNT].Play();//エンカウント音
        hpObj.SetActive(true);//体力の表示
        fadeIn = Params.FADE_IN1;//フェードの開始
        turn = 0;//戦闘ターンのリセット
        ResetSelect();//選択肢のリセット
        phase = Params.PHASE_BATTLE_START;//行動選択フェーズへ
    }

    /// <summary>
    /// 選択ボタンの表示・非表示
    /// </summary>
    public void ActiveSelectButtons(bool flag)
    {
        messageObj.transform.GetChild(2).gameObject.SetActive(flag);//戦闘ボタンのアクティブ
    }

    /// <summary>
    /// シナリオ読み進め
    /// </summary>
    /// <returns></returns>
    public void ScenarioNext()
    {
        switch (scenarioNo)//イベント系
        {
            case Params.SCENARIO_PROLOGUE_1:
                messageText.text = "王様「まずは力をみせてもらおう\n　　　さぁ、『戦う』を選ぶのだ」▽";
                break;
            case Params.SCENARIO_TUTERIAL_1:
                Encount(0);
                messageText.text = "兵士が現れた！";
                break;
            case Params.SCENARIO_TUTERIAL_2:
                messageText.text = "王様「そなたの力、見せて貰ったぞ。\n　　　東の果てにいる魔王を倒してくれ」▽";
                break;
            case Params.SCENARIO_TUTERIAL_3:
                messageText.text = "王様「支度金を用意した。\n　　　まずはこれで、装備を整えるのだ」▽";
                break;
            case Params.SCENARIO_TUTERIAL_4:
                goldObj.SetActive(true);//お金の表示
                sounds.GetComponents<AudioSource>()[Params.SE_COIN].Play();//お金効果音
                messageText.text = "王様から300 Gを貰った。▽";
                break;
            case Params.SCENARIO_TUTERIAL_5:
                messageText.text = "王様「それと、今のそなたでは魔王に勝てん。\n　　　戦いを繰り返し、強くなるのだ」▽";
                break;
            case Params.SCENARIO_TUTERIAL_6:
                messageText.text = "王様「さぁ行け、勇者よ！\n　　　強くなり、魔王を倒すのだ！」▽";
                break;
            case Params.SCENARIO_LOSE_1:
                clickObj.SetActive(true);
                enemyObj.SetActive(false);//敵の非アクティブ
                battleObj.transform.GetChild(1).gameObject.SetActive(true);//王様のアクティブ
                battleObj.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite =
                    Resources.Load<Sprite>("Graph/b_cas03a_m_240");//城
                SetHP(99);//HPを回復
                self.gold /= 2;//所持金が半分になる
                SetGold();
                messageText.text = "王様「おお、勇者よ。\n　　　死んでしまうとは情けない」▽";
                break;
            case Params.SCENARIO_LOSE_2:
                messageText.text = "王様「レベルを上げて、良い装備を買うのだ。\n　　　期待しておるぞ」▽";
                fadeIn = Params.FADE_OUT1;//フェードアウト
                break;
            case Params.SCENARIO_TUTERIAL_END:
                commandObj.SetActive(true);//城のコマンドリストを表示
                clickObj.SetActive(false);//テキストクリック判定の消去
                messageObj.SetActive(false);//メッセージウィンドウを閉じる
                phase = Params.PHASE_CASTLE;//城へ
                break;
            case Params.SCENARIO_LOSE_END:
                sounds.GetComponents<AudioSource>()[Params.BGM].clip = (AudioClip)Resources.Load("Sounds/siro06");
                sounds.GetComponents<AudioSource>()[Params.BGM].Play();
                commandObj.SetActive(true);//城のコマンドリストを表示
                clickObj.SetActive(false);//テキストクリック判定の消去
                messageObj.SetActive(false);//メッセージウィンドウを閉じる
                phase = Params.PHASE_CASTLE;//城へ
                break;
            case Params.SCENARIO_LASTBATTLE_1:
                messageText.text = "魔王「だが、全ては無意味\n　　　さぁ、滅びるがよい」▽";
                break;
            case Params.SCENARIO_LASTBATTLE_END:
                clickObj.SetActive(false);//テキストクリック判定の消去
                selectButtons.SetActive(true);//選択ボタンのアクティブ化
                messageText.text = enemy.name + "が現れた！";
                phase = Params.PHASE_SELECT;
                break;
            case Params.SCENARIO_EPLOGUE_1:
                selectButtons.SetActive(false);//選択ボタンの非アクティブ化
                messageText.text = "魔王「見事だ…　だが、知識は不変ではない。\n　　　それをゆめゆめ忘れるな…」▽";
                break;
            case Params.SCENARIO_EPLOGUE_2:
                fadeIn = Params.FADE_IN2;
                messageText.text = "こうして王国に平和が戻った。\nだが、あなたは旅を続けている。▽";
                break;
            case Params.SCENARIO_EPLOGUE_3:
                messageText.text = "魔王の言葉を受け、知識を求めて\n新たな地へと向かったのである。▽";
                break;
            case Params.SCENARIO_EPLOGUE_END:
                messageText.text = "～ゲームクリア！～\nプレイしていただき、ありがとうございました。";
                break;
        }
        scenarioNo++;//次のシナリオを呼び出し
    }

    /// <summary>
    /// 戦闘処理
    /// </summary>
    /// <param name="selectNo"></param>
    public void SelectAction(int selectNo)
    {
        switch (selectNo)
        {
            case 0://戦う
                phase = Params.PHASE_QUIZ;//問題の表示と解答選択へ
                messageText.text = "あなたの攻撃！";
                break;
            case 1://防御
                if (scenarioNo == Params.SCENARIO_TUTERIAL_2)//チュートリアル中なら
                {
                    messageText.text = "王様「守っていては勝てんぞ」";
                    break;
                }
                phase = Params.PHASE_GUARD;//防御処理へ
                selectButtons.SetActive(false);//選択ボタンの非表示
                clickObj.SetActive(true);//クリック判定の復活
                messageText.text = "あなたは身構えている▽";
                break;
            case 2://薬草
                if (item[Params.ITEM_HEAL] == 0)//薬草が無い場合
                {
                    messageText.text = "薬草を持っていない…";
                    break;
                }
                sounds.GetComponents<AudioSource>()[Params.SE_HEAL].Play();//回復音
                selectButtons.SetActive(false);//選択ボタンの非表示
                clickObj.SetActive(true);//クリック判定の復活
                phase = Params.PHASE_HEAL;//回復処理へ
                SetHP(30);
                item[Params.ITEM_HEAL]--;
                messageText.text = "薬草を使った。\nHPが30回復▽";
                break;
            case 3://逃走
                if (scenarioNo == Params.SCENARIO_TUTERIAL_2)
                {
                    messageText.text = "王様「どこへ行くつもりだ？」";
                    break;
                }
                enemyObj.SetActive(false);//敵オブジェクトの非表示
                selectButtons.SetActive(false);//選択ボタンの非表示
                clickObj.SetActive(true);//クリック判定の復活
                messageText.text = "あなたは逃げ出した▽";
                phase = Params.PHASE_BATTLE_END;//逃走フェーズへ
                break;
        }
    }

    /// <summary>
    /// 自分の攻撃
    /// </summary>
    /// <returns></returns>
    public void SelfAttack(int selectNo)
    {
        questionObj.SetActive(false);//問題を閉じる
                                     
        if (int.Parse(csvDatas[quetionNo][6]) == selectNo)//解答チェック:正解
        {
            damageValue = self.attack + weaponAttack - enemy.def;//ダメージ計算
            sounds.GetComponents<AudioSource>()[Params.SE_SUCCSESS].Play();//正解音
        }
        else//解答チェック:失敗
        {
            damageValue = 0;//与ダメージは0
            sounds.GetComponents<AudioSource>()[Params.SE_FALUD].Play();//失敗音
        }
        attackPhase = 1;//攻撃エフェクトの表示
        //正解・不正解の表示
        for (int i = 0; i < Params.SELECT_BUTTON_LENGTH; i++)
        {
            if (i == int.Parse(csvDatas[quetionNo][6]))//正解
            {
                selectButtons.transform.GetChild(i).GetComponent<Image>().color = new Color(0.3f, 0.6f, 0.3f);
            }
        }
        phase = Params.PHASE_ATTACK;
        messageText.text = "あなたの攻撃！";
    }

    /// <summary>
    /// 敵の攻撃
    /// </summary>
    /// <returns></returns>
    public void EnemyAttack(bool def)
    {
        questionObj.SetActive(false);
        sounds.GetComponents<AudioSource>()[Params.SE_DAMAGE].Play();//ダメージ音
        damagePhase = 1;
        int d = enemy.attack - self.def - armorDef;
        if (def) d /= 2;//防御選択時は半減        
        if (d < 1) d = 1;//最低ダメージは１
        SetHP(-d);//HPの再表示
        phase = Params.PHASE_ENEMY_ACTION;//判定へ
        clickObj.SetActive(true);//クリック判定のアクティブ化
        if (self.hp <= 0)
        {
            fadeIn = Params.FADE_IN2;
            messageText.text = enemy.name + "の攻撃！" + d + "のダメージ！\nあなたは死んでしまった…▽";
            sounds.GetComponents<AudioSource>()[Params.BGM].clip = (AudioClip)Resources.Load("Sounds/gameover");//ゲームオーバー
            sounds.GetComponents<AudioSource>()[Params.BGM].Play();
            scenarioNo = Params.SCENARIO_LOSE_1;
            phase = Params.PHASE_TALK;
            return;
        }
        messageText.text = enemy.name + "の攻撃！" + d + "のダメージ！▽";
    }

    /// <summary>
    /// 敵の行動後処理
    /// </summary>
    public void ResetSelect()
    {
        for(int i = 0; i < Params.SELECT_BUTTON_LENGTH; i++)
        {
            selectButtons.transform.GetChild(i).GetComponent<Image>().color = new Color(0.3f, 0.2f, 0.2f);
        }
        selectButtons.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "戦う";
        selectButtons.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = "守る";
        selectButtons.transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "薬草("+item[0]+")";
        selectButtons.transform.GetChild(3).transform.GetChild(0).GetComponent<Text>().text = "逃げる";
        phase = Params.PHASE_SELECT;//行動選択
        clickObj.SetActive(false);
    }

    /// <summary>
    /// 戦闘終了判定
    /// </summary>
    public bool CheckBattleEnd()
    {
        turn++;//ターン数加算
        if (scenarioNo == Params.SCENARIO_TUTERIAL_2 && turn == 3)//チュートリアル中は3ターン目に戦闘終了
        {
            enemyObj.SetActive(false);//敵画像の非アクティブ化
            messageText.text = "王様「それまで！」▽";
            phase = Params.PHASE_TALK;//戦闘終了
            return true;
        }
        messageText.text = "";
        messageObj.transform.GetChild(2).gameObject.SetActive(true);//戦闘ボタンのアクティブ
        return false;//戦闘継続
    }

    /// <summary>
    /// 戦闘勝利メッセージ
    /// </summary>
    /// <returns></returns>
    public void GetBattleEndMessage()
    {
        selectButtons.SetActive(false);
        clickObj.SetActive(true);
        self.exp += enemy.exp;

        sounds.GetComponents<AudioSource>()[Params.BGM].clip = (AudioClip)Resources.Load("Sounds/elepijingle");
        sounds.GetComponents<AudioSource>()[Params.BGM].Play();

        //レベルアップのチェック
        if (self.exp > self.level * self.level * 5)
        {
            phase = Params.PHASE_LEVELUP;//レベルアップフェーズへ
        }
        else//レベルアップしていない場合は終了へ
        {
            phase = Params.PHASE_BATTLE_END;
        }
        mapObj.transform.GetChild(5 - enemy.no).gameObject.SetActive(true);//次ステージの解放
        self.gold += enemy.gold;//所持金を加算
        SetGold();//所持金の再表示
        messageText.text = enemy.name +"を倒した！\n" + enemy.exp + "の経験値と"+enemy.gold+" Gを獲得！▽";
    }

    /// <summary>
    /// レベルアップ処理
    /// </summary>
    /// <returns></returns>
    public void LevelUp()
    {
        self.LevelUp();//レベルアップ
        SetStatus();//ステータス表示への反映
        SetHP(0);//HPの反映（回復はしない）
        phase = Params.PHASE_BATTLE_END;
        sounds.GetComponents<AudioSource>()[Params.SE_LEVELUP].Play();//レベルアップ
        messageText.text = "レベルアップ！\n各種能力が上がり、レベルが" +self.level+"になった！";
    }

    /// <summary>
    /// 戦闘終了後処理
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
    /// 城を出る
    /// </summary>
    public void ExitTown()
    {
        fadeIn = Params.FADE_IN1;//フェードイン開始
        battleObj.transform.GetChild(1).gameObject.SetActive(false);//王様の非アクティブ
        sounds.GetComponents<AudioSource>()[Params.BGM].clip = (AudioClip)Resources.Load("Sounds/map");
        sounds.GetComponents<AudioSource>()[Params.BGM].Play();
        phase = Params.PHASE_MAP;//マップへ
    }

    /// <summary>
    /// 城に入る
    /// </summary>
    public void EnterTown()
    {
        fadeIn = Params.FADE_IN1;//フェードイン開始
        phase = Params.PHASE_CASTLE;//城へ
        sounds.GetComponents<AudioSource>()[Params.BGM].clip = (AudioClip)Resources.Load("Sounds/siro06");
        sounds.GetComponents<AudioSource>()[Params.BGM].Play();
        battleObj.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite =
            Resources.Load<Sprite>("Graph/b_cas03a_m_240");
    }

    /// <summary>
    /// ステータスの再表示
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
    /// HP・レベルの再表示
    /// </summary>
    /// <param name="value"></param>
    public void SetHP(int value)
    {
        self.hp += value;
        if (self.hp > self.maxHp) self.hp = self.maxHp;//最大HPは超えない
        if (self.hp < 0) self.hp = 0;//0より低くはならない
        hpObj.transform.GetChild(1).GetComponent<Text>().text = self.hp + "/" + self.maxHp +"\n" + self.level;
    }

    /// <summary>
    /// お金の再表示
    /// </summary>
    public void SetGold()
    {
        goldObj.transform.GetChild(1).GetComponent<Text>().text = self.gold+" G" ;
    }

    /// <summary>
    /// アイテムの購入
    /// </summary>
    /// <param name="itemNo"></param>
    /// <returns></returns>
    public bool BuyItem(int itemNo)
    {
        if (self.gold < Params.itemPrice[itemNo]) return false;
        switch (itemNo)//アイテム種で更新対象を決定
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
        self.gold -= Params.itemPrice[itemNo];//所持金の減算
        SetGold();//所持金の再表示
        return true;
    }

    /// <summary>
    /// 宿での休息
    /// </summary>
    public void RestInn()
    {
        if (self.gold < Params.REST_COST)//宿代が足りない場合
        {
            return;
        }
        self.gold -= Params.REST_COST;
        SetGold();
        sounds.GetComponents<AudioSource>()[Params.SE_HEAL].Play();//回復音
        SetHP(99);//全回復
    }

    /// <summary>
    /// アイテム選択時の増減量計算
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
    /// 時間経過処理
    /// </summary>
    /// <returns></returns>
    public void Update()
    {
        //フェードイン処理
        switch (fadeIn)
        {
            case Params.FADE_IN1:
                fadeObj.SetActive(true);
                alpha += 0.1f;
                fadeObj.GetComponent<Image>().color = new Color(0, 0, 0, alpha);
                if (alpha > 1.4f)
                {
                    fadeIn = Params.FADE_OUT1;
                    switch (phase)//フェーズで分岐
                    {
                        case Params.PHASE_CASTLE:
                            battleObj.SetActive(true);
                            commandObj.SetActive(true);
                            mapObj.SetActive(false);
                            battleObj.transform.GetChild(1).gameObject.SetActive(true);//王様のアクティブ
                            break;
                        case Params.PHASE_MAP:
                            battleObj.SetActive(false);
                            commandObj.SetActive(false);
                            mapObj.SetActive(true);
                            break;
                        case Params.PHASE_BATTLE_START:
                            mapObj.SetActive(false);//マップの非表示
                            battleObj.SetActive(true);//戦闘オブジェクトの表示
                            battleObj.transform.GetChild(3).gameObject.SetActive(true);//敵画像のアクティブ化
                            messageObj.SetActive(true);//メッセージの表示
                            if (enemy.no == 6)//魔王戦
                            {
                                scenarioNo = Params.SCENARIO_LASTBATTLE_1;
                                turn = 0;//戦闘ターンのリセット
                                fadeIn = Params.FADE_IN1;//フェードの開始
                                phase = Params.PHASE_TALK;//魔王戦は会話から開始
                                clickObj.SetActive(true);
                                messageText.text = "魔王「よくぞここまで辿り着いた」▽";
                                break;
                            }
                            selectButtons.SetActive(true);//選択ボタンのアクティブ化
                            messageText.text = enemy.name + "が現れた！";
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
            case Params.FADE_IN2://敗北時フェードイン
                fadeObj.SetActive(true);
                alpha += 0.1f;
                fadeObj.GetComponent<Image>().color = new Color(0, 0, 0, alpha);
                break;
            case Params.FADE_OUT2://敗北時フェードイン
                fadeObj.SetActive(true);
                alpha -= 0.05f;
                fadeObj.GetComponent<Image>().color = new Color(0, 0, 0, alpha);
                break;
        }

        //攻撃エフェクト
        if (attackPhase == 1)
        {
            attackObj.SetActive(true);
            attackObj.transform.position = attackFirstPosition;
            attackPhase = 2;
        }
        else if (attackPhase == 2)
        {
            attackObj.transform.position -= new Vector3(0.3f, 0.3f, 0f);//攻撃エフェクトの移動
            if (Vector3.Distance(attackObj.transform.position, attackFirstPosition) > 3.3f)
            {
                if (damageValue == 0)
                {
                    sounds.GetComponents<AudioSource>()[Params.SE_MISS].Play();
                    enemyObj.SetActive(true);//敵画像のアクティブ化
                    attackPhase = 0;
                    attackObj.SetActive(false);
                    clickObj.SetActive(true);
                    messageText.text = "不正解！　攻撃は避けられた…▽";
                }
                else
                {
                    sounds.GetComponents<AudioSource>()[Params.SE_ATTACK].Play();
                    enemyObj.SetActive(true);//敵画像のアクティブ化
                    attackPhase = 0;
                    attackObj.SetActive(false);
                    clickObj.SetActive(true);
                    enemy.hp -= damageValue;
                    if(enemy.hp <= 0)
                    {
                        phase = Params.PHASE_BATTLE_WIN;//戦闘勝利後処理へ
                        if (enemy.no == 6)//魔王戦の場合はオブジェクトを消さず会話へ
                        {
                            scenarioNo = Params.SCENARIO_EPLOGUE_1;//エピローグの開始
                            phase = Params.PHASE_TALK;//会話フェーズへ
                        }
                        else
                        {
                            enemyObj.SetActive(false);//敵オブジェクトの消滅
                        }
                    }
                    messageText.text = "正解！　" +enemy.name + "に" + damageValue + "のダメージ！▽";
                }
            }
            else if (Vector3.Distance(attackObj.transform.position, attackFirstPosition) > 2.5f)
            {
                enemyObj.SetActive(false);//敵画像の非アクティブ化
            }
        }

        //ダメージ演出
        if (damagePhase == 1)
        {
            Camera.main.transform.position += new Vector3(0.03f, 0.03f, -10);
            if (Camera.main.transform.position.y > 0.1f) damagePhase = 2;
        }
        else if(damagePhase == 2)//ダメージ演出終了
        {
            Camera.main.transform.position -= new Vector3(0.03f, 0.03f, -10);
            if(Camera.main.transform.position.y < 0f){
                damagePhase = 0;
                Camera.main.transform.position = new Vector3(0, 0, -10);
            }
        }

        //戦闘フェーズ
        if (phase == Params.PHASE_QUIZ)//出題フェーズなら
        {
            sounds.GetComponents<AudioSource>()[Params.SE_QUESTION].Play();//出題音
            quetionNo = Random.Range(0,csvDatas.Count);//ランダムで問題を選択
            questionObj.SetActive(true);//問題オブジェクトの表示
            questionObj.transform.GetChild(1).gameObject.GetComponent<Text>().text = csvDatas[quetionNo][1];
            selectButtons.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = csvDatas[quetionNo][2];//後で変更
            selectButtons.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = csvDatas[quetionNo][3];//後で変更
            selectButtons.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = csvDatas[quetionNo][4];//後で変更
            selectButtons.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = csvDatas[quetionNo][5];//後で変更
            time = Params.ANSWER_TIME;//回答時間の初期化
            phase = Params.PHASE_ANSER;//回答フェーズへ
        }
        else if(phase == Params.PHASE_ANSER)//回答フェーズなら
        {
            if (time <= 0) SelfAttack(4);//時間切れは攻撃に失敗
            else if (enemy.speed - self.speed <= 0) time--;//速度差がありすぎる場合は一定
            else time -= enemy.speed - self.speed;//時間経過
            timeObj.transform.localScale = new Vector3(time * 1.0f/Params.ANSWER_TIME * timeLength, 1,1);
        }
    }

}
