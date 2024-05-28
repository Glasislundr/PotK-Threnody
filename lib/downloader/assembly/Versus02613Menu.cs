// Decompiled with JetBrains decompiler
// Type: Versus02613Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Versus02613Menu : BackButtonMenuBase
{
  [SerializeField]
  protected UILabel txt_RecordTitleString;
  [SerializeField]
  protected UILabel txt_RecordString01;
  [SerializeField]
  protected UILabel txt_RecordString02;
  [SerializeField]
  protected UILabel txt_RecordString03;
  [SerializeField]
  protected UILabel txt_RecordString04;
  [SerializeField]
  protected UILabel txt_RecordString05;
  [SerializeField]
  protected UILabel txt_RecordString06;
  [SerializeField]
  protected UILabel txt_RecordString07;
  [SerializeField]
  protected UILabel txt_RecordString08;
  [SerializeField]
  protected UILabel txt_RecordString09;
  [SerializeField]
  protected UILabel txt_RecordString10;
  [SerializeField]
  protected UILabel txt_RecordString11;
  [SerializeField]
  protected UILabel txt_RecordString12;
  [SerializeField]
  protected UILabel txt_RandomRecordNum01;
  [SerializeField]
  protected UILabel txt_RandomRecordNum02;
  [SerializeField]
  protected UILabel txt_RandomRecordNum03;
  [SerializeField]
  protected UILabel txt_RandomRecordNum04;
  [SerializeField]
  protected UILabel txt_RandomRecordNum05;
  [SerializeField]
  protected UILabel txt_RandomRecordNum06;
  [SerializeField]
  protected UILabel txt_RandomRecordNum07;
  [SerializeField]
  protected UILabel txt_RandomRecordNum08;
  [SerializeField]
  protected UILabel txt_RandomRecordNum09;
  [SerializeField]
  protected UILabel txt_RandomRecordNum10;
  [SerializeField]
  protected UILabel txt_RandomRecordNum11;
  [SerializeField]
  protected UILabel txt_RandomRecordNum12;
  [SerializeField]
  protected UILabel txt_SeasonTitleString;
  [SerializeField]
  protected UILabel txt_SeasonString01;
  [SerializeField]
  protected UILabel txt_SeasonString02;
  [SerializeField]
  protected UILabel txt_SeasonString03;
  [SerializeField]
  protected UILabel txt_SeasonString04;
  [SerializeField]
  protected UILabel txt_SeasonString05;
  [SerializeField]
  protected UILabel txt_SeasonRecordNum01;
  [SerializeField]
  protected UILabel txt_SeasonRecordNum02;
  [SerializeField]
  protected UILabel txt_SeasonRecordNum03;
  [SerializeField]
  protected UILabel txt_SeasonRecordNum04;
  [SerializeField]
  protected UILabel txt_SeasonRecordNum05;
  [SerializeField]
  protected GameObject dir_Ranking;
  [SerializeField]
  protected UILabel txt_RankingNum01;
  [SerializeField]
  protected UILabel txt_RankingNum02;
  [SerializeField]
  protected UILabel txt_RankingNum03;
  [SerializeField]
  protected UILabel txt_RankingNum04;
  [SerializeField]
  protected UILabel txt_RankingNum05;
  [SerializeField]
  protected UILabel txt_RankingNum06;
  [SerializeField]
  protected UILabel txt_RankingNum07;
  [SerializeField]
  protected UILabel txt_RankingNum08;
  [SerializeField]
  protected UILabel txt_RankingNum09;
  [SerializeField]
  protected UILabel txt_RankingNum10;
  [SerializeField]
  protected UILabel txt_RankingNum11;
  [SerializeField]
  protected UILabel txt_RankingNum12;
  [SerializeField]
  protected UILabel txt_RankingString01;
  [SerializeField]
  protected UILabel txt_RankingString02;
  [SerializeField]
  protected UILabel txt_RankingString03;
  [SerializeField]
  protected UILabel txt_RankingString04;
  [SerializeField]
  protected UILabel txt_RankingString05;
  [SerializeField]
  protected UILabel txt_RankingString06;
  [SerializeField]
  protected UILabel txt_RankingString07;
  [SerializeField]
  protected UILabel txt_RankingString08;
  [SerializeField]
  protected UILabel txt_RankingString09;
  [SerializeField]
  protected UILabel txt_RankingString10;
  [SerializeField]
  protected UILabel txt_RankingString11;
  [SerializeField]
  protected UILabel txt_RankingString12;
  [SerializeField]
  private UIScrollView scroll;
  [SerializeField]
  private UIGrid grid;
  [SerializeField]
  private UIButton ibtnEanedTitle;
  [SerializeField]
  private UIButton ibtnRecords;
  private Versus02613Scene.BootParam param_;

  public IEnumerator Initialize(Versus02613Scene.BootParam param)
  {
    this.param_ = param;
    this.txt_RecordTitleString.text = Consts.GetInstance().PVP_CLASS_RECORD_TITLE;
    this.txt_RecordString01.text = Consts.GetInstance().PVP_RECORD_ENTRY;
    this.txt_RecordString02.text = Consts.GetInstance().PVP_RECORD_WIN;
    this.txt_RecordString03.text = Consts.GetInstance().PVP_RECORD_MAX_CONSECUTIVE_WIN;
    this.txt_RecordString04.text = Consts.GetInstance().PVP_RECORD_CURRENT_CONSECUTIVE_WIN;
    this.txt_RecordString05.text = Consts.GetInstance().PVP_RECORD_LOSS;
    this.txt_RecordString06.text = Consts.GetInstance().PVP_RECORD_CURRENT_CONSECUTIVE_LOSS;
    this.txt_RecordString07.text = Consts.GetInstance().PVP_RECORD_DRAW;
    this.txt_RecordString08.text = Consts.GetInstance().PVP_RECORD_DISCONNECTED;
    this.txt_RecordString09.text = Consts.GetInstance().PVP_RECORD_EXCELLENT_WIN;
    this.txt_RecordString10.text = Consts.GetInstance().PVP_RECORD_GREAT_WIN;
    this.txt_RecordString11.text = Consts.GetInstance().PVP_RECORD_POINT_WIN;
    this.txt_RecordString12.text = Consts.GetInstance().PVP_RECORD_BEST_CLASS;
    this.txt_RandomRecordNum01.SetTextLocalize(param.current.info.pvp_record.entry);
    this.txt_RandomRecordNum02.SetTextLocalize(param.current.info.pvp_record.win);
    this.txt_RandomRecordNum03.SetTextLocalize(param.current.info.pvp_record.max_consecutive_win);
    this.txt_RandomRecordNum04.SetTextLocalize(param.current.info.pvp_record.current_consecutive_win);
    this.txt_RandomRecordNum05.SetTextLocalize(param.current.info.pvp_record.loss);
    this.txt_RandomRecordNum06.SetTextLocalize(param.current.info.pvp_record.current_consecutive_loss);
    this.txt_RandomRecordNum07.SetTextLocalize(param.current.info.pvp_record.draw);
    this.txt_RandomRecordNum08.SetTextLocalize(param.current.info.pvp_record.disconnected);
    this.txt_RandomRecordNum09.SetTextLocalize(param.current.info.pvp_record.excellent_win);
    this.txt_RandomRecordNum10.SetTextLocalize(param.current.info.pvp_record.great_win);
    this.txt_RandomRecordNum11.SetTextLocalize(param.current.info.pvp_record.point_win);
    this.txt_RandomRecordNum12.SetTextLocalize(MasterData.PvpClassKind[param.current.bestClass].name);
    this.txt_SeasonTitleString.text = Consts.GetInstance().PVP_CLASS_SEASON_TITLE;
    this.txt_SeasonString01.text = Consts.GetInstance().PVP_CLASS_SEASON_TITLE_VALUE;
    this.txt_SeasonString02.text = Consts.GetInstance().PVP_CLASS_SEASON_PROMOTION;
    this.txt_SeasonString03.text = Consts.GetInstance().PVP_CLASS_SEASON_RESIDUAL;
    this.txt_SeasonString04.text = Consts.GetInstance().PVP_CLASS_SEASON_DEMOTION;
    this.txt_SeasonString05.text = Consts.GetInstance().PVP_CLASS_SEASON_CONSECUTIVE_TITLE;
    this.txt_SeasonRecordNum01.SetTextLocalize(param.current.info.season_title_count);
    this.txt_SeasonRecordNum02.SetTextLocalize(param.current.info.season_class_up);
    this.txt_SeasonRecordNum03.SetTextLocalize(param.current.info.season_class_stay);
    this.txt_SeasonRecordNum04.SetTextLocalize(param.current.info.season_class_down);
    this.txt_SeasonRecordNum05.SetTextLocalize(param.current.info.season_consecutive_title);
    if (Player.Current.IsClassMatchShowRanking())
    {
      this.dir_Ranking.SetActive(true);
      this.txt_RankingString01.text = Consts.GetInstance().PVP_CLASS_RANKING_MOST;
      this.txt_RankingString02.text = Consts.GetInstance().PVP_CLASS_RANKING_1;
      this.txt_RankingString03.text = Consts.GetInstance().PVP_CLASS_RANKING_3;
      this.txt_RankingString04.text = Consts.GetInstance().PVP_CLASS_RANKING_10;
      this.txt_RankingString05.text = Consts.GetInstance().PVP_CLASS_RANKING_30;
      this.txt_RankingString06.text = Consts.GetInstance().PVP_CLASS_RANKING_100;
      this.txt_RankingString07.text = Consts.GetInstance().PVP_CLASS_RANKING_1000;
      this.txt_RankingString08.text = Consts.GetInstance().PVP_CLASS_RANKING_TOP;
      this.txt_RankingString09.text = Consts.GetInstance().PVP_CLASS_RANKING_TOP3;
      this.txt_RankingString10.text = Consts.GetInstance().PVP_CLASS_RANKING_TOP10;
      this.txt_RankingString11.text = Consts.GetInstance().PVP_CLASS_RANKING_TOP30;
      this.txt_RankingString12.text = Consts.GetInstance().PVP_CLASS_RANKING_TOP100;
      this.txt_RankingNum01.SetTextLocalize(param.current.info.best_ranking);
      this.txt_RankingNum02.SetTextLocalize(param.current.info.top);
      this.txt_RankingNum03.SetTextLocalize(param.current.info.top3);
      this.txt_RankingNum04.SetTextLocalize(param.current.info.top10);
      this.txt_RankingNum05.SetTextLocalize(param.current.info.top30);
      this.txt_RankingNum06.SetTextLocalize(param.current.info.top100);
      this.txt_RankingNum07.SetTextLocalize(param.current.info.top1000);
      this.txt_RankingNum08.SetTextLocalize(param.current.info.consecutive_top);
      this.txt_RankingNum09.SetTextLocalize(param.current.info.consecutive_top3);
      this.txt_RankingNum10.SetTextLocalize(param.current.info.consecutive_top10);
      this.txt_RankingNum11.SetTextLocalize(param.current.info.consecutive_top30);
      this.txt_RankingNum12.SetTextLocalize(param.current.info.consecutive_top100);
    }
    else
      this.dir_Ranking.SetActive(false);
    this.grid.Reposition();
    this.scroll.ResetPosition();
    ((UIButtonColor) this.ibtnRecords).isEnabled = param.current.info.has_pvp_battle_history;
    yield break;
  }

  public void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    this.param_.pop();
    this.backScene();
  }

  public override void onBackButton() => this.IbtnBack();

  public void onIbtnEanedTitle()
  {
    if (this.IsPushAndSet())
      return;
    Title0241Scene.ChangeScene00241(true, this.param_.current.playerId);
  }

  public void onIbtnRecords()
  {
    if (this.IsPushAndSet())
      return;
    Versus026DirWinLossRecordsScene.ChangeScene(true, this.param_);
  }
}
