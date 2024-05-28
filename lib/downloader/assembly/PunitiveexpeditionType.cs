// Decompiled with JetBrains decompiler
// Type: PunitiveexpeditionType
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;

#nullable disable
public static class PunitiveexpeditionType
{
  public static bool IsGuild(this WebAPI.Response.EventTop eventTop) => eventTop.period_type == 2;

  public static bool IsGuild(this EventBattleFinish eventBattleFinish)
  {
    return eventBattleFinish.period_type == 2;
  }

  public static bool IsGuild(this EventInfo eventInfo) => eventInfo.period_type == 2;
}
