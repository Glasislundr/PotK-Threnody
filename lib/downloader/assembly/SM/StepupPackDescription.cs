// Decompiled with JetBrains decompiler
// Type: SM.StepupPackDescription
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class StepupPackDescription : KeyCompare
  {
    public string body;
    public int? image_height;
    public string image_url;
    public int? image_width;

    public StepupPackDescription()
    {
    }

    public StepupPackDescription(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.body = json[nameof (body)] == null ? (string) null : (string) json[nameof (body)];
      long? nullable1;
      int? nullable2;
      if (json[nameof (image_height)] != null)
      {
        nullable1 = (long?) json[nameof (image_height)];
        nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable2 = new int?();
      this.image_height = nullable2;
      this.image_url = json[nameof (image_url)] == null ? (string) null : (string) json[nameof (image_url)];
      int? nullable3;
      if (json[nameof (image_width)] != null)
      {
        nullable1 = (long?) json[nameof (image_width)];
        nullable3 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable3 = new int?();
      this.image_width = nullable3;
    }
  }
}
