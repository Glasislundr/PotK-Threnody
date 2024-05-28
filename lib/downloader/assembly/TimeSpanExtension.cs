// Decompiled with JetBrains decompiler
// Type: TimeSpanExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;

#nullable disable
public static class TimeSpanExtension
{
  public static string DisplayString(this TimeSpan self)
  {
    if (self.TotalDays >= 1.0)
      return Consts.Format(Consts.GetInstance().DISPLAY_STRING, (IDictionary) new Hashtable()
      {
        {
          (object) "value",
          (object) ((int) self.TotalDays).ToString()
        },
        {
          (object) "time",
          (object) Consts.GetInstance().COLOSSEUM_BONUS_TIME_DAY
        }
      });
    if (self.TotalHours >= 1.0)
      return Consts.Format(Consts.GetInstance().DISPLAY_STRING, (IDictionary) new Hashtable()
      {
        {
          (object) "value",
          (object) ((int) self.TotalHours).ToString()
        },
        {
          (object) "time",
          (object) Consts.GetInstance().COLOSSEUM_BONUS_TIME_HOUR
        }
      });
    if (self.TotalMinutes >= 1.0)
      return Consts.Format(Consts.GetInstance().DISPLAY_STRING, (IDictionary) new Hashtable()
      {
        {
          (object) "value",
          (object) ((int) self.TotalMinutes).ToString()
        },
        {
          (object) "time",
          (object) Consts.GetInstance().COLOSSEUM_BONUS_TIME_MINUE
        }
      });
    if (self.TotalSeconds >= 1.0)
      return Consts.Format(Consts.GetInstance().DISPLAY_STRING, (IDictionary) new Hashtable()
      {
        {
          (object) "value",
          (object) ((int) self.TotalSeconds).ToString()
        },
        {
          (object) "time",
          (object) Consts.GetInstance().COLOSSEUM_BONUS_TIME_SECOND
        }
      });
    return Consts.Format(Consts.GetInstance().DISPLAY_STRING, (IDictionary) new Hashtable()
    {
      {
        (object) "value",
        (object) "0"
      },
      {
        (object) "time",
        (object) Consts.GetInstance().COLOSSEUM_BONUS_TIME_SECOND
      }
    });
  }

