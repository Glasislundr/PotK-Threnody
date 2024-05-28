// Decompiled with JetBrains decompiler
// Type: BattleUI05RewardMenu
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
public class BattleUI05RewardMenu : ResultMenuBase
{
  [SerializeField]
  private GameObject mainObject;
  [SerializeField]
  private GameObject mainTitleObject;
  [SerializeField]
  private GameObject Title;
  private const float tweenDuration = 0.25f;
  private const float LINK_WIDTH = 136f;
  private const float LINK_DEFWIDTH = 136f;
  private const float scale = 1f;
  [SerializeField]
  private GameObject Grid;
  [SerializeField]
  private GameObject ScrollView;
  [SerializeField]
  private GameObject NoneMessage;
  private GameObject mUnitPrefab;
  private GameObject mItemIconPrefab;
  private GameObject mUniqueIconsPrefab;
  private NGSoundManager soundManager;
  private bool isQuestAutoLap;
  private List<BattleUI05RewardMenu.Reward> PlayRewards = new List<BattleUI05RewardMenu.Reward>();

  private void Awake() => this.soundManager = Singleton<NGSoundManager>.GetInstance();

  public override void OnDestroy()
  {
    this.PlayRewards.Clear();
    base.OnDestroy();
  }

  private IEnumerator ResourceLoad(BattleInfo info)
  {
    Future<GameObject> prefabF = !Singleton<NGGameDataManager>.GetInstance().IsSea || info.seaQuest == null ? Res.Prefabs.UnitIcon.normal.Load<GameObject>() : Res.Prefabs.Sea.UnitIcon.normal_sea.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.mUnitPrefab = prefabF.Result;
    prefabF = (Future<GameObject>) null;
    prefabF = !Singleton<NGGameDataManager>.GetInstance().IsSea || info.seaQuest == null ? Res.Prefabs.ItemIcon.prefab.Load<GameObject>() : new ResourceObject("Prefabs/Sea/ItemIcon/prefab_sea").Load<GameObject>();
    yield return (object) prefabF.Wait();
    this.mItemIconPrefab = prefabF.Result;
    prefabF = (Future<GameObject>) null;
    prefabF = !Singleton<NGGameDataManager>.GetInstance().IsSea || info.seaQuest == null ? Res.Icons.UniqueIconPrefab.Load<GameObject>() : Res.Icons.UniqueIconPrefab_sea.Load<GameObject>();
    yield return (object) prefabF.Wait();
    this.mUniqueIconsPrefab = prefabF.Result;
    prefabF = (Future<GameObject>) null;
  }

