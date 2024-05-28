// Decompiled with JetBrains decompiler
// Type: MasterDataTable.BattleskillEffectLogic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class BattleskillEffectLogic
  {
    public int ID;
    public string name;
    public int arg1_BattleskillEffectLogicArgumentEnum;
    public int arg2_BattleskillEffectLogicArgumentEnum;
    public int arg3_BattleskillEffectLogicArgumentEnum;
    public int arg4_BattleskillEffectLogicArgumentEnum;
    public int arg5_BattleskillEffectLogicArgumentEnum;
    public int arg6_BattleskillEffectLogicArgumentEnum;
    public int arg7_BattleskillEffectLogicArgumentEnum;
    public int arg8_BattleskillEffectLogicArgumentEnum;
    public int arg9_BattleskillEffectLogicArgumentEnum;
    public int arg10_BattleskillEffectLogicArgumentEnum;
    public int effect_tag1_BattleskillEffectTag;
    public int effect_tag2_BattleskillEffectTag;
    public int effect_tag3_BattleskillEffectTag;
    public int effect_tag4_BattleskillEffectTag;
    public int effect_tag5_BattleskillEffectTag;
    public int opt_test1;
    public int opt_test2;
    public int opt_test3;
    public int opt_test4;

    public bool HasTag(BattleskillEffectTag tag)
    {
      return tag == this.effect_tag1 || tag == this.effect_tag2 || tag == this.effect_tag3 || tag == this.effect_tag4 || tag == this.effect_tag5;
    }

    public BattleskillEffectLogicEnum Enum => (BattleskillEffectLogicEnum) this.ID;

    public static BattleskillEffectLogicArgumentEnum GetLogicArgumentEnum(string enumName)
    {
      try
      {
        return (BattleskillEffectLogicArgumentEnum) System.Enum.Parse(typeof (BattleskillEffectLogicArgumentEnum), enumName, true);
      }
      catch
      {
        return BattleskillEffectLogicArgumentEnum.none;
      }
    }

    public static BattleskillEffectLogic Parse(MasterDataReader reader)
    {
      return new BattleskillEffectLogic()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        arg1_BattleskillEffectLogicArgumentEnum = reader.ReadInt(),
        arg2_BattleskillEffectLogicArgumentEnum = reader.ReadInt(),
        arg3_BattleskillEffectLogicArgumentEnum = reader.ReadInt(),
        arg4_BattleskillEffectLogicArgumentEnum = reader.ReadInt(),
        arg5_BattleskillEffectLogicArgumentEnum = reader.ReadInt(),
        arg6_BattleskillEffectLogicArgumentEnum = reader.ReadInt(),
        arg7_BattleskillEffectLogicArgumentEnum = reader.ReadInt(),
        arg8_BattleskillEffectLogicArgumentEnum = reader.ReadInt(),
        arg9_BattleskillEffectLogicArgumentEnum = reader.ReadInt(),
        arg10_BattleskillEffectLogicArgumentEnum = reader.ReadInt(),
        effect_tag1_BattleskillEffectTag = reader.ReadInt(),
        effect_tag2_BattleskillEffectTag = reader.ReadInt(),
        effect_tag3_BattleskillEffectTag = reader.ReadInt(),
        effect_tag4_BattleskillEffectTag = reader.ReadInt(),
        effect_tag5_BattleskillEffectTag = reader.ReadInt(),
        opt_test1 = reader.ReadInt(),
        opt_test2 = reader.ReadInt(),
        opt_test3 = reader.ReadInt(),
        opt_test4 = reader.ReadInt()
      };
    }

    public BattleskillEffectLogicArgumentEnum arg1
    {
      get => (BattleskillEffectLogicArgumentEnum) this.arg1_BattleskillEffectLogicArgumentEnum;
    }

    public BattleskillEffectLogicArgumentEnum arg2
    {
      get => (BattleskillEffectLogicArgumentEnum) this.arg2_BattleskillEffectLogicArgumentEnum;
    }

    public BattleskillEffectLogicArgumentEnum arg3
    {
      get => (BattleskillEffectLogicArgumentEnum) this.arg3_BattleskillEffectLogicArgumentEnum;
    }

    public BattleskillEffectLogicArgumentEnum arg4
    {
      get => (BattleskillEffectLogicArgumentEnum) this.arg4_BattleskillEffectLogicArgumentEnum;
    }

    public BattleskillEffectLogicArgumentEnum arg5
    {
      get => (BattleskillEffectLogicArgumentEnum) this.arg5_BattleskillEffectLogicArgumentEnum;
    }

    public BattleskillEffectLogicArgumentEnum arg6
    {
      get => (BattleskillEffectLogicArgumentEnum) this.arg6_BattleskillEffectLogicArgumentEnum;
    }

    public BattleskillEffectLogicArgumentEnum arg7
    {
      get => (BattleskillEffectLogicArgumentEnum) this.arg7_BattleskillEffectLogicArgumentEnum;
    }

    public BattleskillEffectLogicArgumentEnum arg8
    {
      get => (BattleskillEffectLogicArgumentEnum) this.arg8_BattleskillEffectLogicArgumentEnum;
    }

    public BattleskillEffectLogicArgumentEnum arg9
    {
      get => (BattleskillEffectLogicArgumentEnum) this.arg9_BattleskillEffectLogicArgumentEnum;
    }

    public BattleskillEffectLogicArgumentEnum arg10
    {
      get => (BattleskillEffectLogicArgumentEnum) this.arg10_BattleskillEffectLogicArgumentEnum;
    }

    public BattleskillEffectTag effect_tag1
    {
      get => (BattleskillEffectTag) this.effect_tag1_BattleskillEffectTag;
    }

    public BattleskillEffectTag effect_tag2
    {
      get => (BattleskillEffectTag) this.effect_tag2_BattleskillEffectTag;
    }

    public BattleskillEffectTag effect_tag3
    {
      get => (BattleskillEffectTag) this.effect_tag3_BattleskillEffectTag;
    }

    public BattleskillEffectTag effect_tag4
    {
      get => (BattleskillEffectTag) this.effect_tag4_BattleskillEffectTag;
    }

    public BattleskillEffectTag effect_tag5
    {
      get => (BattleskillEffectTag) this.effect_tag5_BattleskillEffectTag;
    }
  }
}
