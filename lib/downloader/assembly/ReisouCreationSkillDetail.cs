// Decompiled with JetBrains decompiler
// Type: ReisouCreationSkillDetail
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ReisouCreationSkillDetail : MonoBehaviour
{
  [SerializeField]
  protected GameObject dirIcon;
  [SerializeField]
  protected UILabel txtName;
  [SerializeField]
  protected UILabel[] txtParameter;
  protected GameObject reisouPopupReisouCreationPrefab;
  protected GearReisouChaosCreation recipe;
  protected PlayerItem playerItem;
  protected Action cbCreation;

  public virtual IEnumerator Init(GearReisouChaosCreation recipe, Action cbCreation)
  {
    ReisouCreationSkillDetail creationSkillDetail1 = this;
    creationSkillDetail1.recipe = recipe;
    creationSkillDetail1.playerItem = new PlayerItem(recipe.chaos_ID, MasterDataTable.CommonRewardType.gear);
    creationSkillDetail1.cbCreation = cbCreation;
    ReisouCreationSkillDetail creationSkillDetail = creationSkillDetail1;
    Future<GameObject> prefabF = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ItemIcon itemIcon = prefabF.Result.Clone(creationSkillDetail1.dirIcon.transform).GetComponent<ItemIcon>();
    e = itemIcon.InitByPlayerItem(creationSkillDetail1.playerItem);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    itemIcon.EnableLongPressEventReisou(creationSkillDetail1.playerItem);
    itemIcon.onClick = (Action<ItemIcon>) (item => itemIcon.OpenReisouDetailPopup(item.ItemInfo, creationSkillDetail.playerItem));
    prefabF = (Future<GameObject>) null;
    creationSkillDetail1.txtName.SetTextLocalize(creationSkillDetail1.playerItem.name);
    for (int index = 0; index < creationSkillDetail1.txtParameter.Length; ++index)
      creationSkillDetail1.txtParameter[index].SetTextLocalize("");
    GearReisouFusion fusionPossibleRecipe = creationSkillDetail1.playerItem.GetReisouFusionPossibleRecipe(SMManager.Get<PlayerItem[]>());
    if (fusionPossibleRecipe != null)
    {
      GearGear holyId = fusionPossibleRecipe.holy_ID;
      List<string> paramTextList = new List<string>();
      creationSkillDetail1.SetChaosParamSkillStrings(paramTextList, creationSkillDetail1.playerItem, creationSkillDetail1.playerItem.reisouRankIncr, holyId);
      for (int index = 0; index < creationSkillDetail1.txtParameter.Length; ++index)
      {
        if (paramTextList.Count > index)
          creationSkillDetail1.txtParameter[index].SetTextLocalize(paramTextList[index]);
      }
    }
  }

  protected void SetChaosParamSkillStrings(
    List<string> paramTextList,
    PlayerItem playerItem,
    ReisouRankIncr rankIncr,
    GearGear holyReisou)
  {
    if (rankIncr.hp_incremental > 0 && rankIncr.hp_incremental != 100)
    {
      double num = (double) rankIncr.hp_incremental / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_HP.F((object) num);
      paramTextList.Add(str);
    }
    if (rankIncr.strength_incremental > 0 && rankIncr.strength_incremental != 100)
    {
      double num = (double) rankIncr.strength_incremental / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_POWER.F((object) num);
      paramTextList.Add(str);
    }
    if (rankIncr.intelligence_incremental > 0 && rankIncr.intelligence_incremental != 100)
    {
      double num = (double) rankIncr.intelligence_incremental / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_MAGIC_POWER.F((object) num);
      paramTextList.Add(str);
    }
    if (rankIncr.vitality_incremental > 0 && rankIncr.vitality_incremental != 100)
    {
      double num = (double) rankIncr.vitality_incremental / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_VITALITY.F((object) num);
      paramTextList.Add(str);
    }
    if (rankIncr.mind_incremental > 0 && rankIncr.mind_incremental != 100)
    {
      double num = (double) rankIncr.mind_incremental / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_MIND.F((object) num);
      paramTextList.Add(str);
    }
    if (rankIncr.agility_incremental > 0 && rankIncr.agility_incremental != 100)
    {
      double num = (double) rankIncr.agility_incremental / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_AGILITY.F((object) num);
      paramTextList.Add(str);
    }
    if (rankIncr.dexterity_incremental > 0 && rankIncr.dexterity_incremental != 100)
    {
      double num = (double) rankIncr.dexterity_incremental / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_DEXTERITY.F((object) num);
      paramTextList.Add(str);
    }
    if (rankIncr.lucky_incremental > 0 && rankIncr.lucky_incremental != 100)
    {
      double num = (double) rankIncr.lucky_incremental / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_LUCK.F((object) num);
      paramTextList.Add(str);
    }
    int num1 = 0;
    if (playerItem.gear.attack_type == GearAttackType.physical)
      num1 = rankIncr.power;
    if (num1 > 0 && num1 != 100)
    {
      double num2 = (double) num1 / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_PHY_POW.F((object) num2);
      paramTextList.Add(str);
    }
    int num3 = 0;
    if (playerItem.gear.attack_type == GearAttackType.magic)
      num3 = rankIncr.power;
    if (num3 > 0 && num3 != 100)
    {
      double num4 = (double) num3 / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_MAG_POW.F((object) num4);
      paramTextList.Add(str);
    }
    if (rankIncr.physical_defense > 0 && rankIncr.physical_defense != 100)
    {
      double num5 = (double) rankIncr.physical_defense / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_PHY_DEF.F((object) num5);
      paramTextList.Add(str);
    }
    if (rankIncr.magic_defense > 0 && rankIncr.magic_defense != 100)
    {
      double num6 = (double) rankIncr.magic_defense / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_MAG_DEF.F((object) num6);
      paramTextList.Add(str);
    }
    if (rankIncr.hit > 0 && rankIncr.hit != 100)
    {
      double num7 = (double) rankIncr.hit / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_HIT.F((object) num7);
      paramTextList.Add(str);
    }
    if (rankIncr.critical > 0 && rankIncr.critical != 100)
    {
      double num8 = (double) rankIncr.critical / 100.0;
      string str = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_CRITICAL.F((object) num8);
      paramTextList.Add(str);
    }
    if (rankIncr.evasion <= 0 || rankIncr.evasion == 100)
      return;
    double num9 = (double) rankIncr.evasion / 100.0;
    string str1 = Consts.GetInstance().UNIT_0044_CHAOS_REISOU_EVASION.F((object) num9);
    paramTextList.Add(str1);
  }

  public void onBtnCreation() => this.StartCoroutine(this.OpenReisouCreationPopupAsync());

  protected IEnumerator OpenReisouCreationPopupAsync()
  {
    IEnumerator e;
    if (Object.op_Equality((Object) this.reisouPopupReisouCreationPrefab, (Object) null))
    {
      Future<GameObject> popupPrefabF = new ResourceObject("Prefabs/popup/popup_Reisou_Creation").Load<GameObject>();
      e = popupPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.reisouPopupReisouCreationPrefab = popupPrefabF.Result;
      popupPrefabF = (Future<GameObject>) null;
    }
    GameObject popup = this.reisouPopupReisouCreationPrefab.Clone();
    PopupReisouCreation component = popup.GetComponent<PopupReisouCreation>();
    popup.SetActive(false);
    e = component.Init(this.recipe, this.cbCreation);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    popup.SetActive(true);
    Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
  }
}
