// Decompiled with JetBrains decompiler
// Type: ItemDetailPopupBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class ItemDetailPopupBase : BackButtonMenuBase
{
  [SerializeField]
  private GameObject itemDetails_01;
  [SerializeField]
  private GameObject itemDetails_02;
  [SerializeField]
  protected GameObject commonPop;
  [SerializeField]
  protected Shop00742Unit unitPop;
  [SerializeField]
  protected Shop00742Item itemPop;
  [SerializeField]
  protected Shop00742Bugu buguPop;
  [SerializeField]
  protected Shop00742Key keyPop;
  [SerializeField]
  protected Shop00742KisekiAndMedal kisekiPop;
  [SerializeField]
  protected Shop00742BuguOther buguOtherPop;
  [SerializeField]
  protected Shop00742UnitOther unitOtherPop;
  [SerializeField]
  protected Shop00742SeasonTicket seasonTicketPop;
  [SerializeField]
  protected Shop00742AwakeSkill awakeSkill;
  [SerializeField]
  protected Shop00742Bugu buguCrystal;
  [SerializeField]
  protected Shop00742CommonPoint commonPoint;
  [SerializeField]
  protected Shop00742CommonTicket commonTicket;
  private Action noAction;

  public void SetAction(Action act) => this.noAction = act;

  public IEnumerator SetInfo(MasterDataTable.CommonRewardType type, int entity_id, int count = 0)
  {
    ItemDetailPopupBase itemDetailPopupBase = this;
    ((UIRect) ((Component) itemDetailPopupBase).GetComponent<UIWidget>()).alpha = 0.0f;
    ((Component) itemDetailPopupBase.unitPop).gameObject.SetActive(false);
    ((Component) itemDetailPopupBase.itemPop).gameObject.SetActive(false);
    ((Component) itemDetailPopupBase.buguPop).gameObject.SetActive(false);
    ((Component) itemDetailPopupBase.keyPop).gameObject.SetActive(false);
    ((Component) itemDetailPopupBase.buguOtherPop).gameObject.SetActive(false);
    ((Component) itemDetailPopupBase.unitOtherPop).gameObject.SetActive(false);
    ((Component) itemDetailPopupBase.seasonTicketPop).gameObject.SetActive(false);
    ((Component) itemDetailPopupBase.awakeSkill).gameObject.SetActive(false);
    ((Component) itemDetailPopupBase.buguCrystal).gameObject.SetActive(false);
    IEnumerator e;
    if (type == MasterDataTable.CommonRewardType.unit || type == MasterDataTable.CommonRewardType.material_unit)
    {
      UnitUnit target = MasterData.UnitUnit[entity_id];
      if (target.IsNormalUnit)
      {
        ((Component) itemDetailPopupBase.unitPop).gameObject.SetActive(true);
        e = itemDetailPopupBase.unitPop.Init(target);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      else
      {
        ((Component) itemDetailPopupBase.unitOtherPop).gameObject.SetActive(true);
        e = itemDetailPopupBase.unitOtherPop.Initialize(target);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
    else if (type == MasterDataTable.CommonRewardType.money || type == MasterDataTable.CommonRewardType.player_exp || type == MasterDataTable.CommonRewardType.friend_point)
    {
      itemDetailPopupBase.itemDetails_01.SetActive(false);
      itemDetailPopupBase.itemDetails_02.SetActive(true);
      ((Component) itemDetailPopupBase.commonPoint).gameObject.SetActive(true);
      itemDetailPopupBase.commonPoint.Init(type, count);
    }
    else if (type == MasterDataTable.CommonRewardType.supply)
    {
      ((Component) itemDetailPopupBase.itemPop).gameObject.SetActive(true);
      e = itemDetailPopupBase.itemPop.Init(entity_id);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else if (type == MasterDataTable.CommonRewardType.gear || type == MasterDataTable.CommonRewardType.material_gear)
    {
      GearGear target = MasterData.GearGear[entity_id];
      if (target.kind.isEquip)
      {
        ((Component) itemDetailPopupBase.buguPop).gameObject.SetActive(true);
        e = itemDetailPopupBase.buguPop.Init(target);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      else
      {
        ((Component) itemDetailPopupBase.buguOtherPop).gameObject.SetActive(true);
        e = itemDetailPopupBase.buguOtherPop.Initialize(target);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
    else if (type == MasterDataTable.CommonRewardType.gear_body)
    {
      GearGear target = MasterData.GearGear[entity_id];
      ((Component) itemDetailPopupBase.buguCrystal).gameObject.SetActive(true);
      e = itemDetailPopupBase.buguCrystal.Init(target);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else if (type == MasterDataTable.CommonRewardType.coin || type == MasterDataTable.CommonRewardType.medal || type == MasterDataTable.CommonRewardType.battle_medal)
    {
      ((Component) itemDetailPopupBase.keyPop).gameObject.SetActive(true);
      ((Behaviour) itemDetailPopupBase.kisekiPop).enabled = true;
      ((Behaviour) itemDetailPopupBase.keyPop).enabled = false;
      e = itemDetailPopupBase.kisekiPop.Init(type);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      switch (type)
      {
        case MasterDataTable.CommonRewardType.quest_key:
          ((Component) itemDetailPopupBase.keyPop).gameObject.SetActive(true);
          ((Behaviour) itemDetailPopupBase.keyPop).enabled = true;
          ((Behaviour) itemDetailPopupBase.kisekiPop).enabled = false;
          e = itemDetailPopupBase.keyPop.Init(entity_id);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          break;
        case MasterDataTable.CommonRewardType.season_ticket:
          ((Component) itemDetailPopupBase.seasonTicketPop).gameObject.SetActive(true);
          e = itemDetailPopupBase.seasonTicketPop.Init(entity_id);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          break;
        case MasterDataTable.CommonRewardType.awake_skill:
          itemDetailPopupBase.commonPop.gameObject.SetActive(false);
          ((Component) itemDetailPopupBase.awakeSkill).gameObject.SetActive(true);
          e = itemDetailPopupBase.awakeSkill.Init(entity_id, itemDetailPopupBase);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          break;
        default:
          if (type == MasterDataTable.CommonRewardType.gacha_ticket || type == MasterDataTable.CommonRewardType.unit_ticket || type == MasterDataTable.CommonRewardType.reincarnation_type_ticket || type == MasterDataTable.CommonRewardType.stamp || type == MasterDataTable.CommonRewardType.recovery_item || type == MasterDataTable.CommonRewardType.common_ticket)
          {
            itemDetailPopupBase.itemDetails_01.SetActive(false);
            itemDetailPopupBase.itemDetails_02.SetActive(true);
            ((Component) itemDetailPopupBase.commonTicket).gameObject.SetActive(true);
            e = itemDetailPopupBase.commonTicket.Init(type, entity_id);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
          }
          break;
      }
    }
    ((UIRect) ((Component) itemDetailPopupBase).GetComponent<UIWidget>()).alpha = 1f;
  }

  public virtual void IbtnNo()
  {
    if (this.noAction == null)
    {
      if (this.IsPushAndSet())
        return;
      Singleton<PopupManager>.GetInstance().onDismiss();
    }
    else
      this.noAction();
  }

  public override void onBackButton() => this.IbtnNo();
}
