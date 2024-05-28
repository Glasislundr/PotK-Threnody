// Decompiled with JetBrains decompiler
// Type: SM.LimitShopInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class LimitShopInfo : KeyCompare
  {
    public int banner_id;
    public DateTime limit_shop_start_at;
    public DateTime limit_shop_end_at;

    public LimitShopInfo()
    {
    }

    public LimitShopInfo(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.banner_id = (int) (long) json[nameof (banner_id)];
      this.limit_shop_start_at = DateTime.Parse((string) json[nameof (limit_shop_start_at)]);
      this.limit_shop_end_at = DateTime.Parse((string) json[nameof (limit_shop_end_at)]);
    }
  }
}
