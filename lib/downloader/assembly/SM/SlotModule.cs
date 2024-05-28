// Decompiled with JetBrains decompiler
// Type: SM.SlotModule
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class SlotModule : KeyCompare
  {
    public SlotModuleSlot[] slot;

    public SlotModule()
    {
    }

    public SlotModule(Dictionary<string, object> json)
    {
      this._hasKey = false;
      List<SlotModuleSlot> slotModuleSlotList = new List<SlotModuleSlot>();
      foreach (object json1 in (List<object>) json[nameof (slot)])
        slotModuleSlotList.Add(json1 == null ? (SlotModuleSlot) null : new SlotModuleSlot((Dictionary<string, object>) json1));
      this.slot = slotModuleSlotList.ToArray();
    }
  }
}
