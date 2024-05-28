// Decompiled with JetBrains decompiler
// Type: ExtraskillUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;

#nullable disable
public class ExtraskillUtil
{
  private IEnumerator WebAPIAwakeSkillEquip(int idx, PlayerAwakeSkill skill, PlayerUnit unit)
  {
    Future<WebAPI.Response.AwakeSkillEquip> equip = WebAPI.AwakeSkillEquip(idx, skill == null ? new int?() : new int?(skill.id), unit.id, (Action<WebAPI.Response.UserError>) (e =>
    {
      WebAPI.DefaultUserErrorCallback(e);
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    }));
    IEnumerator e1 = equip.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    WebAPI.Response.AwakeSkillEquip result = equip.Result;
  }

  public IEnumerator RemoveExtraSkillAsync(PlayerUnit unit, Action endAct)
  {
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    IEnumerator e = this.WebAPIAwakeSkillEquip(1, (PlayerAwakeSkill) null, unit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (GuildUtil.gvgPopupState != GuildUtil.GvGPopupState.None)
    {
      if (GuildUtil.gvgPopupState == GuildUtil.GvGPopupState.AtkTeam)
      {
        e = GuildUtil.UpdateGuildDeckAttack(PlayerAffiliation.Current.guild_id, Player.Current.id);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      if (GuildUtil.gvgPopupState == GuildUtil.GvGPopupState.DefTeam)
      {
        e = GuildUtil.UpdateGuildDeckDefanse(PlayerAffiliation.Current.guild_id, Player.Current.id);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    endAct();
  }

  public IEnumerator EquipExtraSkillAsync(PlayerAwakeSkill skill, PlayerUnit unit, Action endAct)
  {
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    IEnumerator e = this.WebAPIAwakeSkillEquip(1, skill, unit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (GuildUtil.gvgPopupState != GuildUtil.GvGPopupState.None)
    {
      if (GuildUtil.gvgPopupState == GuildUtil.GvGPopupState.AtkTeam)
      {
        e = GuildUtil.UpdateGuildDeckAttack(PlayerAffiliation.Current.guild_id, Player.Current.id);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      if (GuildUtil.gvgPopupState == GuildUtil.GvGPopupState.DefTeam)
      {
        e = GuildUtil.UpdateGuildDeckDefanse(PlayerAffiliation.Current.guild_id, Player.Current.id);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    endAct();
  }

  public IEnumerator ChangeEquipExtraSkillAsync(
    PlayerAwakeSkill beforeSkill,
    PlayerAwakeSkill afterSkill,
    PlayerUnit unit,
    Action endAct)
  {
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    PlayerUnit equpmentUnit = beforeSkill != null ? beforeSkill.EqupmentUnit : (PlayerUnit) null;
    PlayerUnit afterSkillUnit = afterSkill != null ? afterSkill.EqupmentUnit : (PlayerUnit) null;
    IEnumerator e;
    if (equpmentUnit != (PlayerUnit) null)
    {
      e = this.WebAPIAwakeSkillEquip(1, (PlayerAwakeSkill) null, equpmentUnit);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    if (afterSkillUnit != (PlayerUnit) null)
    {
      e = this.WebAPIAwakeSkillEquip(1, (PlayerAwakeSkill) null, afterSkillUnit);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    e = this.WebAPIAwakeSkillEquip(1, afterSkill, unit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (GuildUtil.gvgPopupState != GuildUtil.GvGPopupState.None)
    {
      if (GuildUtil.gvgPopupState == GuildUtil.GvGPopupState.AtkTeam)
      {
        e = GuildUtil.UpdateGuildDeckAttack(PlayerAffiliation.Current.guild_id, Player.Current.id);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      if (GuildUtil.gvgPopupState == GuildUtil.GvGPopupState.DefTeam)
      {
        e = GuildUtil.UpdateGuildDeckDefanse(PlayerAffiliation.Current.guild_id, Player.Current.id);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    endAct();
  }

  public IEnumerator SwapEquipExtraSkillAsync(
    PlayerAwakeSkill skill,
    PlayerUnit nowEquipUnit,
    PlayerUnit unit,
    Action endAct)
  {
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    IEnumerator e = this.WebAPIAwakeSkillEquip(1, (PlayerAwakeSkill) null, nowEquipUnit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.WebAPIAwakeSkillEquip(1, skill, unit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (GuildUtil.gvgPopupState != GuildUtil.GvGPopupState.None)
    {
      if (GuildUtil.gvgPopupState == GuildUtil.GvGPopupState.AtkTeam)
      {
        e = GuildUtil.UpdateGuildDeckAttack(PlayerAffiliation.Current.guild_id, Player.Current.id);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      if (GuildUtil.gvgPopupState == GuildUtil.GvGPopupState.DefTeam)
      {
        e = GuildUtil.UpdateGuildDeckDefanse(PlayerAffiliation.Current.guild_id, Player.Current.id);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    endAct();
  }
}
