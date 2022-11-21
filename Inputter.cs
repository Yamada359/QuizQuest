using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ユーザーからの入力全般
/// </summary>
public class Inputter : MonoBehaviour
{
    //各種Unityオブジェクト
    [SerializeField] GameObject mainObj;
    private Main main;//メインへの呼び出し用

    /// <summary>
    /// 起動時処理
    /// </summary>
    private void Start()
    {
        main = mainObj.GetComponent<Main>();//メインへの呼び出し用へ呼び出しを簡略化
    }

    /// <summary>
    /// メッセージウィンドウのクリック
    /// </summary>
    public void ClickText()
    {
        switch (main.GetPhase())//フェーズによる分岐
        {
            case Params.PHASE_BATTLE_START:
                main.ActiveSelectButtons(true);//戦闘ボタンのアクティブ化
                break;
            case Params.PHASE_ATTACK:
                main.EnemyAttack(false);//敵の攻撃
                main.ActiveSelectButtons(false);//戦闘ボタンの非アクティブ化
                break;
            case Params.PHASE_ENEMY_ACTION:
                if (main.CheckBattleEnd())
                    break;//戦闘継続・終了のチェック
                main.ResetSelect();//択の初期化
                break;
            case Params.PHASE_GUARD:
                main.EnemyAttack(true);//敵の攻撃（防御あり）
                break;
            case Params.PHASE_HEAL://回復を選択
                main.EnemyAttack(false);//敵の攻撃へ（防御なし）
                break;
            case Params.PHASE_BATTLE_WIN://戦闘に勝利
                main.GetBattleEndMessage();//勝利メッセージの取得へ
                break;
            case Params.PHASE_TALK://会話フェーズ
            case Params.PHASE_BATTLE_LOSE://戦闘に敗北
                main.ScenarioNext();//次テキストの確認
                break;
            case Params.PHASE_LEVELUP://レベルアップフェーズ
                main.LevelUp();//レベルアップ処理
                break;
            case Params.PHASE_BATTLE_END://戦闘終了フェーズ
                main.EndBattle();
                break;
        }
    }

    /// <summary>
    /// 行動ボタンのクリック（0～3）
    /// </summary>
    /// <param name="i"></param>
    public void ClickSelectButton(int i)
    {
        if(main.GetPhase() == Params.PHASE_SELECT)//行動選択時
            main.SelectAction(i);//ボタン番号の受け渡し
        if (main.GetPhase() == Params.PHASE_ANSER)//解答選択時
            main.SelfAttack(i);//ボタン番号の受け渡し
    }

    /// <summary>
    /// 城から出るボタンのクリック
    /// </summary>
    public void ClickExit()
    {
        main.ExitTown();
    }

    /// <summary>
    /// 城ボタンのクリック
    /// </summary>
    public void ClickTown()
    {
        main.EnterTown();
    }

    /// <summary>
    /// アイテムの購入（1～8）
    /// </summary>
    /// <param name="no"></param>
    public void BuyItem(int no)
    {
        main.BuyItem(no);
        main.SetStatus();
    }

    /// <summary>
    /// 休息ボタンのクリック
    /// </summary>
    public void RestInn()
    {
        main.RestInn();//休息処理
    }

    /// <summary>
    /// マップのクリック
    /// </summary>
    public void ClickMap(int no)
    {
        main.Encount(no);
    }

    /// <summary>
    /// アイテムの上にカーソルをのせる
    /// </summary>
    /// <param name="no"></param>
    public void OnMouseItem(int no)
    {
        main.OnMouseItem(no);
    }

}
