// Decompiled with JetBrains decompiler
// Type: SM.LimitedShopBanner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class LimitedShopBanner : KeyCompare
  {
    public string name;
    public int id;
    public string banner_url;
    public DateTime? end_at;

    public LimitedShopBanner()
    {
    }

    public LimitedShopBanner(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.name = (string) json[nameof (name)];
      this.id = (int) (long) json[nameof (id)];
      this.banner_url = (string) json[nameof (banner_url)];
      this.end_at = json[nameof (end_at)] == null ? new DateTime?() : new DateTime?(DateTime.Parse((string) json[nameof (end_at)]));
    }
  }
}
