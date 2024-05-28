// Decompiled with JetBrains decompiler
// Type: HeadLabelExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public static class HeadLabelExtension
{
  public static bool SetTextHeadline(
    this UILabel self,
    string text,
    int lengthPerLine,
    int maxLine = 1)
  {
    if (Object.op_Equality((Object) self, (Object) null))
      return false;
    string text1 = text.Replace("\r\n", "\n").Replace("\r", "\n");
    int num = lengthPerLine * maxLine;
    bool flag = false;
    if (!string.IsNullOrEmpty(text1) && num > 0)
    {
      string str1 = "．．．";
      if (text1.Length > num)
      {
        flag = true;
        text1 = text1.Replace("\n", "").Substring(0, num - str1.Length) + str1;
      }
      else if (text1.Length > maxLine)
      {
        string[] source = text1.Split('\n');
        if (source.Length > 1)
        {
          if (source.Length > maxLine)
            flag = true;
          else if (((IEnumerable<string>) source).Max<string>((Func<string, int>) (x => x.Length)) > lengthPerLine)
            flag = true;
          if (flag)
          {
            int count = Mathf.Min(source.Length - 1, maxLine - 1);
            string str2 = count <= 0 ? string.Empty : string.Join("\n", ((IEnumerable<string>) source).Take<string>(count).ToArray<string>());
            text1 = (source[count].Length + str1.Length <= lengthPerLine ? str2 + source[count] : str2 + source[count].Substring(0, lengthPerLine - str1.Length)) + str1;
          }
        }
      }
    }
    self.SetTextLocalize(text1);
    return flag;
  }
}
