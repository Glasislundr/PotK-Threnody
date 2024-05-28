// Decompiled with JetBrains decompiler
// Type: battle01718aMission
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class battle01718aMission : MonoBehaviour
{
  [SerializeField]
  private UILabel TxtGetName;
  [SerializeField]
  private UISprite SlcGet;
  [SerializeField]
  private UILabel TxtDescription;
  [SerializeField]
  private UISprite SlcUnAttainment;
  [SerializeField]
  private UISprite SlcAttainment;
  [SerializeField]
  private UISprite SlcStarUnAttainment;
  [SerializeField]
  private UISprite SlcStarAttainment;
  [SerializeField]
  private UISprite SlcDescriptionBase;
  [SerializeField]
  private GameObject LinkPrefab;
  [SerializeField]
  private Transform LinkParent;
  [SerializeField]
  private CreateIconObject UniqueIcon;

  public IEnumerator SetSprite(bool clearflag, QuestStoryMission story)
  {
    this.TxtDescription.SetTextLocalize(story.name);
    this.UniqueIcon = ((Component) this.LinkParent).gameObject.GetOrAddComponent<CreateIconObject>();
    if (story != null)
    {
      IEnumerator e = this.UniqueIcon.CreateThumbnail(story.entity_type, story.entity_id);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.SetTxtGetName(this.GetName(story.entity_type, story.entity_id), story.entity_type, story.quantity);
    }
    this.SetValue(clearflag);
  }

  public IEnumerator SetSprite(bool clearflag, QuestExtraMission extra)
  {
    this.TxtDescription.SetTextLocalize(extra.name);
    this.UniqueIcon = ((Component) this.LinkParent).gameObject.GetOrAddComponent<CreateIconObject>();
    if (extra != null)
    {
      IEnumerator e = this.UniqueIcon.CreateThumbnail(extra.entity_type, extra.entity_id);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.SetTxtGetName(this.GetName(extra.entity_type, extra.entity_id), extra.entity_type, extra.quantity);
    }
    this.SetValue(clearflag);
  }

  public IEnumerator SetSprite(bool clearflag, QuestSeaMission story)
  {
    this.TxtDescription.SetTextLocalize(story.name);
    this.UniqueIcon = ((Component) this.LinkParent).gameObject.GetOrAddComponent<CreateIconObject>();
    if (story != null)
    {
      IEnumerator e = this.UniqueIcon.CreateThumbnail(story.entity_type, story.entity_id);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.SetTxtGetName(this.GetName(story.entity_type, story.entity_id), story.entity_type, story.quantity);
    }
    this.SetValue(clearflag);
  }

  public IEnumerator SetSprite(bool clearflag, CorpsMissionReward mission)
  {
    if (mission != null)
    {
      this.TxtDescription.SetTextLocalize(mission.name);
      this.UniqueIcon = ((Component) this.LinkParent).gameObject.GetOrAddComponent<CreateIconObject>();
      IEnumerator e = this.UniqueIcon.CreateThumbnail(mission.entity_type, mission.entity_id);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.SetTxtGetName(this.GetName(mission.entity_type, mission.entity_id), mission.entity_type, mission.quantity);
    }
    this.SetValue(clearflag);
  }

  public void SetValue(bool clearflag)
  {
    this.LinkPrefab = this.UniqueIcon.GetIcon();
    ((Component) this.SlcAttainment).gameObject.SetActive(clearflag);
    ((Component) this.SlcUnAttainment).gameObject.SetActive(!clearflag);
    ((Component) this.SlcStarAttainment).gameObject.SetActive(clearflag);
    ((Component) this.SlcStarUnAttainment).gameObject.SetActive(!clearflag);
    ((Component) this.SlcGet).gameObject.SetActive(false);
    if (((Component) this.SlcAttainment).gameObject.activeSelf)
    {
      Color color = Color.Lerp(Color.white, Color.gray, 1f);
      if (Object.op_Inequality((Object) this.LinkPrefab.GetComponent<IconPrefabBase>(), (Object) null))
        this.LinkPrefab.GetComponent<IconPrefabBase>().Gray = true;
      else
        this.LinkPrefab.GetComponent<UIWidget>().color = color;
      if (Singleton<NGGameDataManager>.GetInstance().IsSea)
      {
        ((Component) this.TxtDescription).GetComponent<UIWidget>().color = new Color(1f, 1f, 1f, 0.8f);
        ((Component) this.SlcDescriptionBase).GetComponent<UIWidget>().color = new Color(1f, 1f, 1f, 0.482352942f);
      }
      else
      {
        ((Component) this.TxtDescription).GetComponent<UIWidget>().color = color;
        ((Component) this.SlcDescriptionBase).GetComponent<UIWidget>().color = color;
      }
    }
    if (!Object.op_Inequality((Object) this.LinkPrefab, (Object) null))
      return;
    this.LinkPrefab.transform.localScale = new Vector3(0.65f, 0.65f, 0.65f);
  }

  private string GetName(MasterDataTable.CommonRewardType reward, int entity_id)
  {
    Consts instance = Consts.GetInstance();
    try
    {
      switch (reward)
      {
        case MasterDataTable.CommonRewardType.unit:
        case MasterDataTable.CommonRewardType.material_unit:
          return MasterData.UnitUnit[entity_id].name;
        case MasterDataTable.CommonRewardType.supply:
          return MasterData.SupplySupply[entity_id].name;
        case MasterDataTable.CommonRewardType.gear:
        case MasterDataTable.CommonRewardType.material_gear:
        case MasterDataTable.CommonRewardType.gear_body:
          return MasterData.GearGear[entity_id].name;
        case MasterDataTable.CommonRewardType.money:
          return instance.UNIQUE_ICON_ZENY;
        case MasterDataTable.CommonRewardType.player_exp:
          return instance.UNIQUE_ICON_PLAYEREXP;
        case MasterDataTable.CommonRewardType.coin:
          return instance.UNIQUE_ICON_KISEKI;
        case MasterDataTable.CommonRewardType.recover:
          return instance.UNIQUE_ICON_APRECOVER;
        case MasterDataTable.CommonRewardType.max_unit:
          return instance.UNIQUE_ICON_MAXUNIT;
        case MasterDataTable.CommonRewardType.max_item:
          return instance.UNIQUE_ICON_MAXITEM;
        case MasterDataTable.CommonRewardType.medal:
          return instance.UNIQUE_ICON_MEDAL;
        case MasterDataTable.CommonRewardType.friend_point:
          return instance.UNIQUE_ICON_POINT;
        case MasterDataTable.CommonRewardType.emblem:
          return instance.UNIQUE_ICON_EMBLEM;
        case MasterDataTable.CommonRewardType.battle_medal:
          return instance.UNIQUE_ICON_BATTLE_MEDAL;
        case MasterDataTable.CommonRewardType.cp_recover:
          return instance.UNIQUE_ICON_CPRECOVER;
        case MasterDataTable.CommonRewardType.quest_key:
          return MasterData.QuestkeyQuestkey[entity_id].name;
        case MasterDataTable.CommonRewardType.gacha_ticket:
          return MasterData.GachaTicket[entity_id].name;
        case MasterDataTable.CommonRewardType.mp_recover:
          return instance.UNIQUE_ICON_MPRECOVER;
        case MasterDataTable.CommonRewardType.awake_skill:
          return MasterData.BattleskillSkill[entity_id].name;
        case MasterDataTable.CommonRewardType.common_ticket:
          return CommonRewardType.GetRewardName(reward, entity_id);
        default:
          return "";
      }
    }
    catch
    {
      Debug.LogWarning((object) (reward.ToString() + "ID:" + (object) entity_id + "が見つかりませんでした。"));
      return "";
    }
  }

  private void SetTxtGetName(string label, MasterDataTable.CommonRewardType reward, int quantity)
  {
    string str = "x";
    if (reward == MasterDataTable.CommonRewardType.money || reward == MasterDataTable.CommonRewardType.friend_point || reward == MasterDataTable.CommonRewardType.unit_exp || reward == MasterDataTable.CommonRewardType.player_exp || reward == MasterDataTable.CommonRewardType.max_unit || reward == MasterDataTable.CommonRewardType.max_item || reward == MasterDataTable.CommonRewardType.cp_recover || reward == MasterDataTable.CommonRewardType.mp_recover)
      str = "";
    this.TxtGetName.SetText(string.Format("{0}  {1}{2}", (object) label, (object) str, (object) quantity));
  }
}
