// Decompiled with JetBrains decompiler
// Type: GuildMemberScoreScroll
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class GuildMemberScoreScroll : MonoBehaviour
{
  [SerializeField]
  private GameObject link_Character;
  [SerializeField]
  private UI2DSprite playerTitle;
  [SerializeField]
  private UILabel lblPlayerName;
  [SerializeField]
  private UILabel lblContributionValue;
  [SerializeField]
  private UILabel lblAttack;
  [SerializeField]
  private UILabel lblAttack2;
  [SerializeField]
  private UILabel lblDefense;
  [SerializeField]
  private UILabel lblDefense2;
  [SerializeField]
  private UILabel lblStarNumAttack;
  [SerializeField]
  private UILabel lblStarNumDefense;
  [SerializeField]
  private GameObject slc_Listbase_guild_member;
  [SerializeField]
  private GameObject slc_Listbase_player;
  [SerializeField]
  private UILabel txt_stars_get;
  [SerializeField]
  private UILabel lbl_star_acquired;
  [SerializeField]
  private GameObject slc_master_icon;
  [SerializeField]
  private GameObject slc_sub_master_icon;
  [SerializeField]
  private GameObject topDefenseInfo_;
  [SerializeField]
  private UILabel txtStarOnDefense_;
  private GvgPlayerHistory score;
  private GameObject unitIconPrefab;

  private IEnumerator SetMemberScoreData(GvgPlayerHistory score)
  {
    this.slc_Listbase_guild_member.SetActive(!score.player_id.Equals(Player.Current.id));
    this.slc_Listbase_player.SetActive(score.player_id.Equals(Player.Current.id));
    UnitIcon unitIcon = this.link_Character.GetComponentInChildren<UnitIcon>();
    if (Object.op_Inequality((Object) unitIcon, (Object) null))
      Object.Destroy((Object) ((Component) unitIcon).gameObject);
    unitIcon = (UnitIcon) null;
    IEnumerator e;
    if (Object.op_Equality((Object) this.unitIconPrefab, (Object) null))
    {
      Future<GameObject> unitIconPrefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
      e = unitIconPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.unitIconPrefab = unitIconPrefabF.Result;
      unitIconPrefabF = (Future<GameObject>) null;
    }
    if (score != null && Object.op_Inequality((Object) this.unitIconPrefab, (Object) null))
    {
      PlayerUnit player = new PlayerUnit();
      player._unit = score.leader_unit_unit_id.Value;
      player._unit_type = score.leader_unit_unit_type_id.Value;
      player.level = score.leader_unit_level.Value;
      player.job_id = score.leader_unit_job_id.Value;
      unitIcon = this.unitIconPrefab.CloneAndGetComponent<UnitIcon>(this.link_Character);
      e = unitIcon.setSimpleUnit(player);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      unitIcon.setLevelText(player);
      unitIcon.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
      player = (PlayerUnit) null;
    }
    this.SetLabel();
    this.slc_master_icon.SetActive(score.role.Value == 3);
    this.slc_sub_master_icon.SetActive(score.role.Value == 2);
    Future<Sprite> image = EmblemUtility.LoadEmblemSprite(score.player_emblem_id.Value);
    e = image.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.playerTitle.sprite2D = image.Result;
    this.lblPlayerName.SetTextLocalize(score.player_name);
    this.lblStarNumAttack.SetTextLocalize(score.attack_count.Value);
    this.lblStarNumDefense.SetTextLocalize(score.defense_count.Value);
    this.lblContributionValue.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_GUILD_BATTLE_MEMBER_SCORE_CONTRIBUTION_VALUE, (IDictionary) new Hashtable()
    {
      {
        (object) "contribution",
        (object) score.contribution.Value
      }
    }));
    this.lbl_star_acquired.SetTextLocalize(score.attack_star.Value);
    this.topDefenseInfo_.SetActive(score.defense_star.HasValue);
    if (this.topDefenseInfo_.activeSelf)
      this.txtStarOnDefense_.SetTextLocalize(string.Format("[FFF200]{0}[-]", (object) score.defense_star.Value));
  }

  private void SetLabel()
  {
    this.lblAttack.SetTextLocalize(Consts.GetInstance().POPUP_GUILD_BATTLE_MEMBER_SCORE_ATTACK);
    this.lblAttack2.SetTextLocalize(Consts.GetInstance().POPUP_GUILD_BATTLE_MEMBER_SCORE_MATCH_UNIT);
    this.lblDefense.SetTextLocalize(Consts.GetInstance().POPUP_GUILD_BATTLE_MEMBER_SCORE_DEFENSE);
    this.lblDefense2.SetTextLocalize(Consts.GetInstance().POPUP_GUILD_BATTLE_MEMBER_SCORE_MATCH_UNIT);
    this.txt_stars_get.SetTextLocalize(Consts.GetInstance().POPUP_GUILD_BATTLE_MEMBER_SCORE_ACQUIRED_STAR);
  }

  public IEnumerator InitializeAsync(GvgPlayerHistory score)
  {
    this.score = score;
    IEnumerator e = this.SetMemberScoreData(score);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
