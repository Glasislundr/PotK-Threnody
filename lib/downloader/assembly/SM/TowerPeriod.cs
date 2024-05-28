// Decompiled with JetBrains decompiler
// Type: SM.TowerPeriod
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class TowerPeriod : KeyCompare
  {
    public int priority;
    public DateTime start_at;
    public int tower_id;
    public DateTime end_at;
    public DateTime final_at;
    public string banner_image_url;
    public int category_id;
    public int id;

    public TowerPeriod()
    {
    }

    public TowerPeriod(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.priority = (int) (long) json[nameof (priority)];
      this.start_at = DateTime.Parse((string) json[nameof (start_at)]);
      this.tower_id = (int) (long) json[nameof (tower_id)];
      this.end_at = DateTime.Parse((string) json[nameof (end_at)]);
      this.final_at = DateTime.Parse((string) json[nameof (final_at)]);
      this.banner_image_url = (string) json[nameof (banner_image_url)];
      this.category_id = (int) (long) json[nameof (category_id)];
      this.id = (int) (long) json[nameof (id)];
    }
  }
}
