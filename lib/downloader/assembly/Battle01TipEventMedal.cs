// Decompiled with JetBrains decompiler
// Type: Battle01TipEventMedal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Battle01TipEventMedal : Battle01TipEventBase
{
  public override IEnumerator onInitAsync()
  {
    Battle01TipEventMedal battle01TipEventMedal = this;
    Future<GameObject> f = Res.Icons.UniqueIconPrefab.Load<GameObject>();
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    UniqueIcons icon = battle01TipEventMedal.cloneIcon<UniqueIcons>(f.Result);
    e = icon.SetMedal();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    icon.BackGroundActivated = false;
    icon.LabelActivated = false;
    battle01TipEventMedal.selectIcon(0);
  }

  public override void setData(BL.DropData e, BL.Unit unit)
  {
    if (e.reward.Type != MasterDataTable.CommonRewardType.medal)
      return;
    this.setText(Consts.Format(Consts.GetInstance().TipEvent_text_medal, (IDictionary) new Dictionary<string, int>()
    {
      ["medal"] = e.reward.Quantity
    }));
  }
}
