// Decompiled with JetBrains decompiler
// Type: UnitStatusInformation.Util
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Reflection;
using UniLinq;
using UnityEngine;

#nullable disable
namespace UnitStatusInformation
{
  public static class Util
  {
    private static Util.GrowthDegreeTable[] growthRankTable = new Util.GrowthDegreeTable[8]
    {
      new Util.GrowthDegreeTable()
      {
        threshold_ = 0,
        degree_ = GrowthDegree.E
      },
      new Util.GrowthDegreeTable()
      {
        threshold_ = 40,
        degree_ = GrowthDegree.D
      },
      new Util.GrowthDegreeTable()
      {
        threshold_ = 54,
        degree_ = GrowthDegree.C
      },
      new Util.GrowthDegreeTable()
      {
        threshold_ = 66,
        degree_ = GrowthDegree.B
      },
      new Util.GrowthDegreeTable()
      {
        threshold_ = 74,
        degree_ = GrowthDegree.A
      },
      new Util.GrowthDegreeTable()
      {
        threshold_ = 80,
        degree_ = GrowthDegree.S
      },
      new Util.GrowthDegreeTable()
      {
        threshold_ = 86,
        degree_ = GrowthDegree.SS
      },
      new Util.GrowthDegreeTable()
      {
        threshold_ = 88,
        degree_ = GrowthDegree.SSS
      }
    };
    private const float TAKEOVER_RATE = 0.1f;

    public static void SetTextIntegersWithStateColor<T>(
      Dictionary<string, UILabel> dest,
      T now,
      T prev)
      where T : class
    {
      System.Type type = typeof (T);
      FieldInfo[] array1 = ((IEnumerable<FieldInfo>) type.GetFields()).Where<FieldInfo>((Func<FieldInfo, bool>) (x => x.FieldType.IsValueType)).ToArray<FieldInfo>();
      PropertyInfo[] array2 = ((IEnumerable<PropertyInfo>) type.GetProperties()).Where<PropertyInfo>((Func<PropertyInfo, bool>) (x => x.PropertyType.IsValueType)).ToArray<PropertyInfo>();
      foreach (KeyValuePair<string, UILabel> keyValuePair in dest)
      {
        KeyValuePair<string, UILabel> c = keyValuePair;
        if (Object.op_Equality((Object) c.Value, (Object) null))
        {
          Debug.LogError((object) ("\"int " + type.Name + "." + c.Key + "\" set UILabel is null"));
        }
        else
        {
          FieldInfo fieldInfo = Array.Find<FieldInfo>(array1, (Predicate<FieldInfo>) (x => x.Name == c.Key));
          int now1;
          int prev1;
          if (fieldInfo != (FieldInfo) null)
          {
            now1 = (int) fieldInfo.GetValue((object) now);
            prev1 = (int) fieldInfo.GetValue((object) prev);
          }
          else
          {
            PropertyInfo propertyInfo = Array.Find<PropertyInfo>(array2, (Predicate<PropertyInfo>) (x => x.Name == c.Key));
            if (propertyInfo != (PropertyInfo) null)
            {
              now1 = (int) propertyInfo.GetValue((object) now);
              prev1 = (int) propertyInfo.GetValue((object) prev);
            }
            else
            {
              Debug.LogError((object) ("Not Found Field or Property \"int " + c.Key + "\" in " + type.Name));
              continue;
            }
          }
          Util.SetTextIntegerWithStateColor(c.Value, now1, prev1);
        }
      }
    }

    public static void SetTextIntegerWithStateColor(UILabel label, int now, int prev)
    {
      if (!Object.op_Inequality((Object) label, (Object) null))
        return;
      label.text = now.ToString();
      ((UIWidget) label).color = now == prev ? Color.white : (now > prev ? Color.yellow : Color.red);
    }

    public static void SetTextIntegers<T>(Dictionary<string, UILabel> dest, T dat, Color fontColor) where T : class
    {
      System.Type type = typeof (T);
      FieldInfo[] array1 = ((IEnumerable<FieldInfo>) type.GetFields()).Where<FieldInfo>((Func<FieldInfo, bool>) (x => x.FieldType.IsValueType)).ToArray<FieldInfo>();
      PropertyInfo[] array2 = ((IEnumerable<PropertyInfo>) type.GetProperties()).Where<PropertyInfo>((Func<PropertyInfo, bool>) (x => x.PropertyType.IsValueType)).ToArray<PropertyInfo>();
      foreach (KeyValuePair<string, UILabel> keyValuePair in dest)
      {
        KeyValuePair<string, UILabel> c = keyValuePair;
        if (Object.op_Equality((Object) c.Value, (Object) null))
        {
          Debug.LogError((object) ("\"int " + type.Name + "." + c.Key + "\" set UILabel is null"));
        }
        else
        {
          FieldInfo fieldInfo = Array.Find<FieldInfo>(array1, (Predicate<FieldInfo>) (x => x.Name == c.Key));
          int num;
          if (fieldInfo != (FieldInfo) null)
          {
            num = (int) fieldInfo.GetValue((object) dat);
          }
          else
          {
            PropertyInfo propertyInfo = Array.Find<PropertyInfo>(array2, (Predicate<PropertyInfo>) (x => x.Name == c.Key));
            if (propertyInfo != (PropertyInfo) null)
            {
              num = (int) propertyInfo.GetValue((object) dat);
            }
            else
            {
              Debug.LogError((object) ("Not Found Field or Property \"int " + c.Key + "\" in " + type.Name));
              continue;
            }
          }
          c.Value.text = num.ToString();
          ((UIWidget) c.Value).color = fontColor;
        }
      }
    }

    public static GrowthDegree CalcGrowthDegree(int[] takeovers, int[] maxTakeovers)
    {
      long num1 = ((IEnumerable<int>) maxTakeovers).Sum<int>((Func<int, long>) (x => (long) x));
      long num2 = ((IEnumerable<int>) takeovers).Sum<int>((Func<int, long>) (x => (long) x)) * 100L / num1;
      int index1 = 1;
      while (index1 < Util.growthRankTable.Length && num2 >= (long) Util.growthRankTable[index1].threshold_)
        ++index1;
      int index2 = index1 - 1;
      return Util.growthRankTable[index2].degree_;
    }

    public static int CalcMaxTakeover(
      int initial,
      int inheritance,
      int levelup_max,
      int combine_max)
    {
      return Mathf.CeilToInt((float) (initial + inheritance + levelup_max + combine_max) * 0.1f);
    }

    private class GrowthDegreeTable
    {
      public int threshold_;
      public GrowthDegree degree_;
    }
  }
}
