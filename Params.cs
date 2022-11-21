using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Params 
{
    //ゲームバランス各種
    public const int ANSWER_TIME = 250;//基本回答時間
    public const int REST_COST = 50;//休息費用（Unity側も変更が必要）

    //基礎システム
    public const int SELECT_BUTTON_LENGTH = 4;//回答ボタン長さ

    public const int FADE_IN1 = 1;//フェードインからアウトを即座に行う
    public const int FADE_OUT1 = 2;//フェードアウトを即座に行う
    public const int FADE_IN2 = 3;//フェードインから暗転を維持（敗北時のみ使用）
    public const int FADE_OUT2 = 4;//フェードアウトをゆっくり

    //シナリオ番号
    public const int SCENARIO_PROLOGUE_1 = 0;//プロローグ
    public const int SCENARIO_TUTERIAL_1 = 1;//チュートリアル
    public const int SCENARIO_TUTERIAL_2 = 2;
    public const int SCENARIO_TUTERIAL_3 = 3;
    public const int SCENARIO_TUTERIAL_4 = 4;
    public const int SCENARIO_TUTERIAL_5 = 5;
    public const int SCENARIO_TUTERIAL_6 = 6;
    public const int SCENARIO_TUTERIAL_END = 7;

    public const int SCENARIO_LOSE_1 = 20;//戦闘敗北
    public const int SCENARIO_LOSE_2 = 21;
    public const int SCENARIO_LOSE_END = 22;

    public const int SCENARIO_LASTBATTLE_1 = 30;//最終戦闘
    public const int SCENARIO_LASTBATTLE_END = 31;

    public const int SCENARIO_EPLOGUE_1 = 40;//最終戦闘勝利
    public const int SCENARIO_EPLOGUE_2 = 41;
    public const int SCENARIO_EPLOGUE_3 = 42;
    public const int SCENARIO_EPLOGUE_END = 43;

    //音
    public const int SE_SUCCSESS = 0;//回答当たり音
    public const int SE_FALUD = 1;//回答はずれ音
    public const int SE_ATTACK = 2;//攻撃音
    public const int SE_MISS = 3;//攻撃外れ音
    public const int SE_DAMAGE = 4;//ダメージ音
    public const int SE_COIN = 5;//コイン音
    public const int BGM = 6;//BGM
    public const int SE_QUESTION = 7;//問題音
    public const int SE_ENCOUNT = 8;//エンカウント音
    public const int SE_HEAL = 9;//回復音
    public const int SE_LEVELUP = 10;//レベルアップ音

    //フェーズ（処理順）
    public const int PHASE_TALK = 0;//会話フェーズ
    public const int PHASE_CASTLE = 1;//城フェーズ
    public const int PHASE_MAP = 2;//マップ行動フェーズ
    public const int PHASE_BATTLE_START = 3;//戦闘開始フェーズ
    public const int PHASE_SELECT = 4;//行動選択フェーズ(攻撃:2 防御: 回復: 逃走:）
    public const int PHASE_QUIZ = 5;//出題フェーズ
    public const int PHASE_ANSER = 6;//回答フェーズ
    public const int PHASE_ATTACK = 7;//攻撃フェーズ
    public const int PHASE_ENEMY_ACTION = 8;//敵の行動フェーズ
    public const int PHASE_GUARD = 9;//防御フェーズ
    public const int PHASE_HEAL = 10;//回復フェーズ
    public const int PHASE_BATTLE_WIN = 11;//戦闘勝利フェーズ
    public const int PHASE_LEVELUP = 12;//レベルアップフェーズ
    public const int PHASE_BATTLE_END = 13;//戦闘終了フェーズ
    public const int PHASE_BATTLE_LOSE = 14;//戦闘敗北フェーズ
    public const int PHASE_BATTLE_ESCAPE = 15;//逃走フェーズ

    //アイテム各種
    public const int ITEM_HEAL = 0;//薬草
    public const int ITEM_SWORD = 1;//剣
    public const int ITEM_ARMOR = 2;//鎧
    public const int ITEM_BOOTS = 3;//靴

    public const int ITEM_LENGTH = 4;//アイテム長さ

    public static string[] itemName = { "なし", "鉄の剣", "鋼の剣", "竜の剣", "鉄の鎧", "鋼の鎧", "サンダル", "ブーツ" };//アイテム名称
    public static int[] itemPower = { 0, 3, 8, 15, 8, 15, 4, 10 };//アイテム上昇値
    public static int[] itemPrice = { 0, 200, 800, 2200, 700, 1900, 180, 900, 50 };//アイテム価格

    
}
