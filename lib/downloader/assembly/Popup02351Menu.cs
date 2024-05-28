// Decompiled with JetBrains decompiler
// Type: Popup02351Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Popup02351Menu : Popup0235MenuBase
{
  [SerializeField]
  private UI2DSprite icon;
  [SerializeField]
  private UILabel TxtTitle;
  [SerializeField]
  private UILabel TxtDescription1;
  [SerializeField]
  private UILabel TxtDescription2;

  public override IEnumerator Init(
    ResultMenuBase.CampaignReward reward,
    ResultMenuBase.CampaignNextReward nextReward,
    GameObject gearObject,
    GameObject unitObject,
    GameObject uniqueObject)
  {
    this.TxtTitle.SetText(reward.show_title);
    this.TxtDescription1.SetText(reward.show_text);
    this.TxtDescription2.SetText(reward.show_text2);
    IEnumerator e;
    if (reward.reward_type_id == 3 || reward.reward_type_id == 26 || reward.reward_type_id == 35)
    {
      GearGear gear = ((IEnumerable<GearGear>) MasterData.GearGearList).FirstOrDefault<GearGear>((Func<GearGear, bool>) (x => x.ID == reward.reward_id));
      if (reward.reward_type_id == 35)
      {
        e = ColosseumUtility.CreateWeaponMaterialIcon(gearObject, gear, ((Component) this.icon).transform);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      else
      {
        e = ColosseumUtility.CreateGearIcon(gearObject, gear, ((Component) this.icon).transform);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
    else if (reward.reward_type_id == 1 || reward.reward_type_id == 24)
    {
      e = ColosseumUtility.CreateUnitIcon(unitObject, ((IEnumerable<UnitUnit>) MasterData.UnitUnitList).FirstOrDefault<UnitUnit>((Func<UnitUnit, bool>) (x => x.ID == reward.reward_id)), ((Component) this.icon).transform);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else if (reward.reward_type_id == 2)
    {
      e = ColosseumUtility.CreateSupplyIcon(gearObject, ((IEnumerable<SupplySupply>) MasterData.SupplySupplyList).FirstOrDefault<SupplySupply>((Func<SupplySupply, bool>) (x => x.ID == reward.reward_id)), ((Component) this.icon).transform);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      e = ColosseumUtility.CreateUniqueIcon(uniqueObject, ((Component) this.icon).transform, reward.reward_type_id, reward.reward_id, 0, false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }
}
