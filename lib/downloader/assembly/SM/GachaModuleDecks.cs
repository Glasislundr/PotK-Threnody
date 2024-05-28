// Decompiled with JetBrains decompiler
// Type: SM.GachaModuleDecks
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class GachaModuleDecks : KeyCompare
  {
    public GachaModuleDecksProbabilities[] probabilities;
    public GachaModuleDecksEntities[] entities;
    public int id;

    public GachaModuleDecks()
    {
    }

    public GachaModuleDecks(Dictionary<string, object> json)
    {
      this._hasKey = false;
      List<GachaModuleDecksProbabilities> decksProbabilitiesList = new List<GachaModuleDecksProbabilities>();
      foreach (object json1 in (List<object>) json[nameof (probabilities)])
        decksProbabilitiesList.Add(json1 == null ? (GachaModuleDecksProbabilities) null : new GachaModuleDecksProbabilities((Dictionary<string, object>) json1));
      this.probabilities = decksProbabilitiesList.ToArray();
      List<GachaModuleDecksEntities> moduleDecksEntitiesList = new List<GachaModuleDecksEntities>();
      foreach (object json2 in (List<object>) json[nameof (entities)])
        moduleDecksEntitiesList.Add(json2 == null ? (GachaModuleDecksEntities) null : new GachaModuleDecksEntities((Dictionary<string, object>) json2));
      this.entities = moduleDecksEntitiesList.ToArray();
      this.id = (int) (long) json[nameof (id)];
    }
  }
}
