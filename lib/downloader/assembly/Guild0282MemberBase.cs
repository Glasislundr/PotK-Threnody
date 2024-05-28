// Decompiled with JetBrains decompiler
// Type: Guild0282MemberBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Guild0282MemberBase : MonoBehaviour
{
  [SerializeField]
  private bool isEnemy;
  private GuildMembership member;
  private Guild0282Menu menu;
  [SerializeField]
  private MemberBaseItem memberBaseItem;
  [SerializeField]
  private List<GameObject> townAnimObj;
  [SerializeField]
  private SpreadColorSprite[] townAnimSprite;
  private List<List<GameObject>> townAnimObjList;
  private GuildImageCache guildImageCache;
  [SerializeField]
  private GameObject dir_lamp_base;
  [SerializeField]
  private GameObject dir_lamp_own;
  [SerializeField]
  private GameObject dir_lamp_enemy;
  [SerializeField]
  private GameObject[] ownLampList;
  [SerializeField]
  private GameObject[] enemyLampList;
  [SerializeField]
  private GameObject[] objsOffLamp_;
  [SerializeField]
  private Animator dir_guild_battle_anim;
  [SerializeField]
  private GameObject dir_star;
  [SerializeField]
  private Animator animStarCount_;
  [SerializeField]
  private Animator animStarBreak_;
  [SerializeField]
  private Animator DamageAnimator;
  [SerializeField]
  private GameObject whiteFlag;
  private bool endAnimation;
  private bool isInitializePlayrtSituation;
  [SerializeField]
  private GameObject slc_icon_caution;
  [SerializeField]
  private GameObject slc_icon_have_battled;
  [SerializeField]
  private GameObject slc_icon_status_open;
  [SerializeField]
  private Guild0282MemberBaseClip clipStarCount_;
  [SerializeField]
  private SpriteDecimalControl starNumerator_;
  [SerializeField]
  private SpriteDecimalControl starDenominator_;
  [SerializeField]
  private GameObject topSimpleBottom_;
  [SerializeField]
  private UILabel txtSimpleBottomNumerator_;
  [SerializeField]
  private UILabel txtSimpleBottomDenominator_;
  private int nowStar_;
  private int toStar_;
  private const int STAR_LOST_TWEEN = 281220;
  private const int LAMP_LOST_TWEEN = 281220;
  private const int situationMemberBaseDepth = 200;
  private readonly string TRIGGER_MAJOR_DAMAGE = "major_damage_wait1";
  private readonly string TRIGGER_HALF_DAMAGE = "half_damage_wait1";
  private readonly string TRIGGER_START_COUNT = "star_num_start";
  private readonly string TRIGGER_END_COUNT = "star_num_loop_end";
  private readonly string TRIGGER_STAR_BREAK = "star_brake";
  private bool endCountDown_;
  private Dictionary<string, Color> dicColor_;
  private UISprite[] numSprites_;

  public bool IsEnemy => this.isEnemy;

  public GuildMembership Member => this.member;

  public void PlayAnim(int nowStar, int breakStar)
  {
    this.SetStar(nowStar);
    if (nowStar == breakStar)
    {
      this.endAnimation = true;
    }
    else
    {
      this.endAnimation = false;
      this.AnimationStar(nowStar, breakStar);
    }
  }

  public void PlayAnim()
  {
  }

  public bool EndAnimation() => this.endAnimation;

  private IEnumerator DamageAnimation(int lastStar)
  {
    if (lastStar == 0)
      this.DamageAnimator.SetTrigger(this.TRIGGER_MAJOR_DAMAGE);
    else
      this.DamageAnimator.SetTrigger(this.TRIGGER_HALF_DAMAGE);
    AnimatorStateInfo animatorStateInfo;
    do
    {
      yield return (object) null;
      animatorStateInfo = this.DamageAnimator.GetCurrentAnimatorStateInfo(0);
    }
    while (!((AnimatorStateInfo) ref animatorStateInfo).IsName("end"));
    this.endAnimation = true;
  }

  private void SetStar(int nowStar, int maxStar)
  {
    if (Object.op_Inequality((Object) this.topSimpleBottom_, (Object) null) && this.topSimpleBottom_.activeSelf)
    {
      this.txtSimpleBottomNumerator_.SetTextLocalize(nowStar);
      this.txtSimpleBottomDenominator_.SetTextLocalize(maxStar);
    }
    else
    {
      this.SetStar(nowStar);
      this.starDenominator_.resetNumber(maxStar);
    }
  }

  private void SetStar(int nowStar) => this.starNumerator_.resetNumber(nowStar);

  private void AnimationStar(int nowStar, int breakStar)
  {
    if (Object.op_Equality((Object) this.animStarCount_, (Object) null))
      return;
    this.nowStar_ = nowStar;
    this.toStar_ = breakStar;
    this.endCountDown_ = false;
    this.numSprites_ = ((Component) this.starNumerator_).gameObject.GetComponentsInChildren<UISprite>(true);
    this.clipStarCount_.setEventSetNumberColor(new Action<string>(this.onSetNumberColor));
    this.clipStarCount_.setEventCountDownStar(new Action(this.onCountDownStar));
    this.animStarCount_.SetTrigger(this.TRIGGER_START_COUNT);
  }

  private void onCountDownStar()
  {
    if (this.endCountDown_)
      return;
    --this.nowStar_;
    this.endCountDown_ = this.toStar_ >= this.nowStar_;
    this.SetStar(this.endCountDown_ ? this.toStar_ : this.nowStar_);
    if (!this.endCountDown_)
      return;
    this.clipStarCount_.setEventCountDownStar();
    this.animStarCount_.SetTrigger(this.TRIGGER_END_COUNT);
    if (Object.op_Inequality((Object) this.animStarBreak_, (Object) null))
      this.animStarBreak_.SetTrigger(this.TRIGGER_STAR_BREAK);
    this.StartCoroutine(this.DamageAnimation(this.toStar_));
  }

  private Color parseColor(string sColor)
  {
    Color white = Color.white;
    if (string.IsNullOrEmpty(sColor))
      return white;
    if (this.dicColor_ == null)
      this.dicColor_ = new Dictionary<string, Color>();
    if (!this.dicColor_.TryGetValue(sColor, out white))
    {
      if (!ColorUtility.TryParseHtmlString(sColor, ref white))
        ColorUtility.TryParseHtmlString("#" + sColor, ref white);
      this.dicColor_.Add(sColor, white);
    }
    return white;
  }

  private void onSetNumberColor(string sColor)
  {
    if (this.numSprites_ == null)
      return;
    Color color = this.parseColor(sColor);
    foreach (UIWidget numSprite in this.numSprites_)
      numSprite.color = color;
  }

  private void SetLamp(GameObject[] listObj, int num)
  {
    for (int index = 0; index < listObj.Length; ++index)
    {
      GameObject gameObject = listObj[index];
      gameObject.SetActive(num > index);
      this.indicateOffLamp(index, !gameObject.activeSelf);
      foreach (UITweener uiTweener in ((IEnumerable<UITweener>) gameObject.GetComponentsInChildren<UITweener>()).Where<UITweener>((Func<UITweener, bool>) (x => x.tweenGroup == 281220)))
        uiTweener.ResetToBeginning();
    }
  }

  private void AnimationLamp(GameObject[] listObj, int nowLamp, int lostLamp)
  {
    int num = 0;
    int index = nowLamp - 1;
    while (num < lostLamp)
    {
      this.indicateOffLamp(index, true);
      foreach (UITweener uiTweener in ((IEnumerable<UITweener>) listObj[index].GetComponentsInChildren<UITweener>()).Where<UITweener>((Func<UITweener, bool>) (x => x.tweenGroup == 281220)))
        uiTweener.PlayForward();
      ++num;
      --index;
    }
  }

  private void indicateOffLamp(int index, bool bIndicate)
  {
    if (this.objsOffLamp_ == null || this.objsOffLamp_.Length <= index)
      return;
    this.objsOffLamp_[index].SetActive(bIndicate);
  }

  public void Initialize(
    GuildMembership memberData,
    Guild0282Menu guild0282menu,
    GuildImageCache imageCache,
    bool enemy,
    GvgStatus gvgStatus)
  {
    this.isEnemy = enemy;
    this.member = memberData;
    this.menu = guild0282menu;
    this.memberBaseItem.SetActive(true);
    this.memberBaseItem.Initialize(this.IsEnemy, this.member);
    this.SetSprite(this.member, imageCache);
    if (Object.op_Inequality((Object) this.topSimpleBottom_, (Object) null))
      this.topSimpleBottom_.SetActive(false);
    this.dir_star.SetActive(this.member.is_defense_membership && GuildUtil.isBattleOrPreparing(gvgStatus));
    this.dir_lamp_base.SetActive(GuildUtil.isBattleOrPreparing(gvgStatus));
    this.SetBattleIcon(this.member.in_battle, this.member.own_star);
    this.SetWhiteFlag(GuildUtil.isBattleOrPreparing(gvgStatus) && !this.member.is_defense_membership);
    this.SetStar(this.member.own_star, PlayerAffiliation.Current.guild.gvg_max_star_possession);
    this.SetTownColor(this.member.in_battle, this.member.own_star, gvgStatus, this.member.is_defense_membership);
    if (this.isEnemy)
    {
      this.dir_lamp_enemy.SetActive(true);
      this.dir_lamp_own.SetActive(false);
      this.SetLamp(this.enemyLampList, this.member.action_point);
      this.RotateTownAnimObj(new Vector3(0.0f, 180f, 0.0f));
    }
    else
    {
      this.dir_lamp_enemy.SetActive(false);
      this.dir_lamp_own.SetActive(true);
      this.SetLamp(this.ownLampList, this.member.action_point);
    }
    this.ControlSituationIcon();
  }

  public void InitializeUpdate(
    GuildMembership memberData,
    Guild0282Menu guild0282menu,
    GuildImageCache imageCache,
    bool enemy,
    GvgStatus gvgStatus)
  {
    this.isEnemy = enemy;
    this.memberBaseItem.Initialize(this.IsEnemy, memberData);
    this.menu = guild0282menu;
    this.dir_star.SetActive(memberData.is_defense_membership && GuildUtil.isBattleOrPreparing(gvgStatus));
    this.dir_lamp_base.SetActive(GuildUtil.isBattleOrPreparing(gvgStatus));
    this.SetBattleIcon(memberData.in_battle, memberData.own_star);
    this.SetWhiteFlag(GuildUtil.isBattleOrPreparing(gvgStatus) && !memberData.is_defense_membership);
    this.SetTownColor(memberData.in_battle, memberData.own_star, gvgStatus, memberData.is_defense_membership);
    if (this.member.own_star > memberData.own_star)
      this.AnimationStar(this.member.own_star, memberData.own_star);
    else
      this.SetStar(memberData.own_star);
    if (this.member.action_point > memberData.action_point)
    {
      if (this.isEnemy)
        this.AnimationLamp(this.enemyLampList, this.member.action_point, this.member.action_point - memberData.action_point);
      else
        this.AnimationLamp(this.ownLampList, this.member.action_point, this.member.action_point - memberData.action_point);
    }
    else if (this.isEnemy)
      this.SetLamp(this.enemyLampList, memberData.action_point);
    else
      this.SetLamp(this.ownLampList, memberData.action_point);
    this.ControlSituationIcon();
    this.member = memberData;
  }

  public void ControlSituationIcon()
  {
    if (this.isEnemy)
    {
      if (Object.op_Inequality((Object) this.slc_icon_caution, (Object) null))
      {
        GuildMembership guildMembership = ((IEnumerable<GuildMembership>) PlayerAffiliation.Current.guild.memberships).FirstOrDefault<GuildMembership>((Func<GuildMembership, bool>) (x => x.player.player_id == Player.Current.id));
        if (guildMembership != null)
          this.slc_icon_caution.SetActive(guildMembership.total_attack < this.member.total_defense);
      }
      if (Object.op_Inequality((Object) this.slc_icon_have_battled, (Object) null))
        this.slc_icon_have_battled.SetActive(this.menu.IsOpposedPlayer(this.member.player.player_id));
      if (!Object.op_Inequality((Object) this.slc_icon_status_open, (Object) null))
        return;
      this.slc_icon_status_open.SetActive(this.member.scouted);
    }
    else
    {
      if (Object.op_Inequality((Object) this.slc_icon_caution, (Object) null))
        this.slc_icon_caution.SetActive(false);
      if (Object.op_Inequality((Object) this.slc_icon_have_battled, (Object) null))
        this.slc_icon_have_battled.SetActive(false);
      if (!Object.op_Inequality((Object) this.slc_icon_status_open, (Object) null))
        return;
      this.slc_icon_status_open.SetActive(false);
    }
  }

  public void Focus()
  {
    if (!Object.op_Inequality((Object) this.menu, (Object) null))
      return;
    this.menu.onButtonMemberBase(this, this.member);
  }

  public void SetSprite(GuildMembership memberData, GuildImageCache imageCache)
  {
    this.guildImageCache = imageCache;
    this.InitTownAnimObject();
  }

  private void GetUIWidgetChildren(Transform trans, List<UIWidget> list)
  {
    foreach (Transform child in trans.GetChildren())
    {
      UIWidget component = ((Component) child).GetComponent<UIWidget>();
      if (Object.op_Inequality((Object) component, (Object) null))
        list.Add(component);
      this.GetUIWidgetChildren(child, list);
    }
  }

  public void InitializePlayerSituation(GuildMembership memberData, GuildImageCache imageCache)
  {
    this.isEnemy = false;
    this.guildImageCache = imageCache;
    this.memberBaseItem.SetActive(false);
    bool defenseMembership = memberData.is_defense_membership;
    if (Object.op_Inequality((Object) this.topSimpleBottom_, (Object) null))
      this.topSimpleBottom_.SetActive(defenseMembership);
    this.dir_star.SetActive(defenseMembership);
    this.dir_lamp_base.SetActive(false);
    this.SetBattleIcon(memberData.in_battle, memberData.own_star);
    this.SetWhiteFlag(!memberData.is_defense_membership);
    GuildRegistration guild = PlayerAffiliation.Current.guild;
    this.SetStar(memberData.own_star, guild.gvg_max_star_possession);
    this.InitTownAnimObject();
    this.SetTownColor(memberData.in_battle, memberData.own_star, GvgStatus.fighting, memberData.is_defense_membership);
    if (!this.isInitializePlayrtSituation)
    {
      this.isInitializePlayrtSituation = true;
      List<UIWidget> list = new List<UIWidget>();
      this.GetUIWidgetChildren(((Component) this).transform, list);
      list.ForEach((Action<UIWidget>) (x => x.depth += 200));
    }
    this.ControlSituationIcon();
  }

  public void InitializeGB(
    int townLevel,
    int ownStar,
    GuildImageCache imageCache,
    bool isTestBattle)
  {
    this.isEnemy = true;
    this.guildImageCache = imageCache;
    this.memberBaseItem.SetActive(false);
    if (isTestBattle)
    {
      if (Object.op_Inequality((Object) this.topSimpleBottom_, (Object) null))
        this.topSimpleBottom_.SetActive(false);
      this.dir_star.SetActive(false);
    }
    else
    {
      if (Object.op_Inequality((Object) this.topSimpleBottom_, (Object) null))
        this.topSimpleBottom_.SetActive(true);
      this.dir_star.SetActive(true);
    }
    this.dir_lamp_base.SetActive(false);
    this.SetBattleIcon(false, ownStar);
    this.SetWhiteFlag(false);
    GuildRegistration guild = PlayerAffiliation.Current.guild;
    this.SetStar(ownStar, guild.gvg_max_star_possession);
    this.InitTownAnimObject();
    this.SetTownColor(false, ownStar, GvgStatus.fighting, true);
  }

  private void SetBattleIcon(bool isBattle, int ownStar)
  {
    if (Object.op_Equality((Object) this.dir_guild_battle_anim, (Object) null))
      return;
    this.dir_guild_battle_anim.SetBool("isAttack", isBattle);
  }

  private void SetTownColor(bool isBattle, int ownStar, GvgStatus gvgStatus, bool isDefenseMember)
  {
    if (isBattle || GuildUtil.isBattle(gvgStatus) && (ownStar == 0 || !isDefenseMember))
    {
      this.ColorChangeTownAnimSprite(Color.gray);
    }
    else
    {
      if (ownStar == 0)
        return;
      this.ColorChangeTownAnimSprite(Color.white);
    }
  }

  private void SetWhiteFlag(bool isNotDefense)
  {
    if (!Object.op_Inequality((Object) this.whiteFlag, (Object) null))
      return;
    this.whiteFlag.SetActive(isNotDefense);
  }

  private void SetPattern(
    int level,
    GuildBaseType type,
    GameObject target,
    List<GameObject> animList)
  {
    GuildImagePattern pattern = GuildImagePattern.Find(type, level);
    target.GetComponent<UISprite>().SetSprite(pattern.TownSpriteName());
    target.transform.localPosition = new Vector3(pattern.base_pos.baseXpos, pattern.base_pos.baseYpos);
    GuildBaseAnimation[] array = ((IEnumerable<GuildBaseAnimation>) MasterData.GuildBaseAnimationList).Where<GuildBaseAnimation>((Func<GuildBaseAnimation, bool>) (x => x.anim_group_ID == pattern.base_animation_id)).OrderBy<GuildBaseAnimation, int>((Func<GuildBaseAnimation, int>) (x => x.ID)).ToArray<GuildBaseAnimation>();
    for (int index = 0; index < array.Length; ++index)
      this.SetAnimation(target, animList, array[index].animPrefixSprite, array[index].animframe, array[index].animXpos, array[index].animYpos, index + 1);
    target.SetActive(false);
    target.SetActive(true);
  }

  private void OnPress(bool isDown)
  {
    if (!Object.op_Inequality((Object) this.menu, (Object) null))
      return;
    this.menu.OnPress(isDown);
  }

  private void OnDrag(Vector2 delta)
  {
    if (!Object.op_Inequality((Object) this.menu, (Object) null))
      return;
    this.menu.OnDrag(delta);
  }

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

  private void Update()
  {
    if (!Object.op_Equality((Object) this.dir_guild_battle_anim, (Object) null))
      return;
    foreach (UIRect uiRect in this.townAnimSprite)
      uiRect.Invalidate(true);
  }

  private void InitTownAnimObject()
  {
    if (this.townAnimObjList == null)
      this.townAnimObjList = new List<List<GameObject>>();
    for (int index = 0; index < this.townAnimObj.Count; ++index)
    {
      if (this.townAnimObjList.Count < this.townAnimObj.Count)
        this.townAnimObjList.Add(new List<GameObject>());
      this.townAnimObjList[index] = this.ClearAnimation(this.townAnimObjList[index]);
      this.SetPattern(1, GuildBaseType.town, this.townAnimObj[index], this.townAnimObjList[index]);
    }
  }

  private void ColorChangeTownAnimSprite(Color color)
  {
    foreach (UIWidget uiWidget in this.townAnimSprite)
      uiWidget.color = color;
  }

  private void RotateTownAnimObj(Vector3 localEulerAngles)
  {
    foreach (Component component in this.townAnimSprite)
      component.transform.localEulerAngles = localEulerAngles;
  }
}
