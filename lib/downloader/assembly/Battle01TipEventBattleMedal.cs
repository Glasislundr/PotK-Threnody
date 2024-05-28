// Decompiled with JetBrains decompiler
// Type: Battle01TipEventBattleMedal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Battle01TipEventBattleMedal : Battle01TipEventBase
{
  public override IEnumerator onInitAsync()
  {
    Battle01TipEventBattleMedal eventBattleMedal = this;
    Future<GameObject> f = Res.Icons.UniqueIconPrefab.Load<GameObject>();
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    UniqueIcons icon = eventBattleMedal.cloneIcon<UniqueIcons>(f.Result);
    e = icon.SetBattleMedal();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    icon.BackGroundActivated = false;
    icon.LabelActivated = false;
    eventBattleMedal.selectIcon(0);
  }

  public override void setData(BL.DropData e, BL.Unit unit)
  {
    if (e.reward.Type != MasterDataTable.CommonRewardType.battle_medal)
      return;
    Dictionary<string, int> args = new Dictionary<string, int>()
    {
      {
        "medal",
        e.reward.Quantity
      }
    };
    if (e.reward.Id != 0)
      this.setText(Consts.Format(Consts.GetInstance().TipEvent_text_limited_battle_medal, (IDictionary) args));
    else
      this.setText(Consts.Format(Consts.GetInstance().TipEvent_text_battle_medal, (IDictionary) args));
  }
}
