// Decompiled with JetBrains decompiler
// Type: MasterDataTable.GearModelKind
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class GearModelKind
  {
    public int ID;
    public string name;
    public int kind_GearKind;

    public static GearModelKind Parse(MasterDataReader reader)
    {
      return new GearModelKind()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        kind_GearKind = reader.ReadInt()
      };
    }

    public GearKind kind
    {
      get
      {
        GearKind kind;
        if (!MasterData.GearKind.TryGetValue(this.kind_GearKind, out kind))
          Debug.LogError((object) ("Key not Found: MasterData.GearKind[" + (object) this.kind_GearKind + "]"));
        return kind;
      }
    }
  }
}
