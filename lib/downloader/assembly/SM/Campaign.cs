// Decompiled with JetBrains decompiler
// Type: SM.Campaign
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class Campaign : KeyCompare
  {
    public string name;
    public int value;
    public int campaign_type_id;

    public Campaign()
    {
    }

    public Campaign(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.name = (string) json[nameof (name)];
      this.value = (int) (long) json[nameof (value)];
      this.campaign_type_id = (int) (long) json[nameof (campaign_type_id)];
    }
  }
}
