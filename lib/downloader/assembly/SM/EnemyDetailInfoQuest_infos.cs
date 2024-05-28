// Decompiled with JetBrains decompiler
// Type: SM.EnemyDetailInfoQuest_infos
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class EnemyDetailInfoQuest_infos : KeyCompare
  {
    public int? quest_m_id;
    public int? quest_type_id;
    public string enemy_text;
    public bool is_play;
    public int min_point;
    public int order;
    public int? quest_s_id;

    public EnemyDetailInfoQuest_infos()
    {
    }

    public EnemyDetailInfoQuest_infos(Dictionary<string, object> json)
    {
      this._hasKey = false;
      long? nullable1;
      int? nullable2;
      if (json[nameof (quest_m_id)] != null)
      {
        nullable1 = (long?) json[nameof (quest_m_id)];
        nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable2 = new int?();
      this.quest_m_id = nullable2;
      int? nullable3;
      if (json[nameof (quest_type_id)] != null)
      {
        nullable1 = (long?) json[nameof (quest_type_id)];
        nullable3 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable3 = new int?();
      this.quest_type_id = nullable3;
      this.enemy_text = (string) json[nameof (enemy_text)];
      this.is_play = (bool) json[nameof (is_play)];
      this.min_point = (int) (long) json[nameof (min_point)];
      this.order = (int) (long) json[nameof (order)];
      int? nullable4;
      if (json[nameof (quest_s_id)] != null)
      {
        nullable1 = (long?) json[nameof (quest_s_id)];
        nullable4 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable4 = new int?();
      this.quest_s_id = nullable4;
    }
  }
}
