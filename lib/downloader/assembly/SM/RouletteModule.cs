// Decompiled with JetBrains decompiler
// Type: SM.RouletteModule
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class RouletteModule : KeyCompare
  {
    public RouletteModuleRoulette[] roulette;

    public RouletteModule()
    {
    }

    public RouletteModule(Dictionary<string, object> json)
    {
      this._hasKey = false;
      List<RouletteModuleRoulette> rouletteModuleRouletteList = new List<RouletteModuleRoulette>();
      foreach (object json1 in (List<object>) json[nameof (roulette)])
        rouletteModuleRouletteList.Add(json1 == null ? (RouletteModuleRoulette) null : new RouletteModuleRoulette((Dictionary<string, object>) json1));
      this.roulette = rouletteModuleRouletteList.ToArray();
    }
  }
}
