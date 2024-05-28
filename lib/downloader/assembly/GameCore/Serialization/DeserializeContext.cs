// Decompiled with JetBrains decompiler
// Type: GameCore.Serialization.DeserializeContext
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace GameCore.Serialization
{
  public class DeserializeContext
  {
    internal AssocList<int, TypeObject> objects = new AssocList<int, TypeObject>();
    internal AssocList<int, TreeObject> trees = new AssocList<int, TreeObject>();
    internal Dictionary<int, object> objectIdToObj = new Dictionary<int, object>();
    internal Dictionary<int, object> treeIdToObj = new Dictionary<int, object>();

    public void Clean(int treeId)
    {
      HashSet<int> usedTree = new HashSet<int>();
      HashSet<int> usedBlob = new HashSet<int>();
      SerializeContext.CleanRec(treeId, (IDictionary<int, TreeObject>) this.trees, usedTree, usedBlob, (Func<KeyValuePair<string, int>, int[]>) (x => this.objects[this.trees[x.Value].objectId].GetIntArray()));
      this.trees.RemoveWhere((Func<KeyValuePair<int, TreeObject>, bool>) (x => !usedTree.Contains(x.Key)));
      this.objects.RemoveWhere((Func<KeyValuePair<int, TypeObject>, bool>) (x => !usedBlob.Contains(x.Key)));
      foreach (int key in this.treeIdToObj.Keys.ToArray<int>())
      {
        if (!usedTree.Contains(key))
          this.treeIdToObj.Remove(key);
      }
      foreach (int key in this.objectIdToObj.Keys.ToArray<int>())
      {
        if (!usedBlob.Contains(key))
          this.objectIdToObj.Remove(key);
      }
    }
  }
}
