// Decompiled with JetBrains decompiler
// Type: MasterDataTable.XLevelLimits
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class XLevelLimits
  {
    public int ID;
    public string levels;

    public static XLevelLimits Parse(MasterDataReader reader)
    {
      return new XLevelLimits()
      {
        ID = reader.ReadInt(),
        levels = reader.ReadString(true)
      };
    }

    public int getLimit(int level)
    {
      string[] strArray = this.levels.Split(',');
      if (strArray.Length == 0)
      {
        Debug.LogError((object) string.Format("MasterData Error!! XLevelLimits(ID={0}, levels={1})", (object) this.ID, (object) this.levels));
        return 0;
      }
      if (strArray.Length <= level)
      {
        Debug.LogError((object) string.Format("MasterData Error!! XLevelLimis(ID={0}, levels={1}) Out of Range Level({2})", (object) this.ID, (object) this.levels, (object) level));
        return 0;
      }
      int result;
      if (int.TryParse(strArray[level], out result))
        return result;
      Debug.LogError((object) string.Format("MasterData Error!! XLevelLimits(ID={0}, levels={1}) Parse Failed \"{2}\"", (object) this.ID, (object) this.levels, (object) strArray[level]));
      return 0;
    }
  }
}