  public override IEnumerator Init(BattleInfo info, BattleEnd result)
  {
    IEnumerator e = this.ResourceLoad(info);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Dictionary<int, PlayerUnit> dictionary1 = ((IEnumerable<PlayerUnit>) SMManager.Get<PlayerUnit[]>()).ToDictionary<PlayerUnit, int>((Func<PlayerUnit, int>) (x => x.id));
    foreach (BattleEndDrop_unit_entities dropUnitEntity in result.drop_unit_entities)
      this.AddReward((object) dictionary1[dropUnitEntity.reward_id.Value], dropUnitEntity.is_new, dropUnitEntity.reward_quantity, dropUnitEntity.reward_id.Value, dropUnitEntity.reward_type_id);
    PlayerMaterialUnit[] source1 = SMManager.Get<PlayerMaterialUnit[]>();
    if (source1 != null && result.drop_material_unit_entities != null)
    {
      Dictionary<int, PlayerMaterialUnit> dictionary2 = ((IEnumerable<PlayerMaterialUnit>) source1).ToDictionary<PlayerMaterialUnit, int>((Func<PlayerMaterialUnit, int>) (x => x.id));
      foreach (BattleEndDrop_material_unit_entities materialUnitEntity in result.drop_material_unit_entities)
        this.AddReward((object) dictionary2[materialUnitEntity.reward_id.Value], materialUnitEntity.is_new, materialUnitEntity.reward_quantity, materialUnitEntity.reward_id.Value, materialUnitEntity.reward_type_id);
    }
    Dictionary<int, PlayerItem> dictionary3 = ((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>().AllGears(SMManager.Get<Player>())).ToDictionary<PlayerItem, int>((Func<PlayerItem, int>) (x => x.id));
    foreach (BattleEndDrop_gear_entities dropGearEntity in result.drop_gear_entities)
      this.AddReward((object) dictionary3[dropGearEntity.reward_id.Value], dropGearEntity.is_new, dropGearEntity.reward_quantity, dropGearEntity.reward_id.Value, dropGearEntity.reward_type_id);
    PlayerMaterialGear[] source2 = SMManager.Get<PlayerMaterialGear[]>();
    if (source2 != null && result.drop_material_gear_entities != null)
    {
      Dictionary<int, PlayerMaterialGear> dictionary4 = ((IEnumerable<PlayerMaterialGear>) source2).ToDictionary<PlayerMaterialGear, int>((Func<PlayerMaterialGear, int>) (x => x.id));
      foreach (BattleEndDrop_material_gear_entities materialGearEntity in result.drop_material_gear_entities)
        this.AddReward((object) dictionary4[materialGearEntity.reward_id.Value], materialGearEntity.is_new, materialGearEntity.reward_quantity, materialGearEntity.reward_id.Value, materialGearEntity.reward_type_id);
    }
    if (result.drop_gacha_ticket_entities != null && result.drop_gacha_ticket_entities.Length != 0)
    {
      foreach (BattleEndDrop_gacha_ticket_entities gachaTicketEntity in result.drop_gacha_ticket_entities)
        this.AddReward((object) MasterData.GachaTicket[gachaTicketEntity.reward_id.Value], gachaTicketEntity.is_new, gachaTicketEntity.reward_quantity, gachaTicketEntity.reward_id.Value, gachaTicketEntity.reward_type_id);
    }
    if (result.drop_supply_entities != null && result.drop_supply_entities.Length != 0)
    {
      foreach (BattleEndDrop_supply_entities dropSupplyEntity in result.drop_supply_entities)
        this.AddReward((object) MasterData.SupplySupply[dropSupplyEntity.reward_id.Value], dropSupplyEntity.is_new, dropSupplyEntity.reward_quantity, dropSupplyEntity.reward_id.Value, dropSupplyEntity.reward_type_id);
    }
    if (result.drop_unit_type_ticket_entities != null && result.drop_unit_type_ticket_entities.Length != 0)
    {
      foreach (BattleEndDrop_unit_type_ticket_entities typeTicketEntity in result.drop_unit_type_ticket_entities)
        this.AddReward((object) MasterData.UnitTypeTicket[typeTicketEntity.reward_id.Value], typeTicketEntity.is_new, typeTicketEntity.reward_quantity, typeTicketEntity.reward_id.Value, typeTicketEntity.reward_type_id);
    }
    if (result.drop_quest_key_entities != null && result.drop_quest_key_entities.Length != 0)
    {
      PlayerQuestKey[] array = SMManager.Get<PlayerQuestKey[]>();
      foreach (BattleEndDrop_quest_key_entities dropQuestKeyEntity in result.drop_quest_key_entities)
      {
        BattleEndDrop_quest_key_entities r = dropQuestKeyEntity;
        this.AddReward((object) Array.Find<PlayerQuestKey>(array, (Predicate<PlayerQuestKey>) (x =>
        {
          int questKeyId = x.quest_key_id;
          int? rewardId = r.reward_id;
          int valueOrDefault = rewardId.GetValueOrDefault();
          return questKeyId == valueOrDefault & rewardId.HasValue;
        })), r.is_new, r.reward_quantity, r.reward_id.Value, r.reward_type_id);
      }
    }
    if (result.drop_unit_ticket_entities != null && result.drop_unit_ticket_entities.Length != 0)
    {
      foreach (BattleEndDrop_unit_ticket_entities unitTicketEntity in result.drop_unit_ticket_entities)
        this.AddReward((object) MasterData.SelectTicket[unitTicketEntity.reward_id.Value], unitTicketEntity.is_new, unitTicketEntity.reward_quantity, unitTicketEntity.reward_id.Value, unitTicketEntity.reward_type_id);
    }
    if (result.drop_common_ticket_entities != null && result.drop_common_ticket_entities.Length != 0)
    {
      foreach (BattleEndDrop_common_ticket_entities commonTicketEntity in result.drop_common_ticket_entities)
        this.AddReward((object) MasterData.CommonTicket[commonTicketEntity.reward_id.Value], commonTicketEntity.is_new, commonTicketEntity.reward_quantity, commonTicketEntity.reward_id.Value, commonTicketEntity.reward_type_id);
    }
    e = this.SetReward();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.Grid.GetComponent<UIGrid>().Reposition();
    this.ScrollView.GetComponent<UIScrollView>().ResetPosition();
    this.mainObject.SetActive(false);
    this.mainTitleObject.SetActive(false);
  }

  public override IEnumerator Init(BattleInfo info, WebAPI.Response.TowerBattleFinish result)
  {
    IEnumerator e = this.ResourceLoad(info);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Dictionary<int, PlayerUnit> dictionary1 = ((IEnumerable<PlayerUnit>) SMManager.Get<PlayerUnit[]>()).ToDictionary<PlayerUnit, int>((Func<PlayerUnit, int>) (x => x.id));
    foreach (WebAPI.Response.TowerBattleFinishDrop_unit_entities dropUnitEntity in result.drop_unit_entities)
      this.AddReward((object) dictionary1[dropUnitEntity.reward_id.Value], dropUnitEntity.is_new, dropUnitEntity.reward_quantity, dropUnitEntity.reward_id.Value, dropUnitEntity.reward_type_id);
    PlayerMaterialUnit[] source1 = SMManager.Get<PlayerMaterialUnit[]>();
    if (source1 != null && result.drop_material_unit_entities != null)
    {
      Dictionary<int, PlayerMaterialUnit> dictionary2 = ((IEnumerable<PlayerMaterialUnit>) source1).ToDictionary<PlayerMaterialUnit, int>((Func<PlayerMaterialUnit, int>) (x => x.id));
      foreach (WebAPI.Response.TowerBattleFinishDrop_material_unit_entities materialUnitEntity in result.drop_material_unit_entities)
        this.AddReward((object) dictionary2[materialUnitEntity.reward_id.Value], materialUnitEntity.is_new, materialUnitEntity.reward_quantity, materialUnitEntity.reward_id.Value, materialUnitEntity.reward_type_id);
    }
    Dictionary<int, PlayerItem> dictionary3 = ((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>().AllGears(SMManager.Get<Player>())).ToDictionary<PlayerItem, int>((Func<PlayerItem, int>) (x => x.id));
    foreach (WebAPI.Response.TowerBattleFinishDrop_gear_entities dropGearEntity in result.drop_gear_entities)
      this.AddReward((object) dictionary3[dropGearEntity.reward_id.Value], dropGearEntity.is_new, dropGearEntity.reward_quantity, dropGearEntity.reward_id.Value, dropGearEntity.reward_type_id);
    PlayerMaterialGear[] source2 = SMManager.Get<PlayerMaterialGear[]>();
    if (source2 != null && result.drop_material_gear_entities != null)
    {
      Dictionary<int, PlayerMaterialGear> dictionary4 = ((IEnumerable<PlayerMaterialGear>) source2).ToDictionary<PlayerMaterialGear, int>((Func<PlayerMaterialGear, int>) (x => x.id));
      foreach (WebAPI.Response.TowerBattleFinishDrop_material_gear_entities materialGearEntity in result.drop_material_gear_entities)
        this.AddReward((object) dictionary4[materialGearEntity.reward_id.Value], materialGearEntity.is_new, materialGearEntity.reward_quantity, materialGearEntity.reward_id.Value, materialGearEntity.reward_type_id);
    }
    foreach (WebAPI.Response.TowerBattleFinishDrop_gacha_ticket_entities gachaTicketEntity in result.drop_gacha_ticket_entities)
      this.AddReward((object) MasterData.GachaTicket[gachaTicketEntity.reward_id.Value], gachaTicketEntity.is_new, gachaTicketEntity.reward_quantity, gachaTicketEntity.reward_id.Value, gachaTicketEntity.reward_type_id);
    foreach (WebAPI.Response.TowerBattleFinishDrop_supply_entities dropSupplyEntity in result.drop_supply_entities)
      this.AddReward((object) MasterData.SupplySupply[dropSupplyEntity.reward_id.Value], dropSupplyEntity.is_new, dropSupplyEntity.reward_quantity, dropSupplyEntity.reward_id.Value, dropSupplyEntity.reward_type_id);
    foreach (WebAPI.Response.TowerBattleFinishDrop_unit_type_ticket_entities typeTicketEntity in result.drop_unit_type_ticket_entities)
      this.AddReward((object) MasterData.UnitTypeTicket[typeTicketEntity.reward_id.Value], typeTicketEntity.is_new, typeTicketEntity.reward_quantity, typeTicketEntity.reward_id.Value, typeTicketEntity.reward_type_id);
    PlayerQuestKey[] array = SMManager.Get<PlayerQuestKey[]>();
    foreach (WebAPI.Response.TowerBattleFinishDrop_quest_key_entities dropQuestKeyEntity in result.drop_quest_key_entities)
    {
      WebAPI.Response.TowerBattleFinishDrop_quest_key_entities r = dropQuestKeyEntity;
      this.AddReward((object) Array.Find<PlayerQuestKey>(array, (Predicate<PlayerQuestKey>) (x =>
      {
        int questKeyId = x.quest_key_id;
        int? rewardId = r.reward_id;
        int valueOrDefault = rewardId.GetValueOrDefault();
        return questKeyId == valueOrDefault & rewardId.HasValue;
      })), r.is_new, r.reward_quantity, r.reward_id.Value, r.reward_type_id);
    }
    if (result.drop_unit_ticket_entities != null && result.drop_unit_ticket_entities.Length != 0)
    {
      foreach (WebAPI.Response.TowerBattleFinishDrop_unit_ticket_entities unitTicketEntity in result.drop_unit_ticket_entities)
        this.AddReward((object) MasterData.SelectTicket[unitTicketEntity.reward_id.Value], unitTicketEntity.is_new, unitTicketEntity.reward_quantity, unitTicketEntity.reward_id.Value, unitTicketEntity.reward_type_id);
    }
    if (result.drop_common_ticket_entities != null && result.drop_common_ticket_entities.Length != 0)
    {
      foreach (WebAPI.Response.TowerBattleFinishDrop_common_ticket_entities commonTicketEntity in result.drop_common_ticket_entities)
        this.AddReward((object) MasterData.CommonTicket[commonTicketEntity.reward_id.Value], commonTicketEntity.is_new, commonTicketEntity.reward_quantity, commonTicketEntity.reward_id.Value, commonTicketEntity.reward_type_id);
    }
    e = this.SetReward();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.Grid.GetComponent<UIGrid>().Reposition();
    this.ScrollView.GetComponent<UIScrollView>().ResetPosition();
    this.mainObject.SetActive(false);
    this.mainTitleObject.SetActive(false);
  }

  public override IEnumerator Init(BattleInfo info, WebAPI.Response.QuestCorpsBattleFinish result)
  {
    IEnumerator e = this.ResourceLoad(info);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (result.drop_unit_entities.Length != 0)
    {
      Dictionary<int, PlayerUnit> dictionary = ((IEnumerable<PlayerUnit>) SMManager.Get<PlayerUnit[]>()).ToDictionary<PlayerUnit, int>((Func<PlayerUnit, int>) (x => x.id));
      foreach (WebAPI.Response.QuestCorpsBattleFinishDrop_unit_entities dropUnitEntity in result.drop_unit_entities)
        this.AddReward((object) dictionary[dropUnitEntity.reward_id.Value], dropUnitEntity.is_new, dropUnitEntity.reward_quantity, dropUnitEntity.reward_id.Value, dropUnitEntity.reward_type_id);
    }
    if (result.drop_material_unit_entities.Length != 0)
    {
      Dictionary<int, PlayerMaterialUnit> dictionary = ((IEnumerable<PlayerMaterialUnit>) SMManager.Get<PlayerMaterialUnit[]>()).ToDictionary<PlayerMaterialUnit, int>((Func<PlayerMaterialUnit, int>) (x => x.id));
      foreach (WebAPI.Response.QuestCorpsBattleFinishDrop_material_unit_entities materialUnitEntity in result.drop_material_unit_entities)
        this.AddReward((object) dictionary[materialUnitEntity.reward_id.Value], materialUnitEntity.is_new, materialUnitEntity.reward_quantity, materialUnitEntity.reward_id.Value, materialUnitEntity.reward_type_id);
    }
    if (result.drop_gear_entities.Length != 0)
    {
      Dictionary<int, PlayerItem> dictionary = ((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>().AllGears(SMManager.Get<Player>())).ToDictionary<PlayerItem, int>((Func<PlayerItem, int>) (x => x.id));
      foreach (WebAPI.Response.QuestCorpsBattleFinishDrop_gear_entities dropGearEntity in result.drop_gear_entities)
        this.AddReward((object) dictionary[dropGearEntity.reward_id.Value], dropGearEntity.is_new, dropGearEntity.reward_quantity, dropGearEntity.reward_id.Value, dropGearEntity.reward_type_id);
    }
    if (result.drop_material_gear_entities.Length != 0)
    {
      Dictionary<int, PlayerMaterialGear> dictionary = ((IEnumerable<PlayerMaterialGear>) SMManager.Get<PlayerMaterialGear[]>()).ToDictionary<PlayerMaterialGear, int>((Func<PlayerMaterialGear, int>) (x => x.id));
      foreach (WebAPI.Response.QuestCorpsBattleFinishDrop_material_gear_entities materialGearEntity in result.drop_material_gear_entities)
        this.AddReward((object) dictionary[materialGearEntity.reward_id.Value], materialGearEntity.is_new, materialGearEntity.reward_quantity, materialGearEntity.reward_id.Value, materialGearEntity.reward_type_id);
    }
    foreach (WebAPI.Response.QuestCorpsBattleFinishDrop_gacha_ticket_entities gachaTicketEntity in result.drop_gacha_ticket_entities)
      this.AddReward((object) MasterData.GachaTicket[gachaTicketEntity.reward_id.Value], gachaTicketEntity.is_new, gachaTicketEntity.reward_quantity, gachaTicketEntity.reward_id.Value, gachaTicketEntity.reward_type_id);
    foreach (WebAPI.Response.QuestCorpsBattleFinishDrop_supply_entities dropSupplyEntity in result.drop_supply_entities)
      this.AddReward((object) MasterData.SupplySupply[dropSupplyEntity.reward_id.Value], dropSupplyEntity.is_new, dropSupplyEntity.reward_quantity, dropSupplyEntity.reward_id.Value, dropSupplyEntity.reward_type_id);
    foreach (WebAPI.Response.QuestCorpsBattleFinishDrop_unit_type_ticket_entities typeTicketEntity in result.drop_unit_type_ticket_entities)
      this.AddReward((object) MasterData.UnitTypeTicket[typeTicketEntity.reward_id.Value], typeTicketEntity.is_new, typeTicketEntity.reward_quantity, typeTicketEntity.reward_id.Value, typeTicketEntity.reward_type_id);
    PlayerQuestKey[] array = SMManager.Get<PlayerQuestKey[]>();
    foreach (WebAPI.Response.QuestCorpsBattleFinishDrop_quest_key_entities dropQuestKeyEntity in result.drop_quest_key_entities)
    {
      WebAPI.Response.QuestCorpsBattleFinishDrop_quest_key_entities r = dropQuestKeyEntity;
      this.AddReward((object) Array.Find<PlayerQuestKey>(array, (Predicate<PlayerQuestKey>) (x =>
      {
        int questKeyId = x.quest_key_id;
        int? rewardId = r.reward_id;
        int valueOrDefault = rewardId.GetValueOrDefault();
        return questKeyId == valueOrDefault & rewardId.HasValue;
      })), r.is_new, r.reward_quantity, r.reward_id.Value, r.reward_type_id);
    }
    foreach (WebAPI.Response.QuestCorpsBattleFinishDrop_unit_ticket_entities unitTicketEntity in result.drop_unit_ticket_entities)
      this.AddReward((object) MasterData.SelectTicket[unitTicketEntity.reward_id.Value], unitTicketEntity.is_new, unitTicketEntity.reward_quantity, unitTicketEntity.reward_id.Value, unitTicketEntity.reward_type_id);
    foreach (WebAPI.Response.QuestCorpsBattleFinishDrop_common_ticket_entities commonTicketEntity in result.drop_common_ticket_entities)
      this.AddReward((object) MasterData.CommonTicket[commonTicketEntity.reward_id.Value], commonTicketEntity.is_new, commonTicketEntity.reward_quantity, commonTicketEntity.reward_id.Value, commonTicketEntity.reward_type_id);
    e = this.SetReward();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.Grid.GetComponent<UIGrid>().Reposition();
    this.ScrollView.GetComponent<UIScrollView>().ResetPosition();
    this.mainObject.SetActive(false);
    this.mainTitleObject.SetActive(false);
  }

  public override IEnumerator Run()
  {
    BattleUI05RewardMenu battleUi05RewardMenu = this;
    battleUi05RewardMenu.isQuestAutoLap = Singleton<NGGameDataManager>.GetInstance().questAutoLap;
    BattleUI05ResultMenu component = ((Component) battleUi05RewardMenu).GetComponent<BattleUI05ResultMenu>();
    if (Object.op_Inequality((Object) component, (Object) null))
    {
      if (component.BonusFlg[3])
        component.SetBonus(component.BonusCategory, true);
      else
        component.DisableBonusTitle();
    }
    battleUi05RewardMenu.mainObject.SetActive(true);
    battleUi05RewardMenu.mainTitleObject.SetActive(true);
    battleUi05RewardMenu.Title.SetActive(false);
    battleUi05RewardMenu.NoneMessage.SetActive(false);
    if (battleUi05RewardMenu.PlayRewards.Count <= 0)
    {
      battleUi05RewardMenu.NoneMessage.SetActive(true);
    }
    else
    {
      yield return (object) new WaitForSeconds(0.5f);
      IEnumerator e = battleUi05RewardMenu.OpenRewardIcon();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public override IEnumerator OnFinish()
  {
    foreach (BattleUI05RewardMenu.Reward playReward in this.PlayRewards)
    {
      UnitIcon component1 = playReward.gameObject.GetComponent<UnitIcon>();
      ItemIcon component2 = playReward.gameObject.GetComponent<ItemIcon>();
      if (Object.op_Inequality((Object) component1, (Object) null))
        ((Collider) component1.buttonBoxCollider).enabled = false;
      if (Object.op_Inequality((Object) component2, (Object) null))
      {
        ((Component) component2.gear.button).gameObject.SetActive(false);
        ((Component) component2.supply.button).gameObject.SetActive(false);
      }
    }
    this.mainObject.SetActive(false);
    this.mainTitleObject.SetActive(false);
    yield break;
  }

  private void AddReward(
    object master,
    bool is_new,
    int quantity,
    int entity_id,
    int entity_type)
  {
    this.PlayRewards.Add(new BattleUI05RewardMenu.Reward()
    {
      master = master,
      is_new = is_new,
      quantity = quantity,
      entity_id = entity_id,
      entity_type = entity_type
    });
  }

  private List<BattleUI05RewardMenu.Reward> RewardRoundUp()
  {
    List<BattleUI05RewardMenu.Reward> source = new List<BattleUI05RewardMenu.Reward>();
    foreach (BattleUI05RewardMenu.Reward playReward1 in this.PlayRewards)
    {
      BattleUI05RewardMenu.Reward playReward = playReward1;
      if (playReward.entity_type == 2 || playReward.entity_type == 20 || playReward.entity_type == 34 || playReward.entity_type == 23 || playReward.entity_type == 40)
      {
        if (source.Any<BattleUI05RewardMenu.Reward>((Func<BattleUI05RewardMenu.Reward, bool>) (x => x.entity_type == playReward.entity_type && x.entity_id == playReward.entity_id)))
        {
          source.First<BattleUI05RewardMenu.Reward>((Func<BattleUI05RewardMenu.Reward, bool>) (x => x.entity_type == playReward.entity_type && x.entity_id == playReward.entity_id)).quantity += playReward.quantity;
          continue;
        }
      }
      else if (playReward.entity_type == 24)
      {
        PlayerMaterialUnit master = playReward.master as PlayerMaterialUnit;
        BattleUI05RewardMenu.Reward reward = source.FirstOrDefault<BattleUI05RewardMenu.Reward>((Func<BattleUI05RewardMenu.Reward, bool>) (x => x.entity_type == playReward.entity_type && x.master is PlayerMaterialUnit playerObj && playerObj._unit == master._unit));
        if (reward != null)
        {
          reward.quantity += playReward.quantity;
          continue;
        }
      }
      else if (playReward.entity_type == 3)
      {
        PlayerItem master = playReward.master as PlayerItem;
        PlayerItem playerObj;
        BattleUI05RewardMenu.Reward reward = source.FirstOrDefault<BattleUI05RewardMenu.Reward>((Func<BattleUI05RewardMenu.Reward, bool>) (x => x.entity_type == playReward.entity_type && (playerObj = x.master as PlayerItem) != (PlayerItem) null && playerObj.entity_id == master.entity_id));
        if (reward != null)
        {
          reward.quantity += playReward.quantity;
          continue;
        }
      }
      else if (playReward.entity_type == 26)
      {
        PlayerMaterialGear master = playReward.master as PlayerMaterialGear;
        PlayerMaterialGear playerObj;
        BattleUI05RewardMenu.Reward reward = source.FirstOrDefault<BattleUI05RewardMenu.Reward>((Func<BattleUI05RewardMenu.Reward, bool>) (x => x.entity_type == playReward.entity_type && (playerObj = x.master as PlayerMaterialGear) != (PlayerMaterialGear) null && playerObj.gear_id == master.gear_id));
        if (reward != null)
        {
          reward.quantity += playReward.quantity;
          continue;
        }
      }
      else if (playReward.entity_type == 19)
      {
        PlayerQuestKey master = playReward.master as PlayerQuestKey;
        BattleUI05RewardMenu.Reward reward = source.FirstOrDefault<BattleUI05RewardMenu.Reward>((Func<BattleUI05RewardMenu.Reward, bool>) (x => x.entity_type == playReward.entity_type && x.master is PlayerQuestKey playerObj && playerObj.quest_key_id == master.quest_key_id));
        if (reward != null)
        {
          reward.quantity += playReward.quantity;
          continue;
        }
      }
      source.Add(playReward);
    }
    return source;
  }

  public IEnumerator SetReward()
  {
    UIGrid grid = this.Grid.GetComponent<UIGrid>();
    GameObject unknownUnit = this.CreateUnknownUnit();
    GameObject unknownItem = this.CreateUnknownItem();
    unknownUnit.SetActive(false);
    unknownItem.SetActive(false);
    this.PlayRewards = this.RewardRoundUp();
    foreach (BattleUI05RewardMenu.Reward reward in this.PlayRewards)
    {
      PlayerUnit unit;
      PlayerItem gear;
      UnitIcon icon1;
      IEnumerator e;
      ItemIcon icon2;
      if ((unit = reward.master as PlayerUnit) != (PlayerUnit) null)
      {
        this.createWrapper(reward, ((Component) grid).transform, "unit wrapper", unknownUnit, this.mUnitPrefab);
        icon1 = reward.gameObject.GetComponent<UnitIcon>();
        PlayerUnit[] playerUnits = new PlayerUnit[1]{ unit };
        e = icon1.SetPlayerUnit(unit, playerUnits, (PlayerUnit) null, false, false);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        if (unit.unit.IsMaterialUnit)
        {
          icon1.RarityCenter();
        }
        else
        {
          icon1.setBottom(unit);
          icon1.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
        }
        icon1.Button.onLongPress.Clear();
        EventDelegate.Add(((Component) icon1.Button).gameObject.GetOrAddComponent<UIButton>().onClick, this.UnitClickEvent(unit, reward.is_new));
        icon1.newUnit.SetActive(reward.is_new);
        icon1.BottomModeValue = UnitIconBase.GetBottomMode(unit.unit, (PlayerUnit) null);
        icon1 = (UnitIcon) null;
      }
      else if (reward.master is PlayerMaterialUnit materialUnit)
      {
        this.createWrapper(reward, ((Component) grid).transform, "material unit wrapper", unknownUnit, this.mUnitPrefab);
        PlayerUnit unitData = PlayerUnit.CreateByPlayerMaterialUnit(materialUnit);
        icon1 = reward.gameObject.GetComponent<UnitIcon>();
        PlayerUnit[] playerUnits = new PlayerUnit[1]
        {
          unitData
        };
        e = icon1.SetPlayerUnit(unitData, playerUnits, (PlayerUnit) null, false, false);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        if (unitData.unit.IsMaterialUnit)
        {
          icon1.RarityCenter();
        }
        else
        {
          icon1.setBottom(unitData);
          icon1.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
        }
        icon1.Button.onLongPress.Clear();
        EventDelegate.Add(((Component) icon1.Button).gameObject.GetOrAddComponent<UIButton>().onClick, this.UnitClickEvent(unitData, reward.is_new));
        icon1.SetCounter(reward.quantity);
        icon1.newUnit.SetActive(reward.is_new);
        icon1.BottomModeValue = UnitIconBase.GetBottomMode(materialUnit.unit, (PlayerUnit) null);
        unitData = (PlayerUnit) null;
        icon1 = (UnitIcon) null;
      }
      else if ((gear = reward.master as PlayerItem) != (PlayerItem) null)
      {
        this.createWrapper(reward, ((Component) grid).transform, "gear wrapper", unknownItem, this.mItemIconPrefab);
        icon2 = reward.gameObject.GetComponent<ItemIcon>();
        e = icon2.InitByPlayerItem(gear);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        icon2.gear.broken.SetActive(false);
        icon2.gear.button.onLongPress.Clear();
        EventDelegate.Add(((Component) icon2.gear.button).gameObject.GetOrAddComponent<UIButton>().onClick, this.GearClickEvent(gear, reward.is_new));
        icon2.EnableQuantity(reward.quantity);
        icon2.gear.newGear.SetActive(reward.is_new);
        icon2.BottomModeValue = ItemIcon.BottomMode.Visible;
        icon2.SetQuantityPositionY(45f);
        icon2 = (ItemIcon) null;
      }
      else
      {
        PlayerMaterialGear master1;
        if ((master1 = reward.master as PlayerMaterialGear) != (PlayerMaterialGear) null)
        {
          this.createWrapper(reward, ((Component) grid).transform, "material gear wrapper", unknownItem, this.mItemIconPrefab);
          icon2 = reward.gameObject.GetComponent<ItemIcon>();
          e = icon2.InitByPlayerMaterialGear(master1);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          icon2.gear.broken.SetActive(false);
          icon2.gear.button.onLongPress.Clear();
          EventDelegate.Add(((Component) icon2.gear.button).gameObject.GetOrAddComponent<UIButton>().onClick, this.GearClickEvent(gear, reward.is_new));
          icon2.EnableQuantity(reward.quantity);
          icon2.gear.newGear.SetActive(reward.is_new);
          icon2.BottomModeValue = ItemIcon.BottomMode.Visible;
          icon2.SetQuantityPositionY(45f);
          icon2 = (ItemIcon) null;
        }
        else if (reward.master is SupplySupply master6)
        {
          this.createWrapper(reward, ((Component) grid).transform, "supply wrapper", unknownItem, this.mItemIconPrefab);
          icon2 = reward.gameObject.GetComponent<ItemIcon>();
          e = icon2.InitBySupply(master6);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          icon2.EnableQuantity(reward.quantity);
          icon2.EnableQuantity(reward.quantity);
          icon2.BottomModeValue = ItemIcon.BottomMode.Visible;
          icon2.SetQuantityPositionY(45f);
          icon2 = (ItemIcon) null;
        }
        else if (reward.master is GachaTicket)
        {
          this.createWrapper(reward, ((Component) grid).transform, "gacha ticket wrapper", unknownItem, this.mUniqueIconsPrefab);
          e = reward.gameObject.GetComponent<UniqueIcons>().SetGachaTicket(reward.quantity, reward.entity_id);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
        else if (reward.master is UnitTypeTicket master5)
        {
          this.createWrapper(reward, ((Component) grid).transform, "unit type ticket wrapper", unknownItem, this.mUniqueIconsPrefab);
          e = reward.gameObject.GetComponent<UniqueIcons>().SetReincarnationTypeTicket(master5.ID, reward.quantity);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
        else if (reward.master is PlayerQuestKey master4)
        {
          this.createWrapper(reward, ((Component) grid).transform, "quest key wrapper", unknownItem, this.mUniqueIconsPrefab);
          yield return (object) reward.gameObject.GetComponent<UniqueIcons>().SetKey(master4.quest_key_id, reward.quantity);
        }
        else if (reward.master is MasterDataTable.SelectTicket master3)
        {
          this.createWrapper(reward, ((Component) grid).transform, "unit ticket wrapper", unknownItem, this.mUniqueIconsPrefab);
          UniqueIcons component = reward.gameObject.GetComponent<UniqueIcons>();
          if (master3.category == SelectTicketCategory.Unit)
          {
            e = component.SetKillersTicket(master3.ID, reward.quantity);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
          }
          else
          {
            e = component.SetMaterialTicket(master3.ID, reward.quantity);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
          }
        }
        else if (reward.master is CommonTicket master2)
        {
          this.createWrapper(reward, ((Component) grid).transform, "common ticket wrapper", unknownItem, this.mUniqueIconsPrefab);
          yield return (object) reward.gameObject.GetComponent<UniqueIcons>().SetCommonTicket(master2.ID, reward.quantity);
        }
        else
          Debug.LogError((object) ("クエストドロップに対応していない種別です: " + reward.master));
      }
      reward.unknownObject.SetActive(true);
      reward.gameObject.SetActive(false);
      unit = (PlayerUnit) null;
      materialUnit = (PlayerMaterialUnit) null;
      gear = (PlayerItem) null;
    }
    grid.repositionNow = true;
    Object.Destroy((Object) unknownUnit);
    Object.Destroy((Object) unknownItem);
  }

  private void createWrapper(
    BattleUI05RewardMenu.Reward reward,
    Transform top,
    string objName,
    GameObject unknownObject,
    GameObject iconPrefab)
  {
    GameObject gameObject = new GameObject(objName);
    gameObject.transform.parent = top;
    gameObject.transform.localScale = new Vector3(1f, 1f);
    reward.unknownObject = unknownObject.Clone(gameObject.transform);
    reward.gameObject = iconPrefab.Clone(gameObject.transform);
  }

  private GameObject CreateUnknownUnit(Transform parent = null)
  {
    GameObject unknownUnit = this.mUnitPrefab.Clone(parent);
    UnitIcon component = unknownUnit.GetComponent<UnitIcon>();
    component.BottomModeValue = UnitIconBase.BottomMode.Nothing;
    component.BackgroundModeValue = UnitIcon.BackgroundMode.PlayerShadow;
    component.Unknown = true;
    component.NewUnit = false;
    ((Component) ((Component) component).transform.Find("icon")).gameObject.SetActive(false);
    return unknownUnit;
  }

  private GameObject CreateUnknownItem(Transform parent = null)
  {
    GameObject unknownItem = this.mItemIconPrefab.Clone(parent);
    ItemIcon component = unknownItem.GetComponent<ItemIcon>();
    component.SetEmpty(true);
    component.gear.unknown.SetActive(true);
    component.BottomModeValue = ItemIcon.BottomMode.Nothing;
    return unknownItem;
  }

  private EventDelegate.Callback UnitClickEvent(PlayerUnit unit, bool isNew)
  {
    return (EventDelegate.Callback) (() => { });
  }

  private EventDelegate.Callback GearClickEvent(PlayerItem item, bool isNew)
  {
    return (EventDelegate.Callback) (() => { });
  }

  private IEnumerator InitPopup(GameObject popup, IEnumerator setter)
  {
    BattleUI05RewardMenu battleUi05RewardMenu = this;
    popup.SetActive(false);
    while (setter.MoveNext())
      yield return setter.Current;
    UIWidget component = ((Component) popup.transform.Find("Top")).GetComponent<UIWidget>();
    ((UIRect) ((Component) popup.transform.Find("Bottom")).GetComponent<UIWidget>()).topAnchor.absolute -= 120;
    ((UIRect) component).bottomAnchor.absolute += 195;
    battleUi05RewardMenu.CreateTouchObject((EventDelegate.Callback) (() => Singleton<PopupManager>.GetInstance().dismiss()), popup.transform);
    popup.SetActive(true);
  }

  private IEnumerator OpenRewardIcon()
  {
    BattleUI05RewardMenu battleUi05RewardMenu = this;
    bool isSkip = false;
    GameObject touchObj = battleUi05RewardMenu.CreateTouchObject((EventDelegate.Callback) (() => isSkip = true));
    if (battleUi05RewardMenu.isQuestAutoLap)
      isSkip = true;
    for (int i = 0; i < battleUi05RewardMenu.PlayRewards.Count; ++i)
    {
      BattleUI05RewardMenu.Reward playReward = battleUi05RewardMenu.PlayRewards[i];
      TweenAlpha tweenAlpha1 = playReward.unknownObject.AddComponent<TweenAlpha>();
      tweenAlpha1.to = 0.0f;
      tweenAlpha1.from = 1f;
      ((UITweener) tweenAlpha1).duration = 0.25f;
      playReward.gameObject.SetActive(true);
      TweenAlpha tweenAlpha2 = playReward.gameObject.AddComponent<TweenAlpha>();
      tweenAlpha2.to = 1f;
      tweenAlpha2.from = 0.0f;
      ((UITweener) tweenAlpha2).duration = 0.25f;
      if (!isSkip)
      {
        battleUi05RewardMenu.soundManager.playSE("SE_1021", seChannel: i % 3);
        yield return (object) new WaitForSeconds(0.25f);
      }
    }
    if (isSkip)
      yield return (object) new WaitForSeconds(0.75f);
    Object.Destroy((Object) touchObj);
  }

  public void DisableReward()
  {
    this.mainObject.SetActive(false);
    this.mainTitleObject.SetActive(false);
  }

  private class Reward
  {
    public GameObject unknownObject;
    public GameObject gameObject;
    public object master;
    public bool is_new;
    public int quantity;
    public int entity_id;
    public int entity_type;
  }
}
