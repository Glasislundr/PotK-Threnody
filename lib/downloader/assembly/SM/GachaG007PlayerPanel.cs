// Decompiled with JetBrains decompiler
// Type: SM.GachaG007PlayerPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class GachaG007PlayerPanel : KeyCompare
  {
    public int? reward_quantity;
    public string description;
    public int? reward_type_id;
    public bool is_opened;
    public int position;
    public bool highlight;
    public int? reward_id;

    public GachaG007PlayerPanel()
    {
    }

    public GachaG007PlayerPanel(Dictionary<string, object> json)
    {
      this._hasKey = false;
      long? nullable1;
      int? nullable2;
      if (json[nameof (reward_quantity)] != null)
      {
        nullable1 = (long?) json[nameof (reward_quantity)];
        nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable2 = new int?();
      this.reward_quantity = nullable2;
      this.description = (string) json[nameof (description)];
      int? nullable3;
      if (json[nameof (reward_type_id)] != null)
      {
        nullable1 = (long?) json[nameof (reward_type_id)];
        nullable3 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable3 = new int?();
      this.reward_type_id = nullable3;
      this.is_opened = (bool) json[nameof (is_opened)];
      this.position = (int) (long) json[nameof (position)];
      this.highlight = (bool) json[nameof (highlight)];
      int? nullable4;
      if (json[nameof (reward_id)] != null)
      {
        nullable1 = (long?) json[nameof (reward_id)];
        nullable4 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable4 = new int?();
      this.reward_id = nullable4;
    }
  }
}
