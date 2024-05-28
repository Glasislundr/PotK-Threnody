// Decompiled with JetBrains decompiler
// Type: Raid032MyRankingPlayer
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
public class Raid032MyRankingPlayer : Raid032MyRankingStatus
{
  [SerializeField]
  private Raid032MyRankingPlayer.InsBoard[] rankBoards = new Raid032MyRankingPlayer.InsBoard[5];
  [SerializeField]
  private GameObject topInfo;
  [SerializeField]
  private GameObject lnkUnit;
  [SerializeField]
  private GameObject lnkHonor;
  [SerializeField]
  private UILabel txtName;
  [SerializeField]
  private UILabel txtLevel;
  [SerializeField]
  private UILabel txtRank;
  [SerializeField]
  private UILabel txtGuildName;
  [SerializeField]
  private bool isOwn;
  [SerializeField]
  private GameObject selectBtn;
  [HideInInspector]
  private bool isContainerGuild;
  private string rankingPlayerID;
  private bool isInit;
  private bool isInitImage;
  private Raid032MyRankingPlayer.Data initData;

  public IEnumerator initialize(
    GameObject prefabUnitIcon,
    Raid032MyRankingStatus.StatusEnum estatus,
    bool guildContainer,
    Raid032MyRankingPlayer.Data data = null,
    int?[] points = null)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Raid032MyRankingPlayer raid032MyRankingPlayer = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    if (raid032MyRankingPlayer.isInit)
      return false;
    raid032MyRankingPlayer.initData = data;
    if (data == null)
    {
      raid032MyRankingPlayer.changeDrawRank();
      raid032MyRankingPlayer.changeDrawStatus(estatus);
      raid032MyRankingPlayer.lnkUnit.SetActive(false);
      raid032MyRankingPlayer.lnkHonor.SetActive(false);
      return false;
    }
    raid032MyRankingPlayer.rankingPlayerID = data.playerID;
    raid032MyRankingPlayer.isContainerGuild = guildContainer;
    raid032MyRankingPlayer.txtName.SetTextLocalize(data.name);
    raid032MyRankingPlayer.txtLevel.SetTextLocalize(data.level);
    raid032MyRankingPlayer.txtGuildName.SetTextLocalize(data.guildName);
    raid032MyRankingPlayer.changeDrawRank(data.rank);
    raid032MyRankingPlayer.changeDrawStatus(estatus, data.point);
    if (points != null)
      raid032MyRankingPlayer.setStatusValues(points);
    prefabUnitIcon.Clone(raid032MyRankingPlayer.lnkUnit.transform);
    raid032MyRankingPlayer.lnkUnit.SetActive(false);
    raid032MyRankingPlayer.lnkHonor.SetActive(false);
    raid032MyRankingPlayer.isInit = true;
    return false;
  }

  public IEnumerator coInitImage()
  {
    if (this.initData != null && !this.isInitImage)
    {
      Transform transform = this.lnkUnit.transform.GetChildren().FirstOrDefault<Transform>();
      UnitIcon uniticon = Object.op_Inequality((Object) transform, (Object) null) ? ((Component) transform).GetComponent<UnitIcon>() : (UnitIcon) null;
      PlayerUnit playerunit = this.initData.leaderUnit.playerUnit;
      IEnumerator e;
      if (Object.op_Inequality((Object) uniticon, (Object) null) && playerunit != (PlayerUnit) null)
      {
        e = uniticon.setSimpleUnit(playerunit);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        uniticon.setLevelText(playerunit);
        uniticon.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
        this.lnkUnit.SetActive(true);
      }
      else
        this.lnkUnit.SetActive(false);
      Future<Sprite> ldemblem = EmblemUtility.LoadEmblemSprite(this.initData.emblem);
      e = ldemblem.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (ldemblem.HasResult)
      {
        this.lnkHonor.GetComponent<UI2DSprite>().sprite2D = ldemblem.Result;
        this.lnkHonor.SetActive(true);
      }
      else
        this.lnkHonor.SetActive(false);
      ldemblem = (Future<Sprite>) null;
      this.initData = (Raid032MyRankingPlayer.Data) null;
      this.isInitImage = true;
    }
  }

  public void changeStatus(Raid032MyRankingStatus.StatusEnum estatus)
  {
    this.changeDrawStatus(estatus);
  }

  public void resetStatusValuse(int?[] values) => this.setStatusValues(values);

  public void changeDrawRank(int rank = 0)
  {
    bool flag1 = false;
    Raid032MyRankingPlayer.RankEnum rankEnum;
    switch (rank)
    {
      case 0:
        rankEnum = this.isOwn ? Raid032MyRankingPlayer.RankEnum.Other : Raid032MyRankingPlayer.RankEnum.None;
        flag1 = this.isOwn;
        this.selectBtn.SetActive(false);
        break;
      case 1:
        rankEnum = Raid032MyRankingPlayer.RankEnum._1st;
        break;
      case 2:
        rankEnum = Raid032MyRankingPlayer.RankEnum._2nd;
        break;
      case 3:
        rankEnum = Raid032MyRankingPlayer.RankEnum._3rd;
        break;
      default:
        rankEnum = Raid032MyRankingPlayer.RankEnum.Other;
        flag1 = true;
        break;
    }
    if (this.rankBoards != null)
    {
      foreach (Raid032MyRankingPlayer.InsBoard rankBoard in this.rankBoards)
      {
        if (rankBoard != null && !Object.op_Equality((Object) rankBoard.top, (Object) null))
        {
          bool flag2 = rankBoard.rank == rankEnum;
          rankBoard.top.SetActive(flag2);
          if (flag2)
            rankBoard.drawDecimal(rank);
        }
      }
    }
    if (Object.op_Inequality((Object) this.topInfo, (Object) null))
      this.topInfo.SetActive(rankEnum != 0);
    if (!Object.op_Inequality((Object) this.txtRank, (Object) null))
      return;
    ((Component) this.txtRank).gameObject.SetActive(flag1);
    if (!flag1)
      return;
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
    this.txtRank.SetTextLocalize(Consts.Format(Consts.GetInstance().GUILD_RAID_RANKING_RANK, (IDictionary) args));
  }

  public void OnBtnSelect() => this.StartCoroutine(this.ShowDetail());

  private IEnumerator ShowDetail()
  {
    if (!Singleton<CommonRoot>.GetInstance().isLoading)
    {
      Singleton<NGSceneManager>.GetInstance().changeScene("friend008_6", true, (object) this.rankingPlayerID, (object) Friend0086Scene.Mode.Friend, (object) Res.Prefabs.BackGround.MultiBackground);
      yield break;
    }
  }

  public class Data
  {
    public int rank { get; private set; }

    public string name { get; private set; }

    public int level { get; private set; }

    public string guildName { get; private set; }

    public string playerID { get; private set; }

    public Raid032MyRankingPlayer.Data.SimpleUnit leaderUnit { get; private set; }

    public int emblem { get; private set; }

    public int? point { get; private set; }

    public Data(
      WebAPI.Response.GuildraidRankingPlayerDamage_rankings_in_guild dat)
    {
      this.init(dat.rank, dat.player_name, dat.player_level, dat.player_id, dat.player_guild_name, dat.leader_unit_id, dat.leader_unit_level, dat.leader_unit_job_id, dat.player_emblem_id, new int?(dat.score));
    }

    public Data(
      WebAPI.Response.GuildraidRankingPlayerDamage_rankings_in_all dat)
    {
      this.init(dat.rank, dat.player_name, dat.player_level, dat.player_id, dat.player_guild_name, dat.leader_unit_id, dat.leader_unit_level, dat.leader_unit_job_id, dat.player_emblem_id, new int?(dat.score));
    }

    public Data(int rank, Player player, PlayerUnit unit, PlayerAffiliation playerAffiliation)
    {
      int id = unit != (PlayerUnit) null ? unit.unit.ID : 0;
      int leaderUnitLvl = unit != (PlayerUnit) null ? unit.total_level : 1;
      int jobId = unit != (PlayerUnit) null ? unit.job_id : 0;
      this.init(rank, player.name, player.level, player.id, playerAffiliation.guild.guild_name, id, leaderUnitLvl, jobId, player.current_emblem_id);
    }

    private void init(
      int rank,
      string name,
      int level,
      string playerID,
      string guildName,
      int leaderUnitId,
      int leaderUnitLvl,
      int leaderJobId,
      int emblem,
      int? point = null)
    {
      this.rank = rank;
      this.name = name;
      this.guildName = guildName;
      this.level = level;
      this.playerID = playerID;
      this.leaderUnit = new Raid032MyRankingPlayer.Data.SimpleUnit(leaderUnitId, leaderUnitLvl, leaderJobId);
      this.emblem = emblem;
      this.point = point;
    }

    public class SimpleUnit
    {
      public int id { get; private set; }

      public int level { get; private set; }

      public int job_id { get; private set; }

      public SimpleUnit(int id, int level, int job_id)
      {
        this.id = id;
        this.level = level;
        this.job_id = job_id;
      }

      public PlayerUnit playerUnit
      {
        get
        {
          UnitUnit unit;
          if (!MasterData.UnitUnit.TryGetValue(this.id, out unit))
            return (PlayerUnit) null;
          PlayerUnit byUnitunit = PlayerUnit.create_by_unitunit(unit);
          byUnitunit.level = this.level;
          byUnitunit.job_id = this.job_id;
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
    public GameObject[] number = new GameObject[10];
  }

  [Serializable]
  public class DecimalSprite
  {
    public GameObject top;
    public Raid032MyRankingPlayer.DigitSprite[] digits;
  }

  [Serializable]
  public class InsBoard
  {
    public Raid032MyRankingPlayer.RankEnum rank = Raid032MyRankingPlayer.RankEnum.Num;
    public GameObject top;
    public Raid032MyRankingPlayer.DecimalSprite[] @decimal;
    private const int RADIX_DECIMAL = 10;

    public void drawDecimal(int num)
    {
      if (this.@decimal == null || this.@decimal.Length == 0)
        return;
      int num1 = (int) Mathf.Pow(10f, (float) this.@decimal.Length) - 1;
      if (num > num1)
        num = num1;
      int num2 = num.ToString().Length - 1;
      for (int index = 0; index < this.@decimal.Length; ++index)
      {
        Raid032MyRankingPlayer.DecimalSprite decimalSprite = this.@decimal[index];
        if (decimalSprite != null && !Object.op_Equality((Object) decimalSprite.top, (Object) null))
        {
          if (num2 == index)
            decimalSprite.top.SetActive(this.drawDecimal(decimalSprite.digits, num));
          else
            decimalSprite.top.SetActive(false);
        }
      }
    }

    private bool drawDecimal(Raid032MyRankingPlayer.DigitSprite[] decsp, int num)
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
        ((IEnumerable<GameObject>) decsp[index].number).ToggleOnce(num / num1);
        num %= num1;
        num1 /= 10;
      }
      return true;
    }
  }
}
