// Decompiled with JetBrains decompiler
// Type: Tower029RankingPlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Tower029RankingPlayer : Tower029RankingStatus
{
  [SerializeField]
  private Tower029RankingPlayer.InsBoard[] rankBoards_ = new Tower029RankingPlayer.InsBoard[5];
  [SerializeField]
  private GameObject topInfo_;
  [SerializeField]
  private GameObject lnkUnit_;
  [SerializeField]
  private GameObject lnkHonor_;
  [SerializeField]
  private UILabel txtName_;
  [SerializeField]
  private UILabel txtLevel_;
  [SerializeField]
  private UILabel txtRank_;
  [SerializeField]
  private bool isOwn_;
  private Tower029RankingPlayer.Data initData_;

  public void initialize(
    GameObject prefabUnitIcon,
    Tower029RankingStatus.StatusEnum estatus,
    Tower029RankingPlayer.Data data = null,
    int?[] points = null)
  {
    this.initData_ = data;
    if (data == null)
    {
      this.changeDrawRank();
      this.changeDrawStatus(estatus);
      this.lnkUnit_.SetActive(false);
      this.lnkHonor_.SetActive(false);
    }
    else
    {
      this.txtName_.SetTextLocalize(data.name_);
      this.txtLevel_.SetTextLocalize(data.level_);
      this.changeDrawRank(data.rank_);
      this.changeDrawStatus(estatus, data.point_);
      if (points != null)
        this.setStatusValues(points);
      prefabUnitIcon.Clone(this.lnkUnit_.transform);
      this.lnkUnit_.SetActive(false);
      this.lnkHonor_.SetActive(false);
    }
  }

  public IEnumerator coInitImage()
  {
    if (this.initData_ != null)
    {
      Transform transform = this.lnkUnit_.transform.GetChildren().FirstOrDefault<Transform>();
      UnitIcon uniticon = Object.op_Inequality((Object) transform, (Object) null) ? ((Component) transform).GetComponent<UnitIcon>() : (UnitIcon) null;
      PlayerUnit playerunit = this.initData_.leaderUnit_.playerUnit_;
      IEnumerator e;
      if (Object.op_Inequality((Object) uniticon, (Object) null) && playerunit != (PlayerUnit) null)
      {
        e = uniticon.setSimpleUnit(playerunit);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        uniticon.setLevelText(playerunit);
        uniticon.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
        this.lnkUnit_.SetActive(true);
      }
      else
        this.lnkUnit_.SetActive(false);
      Future<Sprite> ldemblem = EmblemUtility.LoadEmblemSprite(this.initData_.emblem_);
      e = ldemblem.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (ldemblem.HasResult)
      {
        this.lnkHonor_.GetComponent<UI2DSprite>().sprite2D = ldemblem.Result;
        this.lnkHonor_.SetActive(true);
      }
      else
        this.lnkHonor_.SetActive(false);
      ldemblem = (Future<Sprite>) null;
      this.initData_ = (Tower029RankingPlayer.Data) null;
    }
  }

  public void changeStatus(Tower029RankingStatus.StatusEnum estatus)
  {
    this.changeDrawStatus(estatus);
  }

  public void resetStatusValuse(int?[] values) => this.setStatusValues(values);

  public void changeDrawRank(int rank = 0)
  {
    bool flag1 = false;
    Tower029RankingPlayer.RankEnum rankEnum;
    switch (rank)
    {
      case 0:
        rankEnum = this.isOwn_ ? Tower029RankingPlayer.RankEnum.Other : Tower029RankingPlayer.RankEnum.None;
        flag1 = this.isOwn_;
        break;
      case 1:
        rankEnum = Tower029RankingPlayer.RankEnum._1st;
        break;
      case 2:
        rankEnum = Tower029RankingPlayer.RankEnum._2nd;
        break;
      case 3:
        rankEnum = Tower029RankingPlayer.RankEnum._3rd;
        break;
      default:
        rankEnum = Tower029RankingPlayer.RankEnum.Other;
        flag1 = true;
        break;
    }
    if (this.rankBoards_ != null)
    {
      foreach (Tower029RankingPlayer.InsBoard rankBoard in this.rankBoards_)
      {
        if (rankBoard != null && !Object.op_Equality((Object) rankBoard.top_, (Object) null))
        {
          bool flag2 = rankBoard.rank_ == rankEnum;
          rankBoard.top_.SetActive(flag2);
          if (flag2)
            rankBoard.drawDecimal(rank);
        }
      }
    }
    if (Object.op_Inequality((Object) this.topInfo_, (Object) null))
      this.topInfo_.SetActive(rankEnum != 0);
    if (!Object.op_Inequality((Object) this.txtRank_, (Object) null))
      return;
    ((Component) this.txtRank_).gameObject.SetActive(flag1);
    if (!flag1)
      return;
    if (rank <= TowerUtil.MaxRankingNum)
    {
      Hashtable hashtable;
      if (rank <= 0)
      {
        hashtable = new Hashtable()
        {
          {
            (object) nameof (rank),
            (object) Consts.GetInstance().COMMON_NOVALUE
          }
        };
      }
      else
      {
        hashtable = new Hashtable();
        hashtable.Add((object) nameof (rank), (object) rank);
      }
      Hashtable args = hashtable;
      this.txtRank_.SetTextLocalize(Consts.Format(Consts.GetInstance().TOWER_RANKING_RANK, (IDictionary) args));
    }
    else
      this.txtRank_.SetTextLocalize(Consts.GetInstance().TOWER_RANKING_OUTOFRANK);
  }

  public class Data
  {
    public int rank_ { get; private set; }

    public string name_ { get; private set; }

    public int level_ { get; private set; }

    public Tower029RankingPlayer.Data.SimpleUnit leaderUnit_ { get; private set; }

    public int emblem_ { get; private set; }

    public int? point_ { get; private set; }

    public Data(
      WebAPI.Response.TowerScoreRankingSpeedSpeed_rankings dat)
    {
      this.init(dat.rank, dat.player_name, dat.player_level, dat.leader_unit_id, dat.leader_unit_level, dat.player_emblem_id, dat.leader_unit_job_id, new int?(dat.score));
    }

    public Data(
      WebAPI.Response.TowerScoreRankingTechnicalTechnical_rankings dat)
    {
      this.init(dat.rank, dat.player_name, dat.player_level, dat.leader_unit_id, dat.leader_unit_level, dat.player_emblem_id, dat.leader_unit_job_id, new int?(dat.score));
    }

    public Data(
      WebAPI.Response.TowerScoreRankingDamageDamage_rankings dat)
    {
      this.init(dat.rank, dat.player_name, dat.player_level, dat.leader_unit_id, dat.leader_unit_level, dat.player_emblem_id, dat.leader_unit_job_id, new int?(dat.score));
    }

    public Data(int rank, Player player, PlayerUnit playerUnit)
    {
      int id = playerUnit != (PlayerUnit) null ? playerUnit.unit.ID : 0;
      int leaderUnitLvl = playerUnit != (PlayerUnit) null ? playerUnit.level : 1;
      this.init(rank, player.name, player.level, id, leaderUnitLvl, player.current_emblem_id, playerUnit.job_id);
    }

    private void init(
      int rank,
      string name,
      int level,
      int leaderUnitId,
      int leaderUnitLvl,
      int emblem,
      int job_id,
      int? point = null)
    {
      this.rank_ = rank;
      this.name_ = name;
      this.level_ = level;
      this.leaderUnit_ = new Tower029RankingPlayer.Data.SimpleUnit(leaderUnitId, leaderUnitLvl, job_id);
      this.emblem_ = emblem;
      this.point_ = point;
    }

    public class SimpleUnit
    {
      public int id_ { get; private set; }

      public int level_ { get; private set; }

      public int leader_unit_job_id_ { get; private set; }

      public SimpleUnit(int id, int level, int leader_unit_job_id)
      {
        this.id_ = id;
        this.level_ = level;
        this.leader_unit_job_id_ = leader_unit_job_id;
      }

      public PlayerUnit playerUnit_
      {
        get
        {
          UnitUnit unit;
          if (!MasterData.UnitUnit.TryGetValue(this.id_, out unit))
            return (PlayerUnit) null;
          PlayerUnit byUnitunit = PlayerUnit.create_by_unitunit(unit);
          byUnitunit.job_id = this.leader_unit_job_id_;
          byUnitunit.level = this.level_;
          return byUnitunit;
        }
      }
    }
  }

  public enum RankEnum
  {
    None,
    _1st,
    _2nd,
    _3rd,
    Other,
    Num,
  }

  [Serializable]
  public class DigitSprite
  {
    public GameObject[] number_ = new GameObject[10];
  }

  [Serializable]
  public class DecimalSprite
  {
    public GameObject top_;
    public Tower029RankingPlayer.DigitSprite[] digits_;
  }

  [Serializable]
  public class InsBoard
  {
    public Tower029RankingPlayer.RankEnum rank_ = Tower029RankingPlayer.RankEnum.Num;
    public GameObject top_;
    public Tower029RankingPlayer.DecimalSprite[] decimal_;
    private const int RADIX_DECIMAL = 10;

    public void drawDecimal(int num)
    {
      if (this.decimal_ == null || this.decimal_.Length == 0)
        return;
      int num1 = (int) Mathf.Pow(10f, (float) this.decimal_.Length) - 1;
      if (num > num1)
        num = num1;
      int num2 = num.ToString().Length - 1;
      for (int index = 0; index < this.decimal_.Length; ++index)
      {
        Tower029RankingPlayer.DecimalSprite decimalSprite = this.decimal_[index];
        if (decimalSprite != null && !Object.op_Equality((Object) decimalSprite.top_, (Object) null))
        {
          if (num2 == index)
            decimalSprite.top_.SetActive(this.drawDecimal(decimalSprite.digits_, num));
          else
            decimalSprite.top_.SetActive(false);
        }
      }
    }

    private bool drawDecimal(Tower029RankingPlayer.DigitSprite[] decsp, int num)
    {
      if (decsp == null || decsp.Length == 0)
        return false;
      int length = num.ToString().Length;
      if (decsp.Length != length)
        return false;
      int index = length - 1;
      int num1 = (int) Mathf.Pow(10f, (float) index);
      for (; index >= 0; --index)
      {
        ((IEnumerable<GameObject>) decsp[index].number_).ToggleOnce(num / num1);
        num %= num1;
        num1 /= 10;
      }
      return true;
    }
  }
}
