// Decompiled with JetBrains decompiler
// Type: SM.QuestSetting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class QuestSetting : KeyCompare
  {
    public int quest_m_id;
    public int quest_type_id;
    public int quest_s_id;

    public QuestSetting()
    {
    }

    public QuestSetting(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.quest_m_id = (int) (long) json[nameof (quest_m_id)];
      this.quest_type_id = (int) (long) json[nameof (quest_type_id)];
      this.quest_s_id = (int) (long) json[nameof (quest_s_id)];
    }
  }
}
