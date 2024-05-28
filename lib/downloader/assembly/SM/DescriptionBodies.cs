// Decompiled with JetBrains decompiler
// Type: SM.DescriptionBodies
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class DescriptionBodies : KeyCompare
  {
    public string body;
    public int? kind;
    public int? extra_position;
    public int? extra_id;
    public int? image_width;
    public int? image_height;
    public string image_url;
    public int? extra_type;

    public DescriptionBodies()
    {
    }

    public DescriptionBodies(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.body = json[nameof (body)] == null ? (string) null : (string) json[nameof (body)];
      long? nullable1;
      int? nullable2;
      if (json[nameof (kind)] != null)
      {
        nullable1 = (long?) json[nameof (kind)];
        nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable2 = new int?();
      this.kind = nullable2;
      int? nullable3;
      if (json[nameof (extra_position)] != null)
      {
        nullable1 = (long?) json[nameof (extra_position)];
        nullable3 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable3 = new int?();
      this.extra_position = nullable3;
      int? nullable4;
      if (json[nameof (extra_id)] != null)
      {
        nullable1 = (long?) json[nameof (extra_id)];
        nullable4 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable4 = new int?();
      this.extra_id = nullable4;
      int? nullable5;
      if (json[nameof (image_width)] != null)
      {
        nullable1 = (long?) json[nameof (image_width)];
        nullable5 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable5 = new int?();
      this.image_width = nullable5;
      int? nullable6;
      if (json[nameof (image_height)] != null)
      {
        nullable1 = (long?) json[nameof (image_height)];
        nullable6 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable6 = new int?();
      this.image_height = nullable6;
      this.image_url = json[nameof (image_url)] == null ? (string) null : (string) json[nameof (image_url)];
      int? nullable7;
      if (json[nameof (extra_type)] != null)
      {
        nullable1 = (long?) json[nameof (extra_type)];
        nullable7 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable7 = new int?();
      this.extra_type = nullable7;
    }
  }
}
