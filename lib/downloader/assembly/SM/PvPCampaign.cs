// Decompiled with JetBrains decompiler
// Type: SM.PvPCampaign
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class PvPCampaign : KeyCompare
  {
    public string image_url;
    public string button_image_url;
    public Campaign campaign;

    public PvPCampaign()
    {
    }

    public PvPCampaign(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.image_url = (string) json[nameof (image_url)];
      this.button_image_url = (string) json[nameof (button_image_url)];
      this.campaign = json[nameof (campaign)] == null ? (Campaign) null : new Campaign((Dictionary<string, object>) json[nameof (campaign)]);
    }
  }
}
