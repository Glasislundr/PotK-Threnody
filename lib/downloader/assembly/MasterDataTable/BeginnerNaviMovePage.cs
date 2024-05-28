// Decompiled with JetBrains decompiler
// Type: MasterDataTable.BeginnerNaviMovePage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class BeginnerNaviMovePage
  {
    public int ID;
    public string name;
    public string moveScene;
    public string buttonImage;

    public static BeginnerNaviMovePage Parse(MasterDataReader reader)
    {
      return new BeginnerNaviMovePage()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        moveScene = reader.ReadString(true),
        buttonImage = reader.ReadString(true)
      };
    }
  }
}