  public static string DisplayStringForFriendsGuildMember(this TimeSpan self)
  {
    if (self.TotalDays >= 30.0)
      return Consts.Format(Consts.GetInstance().DISPLAY_STRING_FOR_FRIEND, (IDictionary) new Hashtable()
      {
        {
          (object) "value",
          (object) "１"
        },
        {
          (object) "time",
          (object) Consts.GetInstance().DISPLAY_STRING_TIME_MONTH
        },
        {
          (object) "status",
          (object) Consts.GetInstance().OVER
        }
      });
    if (self.TotalDays >= 14.0)
      return Consts.Format(Consts.GetInstance().DISPLAY_STRING_FOR_FRIEND, (IDictionary) new Hashtable()
      {
        {
          (object) "value",
          (object) "１"
        },
        {
          (object) "time",
          (object) Consts.GetInstance().DISPLAY_STRING_TIME_MONTH
        },
        {
          (object) "status",
          (object) Consts.GetInstance().INSIDE
        }
      });
    if (self.TotalDays >= 7.0)
      return Consts.Format(Consts.GetInstance().DISPLAY_STRING_FOR_FRIEND, (IDictionary) new Hashtable()
      {
        {
          (object) "value",
          (object) "１４"
        },
        {
          (object) "time",
          (object) Consts.GetInstance().COLOSSEUM_BONUS_TIME_DAY
        },
        {
          (object) "status",
          (object) Consts.GetInstance().INSIDE
        }
      });
    if (self.TotalDays >= 3.0)
      return Consts.Format(Consts.GetInstance().DISPLAY_STRING_FOR_FRIEND, (IDictionary) new Hashtable()
      {
        {
          (object) "value",
          (object) "７"
        },
        {
          (object) "time",
          (object) Consts.GetInstance().COLOSSEUM_BONUS_TIME_DAY
        },
        {
          (object) "status",
          (object) Consts.GetInstance().INSIDE
        }
      });
    if (self.TotalDays >= 1.0)
      return Consts.Format(Consts.GetInstance().DISPLAY_STRING_FOR_FRIEND, (IDictionary) new Hashtable()
      {
        {
          (object) "value",
          (object) "３"
        },
        {
          (object) "time",
          (object) Consts.GetInstance().COLOSSEUM_BONUS_TIME_DAY
        },
        {
          (object) "status",
          (object) Consts.GetInstance().INSIDE
        }
      });
    if (self.TotalHours >= 12.0)
      return Consts.Format(Consts.GetInstance().DISPLAY_STRING_FOR_FRIEND, (IDictionary) new Hashtable()
      {
        {
          (object) "value",
          (object) "１"
        },
        {
          (object) "time",
          (object) Consts.GetInstance().COLOSSEUM_BONUS_TIME_DAY
        },
        {
          (object) "status",
          (object) Consts.GetInstance().INSIDE
        }
      });
    if (self.TotalHours >= 6.0)
      return Consts.Format(Consts.GetInstance().DISPLAY_STRING_FOR_FRIEND, (IDictionary) new Hashtable()
      {
        {
          (object) "value",
          (object) "１２"
        },
        {
          (object) "time",
          (object) Consts.GetInstance().COLOSSEUM_BONUS_TIME_HOUR
        },
        {
          (object) "status",
          (object) Consts.GetInstance().INSIDE
        }
      });
    if (self.TotalHours >= 1.0)
      return Consts.Format(Consts.GetInstance().DISPLAY_STRING_FOR_FRIEND, (IDictionary) new Hashtable()
      {
        {
          (object) "value",
          (object) "６"
        },
        {
          (object) "time",
          (object) Consts.GetInstance().COLOSSEUM_BONUS_TIME_HOUR
        },
        {
          (object) "status",
          (object) Consts.GetInstance().INSIDE
        }
      });
    if (self.TotalMinutes >= 30.0)
      return Consts.Format(Consts.GetInstance().DISPLAY_STRING_FOR_FRIEND, (IDictionary) new Hashtable()
      {
        {
          (object) "value",
          (object) "１"
        },
        {
          (object) "time",
          (object) Consts.GetInstance().COLOSSEUM_BONUS_TIME_HOUR
        },
        {
          (object) "status",
          (object) Consts.GetInstance().INSIDE
        }
      });
    if (self.TotalMinutes >= 10.0)
      return Consts.Format(Consts.GetInstance().DISPLAY_STRING_FOR_FRIEND, (IDictionary) new Hashtable()
      {
        {
          (object) "value",
          (object) "３０"
        },
        {
          (object) "time",
          (object) Consts.GetInstance().COLOSSEUM_BONUS_TIME_MINUE
        },
        {
          (object) "status",
          (object) Consts.GetInstance().INSIDE
        }
      });
    if (self.TotalMinutes >= 5.0)
      return Consts.Format(Consts.GetInstance().DISPLAY_STRING_FOR_FRIEND, (IDictionary) new Hashtable()
      {
        {
          (object) "value",
          (object) "１０"
        },
        {
          (object) "time",
          (object) Consts.GetInstance().COLOSSEUM_BONUS_TIME_MINUE
        },
        {
          (object) "status",
          (object) Consts.GetInstance().INSIDE
        }
      });
    return Consts.Format(Consts.GetInstance().DISPLAY_STRING_FOR_FRIEND, (IDictionary) new Hashtable()
    {
      {
        (object) "value",
        (object) "５"
      },
      {
        (object) "time",
        (object) Consts.GetInstance().COLOSSEUM_BONUS_TIME_MINUE
      },
      {
        (object) "status",
        (object) Consts.GetInstance().INSIDE
      }
    });
  }

  public static string DisplayStringForFriendsApplied(this TimeSpan self)
  {
    if ((double) self.Days < 1.0)
      return string.Format(Consts.GetInstance().TODAY);
    return Consts.Format(Consts.GetInstance().DISPLAY_STRING_FOR_FRIEND, (IDictionary) new Hashtable()
    {
      {
        (object) "value",
        (object) self.Days.ToString()
      },
      {
        (object) "time",
        (object) Consts.GetInstance().COLOSSEUM_BONUS_TIME_DAY
      },
      {
        (object) "status",
        (object) Consts.GetInstance().FRONT
      }
    });
  }

