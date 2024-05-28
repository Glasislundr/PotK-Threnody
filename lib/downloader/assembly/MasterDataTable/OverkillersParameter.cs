// Decompiled with JetBrains decompiler
// Type: MasterDataTable.OverkillersParameter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class OverkillersParameter
  {
    public int ID;
    public int parameter_no;
    public int unity_value;
    public int hp;
    public int strength;
    public int vitality;
    public int intelligence;
    public int mind;
    public int agility;
    public int dexterity;
    public int lucky;
    private const int DIV_PERCENTAGE = 1000;
    private const int DIV_PERCENTAGE_DECIMAL = 10;

    public static OverkillersParameter Parse(MasterDataReader reader)
    {
      return new OverkillersParameter()
      {
        ID = reader.ReadInt(),
        parameter_no = reader.ReadInt(),
        unity_value = reader.ReadInt(),
        hp = reader.ReadInt(),
        strength = reader.ReadInt(),
        vitality = reader.ReadInt(),
        intelligence = reader.ReadInt(),
        mind = reader.ReadInt(),
        agility = reader.ReadInt(),
        dexterity = reader.ReadInt(),
        lucky = reader.ReadInt()
      };
    }

    public static OverkillersParameter getParameter(int parameterNo, int unityValue)
    {
      if (parameterNo == 0)
        return (OverkillersParameter) null;
      OverkillersParameter parameter = (OverkillersParameter) null;
      foreach (OverkillersParameter overkillersParameter in MasterData.OverkillersParameterList)
      {
        if (overkillersParameter.parameter_no == parameterNo)
        {
          if (overkillersParameter.unity_value <= unityValue)
            parameter = overkillersParameter;
          else
            break;
        }
        else if (parameter != null)
          break;
      }
      return parameter;
    }

    public static int calcParameter(int parameter, int percentage)
    {
      return (parameter * percentage + 999) / 1000;
    }

    public static string toStringPercentage(int percentage)
    {
      int num1 = percentage / 10;
      int num2 = percentage % 10;
      return num2 != 0 ? string.Format("{0}.{1:0}", (object) num1, (object) num2) : num1.ToString();
    }
  }
}
