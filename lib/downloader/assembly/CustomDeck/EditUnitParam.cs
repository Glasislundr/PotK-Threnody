// Decompiled with JetBrains decompiler
// Type: CustomDeck.EditUnitParam
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;

#nullable disable
namespace CustomDeck
{
  public class EditUnitParam
  {
    private object value_;

    public EditUnitParam(
      PlayerCustomDeck d,
      PlayerUnit[] targets,
      int[] usedIds,
      EditUnitParam.Multi mode)
    {
      this.deck = d;
      this.units = targets;
      this.usedUnitIds = usedIds;
      this.value_ = (object) mode;
    }

    public EditUnitParam(
      PlayerCustomDeck d,
      PlayerUnit[] targets,
      int[] usedIds,
      EditUnitParam.Single mode)
    {
      this.deck = d;
      this.units = targets;
      this.usedUnitIds = usedIds;
      this.value_ = (object) mode;
    }

    public bool isMulti => this.value_ is EditUnitParam.Multi;

    public EditUnitParam.Multi multi => this.value_ as EditUnitParam.Multi;

    public EditUnitParam.Single single => this.value_ as EditUnitParam.Single;

    public PlayerCustomDeck deck { get; private set; }

    public PlayerUnit[] units { get; private set; }

    public int[] usedUnitIds { get; private set; }

    public class Multi
    {
      public Action<int[]> setUnits;
    }

    public class Single
    {
      public int index;
      public Action<EditUnitParam.Single, int> setUnit;
    }
  }
}
