// Decompiled with JetBrains decompiler
// Type: SM.CoinBonusDetail
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class CoinBonusDetail : KeyCompare
  {
    public string body;
    public int image_height;
    public string image_url;
    public int image_width;

    public CoinBonusDetail()
    {
    }

    public CoinBonusDetail(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.body = (string) json[nameof (body)];
      this.image_height = (int) (long) json[nameof (image_height)];
      this.image_url = (string) json[nameof (image_url)];
      this.image_width = (int) (long) json[nameof (image_width)];
    }
  }
}
