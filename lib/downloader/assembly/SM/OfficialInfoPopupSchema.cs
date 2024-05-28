// Decompiled with JetBrains decompiler
// Type: SM.OfficialInfoPopupSchema
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class OfficialInfoPopupSchema : KeyCompare
  {
    public string popup_img_url;
    public int officialinfo_id;
    public int popup_priority;
    public int popup_version;

    public OfficialInfoPopupSchema()
    {
    }

    public OfficialInfoPopupSchema(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.popup_img_url = (string) json[nameof (popup_img_url)];
      this.officialinfo_id = (int) (long) json[nameof (officialinfo_id)];
      this.popup_priority = (int) (long) json[nameof (popup_priority)];
      this.popup_version = (int) (long) json[nameof (popup_version)];
    }
  }
}
