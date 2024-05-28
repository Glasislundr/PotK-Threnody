// Decompiled with JetBrains decompiler
// Type: MasterDataTable.AppSetupTuning
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class AppSetupTuning
  {
    public int ID;
    public bool IsNotDeepCopy;
    public bool IsEnable;

    public static AppSetupTuning Parse(MasterDataReader reader)
    {
      return new AppSetupTuning()
      {
        ID = reader.ReadInt(),
        IsNotDeepCopy = reader.ReadBool(),
        IsEnable = reader.ReadBool()
      };
    }
  }
}
