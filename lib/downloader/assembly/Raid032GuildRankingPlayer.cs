// Decompiled with JetBrains decompiler
// Type: Raid032GuildRankingPlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

#nullable disable
public class Raid032GuildRankingPlayer : MonoBehaviour
{
  private const string numSpriteName = ".png__GUI__023-7-1_sozai__023-7-1_sozai_prefab";
  private Vector2 spriteOneXY = new Vector2(20f, 30f);
  private Vector2 spriteNotOneXY = new Vector2(26f, 32f);
  public Action<GuildDirectory> showGuildPopUp;
  [SerializeField]
  private GameObject lnkUnit;
  [SerializeField]
  private GameObject lnkHonor;
  private WebAPI.Response.GuildraidRankingGuildDamage_rankings initData;
  [SerializeField]
  private UISprite[] baseBG;
  [SerializeField]
  private UILabel rankLabel;
  [SerializeField]
  private UILabel guildName;
  [SerializeField]
  private UILabel guildMembers;
  [SerializeField]
  private UILabel guildDamage;
  private GameObject unitIcon;

  private void Start()
  {
    ((Component) this).gameObject.GetComponent<UIButton>().onClick.Add(new EventDelegate((EventDelegate.Callback) (() => this.StartCoroutine(this.ShowGuildInfo()))));
  }

  public void Initialize(
    GameObject prefabUnitIcon,
    WebAPI.Response.GuildraidRankingGuildDamage_rankings data = null,
    int?[] points = null)
  {
    this.initData = data;
    if (data == null)
    {
      this.lnkUnit.SetActive(false);
      this.lnkHonor.SetActive(false);
    }
    else
    {
      this.rankLabel.text = "";
      for (int index = 0; index < this.baseBG.Length; ++index)
      {
        if (index < 3)
          ((Component) ((Component) this.baseBG[index]).transform.parent).gameObject.SetActive(false);
        else
          ((Component) this.baseBG[index]).gameObject.SetActive(false);
      }
      this.guildName.SetTextLocalize(data.guild_name);
      this.guildMembers.SetTextLocalize(Consts.GetInstance().POPUP_RAID_GUILD_MEMBER_NUMBER + " " + Consts.Format(Consts.GetInstance().POPUP_GUILD_MEMBER_NUMBER, (IDictionary) new Hashtable()
      {
        {
          (object) "now",
          (object) data.membership_num
        },
        {
          (object) "max",
          (object) data.membership_capacity
        }
      }));
      this.guildDamage.SetTextLocalize(data.score);
      if (Object.op_Equality((Object) this.unitIcon, (Object) null))
        this.unitIcon = prefabUnitIcon.Clone(this.lnkUnit.transform);
      this.SetRankBaseBG(data.rank);
      if (data.rank <= 3 && data.rank != 0)
        return;
      this.SetRankNum(data.rank);
    }
  }

  public IEnumerator InitImage()
  {
    if (this.initData != null)
    {
      Transform transform = this.lnkUnit.transform.GetChildren().FirstOrDefault<Transform>();
      UnitIcon uniticon = Object.op_Inequality((Object) transform, (Object) null) ? ((Component) transform).GetComponent<UnitIcon>() : (UnitIcon) null;
      PlayerUnit playerunit = (PlayerUnit) null;
      UnitUnit unit;
      if (MasterData.UnitUnit.TryGetValue(this.initData.leader_unit_id, out unit))
      {
        playerunit = PlayerUnit.create_by_unitunit(unit);
        playerunit.level = this.initData.leader_unit_level;
        playerunit.job_id = this.initData.leader_unit_job_id;
      }
      IEnumerator e;
      if (Object.op_Inequality((Object) uniticon, (Object) null) && playerunit != (PlayerUnit) null)
      {
        if (Object.op_Inequality((Object) uniticon, (Object) null) && playerunit != (PlayerUnit) null)
        {
          e = uniticon.setSimpleUnit(playerunit);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          uniticon.setLevelText(playerunit);
          uniticon.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
          ((Collider) uniticon.buttonBoxCollider).enabled = false;
          this.lnkUnit.SetActive(true);
        }
      }
      else
        this.lnkUnit.SetActive(false);
      Future<Sprite> ldemblem = EmblemUtility.LoadGuildEmblemSprite(this.initData.current_emblem_id);
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
    }
  }

  private void SetRankBaseBG(int rank)
  {
    if (rank <= 3 && rank != 0)
      ((Component) ((Component) this.baseBG[rank - 1]).transform.parent).gameObject.SetActive(true);
    else
      ((Component) this.baseBG[3]).gameObject.SetActive(true);
  }

  private void SetRankNum(int rank)
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
    this.rankLabel.SetTextLocalize(Consts.Format(Consts.GetInstance().TOWER_RANKING_RANK, (IDictionary) args));
  }

  private IEnumerator ShowGuildInfo()
  {
    if (string.IsNullOrEmpty(this.initData.guild_id))
    {
      Debug.LogError((object) "guildID is null");
    }
    else
    {
      Future<WebAPI.Response.GuildInfo> future = WebAPI.GuildInfo(this.initData.guild_id);
      IEnumerator e = future.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      WebAPI.Response.GuildInfo result = future.Result;
      if (result != null && this.showGuildPopUp != null)
        this.showGuildPopUp(result.guild);
    }
  }
}
