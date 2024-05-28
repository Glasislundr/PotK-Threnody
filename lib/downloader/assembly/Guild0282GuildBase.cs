// Decompiled with JetBrains decompiler
// Type: Guild0282GuildBase
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
public class Guild0282GuildBase : MonoBehaviour
{
  [SerializeField]
  private bool isEnemy;
  [SerializeField]
  private UIButton button;
  [SerializeField]
  private GuildBaseItem guildBaseItem;
  private GuildRegistration guild;
  private Guild0282Menu menu;
  [SerializeField]
  private GameObject fortressObj;
  private List<GameObject> fortressAnimObjList;
  [SerializeField]
  private GameObject wallObj;
  private List<GameObject> wallAnimObjList;
  [SerializeField]
  private GameObject towerObj;
  private List<GameObject> towerAnimObjList;
  [SerializeField]
  private GameObject scaffoldObj;
  private List<GameObject> scaffoldAnimObjList;
  private Guild0281GuildBaseFacilityAnimation levelupAnim;
  [SerializeField]
  private GameObject dir_guild_main_facility;
  private GuildImageCache guildImageCache;

  public bool IsEnemy => this.isEnemy;

  public void Initialize(
    GuildRegistration guildData,
    Guild0282Menu guild0282menu,
    GuildImageCache imageCache,
    bool enemy)
  {
    this.isEnemy = enemy;
    ((Behaviour) this.button).enabled = true;
    this.guild = guildData;
    this.menu = guild0282menu;
    this.SetSprite(this.guild.appearance, imageCache);
    this.guildBaseItem.SetActive(true);
    this.guildBaseItem.Initialize(this.IsEnemy, guildData.guild_name);
    if (!this.isEnemy)
      return;
    this.dir_guild_main_facility.transform.localEulerAngles = new Vector3(0.0f, 180f, 0.0f);
  }

  public void InitializeUpdate(
    GuildRegistration guildData,
    Guild0282Menu guild0282menu,
    GuildImageCache imageCache,
    bool enemy)
  {
    this.guild = guildData;
    this.menu = guild0282menu;
    this.SetSprite(this.guild.appearance, imageCache);
  }

  public void SetEnemy(bool enemy) => this.isEnemy = enemy;

  public void SetSprite(
    GuildAppearance guildData,
    GuildImageCache imageCache,
    GameObject levelupAnimObj = null)
  {
    this.guildImageCache = imageCache;
    this.fortressAnimObjList = this.ClearAnimation(this.fortressAnimObjList);
    this.scaffoldAnimObjList = this.ClearAnimation(this.scaffoldAnimObjList);
    this.towerAnimObjList = this.ClearAnimation(this.towerAnimObjList);
    this.wallAnimObjList = this.ClearAnimation(this.wallAnimObjList);
    this.SetPattern(guildData.level, guildData.level, imageCache.GetFacilitySprite(GuildBaseType.fortress), GuildBaseType.fortress, this.fortressObj, this.fortressAnimObjList);
    this.SetPattern(guildData.scaffold_rank, guildData.level, imageCache.GetFacilitySprite(GuildBaseType.scaffold), GuildBaseType.scaffold, this.scaffoldObj, this.scaffoldAnimObjList);
    this.SetPattern(guildData.tower_rank, guildData.level, imageCache.GetFacilitySprite(GuildBaseType.tower), GuildBaseType.tower, this.towerObj, this.towerAnimObjList);
    this.SetPattern(guildData.walls_rank, guildData.level, imageCache.GetFacilitySprite(GuildBaseType.walls), GuildBaseType.walls, this.wallObj, this.wallAnimObjList);
    if (!Object.op_Inequality((Object) levelupAnimObj, (Object) null))
      return;
    if (Object.op_Inequality((Object) this.levelupAnim, (Object) null))
      Object.Destroy((Object) ((Component) this.levelupAnim).gameObject);
    this.levelupAnim = levelupAnimObj.Clone(this.dir_guild_main_facility.transform).GetComponent<Guild0281GuildBaseFacilityAnimation>();
  }

  public Vector3 GetRootPosition() => this.dir_guild_main_facility.transform.localPosition;

  public void AdjustRootPositionByFortress(Vector3 basePos)
  {
    Texture2D texture = this.fortressObj.GetComponent<UI2DSprite>().sprite2D.texture;
    float num1 = (float) ((446.0 - (double) ((Texture) texture).width) * 0.25);
    float num2 = (float) ((470.0 - (double) ((Texture) texture).height) * 0.5);
    Vector3 localPosition = this.dir_guild_main_facility.transform.localPosition;
    localPosition.x = basePos.x + num1;
    localPosition.y = basePos.y + num2;
    this.dir_guild_main_facility.transform.localPosition = localPosition;
  }