  public static string DisplayStringForGuildHunting(this TimeSpan self)
  {
    if (self.TotalDays >= 1.0)
      return Consts.Format(Consts.GetInstance().DISPLAY_STRING, (IDictionary) new Hashtable()
      {
        {
          (object) "value",
          (object) ((int) self.TotalDays).ToString()
        },
        {
          (object) "time",
          (object) Consts.GetInstance().COLOSSEUM_BONUS_TIME_DAY
        }
      });
    if (self.TotalHours >= 1.0)
    {
      int num = (int) (self.TotalMinutes / 60.0) + (self.TotalMinutes % 60.0 != 0.0 ? 1 : 0);
      return Consts.Format(Consts.GetInstance().DISPLAY_STRING, (IDictionary) new Hashtable()
      {
        {
          (object) "value",
          (object) num.ToString()
        },
        {
          (object) "time",
          (object) Consts.GetInstance().COLOSSEUM_BONUS_TIME_HOUR
        }
      });
    }
    if (self.TotalMinutes >= 1.0)
    {
      int num = (int) (self.TotalSeconds / 60.0) + (self.TotalSeconds % 60.0 != 0.0 ? 1 : 0);
      return Consts.Format(Consts.GetInstance().DISPLAY_STRING, (IDictionary) new Hashtable()
      {
        {
          (object) "value",
          (object) num.ToString()
        },
        {
          (object) "time",
          (object) Consts.GetInstance().COLOSSEUM_BONUS_TIME_MINUE
        }
      });
    }
    if (self.TotalSeconds >= 1.0)
    {
      int num = (int) (self.TotalMilliseconds / 1000.0) + (self.TotalMilliseconds % 1000.0 != 0.0 ? 1 : 0);
      return Consts.Format(Consts.GetInstance().DISPLAY_STRING, (IDictionary) new Hashtable()
      {
        {
          (object) "value",
          (object) num.ToString()
        },
        {
          (object) "time",
          (object) Consts.GetInstance().COLOSSEUM_BONUS_TIME_SECOND
        }
      });
    }
    return Consts.Format(Consts.GetInstance().DISPLAY_STRING, (IDictionary) new Hashtable()
    {
      {
        (object) "value",
        (object) "0"
      },
      {
        (object) "time",
        (object) Consts.GetInstance().COLOSSEUM_BONUS_TIME_SECOND
      }
    });
  }

  public static void SetDisplayStringForGuildRaid(
    this TimeSpan self,
    ref UILabel left,
    ref UILabel right)
  {
    if (self.TotalDays >= 1.0)
    {
      left.SetTextLocalize(((int) self.TotalDays).ToString());
      right.SetTextLocalize(Consts.GetInstance().COLOSSEUM_BONUS_TIME_DAY);
    }
    else if (self.TotalHours >= 1.0)
    {
      int num = (int) (self.TotalMinutes / 60.0) + (self.TotalMinutes % 60.0 != 0.0 ? 1 : 0);
      left.SetTextLocalize(num.ToString());
      right.SetTextLocalize(Consts.GetInstance().COLOSSEUM_BONUS_TIME_HOUR);
    }
    else if (self.TotalMinutes >= 1.0)
    {
      int num = (int) (self.TotalSeconds / 60.0) + (self.TotalSeconds % 60.0 != 0.0 ? 1 : 0);
      left.SetTextLocalize(num.ToString());
      right.SetTextLocalize(Consts.GetInstance().COLOSSEUM_BONUS_TIME_MINUE);
    }
    else if (self.TotalSeconds >= 1.0)
    {
      int num = (int) (self.TotalMilliseconds / 1000.0) + (self.TotalMilliseconds % 1000.0 != 0.0 ? 1 : 0);
      left.SetTextLocalize(num.ToString());
      right.SetTextLocalize(Consts.GetInstance().COLOSSEUM_BONUS_TIME_SECOND);
    }
    else
    {
      left.SetTextLocalize("0");
      right.SetTextLocalize(Consts.GetInstance().COLOSSEUM_BONUS_TIME_SECOND);
    }
  }
}
