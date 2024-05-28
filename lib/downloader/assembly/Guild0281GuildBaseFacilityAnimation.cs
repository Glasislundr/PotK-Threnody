// Decompiled with JetBrains decompiler
// Type: Guild0281GuildBaseFacilityAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Guild0281GuildBaseFacilityAnimation : MonoBehaviour
{
  [SerializeField]
  private GameObject dir_guild_l_tower;
  [SerializeField]
  private UI2DSprite tower;
  [SerializeField]
  private GameObject dir_guild_wall;
  [SerializeField]
  private UI2DSprite wall;
  [SerializeField]
  private GameObject dir_guild_s_tower;
  [SerializeField]
  private UI2DSprite scaffold;
  [SerializeField]
  private Animator animator;

  public void Initialize(GuildBaseType type, GuildImageCache guildImageCache)
  {
    GameObject gameObject = (GameObject) null;
    UI2DSprite ui2Dsprite = (UI2DSprite) null;
    GuildImagePattern pattern = (GuildImagePattern) null;
    int level = 0;
    switch (type)
    {
      case GuildBaseType.walls:
        gameObject = this.dir_guild_wall;
        ui2Dsprite = this.wall;
        level = PlayerAffiliation.Current.guild.appearance.walls_rank;
        break;
      case GuildBaseType.tower:
        gameObject = this.dir_guild_l_tower;
        ui2Dsprite = this.tower;
        level = PlayerAffiliation.Current.guild.appearance.tower_rank;
        break;
      case GuildBaseType.scaffold:
        gameObject = this.dir_guild_s_tower;
        ui2Dsprite = this.scaffold;
        level = PlayerAffiliation.Current.guild.appearance.scaffold_rank;
        break;
    }
    pattern = GuildImagePattern.Find(type, level);
    if (pattern == null)
      return;
    Sprite facilitySprite = guildImageCache.GetFacilitySprite(type);
    if (!Object.op_Inequality((Object) facilitySprite, (Object) null))
      return;
    ((UIWidget) ((Component) ui2Dsprite).GetComponent<UI2DSprite>()).SetDimensions(((Texture) facilitySprite.texture).width, ((Texture) facilitySprite.texture).height);
    ((Component) ui2Dsprite).GetComponent<UI2DSprite>().sprite2D = facilitySprite;
    gameObject.transform.localPosition = new Vector3(pattern.base_pos.baseXpos, pattern.base_pos.baseYpos);
    GuildBaseAnimation[] array = ((IEnumerable<GuildBaseAnimation>) MasterData.GuildBaseAnimationList).Where<GuildBaseAnimation>((Func<GuildBaseAnimation, bool>) (x => x.anim_group_ID == pattern.base_animation_id)).ToArray<GuildBaseAnimation>();
    for (int index = 0; index < array.Length; ++index)
      this.SetAnimation(((Component) ui2Dsprite).gameObject, array[index].animPrefixSprite, array[index].animframe, array[index].animXpos, array[index].animYpos, index + 1, guildImageCache);
  }

  public void LevelUp(GuildBaseType type)
  {
    switch (type)
    {
      case GuildBaseType.walls:
        this.animator.SetTrigger("wall");
        break;
      case GuildBaseType.tower:
        this.animator.SetTrigger("tower");
        break;
      case GuildBaseType.scaffold:
        this.animator.SetTrigger("scaffold");
        break;
    }
  }

  public void PlaySound(string clip) => Singleton<NGSoundManager>.GetInstance().PlaySe(clip);

  private void SetAnimation(
    GameObject target,
    string prefix,
    int frame,
    float x,
    float y,
    int addDepth,
    GuildImageCache guildImageCache)
  {
    if (prefix == string.Empty)
      return;
    GameObject gameObject = guildImageCache.guildFrameAnim.Clone(target.transform);
    UISpriteAnimation component1 = gameObject.GetComponent<UISpriteAnimation>();
    if (Object.op_Equality((Object) component1, (Object) null))
      return;
    component1.namePrefix = prefix;
    component1.framesPerSecond = frame;
    component1.Reset();
    gameObject.transform.localPosition = new Vector3(x, y);
    UIWidget component2 = target.GetComponent<UIWidget>();
    if (!Object.op_Inequality((Object) component2, (Object) null))
      return;
    gameObject.GetComponent<UIWidget>().depth = component2.depth + addDepth;
  }
}
