// Decompiled with JetBrains decompiler
// Type: MasterDataTable.CorpsStage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class CorpsStage
  {
    public int ID;
    public int setting_id;
    public int difficulty;
    public int number;
    public string name;
    public bool isBoss;
    public int banner_id;
    public int back_ground_CorpsStageBackground;

    public static CorpsStage Parse(MasterDataReader reader)
    {
      return new CorpsStage()
      {
        ID = reader.ReadInt(),
        setting_id = reader.ReadInt(),
        difficulty = reader.ReadInt(),
        number = reader.ReadInt(),
        name = reader.ReadStringOrNull(true),
        isBoss = reader.ReadBool(),
        banner_id = reader.ReadInt(),
        back_ground_CorpsStageBackground = reader.ReadInt()
      };
    }

    public CorpsStageBackground back_ground
    {
      get
      {
        CorpsStageBackground backGround;
        if (!MasterData.CorpsStageBackground.TryGetValue(this.back_ground_CorpsStageBackground, out backGround))
          Debug.LogError((object) ("Key not Found: MasterData.CorpsStageBackground[" + (object) this.back_ground_CorpsStageBackground + "]"));
        return backGround;
      }
    }

    public bool CheckOpen(int[] clearStageNumbers)
    {
      foreach (int conditionNumber in this.GetConditionNumbers())
      {
        if (!((IEnumerable<int>) clearStageNumbers).Contains<int>(conditionNumber))
          return false;
      }
      return true;
    }

    private int[] GetConditionNumbers()
    {
      List<int> intList = new List<int>();
      CorpsStageOpenConditions stageOpenConditions;
      if (MasterData.CorpsStageOpenConditions.TryGetValue(this.ID, out stageOpenConditions))
      {
        string conditionsNumbers = stageOpenConditions.conditions_numbers;
        char[] chArray = new char[1]{ ',' };
        foreach (string str in conditionsNumbers.Split(chArray))
        {
          double result = 0.0;
          if (double.TryParse(str.Trim(), out result))
            intList.Add((int) result);
        }
      }
      return intList.ToArray();
    }
  }
}
