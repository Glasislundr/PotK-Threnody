// Decompiled with JetBrains decompiler
// Type: SM.MpStage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class MpStage : KeyCompare
  {
    public int stage_id;
    public int point;
    public int turns;
    public int annihilation_point;
    public int remaining_time;
    public DateTime end_at;
    public int id;

    public string victory_condition
    {
      get
      {
        return Consts.Format(Consts.GetInstance().VERSUS_VICTORY_CONDITION, (IDictionary) new Hashtable()
        {
          {
            (object) "cnt",
            (object) this.turns.ToLocalizeNumberText()
          }
        });
      }
    }

    public string victory_sub_condition
    {
      get
      {
        return Consts.Format(Consts.GetInstance().VERSUS_VICTORY_SUB_CONDITION1, (IDictionary) new Hashtable()
        {
          {
            (object) "cnt",
            (object) this.turns.ToLocalizeNumberText()
          }
        });
      }
    }

    public string victory_sub_condition2
    {
      get
      {
        return Consts.Format(Consts.GetInstance().VERSUS_VICTORY_SUB_CONDITION2, (IDictionary) new Hashtable()
        {
          {
            (object) "cnt",
            (object) this.turns.ToLocalizeNumberText()
          }
        });
      }
    }

    public string RemainingTime()
    {
      int num1 = this.remaining_time / 1440;
      int num2 = this.remaining_time / 60;
      int num3 = this.remaining_time % 60;
      string str1 = "";
      string str2;
      if (num1 > 0)
        str2 = str1 + Consts.Format(Consts.GetInstance().VERSUS_00261REMAIN_MAP_MESSAGE_DAY, (IDictionary) new Hashtable()
        {
          {
            (object) "day",
            (object) num1
          }
        });
      else if (num2 > 0)
        str2 = str1 + Consts.Format(Consts.GetInstance().VERSUS_00261REMAIN_MAP_MESSAGE_HOUR, (IDictionary) new Hashtable()
        {
          {
            (object) "hour",
            (object) num2
          }
        });
      else
        str2 = str1 + Consts.Format(Consts.GetInstance().VERSUS_00261REMAIN_MAP_MESSAGE_MINUTE, (IDictionary) new Hashtable()
        {
          {
            (object) "min",
            (object) num3
          }
        });
      return str2;
    }

    public MpStage()
    {
    }

    public MpStage(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.stage_id = (int) (long) json[nameof (stage_id)];
      this.point = (int) (long) json[nameof (point)];
      this.turns = (int) (long) json[nameof (turns)];
      this.annihilation_point = (int) (long) json[nameof (annihilation_point)];
      this.remaining_time = (int) (long) json[nameof (remaining_time)];
      this.end_at = DateTime.Parse((string) json[nameof (end_at)]);
      this.id = (int) (long) json[nameof (id)];
    }
  }
}
