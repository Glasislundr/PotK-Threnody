// Decompiled with JetBrains decompiler
// Type: UnitFamilyOrNull
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System;

#nullable disable
[Serializable]
public class UnitFamilyOrNull
{
  public bool hasFamily;
  public UnitFamily unitFamily;

  public int GetFamily() => !this.hasFamily ? 999 : (int) this.unitFamily;
}
