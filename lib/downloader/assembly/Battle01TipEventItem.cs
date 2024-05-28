// Decompiled with JetBrains decompiler
// Type: Battle01TipEventItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Battle01TipEventItem : Battle01TipEventBase
{
  private ItemIcon icon;

  public override IEnumerator onInitAsync()
  {
    Battle01TipEventItem battle01TipEventItem = this;
    Future<GameObject> f = !battle01TipEventItem.battleManager.isSea ? Res.Prefabs.ItemIcon.prefab.Load<GameObject>() : Res.Prefabs.Sea.ItemIcon.prefab_sea.Load<GameObject>();
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    battle01TipEventItem.icon = battle01TipEventItem.cloneIcon<ItemIcon>(f.Result);
    battle01TipEventItem.icon.QuantitySupply = false;
    battle01TipEventItem.selectIcon(0);
  }

  private IEnumerator doSetIcon(SupplySupply supply)
  {
    IEnumerator e = this.icon.InitBySupply(supply);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator doSetIcon(GearGear gear)
  {
    IEnumerator e = this.icon.InitByGear(gear, gear.GetElement());
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public override void setData(BL.DropData e, BL.Unit unit)
  {
    if (e.reward.Type != MasterDataTable.CommonRewardType.gear && e.reward.Type != MasterDataTable.CommonRewardType.supply)
      return;
    Dictionary<string, string> args = new Dictionary<string, string>();
    args["item"] = "";
    switch (e.reward.Type)
    {
      case MasterDataTable.CommonRewardType.supply:
        if (MasterData.SupplySupply.ContainsKey(e.reward.Id))
        {
          SupplySupply supply = MasterData.SupplySupply[e.reward.Id];
          args["item"] = supply.name;
          Singleton<NGBattleManager>.GetInstance().StartCoroutine(this.doSetIcon(supply));
          break;
        }
        break;
      case MasterDataTable.CommonRewardType.gear:
        if (MasterData.GearGear.ContainsKey(e.reward.Id))
        {
          GearGear gear = MasterData.GearGear[e.reward.Id];
          args["item"] = gear.name;
          Singleton<NGBattleManager>.GetInstance().StartCoroutine(this.doSetIcon(gear));
          break;
        }
        break;
    }
    this.setText(Consts.Format(Consts.GetInstance().TipEvent_text_item, (IDictionary) args));
  }
}
