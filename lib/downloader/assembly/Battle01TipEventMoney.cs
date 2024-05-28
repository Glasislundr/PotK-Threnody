// Decompiled with JetBrains decompiler
// Type: Battle01TipEventMoney
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using System.Collections.Generic;

#nullable disable
public class Battle01TipEventMoney : Battle01TipEventBase
{
  protected override IEnumerator Start_Original()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Battle01TipEventMoney battle01TipEventMoney = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    battle01TipEventMoney.selectIcon(0);
    return false;
  }

  public override void setData(BL.DropData e, BL.Unit unit)
  {
    if (e.reward.Type != MasterDataTable.CommonRewardType.money)
      return;
    this.setText(Consts.Format(Consts.GetInstance().TipEvent_text_money, (IDictionary) new Dictionary<string, int>()
    {
      ["money"] = e.reward.Quantity
    }));
  }
}
