// Decompiled with JetBrains decompiler
// Type: MasterDataTable.XJobInformation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class XJobInformation
  {
    public int ID;
    public string param_keys;

    public static XJobInformation Parse(MasterDataReader reader)
    {
      return new XJobInformation()
      {
        ID = reader.ReadInt(),
        param_keys = reader.ReadString(true)
      };
    }

    public string[] paramKeys => this.param_keys.Replace(" ", "").Split(',');
  }
}
