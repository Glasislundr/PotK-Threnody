// Decompiled with JetBrains decompiler
// Type: SM.SelectTicketChoices
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
  public class SelectTicketChoices : KeyCompare
  {
    public int reward_type_id;
    public int[] evolution_unit_ids;
    public int[] unit_types;
    public int? exchangeable_count;
    public int id;
    public int reward_id;

    public SelectTicketChoices()
    {
    }

    public SelectTicketChoices(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.reward_type_id = (int) (long) json[nameof (reward_type_id)];
      this.evolution_unit_ids = ((IEnumerable<object>) json[nameof (evolution_unit_ids)]).Select<object, int>((Func<object, int>) (s => (int) (long) s)).ToArray<int>();
      this.unit_types = ((IEnumerable<object>) json[nameof (unit_types)]).Select<object, int>((Func<object, int>) (s => (int) (long) s)).ToArray<int>();
      int? nullable1;
      if (json[nameof (exchangeable_count)] != null)
      {
        long? nullable2 = (long?) json[nameof (exchangeable_count)];
        nullable1 = nullable2.HasValue ? new int?((int) nullable2.GetValueOrDefault()) : new int?();
      }
      else
        nullable1 = new int?();
      this.exchangeable_count = nullable1;
      this.id = (int) (long) json[nameof (id)];
      this.reward_id = (int) (long) json[nameof (reward_id)];
    }
  }
}
