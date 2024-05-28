// Decompiled with JetBrains decompiler
// Type: Quest0022MissionList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Quest0022MissionList : MonoBehaviour
{
  [SerializeField]
  private UILabel TxtGetName;
  [SerializeField]
  private UISprite SlcGet;
  [SerializeField]
  private UISprite SlcAttainment;
  [SerializeField]
  private UISprite SlcNonAttainment;
  [SerializeField]
  private UISprite StarAttainment;
  [SerializeField]
  private UISprite StarNonAttainment;
  [SerializeField]
  private UISprite StarEffect;
  [SerializeField]
  private UILabel TxtDescription;
  [SerializeField]
  private UISprite SlcDescriptionBase;
  [SerializeField]
  private GameObject LinkPrefab;
  [SerializeField]
  public Transform LinkParent;
  [SerializeField]
  private UniqueIconsSetStory UniqueIcon;
  [SerializeField]
  private UI2DSprite SlcLinkEffect;
  [HideInInspector]
  public string MissionName;

  public bool IsClear { get; private set; }

  public GameObject Animation { get; set; }

  protected MasterDataTable.CommonRewardType RewardType { get; private set; }

  protected int RewardId { get; private set; }

  protected int RewardQuantity { get; private set; }

  public IEnumerator SetValue(QuestStoryMission story, bool clearFlag)
  {
    IEnumerator e = this.SetValue(story.name, clearFlag, story.entity_type, story.entity_id, story.quantity, questType: new CommonQuestType?(CommonQuestType.Story));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator SetValue(QuestExtraMission extra, bool clearFlag)
  {
    IEnumerator e = this.SetValue(extra.name, clearFlag, extra.entity_type, extra.entity_id, extra.quantity, questType: new CommonQuestType?(CommonQuestType.Extra));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator SetValue(QuestSeaMission story, bool clearFlag)
  {
    IEnumerator e = this.SetValue(story.name, clearFlag, story.entity_type, story.entity_id, story.quantity, questType: new CommonQuestType?(CommonQuestType.Sea));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator SetValue(
    string missionName,
    bool clearFlag,
    MasterDataTable.CommonRewardType rewardType,
    int rewardId,
    int quantity,
    bool battleFlag = false,
    CommonQuestType? questType = null)
  {
    if (Object.op_Inequality((Object) this.LinkPrefab, (Object) null))
      Object.DestroyImmediate((Object) this.LinkPrefab);
    this.MissionName = missionName;
    this.IsClear = battleFlag;
    this.RewardType = rewardType;
    this.RewardId = rewardId;
    this.RewardQuantity = quantity;
    this.TxtDescription.SetTextLocalize(missionName);
    ((Component) this.SlcNonAttainment).gameObject.SetActive(!clearFlag);
    ((Component) this.SlcAttainment).gameObject.SetActive(clearFlag);
    ((Component) this.StarNonAttainment).gameObject.SetActive(!clearFlag);
    ((Component) this.StarAttainment).gameObject.SetActive(clearFlag);
    ((Component) this.SlcGet).gameObject.SetActive(false);
    ((Component) this.StarEffect).gameObject.SetActive(false);
    ((Component) this.SlcLinkEffect).gameObject.SetActive(false);
    CreateIconObject target = ((Component) this.LinkParent).gameObject.GetOrAddComponent<CreateIconObject>();
    IEnumerator e = target.CreateThumbnail(rewardType, rewardId);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ((Component) target).gameObject.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
    this.LinkPrefab = target.GetIcon();
    string str = "x";
    if (rewardType == MasterDataTable.CommonRewardType.money || rewardType == MasterDataTable.CommonRewardType.friend_point || rewardType == MasterDataTable.CommonRewardType.unit_exp || rewardType == MasterDataTable.CommonRewardType.player_exp || rewardType == MasterDataTable.CommonRewardType.max_unit || rewardType == MasterDataTable.CommonRewardType.max_item || rewardType == MasterDataTable.CommonRewardType.cp_recover || rewardType == MasterDataTable.CommonRewardType.mp_recover)
      str = "";
    this.TxtGetName.SetText(string.Format("{0}  {1}{2}", (object) this.GetName(rewardType, rewardId), (object) str, (object) quantity));
    Color color = Color.Lerp(Color.white, Color.gray, 1f);
    if (Singleton<NGGameDataManager>.GetInstance().IsSea)
    {
      // ISSUE: explicit constructor call
      ((Color) ref color).\u002Ector(1f, 1f, 1f, 0.8f);
    }
    if (((Component) this.SlcAttainment).gameObject.activeSelf)
    {
      ((Component) this.TxtDescription).GetComponent<UIWidget>().color = color;
      if (Singleton<NGGameDataManager>.GetInstance().IsSea)
      {
        // ISSUE: explicit constructor call
        ((Color) ref color).\u002Ector(1f, 1f, 1f, 0.482352942f);
      }
      ((Component) this.SlcDescriptionBase).GetComponent<UIWidget>().color = color;
      if (Object.op_Inequality((Object) this.LinkPrefab, (Object) null))
        this.LinkPrefab.GetComponent<IconPrefabBase>().Gray = true;
    }
    else
    {
      ((Component) this.TxtDescription).GetComponent<UIWidget>().color = Color.white;
      ((Component) this.SlcDescriptionBase).GetComponent<UIWidget>().color = Color.white;
    }
  }

  public void InitEffects()
  {
    if (!this.IsClear)
      return;
    this.LinkPrefab.GetComponent<IconPrefabBase>().Gray = false;
    ((Component) this.SlcAttainment).gameObject.SetActive(false);
    ((Component) this.StarAttainment).gameObject.SetActive(false);
  }

  public void ResultNowGet()
  {
    ((Component) this.SlcGet).gameObject.SetActive(true);
    ((Component) this.SlcAttainment).gameObject.SetActive(true);
    ((Component) this.StarAttainment).gameObject.SetActive(true);
    ((Component) this.StarEffect).gameObject.SetActive(true);
    ((IEnumerable<UITweener>) ((Component) this.StarAttainment).GetComponents<UITweener>()).ForEach<UITweener>((Action<UITweener>) (x =>
    {
      x.ResetToBeginning();
      x.PlayForward();
    }));
    UITweener component = ((Component) this.StarEffect).GetComponent<UITweener>();
    component.ResetToBeginning();
    component.PlayForward();
    ((IEnumerable<UITweener>) ((Component) this.SlcAttainment).GetComponents<UITweener>()).ForEach<UITweener>((Action<UITweener>) (x =>
    {
      x.ResetToBeginning();
      x.PlayForward();
    }));
    ((Component) this.SlcLinkEffect).gameObject.SetActive(true);
    ((IEnumerable<UITweener>) ((Component) this.SlcLinkEffect).GetComponents<UITweener>()).ForEach<UITweener>((Action<UITweener>) (x =>
    {
      x.ResetToBeginning();
      x.PlayForward();
    }));
    if (!Object.op_Inequality((Object) this.Animation, (Object) null))
      return;
    this.Animation.SetActive(true);
  }

  public void ClearEffectDisable()
  {
    if (!Object.op_Inequality((Object) this.Animation, (Object) null))
      return;
    this.Animation.SetActive(false);
  }

  protected string GetName() => this.GetName(this.RewardType, this.RewardId);

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
          return MasterData.CommonTicket[entity_id].name;
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
}