  public void GuildBankLevelUpSetSprite(int level, GuildImageCache imageCache)
  {
    this.guildImageCache = imageCache;
    GuildRegistration guild = PlayerAffiliation.Current.guild;
    this.fortressAnimObjList = this.ClearAnimation(this.fortressAnimObjList);
    this.scaffoldAnimObjList = this.ClearAnimation(this.scaffoldAnimObjList);
    this.towerAnimObjList = this.ClearAnimation(this.towerAnimObjList);
    this.wallAnimObjList = this.ClearAnimation(this.wallAnimObjList);
    int num = guild.appearance.level - level;
    this.SetPattern(level, level, imageCache.GuildBankLevelupFotressSpriteList[imageCache.GuildBankLevelupFotressSpriteList.Count - num - 1], GuildBaseType.fortress, this.fortressObj, this.fortressAnimObjList);
    this.SetPattern(guild.appearance.scaffold_rank, level, imageCache.GetFacilitySprite(GuildBaseType.scaffold), GuildBaseType.scaffold, this.scaffoldObj, this.scaffoldAnimObjList);
    this.SetPattern(guild.appearance.tower_rank, level, imageCache.GetFacilitySprite(GuildBaseType.tower), GuildBaseType.tower, this.towerObj, this.towerAnimObjList);
    this.SetPattern(guild.appearance.walls_rank, level, imageCache.GetFacilitySprite(GuildBaseType.walls), GuildBaseType.walls, this.wallObj, this.wallAnimObjList);
  }

  public void HQLevelUp(GuildBaseType type, GuildImageCache imageCache)
  {
    if (Object.op_Equality((Object) this.levelupAnim, (Object) null))
      return;
    this.levelupAnim.Initialize(type, imageCache);
    this.levelupAnim.LevelUp(type);
  }

  private void SetPattern(
    int level,
    int guildLevel,
    Sprite sprite,
    GuildBaseType type,
    GameObject target,
    List<GameObject> animList)
  {
    if (!((IEnumerable<GuildBase>) MasterData.GuildBaseList).Any<GuildBase>((Func<GuildBase, bool>) (x => x.base_type == type && x.release_level <= guildLevel)))
    {
      sprite = GuildImageCache.AlphaSprite();
      animList = (List<GameObject>) null;
    }
    GuildImagePattern pattern = GuildImagePattern.Find(type, level);
    if (Object.op_Inequality((Object) sprite, (Object) null))
    {
      ((UIWidget) target.GetComponent<UI2DSprite>()).SetDimensions(((Texture) sprite.texture).width, ((Texture) sprite.texture).height);
      target.GetComponent<UI2DSprite>().sprite2D = sprite;
    }
    if (animList == null || pattern == null)
      return;
    target.transform.localPosition = new Vector3(pattern.base_pos.baseXpos, pattern.base_pos.baseYpos);
    GuildBaseAnimation[] array = ((IEnumerable<GuildBaseAnimation>) MasterData.GuildBaseAnimationList).Where<GuildBaseAnimation>((Func<GuildBaseAnimation, bool>) (x => x.anim_group_ID == pattern.base_animation_id)).OrderBy<GuildBaseAnimation, int>((Func<GuildBaseAnimation, int>) (x => x.ID)).ToArray<GuildBaseAnimation>();
    for (int index = 0; index < array.Length; ++index)
      this.SetAnimation(target, animList, array[index].animPrefixSprite, array[index].animframe, array[index].animXpos, array[index].animYpos, index + 1);
  }

  public void Focus() => this.menu.onButtonGuildBase(this, this.guild);

  private void OnPress(bool isDown) => this.menu.OnPress(isDown);

  private void OnDrag(Vector2 delta) => this.menu.OnDrag(delta);

  private List<GameObject> ClearAnimation(List<GameObject> objList)
  {
    if (objList == null)
      objList = new List<GameObject>();
    if (objList.Count > 0)
    {
      objList.ForEach((Action<GameObject>) (x => Object.Destroy((Object) x)));
      objList.Clear();
    }
    return objList;
  }

  private void SetAnimation(
    GameObject target,
    List<GameObject> objList,
    string prefix,
    int frame,
    float x,
    float y,
    int addDepth)
  {
    if (prefix == string.Empty)
      return;
    GameObject gameObject = this.guildImageCache.guildFrameAnim.Clone(target.transform);
    objList.Add(gameObject);
    UISpriteAnimation component1 = gameObject.GetComponent<UISpriteAnimation>();
    if (Object.op_Equality((Object) component1, (Object) null))
      return;
    if (this.IsEnemy)
      prefix = prefix.Replace("own", "enemy");
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
