// Decompiled with JetBrains decompiler
// Type: SM.OfficialInfoUnitPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace SM
{
  [Serializable]
  public class OfficialInfoUnitPopup : KeyCompare
  {
    public int id;
    public int[] popup_unit_ids;

    public OfficialInfoUnitPopup()
    {
    }

    public OfficialInfoUnitPopup(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.id = (int) (long) json[nameof (id)];
      this.popup_unit_ids = ((IEnumerable<object>) json[nameof (popup_unit_ids)]).Select<object, int>((Func<object, int>) (s => (int) (long) s)).ToArray<int>();
    }
  }
}
