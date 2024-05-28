// Decompiled with JetBrains decompiler
// Type: Battle01TipEventGuildMedal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Battle01TipEventGuildMedal : Battle01TipEventBase
{
  public override IEnumerator onInitAsync()
  {
    Battle01TipEventGuildMedal tipEventGuildMedal = this;
    Future<GameObject> f = Res.Icons.UniqueIconPrefab.Load<GameObject>();
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    UniqueIcons icon = tipEventGuildMedal.cloneIcon<UniqueIcons>(f.Result);
    e = icon.SetGuildMedal();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    icon.BackGroundActivated = false;
    icon.LabelActivated = false;
    tipEventGuildMedal.selectIcon(0);
  }

  public override void setData(BL.DropData e, BL.Unit unit)
  {
    if (e.reward.Type != MasterDataTable.CommonRewardType.guild_medal)
      return;
    Dictionary<string, int> args = new Dictionary<string, int>()
    {
      {
        "medal",
        e.reward.Quantity
      }
    };
    this.setText(Consts.Format(Consts.GetInstance().TipEvent_text_guild_medal, (IDictionary) args));
  }
}
