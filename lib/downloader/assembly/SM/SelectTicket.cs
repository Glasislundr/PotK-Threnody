// Decompiled with JetBrains decompiler
// Type: SM.SelectTicket
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class SelectTicket : KeyCompare
  {
    public DateTime start_at;
    public string description;
    public string name;
    public string detail;
    public DateTime? end_at;
    public bool unit_type_selectable;
    public int cost;
    public bool exchange_limit;
    public int category_id;
    public int id;
    public SelectTicketChoices[] choices;

    public SelectTicket()
    {
    }

    public SelectTicket(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.start_at = DateTime.Parse((string) json[nameof (start_at)]);
      this.description = (string) json[nameof (description)];
      this.name = (string) json[nameof (name)];
      this.detail = (string) json[nameof (detail)];
      this.end_at = json[nameof (end_at)] == null ? new DateTime?() : new DateTime?(DateTime.Parse((string) json[nameof (end_at)]));
      this.unit_type_selectable = (bool) json[nameof (unit_type_selectable)];
      this.cost = (int) (long) json[nameof (cost)];
      this.exchange_limit = (bool) json[nameof (exchange_limit)];
      this.category_id = (int) (long) json[nameof (category_id)];
      this.id = (int) (long) json[nameof (id)];
      List<SelectTicketChoices> selectTicketChoicesList = new List<SelectTicketChoices>();
      foreach (object json1 in (List<object>) json[nameof (choices)])
        selectTicketChoicesList.Add(json1 == null ? (SelectTicketChoices) null : new SelectTicketChoices((Dictionary<string, object>) json1));
      this.choices = selectTicketChoicesList.ToArray();
    }
  }
}
