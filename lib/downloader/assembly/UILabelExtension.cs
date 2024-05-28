// Decompiled with JetBrains decompiler
// Type: UILabelExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using UnityEngine;

#nullable disable
public static class UILabelExtension
{
  public static void SetText(this UILabel label, string text)
  {
    if (!Object.op_Implicit((Object) label))
      return;
    label.text = label.GetStandardizationTextJP(text);
  }

  public static void SetTextLocalize(this UILabel label, string text)
  {
    label.SetText(text.ToConverter());
  }

  public static void SetTextLocalize(this UILabel label, int num)
  {
    label.SetTextLocalize(num.ToString());
  }

  public static void SetTextLocalize(this UILabel label, long num)
  {
    label.SetTextLocalize(num.ToString());
  }

  public static void SetTextLocalize(this UILabel label, float num)
  {
    label.SetTextLocalize(num.ToString());
  }

  public static void SetTextRecommendCombat(this UILabel label, string text)
  {
    if (text.Equals("0"))
      text = "---";
    label.SetText(text.ToConverter());
  }

  public static void SetTextRecommendCombat(this UILabel label, int num)
  {
    label.SetTextRecommendCombat(num.ToString());
  }
}
