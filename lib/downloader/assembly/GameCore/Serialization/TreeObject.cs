// Decompiled with JetBrains decompiler
// Type: GameCore.Serialization.TreeObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace GameCore.Serialization
{
  public class TreeObject
  {
    public int treeId;
    public int objectId;
    public AssocList<string, int> fields = new AssocList<string, int>();

    public override bool Equals(object obj)
    {
      if (obj == null || !(obj is TreeObject treeObject) || this.objectId != treeObject.objectId)
        return false;
      if (this.fields == null)
        return treeObject.fields == null;
      return treeObject.fields != null && this.fields.SequenceEqual<KeyValuePair<string, int>>((IEnumerable<KeyValuePair<string, int>>) treeObject.fields);
    }

    public override int GetHashCode() => this.objectId.Combine(this.fields.Count);

    public Dictionary<string, object> ToJson()
    {
      return new Dictionary<string, object>()
      {
        {
          "treeId",
          (object) this.treeId
        },
        {
          "objectId",
          (object) this.objectId
        },
        {
          "fields",
          (object) this.fields
        }
      };
    }

    public static TreeObject FromJson(IDictionary<string, object> json)
    {
      return new TreeObject()
      {
        treeId = (int) (long) json["treeId"],
        objectId = (int) (long) json["objectId"],
        fields = new AssocList<string, int>((IDictionary<string, int>) ((IEnumerable<KeyValuePair<string, object>>) json["fields"]).ToDictionary<KeyValuePair<string, object>, string, int>((Func<KeyValuePair<string, object>, string>) (x => x.Key), (Func<KeyValuePair<string, object>, int>) (x => (int) (long) x.Value)))
      };
    }
  }
}
