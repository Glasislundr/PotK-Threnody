// Decompiled with JetBrains decompiler
// Type: SM.PaymentBonusPeriod
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class PaymentBonusPeriod : KeyCompare
  {
    public DateTime start_at;
    public int id;
    public DateTime end_at;

    public PaymentBonusPeriod()
    {
    }

    public PaymentBonusPeriod(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.start_at = DateTime.Parse((string) json[nameof (start_at)]);
      this.id = (int) (long) json[nameof (id)];
      this.end_at = DateTime.Parse((string) json[nameof (end_at)]);
    }
  }
}
