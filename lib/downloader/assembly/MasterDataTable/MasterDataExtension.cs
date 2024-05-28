// Decompiled with JetBrains decompiler
// Type: MasterDataTable.MasterDataExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace MasterDataTable
{
  public static class MasterDataExtension
  {
    public static List<int> CommaSeparatedToInts(this string self)
    {
      if (string.IsNullOrEmpty(self))
        return new List<int>();
      string[] array = ((IEnumerable<string>) self.Split(',')).Select<string, string>((Func<string, string>) (s => s.Trim())).ToArray<string>();
      List<int> ints = new List<int>(array.Length);
      bool flag = true;
      foreach (string s in array)
      {
        int result;
        if (int.TryParse(s, out result))
        {
          ints.Add(result);
        }
        else
        {
          flag = false;
          break;
        }
      }
      if (flag)
        return ints;
      ints.Clear();
      foreach (string s in array)
      {
        double result;
        if (double.TryParse(s, out result))
        {
          int num = (int) Math.Floor(result);
          ints.Add(num);
        }
      }
      return ints;
    }
  }
}
