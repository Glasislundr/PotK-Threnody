﻿// Decompiled with JetBrains decompiler
// Type: Battle01TipEventPExp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;

#nullable disable
public class Battle01TipEventPExp : Battle01TipEventBase
{
  public override void setData(BL.DropData e, BL.Unit unit)
  {
    if (e.reward.Type != MasterDataTable.CommonRewardType.player_exp)
      return;
    this.setText(Consts.GetInstance().TipEvent_text_pexp);
  }
}
