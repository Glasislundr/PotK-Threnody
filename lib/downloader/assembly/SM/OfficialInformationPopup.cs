// Decompiled with JetBrains decompiler
// Type: SM.OfficialInformationPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class OfficialInformationPopup : KeyCompare
  {
    public OfficialInfoPopupSchema[] popup_pickups;
    public OfficialInfoUnitPopup[] popup_units;

    public OfficialInformationPopup()
    {
    }

    public OfficialInformationPopup(Dictionary<string, object> json)
    {
      this._hasKey = false;
      List<OfficialInfoPopupSchema> officialInfoPopupSchemaList = new List<OfficialInfoPopupSchema>();
      foreach (object json1 in (List<object>) json[nameof (popup_pickups)])
        officialInfoPopupSchemaList.Add(json1 == null ? (OfficialInfoPopupSchema) null : new OfficialInfoPopupSchema((Dictionary<string, object>) json1));
      this.popup_pickups = officialInfoPopupSchemaList.ToArray();
      List<OfficialInfoUnitPopup> officialInfoUnitPopupList = new List<OfficialInfoUnitPopup>();
      foreach (object json2 in (List<object>) json[nameof (popup_units)])
        officialInfoUnitPopupList.Add(json2 == null ? (OfficialInfoUnitPopup) null : new OfficialInfoUnitPopup((Dictionary<string, object>) json2));
      this.popup_units = officialInfoUnitPopupList.ToArray();
    }
  }
}
