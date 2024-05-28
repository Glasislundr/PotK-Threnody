// Decompiled with JetBrains decompiler
// Type: GuildImageCache
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class GuildImageCache
{
  private Sprite fortress;
  private Sprite walls;
  private Sprite scaffold;
  private Sprite tower;
  public GameObject guildFrameAnim;

  public List<Sprite> GuildBankLevelupFotressSpriteList { get; set; }

  public static Sprite AlphaSprite()
  {
    SpriteMeshType spriteMeshType = (SpriteMeshType) 0;
    uint num1 = 1;
    uint num2 = 100;
    Texture2D texture2D = Resources.Load<Texture2D>("Sprites/1x1_alpha0");
    Sprite sprite = Sprite.Create(texture2D, new Rect(0.0f, 0.0f, (float) ((Texture) texture2D).width, (float) ((Texture) texture2D).height), new Vector2(0.5f, 0.5f), (float) num1, num2, spriteMeshType);
    ((Object) sprite).name = ((Object) texture2D).name;
    return sprite;
  }

  public IEnumerator FacilitiResourceLoad(GuildAppearance guildData)
  {
    Sprite alpha = GuildImageCache.AlphaSprite();
    IEnumerator e = this.SetSprite(GuildBaseType.scaffold, guildData.scaffold_rank, alpha);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.SetSprite(GuildBaseType.tower, guildData.tower_rank, alpha);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.SetSprite(GuildBaseType.walls, guildData.walls_rank, alpha);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator GuildBankLevelUpResourceLoad(int levelUpCount)
  {
    GuildRegistration guildData = PlayerAffiliation.Current.guild;
    Sprite alpha = GuildImageCache.AlphaSprite();
    if (this.GuildBankLevelupFotressSpriteList == null)
      this.GuildBankLevelupFotressSpriteList = new List<Sprite>();
    this.GuildBankLevelupFotressSpriteList.Clear();
    IEnumerator e;
    for (int i = guildData.appearance.level - levelUpCount; i <= guildData.appearance.level; ++i)
    {
      GuildImagePattern guildImagePattern = GuildImagePattern.Find(GuildBaseType.fortress, i);
      if (guildImagePattern == null)
      {
        this.GuildBankLevelupFotressSpriteList.Add(alpha);
      }
      else
      {
        Future<Sprite> sprite = guildImagePattern.LoadSpriteFortress();
        if (sprite == null)
        {
          this.GuildBankLevelupFotressSpriteList.Add(alpha);
        }
        else
        {
          e = sprite.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          this.GuildBankLevelupFotressSpriteList.Add(sprite.Result);
        }
        sprite = (Future<Sprite>) null;
      }
    }
    e = this.FacilitiResourceLoad(guildData.appearance);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.GuildFrameAnimResourceLoad();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator ResourceLoad(GuildAppearance guildData)
  {
    Sprite alpha = GuildImageCache.AlphaSprite();
    IEnumerator e = this.SetSprite(GuildBaseType.fortress, guildData.level, alpha);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.FacilitiResourceLoad(guildData);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.GuildFrameAnimResourceLoad();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator GuildFrameAnimResourceLoad()
  {
    if (Object.op_Equality((Object) this.guildFrameAnim, (Object) null))
    {
      Future<GameObject> f = Res.Prefabs.guild028_2.guild_frame_anim.Load<GameObject>();
      IEnumerator e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.guildFrameAnim = f.Result;
      f = (Future<GameObject>) null;
    }
  }

  private IEnumerator SetSprite(GuildBaseType type, int rank, Sprite alpha)
  {
    GuildImagePattern guildImagePattern = GuildImagePattern.Find(type, rank);
    if (guildImagePattern == null)
    {
      this.SetFacilitySprite(type, alpha);
    }
    else
    {
      Future<Sprite> sprite = guildImagePattern.LoadSpriteFacility(type);
      if (sprite == null)
      {
        this.SetFacilitySprite(type, alpha);
      }
      else
      {
        IEnumerator e = sprite.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.SetFacilitySprite(type, sprite.Result);
      }
      sprite = (Future<Sprite>) null;
    }
  }

  private void SetFacilitySprite(GuildBaseType type, Sprite sprite)
  {
    switch (type)
    {
      case GuildBaseType.walls:
        this.walls = sprite;
        break;
      case GuildBaseType.tower:
        this.tower = sprite;
        break;
      case GuildBaseType.scaffold:
        this.scaffold = sprite;
        break;
      case GuildBaseType.fortress:
        this.fortress = sprite;
        break;
    }
  }

  public Sprite GetFacilitySprite(GuildBaseType type)
  {
    switch (type)
    {
      case GuildBaseType.walls:
        return this.walls;
      case GuildBaseType.tower:
        return this.tower;
      case GuildBaseType.scaffold:
        return this.scaffold;
      case GuildBaseType.fortress:
        return this.fortress;
      default:
        return (Sprite) null;
    }
  }
}
