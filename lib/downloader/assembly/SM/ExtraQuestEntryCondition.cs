// Decompiled with JetBrains decompiler
// Type: SM.ExtraQuestEntryCondition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class ExtraQuestEntryCondition : KeyCompare
  {
    public int banner_category;
    public int id;
    public int quest_id;

    public ExtraQuestEntryCondition()
    {
    }

    public ExtraQuestEntryCondition(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.banner_category = (int) (long) json[nameof (banner_category)];
      this.id = (int) (long) json[nameof (id)];
      this.quest_id = (int) (long) json[nameof (quest_id)];
    }
  }
}
