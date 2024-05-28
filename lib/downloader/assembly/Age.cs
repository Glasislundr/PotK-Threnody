// Decompiled with JetBrains decompiler
// Type: Age
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
public static class Age
{
  public static int GetAge(DateTime birthday)
  {
    DateTime dateTime = birthday;
    DateTime now = DateTime.Now;
    int age = now.Year - dateTime.Year;
    if (new DateTime(now.Year, dateTime.Month, dateTime.Day) > now)
      --age;
    return age;
  }
}
