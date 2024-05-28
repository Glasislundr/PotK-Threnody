// Decompiled with JetBrains decompiler
// Type: SM.Description
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class Description : KeyCompare
  {
    public DescriptionBodies[] bodies;
    public string title;

    public Description()
    {
    }

    public Description(Dictionary<string, object> json)
    {
      this._hasKey = false;
      List<DescriptionBodies> descriptionBodiesList = new List<DescriptionBodies>();
      foreach (object json1 in (List<object>) json[nameof (bodies)])
        descriptionBodiesList.Add(json1 == null ? (DescriptionBodies) null : new DescriptionBodies((Dictionary<string, object>) json1));
      this.bodies = descriptionBodiesList.ToArray();
      this.title = json[nameof (title)] == null ? (string) null : (string) json[nameof (title)];
    }
  }
}
