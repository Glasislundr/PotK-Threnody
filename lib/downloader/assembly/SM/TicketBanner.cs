// Decompiled with JetBrains decompiler
// Type: SM.TicketBanner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class TicketBanner : KeyCompare
  {
    public string front_url;
    public string title_url;
    public int gacha_id;

    public TicketBanner()
    {
    }

    public TicketBanner(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.front_url = (string) json[nameof (front_url)];
      this.title_url = (string) json[nameof (title_url)];
      this.gacha_id = (int) (long) json[nameof (gacha_id)];
    }
  }
}
