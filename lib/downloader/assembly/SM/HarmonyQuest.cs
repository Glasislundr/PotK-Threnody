// Decompiled with JetBrains decompiler
// Type: SM.HarmonyQuest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class HarmonyQuest : KeyCompare
  {
    public int quest_m_id;
    public bool is_disable;
    public bool is_playable;

    public HarmonyQuest()
    {
    }

    public HarmonyQuest(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.quest_m_id = (int) (long) json[nameof (quest_m_id)];
      this.is_disable = (bool) json[nameof (is_disable)];
      this.is_playable = (bool) json[nameof (is_playable)];
    }
  }
}
