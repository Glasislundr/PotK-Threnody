// Decompiled with JetBrains decompiler
// Type: FavorabilityRatingEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class FavorabilityRatingEffect : MonoBehaviour
{
  private const int RELEASE_SKILL = 0;
  private const int RELEASE_SKILL_FRAME = 1;
  private FavorabilityRatingEffect.AnimationType animationType;
  private FavorabilityRatingEffect.AnimState animState;
  private bool isTrustUpperLimit;
  private Action endAction;
  private Modified<Player> playerModified;
  private PlayerUnit unit;
  [SerializeField]
  private UI2DSprite slc_background;
  [SerializeField]
  private Animator animController;
  [SerializeField]
  private Vector3[] txtDearDegreePos;
  [SerializeField]
  private Vector3[] txtAchievementPos;
  [SerializeField]
  private GameObject txtDearDegree;
  [SerializeField]
  private UILabel txtPercent;
  [SerializeField]
  private GameObject txtAchievement;
  [SerializeField]
  private UI2DSprite dynUnitThum;
  [SerializeField]
  private LoveGaugeController loveGaugeController;
  [SerializeField]
  private GameObject[] slcTextSprite;
  [SerializeField]
  private GameObject dyn_unit;
  [SerializeField]
  private GameObject dir_ballon;
  [SerializeField]
  private UI2DSprite dyn_unit_thum;
  [SerializeField]
  private UILabel txt_unit_name;
  [SerializeField]
  private UILabel txt_message;
  private NGxFaceSprite unitFace;
  private string[] serifs;
  private string[] voices;
  private int serifIndex;
  [SerializeField]
  private UILabel txt_skill_name;
  [SerializeField]
  private UILabel txt_skill_description;
  [SerializeField]
  private UI2DSprite dyn_Unit_Skill;
  [SerializeField]
  private UILabel label_SkillLvMax;
  [SerializeField]
  private Transform skillTargetParent1;
  [SerializeField]
  private Transform skillTargetParent2;
  [SerializeField]
  private AnchorCustomAdjustment anchorCustomAdjustment;
  private bool skipUnitAnimation;

  private GameObject CreateGenreIcon(GameObject prefab, Transform trans)
  {
    GameObject genreIcon = prefab.Clone(trans);
    UI2DSprite componentInChildren = genreIcon.GetComponentInChildren<UI2DSprite>();
    if (!Object.op_Inequality((Object) componentInChildren, (Object) null))
      return genreIcon;
    UI2DSprite ui2Dsprite = componentInChildren;
    ((UIWidget) ui2Dsprite).depth = ((UIWidget) ui2Dsprite).depth + 150;
    return genreIcon;
  }

  private IEnumerator SetBackground(string name)
  {
    Future<Sprite> bgF = new ResourceObject(string.Format(Consts.GetInstance().BACKGROUND_BASE_PATH, (object) name)).Load<Sprite>();
    IEnumerator e = bgF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Inequality((Object) bgF.Result, (Object) null))
      this.slc_background.sprite2D = bgF.Result;
  }

  public IEnumerator Init(
    FavorabilityRatingEffect.AnimationType anmType,
    PlayerUnit playerUnit,
    Action action,
    bool skipUnitAnimation = false,
    bool isPreview = false)
  {
    FavorabilityRatingEffect favorabilityRatingEffect = this;
    if (favorabilityRatingEffect.playerModified == null)
      favorabilityRatingEffect.playerModified = SMManager.Observe<Player>();
    favorabilityRatingEffect.animationType = anmType;
    favorabilityRatingEffect.endAction = action;
    favorabilityRatingEffect.unit = playerUnit;
    List<SwitchUnitComponentBase> list = ((IEnumerable<SwitchUnitComponentBase>) ((Component) favorabilityRatingEffect).GetComponentsInChildren<SwitchUnitComponentBase>(true)).ToList<SwitchUnitComponentBase>();
    for (int index = 0; index < list.Count; ++index)
      list[index].SwitchMaterial(favorabilityRatingEffect.unit.unit.ID);
    favorabilityRatingEffect.skipUnitAnimation = skipUnitAnimation;
    favorabilityRatingEffect.animState = FavorabilityRatingEffect.AnimState.Start;
    Future<Sprite> spriteF = playerUnit.unit.LoadSpriteThumbnail();
    IEnumerator e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    favorabilityRatingEffect.dynUnitThum.sprite2D = spriteF.Result;
    spriteF = (Future<Sprite>) null;
    int num = (int) playerUnit.trust_rate;
    if (anmType == FavorabilityRatingEffect.AnimationType.SkillFrameRelease)
      num = PlayerUnit.GetExtraSkillReleaseRate();
    Consts instance = Consts.GetInstance();
    favorabilityRatingEffect.txtPercent.SetTextLocalize(Consts.Format(Consts.GetInstance().UNIT_TRUST_RATE_PERCENT, (IDictionary) new Hashtable()
    {
      {
        (object) "trust_rate",
        (object) string.Format("{0}", (object) (float) ((double) Mathf.FloorToInt((float) num / instance.TRUST_RATE_LEVEL_SIZE) * (double) instance.TRUST_RATE_LEVEL_SIZE))
      }
    }));
    favorabilityRatingEffect.txtDearDegree.transform.localPosition = favorabilityRatingEffect.txtDearDegreePos[(int) favorabilityRatingEffect.animationType];
    favorabilityRatingEffect.txtAchievement.transform.localPosition = favorabilityRatingEffect.txtAchievementPos[(int) favorabilityRatingEffect.animationType];
    ((IEnumerable<GameObject>) favorabilityRatingEffect.slcTextSprite).ToggleOnce((int) favorabilityRatingEffect.animationType);
    if (anmType == FavorabilityRatingEffect.AnimationType.SkillRelease)
    {
      UnitSkillAwake[] awakeSkills = playerUnit.GetAwakeSkills();
      if (awakeSkills != null && awakeSkills.Length != 0)
      {
        GameObject skillGenrePrefab = (GameObject) null;
        Future<GameObject> prefabF = Res.Icons.SkillGenreIcon.Load<GameObject>();
        e = prefabF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        skillGenrePrefab = prefabF.Result;
        BattleskillSkill skill = awakeSkills[0].skill;
        favorabilityRatingEffect.txt_skill_name.SetTextLocalize(skill.name);
        favorabilityRatingEffect.txt_skill_description.SetTextLocalize(skill.description);
        favorabilityRatingEffect.label_SkillLvMax.SetTextLocalize(skill.upper_level);
        spriteF = skill.LoadBattleSkillIcon();
        e = spriteF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        favorabilityRatingEffect.dyn_Unit_Skill.sprite2D = spriteF.Result;
        favorabilityRatingEffect.CreateGenreIcon(skillGenrePrefab, favorabilityRatingEffect.skillTargetParent1).GetComponent<SkillGenreIcon>().Init(skill.genre1);
        favorabilityRatingEffect.CreateGenreIcon(skillGenrePrefab, favorabilityRatingEffect.skillTargetParent2).GetComponent<SkillGenreIcon>().Init(skill.genre2);
        skillGenrePrefab = (GameObject) null;
        prefabF = (Future<GameObject>) null;
        skill = (BattleskillSkill) null;
        spriteF = (Future<Sprite>) null;
      }
      e = favorabilityRatingEffect.SettingTrustGaugeMaxInfo(favorabilityRatingEffect.unit);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      favorabilityRatingEffect.anchorCustomAdjustment.resetAnchors();
      awakeSkills = (UnitSkillAwake[]) null;
    }
    favorabilityRatingEffect.StartCoroutine(favorabilityRatingEffect.loveGaugeController.setValue((int) playerUnit.trust_rate, (int) playerUnit.trust_rate, (int) playerUnit.trust_max_rate, (int) Consts.GetInstance().TRUST_RATE_LEVEL_SIZE, false));
  }

  public void StartEffect()
  {
    if (this.animationType == FavorabilityRatingEffect.AnimationType.SkillRelease)
    {
      if (this.skipUnitAnimation)
        this.animController.SetTrigger("skill_start_without_unit_animation");
      else
        this.animController.SetTrigger("skill_start");
    }
    else
      this.animController.SetTrigger("skill_frame_start");
  }

  private IEnumerator SettingTrustGaugeMaxInfo(PlayerUnit playerUnit)
  {
    FavorabilityRatingEffect favorabilityRatingEffect = this;
    if (favorabilityRatingEffect.skipUnitAnimation)
    {
      favorabilityRatingEffect.animState = FavorabilityRatingEffect.AnimState.Loop;
      favorabilityRatingEffect.animController.SetTrigger("skill_unit_end");
    }
    else
    {
      UnitUnit pUnit = playerUnit.unit;
      favorabilityRatingEffect.dyn_unit.transform.Clear();
      Future<GameObject> future = pUnit.LoadStory();
      IEnumerator e = future.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Sprite sprite = (Sprite) null;
      if (pUnit.ExistSpriteStory())
      {
        Future<Sprite> spriteFuture = pUnit.LoadSpriteStory();
        e = spriteFuture.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        sprite = spriteFuture.Result;
        spriteFuture = (Future<Sprite>) null;
      }
      GameObject spriteObject = future.Result.Clone(favorabilityRatingEffect.dyn_unit.transform);
      spriteObject.GetComponent<UIWidget>().depth = 8;
      pUnit.SetStoryData(spriteObject);
      NGxMaskSpriteWithScale component1 = spriteObject.GetComponent<NGxMaskSpriteWithScale>();
      if (Object.op_Inequality((Object) sprite, (Object) null))
        component1.MainUI2DSprite.sprite2D = sprite;
      component1.SetMaskEnable(false);
      UIWidget component2 = favorabilityRatingEffect.dyn_unit.GetComponent<UIWidget>();
      if (Object.op_Inequality((Object) component2, (Object) null))
      {
        UIWidget w = spriteObject.GetComponent<UIWidget>();
        w.depth = component2.depth;
        UIWidget[] componentsInChildren = spriteObject.GetComponentsInChildren<UIWidget>();
        ((IEnumerable<UIWidget>) componentsInChildren).Where<UIWidget>((Func<UIWidget, bool>) (v => ((Object) ((Component) v).transform).name == "face")).ForEach<UIWidget>((Action<UIWidget>) (v => v.depth = w.depth + 1));
        ((IEnumerable<UIWidget>) componentsInChildren).Where<UIWidget>((Func<UIWidget, bool>) (v => ((Object) ((Component) v).transform).name == "eye")).ForEach<UIWidget>((Action<UIWidget>) (v => v.depth = w.depth + 2));
      }
      Future<Sprite> thumFuture = pUnit.LoadSpriteThumbnail();
      e = thumFuture.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Sprite result = thumFuture.Result;
      favorabilityRatingEffect.dyn_unit_thum.sprite2D = result;
      UI2DSprite dynUnitThum1 = favorabilityRatingEffect.dyn_unit_thum;
      Rect rect1 = result.rect;
      int width = (int) ((Rect) ref rect1).width;
      ((UIWidget) dynUnitThum1).width = width;
      UI2DSprite dynUnitThum2 = favorabilityRatingEffect.dyn_unit_thum;
      Rect rect2 = result.rect;
      int height = (int) ((Rect) ref rect2).height;
      ((UIWidget) dynUnitThum2).height = height;
      favorabilityRatingEffect.txt_unit_name.SetTextLocalize(pUnit.name);
      favorabilityRatingEffect.serifIndex = 0;
      // ISSUE: reference to a compiler-generated method
      UnitTrustUpperLimitEffect upperLimitData = ((IEnumerable<UnitTrustUpperLimitEffect>) MasterData.UnitTrustUpperLimitEffectList).FirstOrDefault<UnitTrustUpperLimitEffect>(new Func<UnitTrustUpperLimitEffect, bool>(favorabilityRatingEffect.\u003CSettingTrustGaugeMaxInfo\u003Eb__41_0));
      favorabilityRatingEffect.serifs = upperLimitData.GetSerif();
      favorabilityRatingEffect.voices = upperLimitData.GetVoice();
      if (!string.IsNullOrEmpty(upperLimitData.background))
      {
        e = favorabilityRatingEffect.SetBackground(upperLimitData.background);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      favorabilityRatingEffect.unitFace = spriteObject.GetComponent<NGxFaceSprite>();
      ((Component) favorabilityRatingEffect.unitFace).GetComponent<UIWidget>().depth = 9;
      e = favorabilityRatingEffect.unitFace.ChangeFace(upperLimitData.face);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (!favorabilityRatingEffect.SetSerif(favorabilityRatingEffect.serifs, favorabilityRatingEffect.voices, favorabilityRatingEffect.serifIndex))
      {
        favorabilityRatingEffect.animState = FavorabilityRatingEffect.AnimState.Loop;
        favorabilityRatingEffect.animController.SetTrigger("skill_unit_end");
      }
    }
  }

  private bool SetSerif(string[] serifs, string[] voiceCues, int index)
  {
    Hashtable args = (Hashtable) null;
    int num = ((IEnumerable<string>) serifs).Count<string>();
    if (serifs == null || num < 0 || num <= index)
      return false;
    if (args == null || args != null && !args.ContainsKey((object) "userName"))
    {
      if (args == null)
        args = new Hashtable();
      args.Add((object) "userName", (object) this.playerModified.Value.name);
    }
    this.txt_message.SetTextLocalize(Consts.Format(serifs[index], (IDictionary) args));
    if (((IEnumerable<string>) voiceCues).Count<string>() > index && !string.IsNullOrEmpty(voiceCues[index]))
    {
      Singleton<NGSoundManager>.GetInstance().StopVoice(time: 0.0f);
      Singleton<NGSoundManager>.GetInstance().playVoiceByStringID(this.unit.unit.unitVoicePattern, voiceCues[index]);
    }
    return true;
  }

  public void TapSerif()
  {
    ++this.serifIndex;
    if (this.SetSerif(this.serifs, this.voices, this.serifIndex))
      return;
    this.animState = FavorabilityRatingEffect.AnimState.Loop;
    this.animController.SetTrigger("skill_unit_end");
    Singleton<NGSoundManager>.GetInstance().StopVoice(time: 0.0f);
  }

  public void onClickSkipBtn()
  {
    string str = string.Empty;
    if (this.animationType == FavorabilityRatingEffect.AnimationType.SkillFrameRelease)
    {
      switch (this.animState)
      {
        case FavorabilityRatingEffect.AnimState.Start:
          this.animState = FavorabilityRatingEffect.AnimState.Loop;
          str = "loop";
          break;
        case FavorabilityRatingEffect.AnimState.Loop:
          this.animState = FavorabilityRatingEffect.AnimState.End;
          str = "skill_release_close_anim";
          break;
      }
    }
    else
    {
      switch (this.animState)
      {
        case FavorabilityRatingEffect.AnimState.Loop:
          this.animState = FavorabilityRatingEffect.AnimState.End;
          str = "skill_loop";
          break;
        case FavorabilityRatingEffect.AnimState.End:
          str = "skill_end";
          break;
      }
    }
    if (string.IsNullOrEmpty(str))
      return;
    this.animController.SetTrigger(str);
  }

  public void StartAnimationEnd()
  {
    this.animState = FavorabilityRatingEffect.AnimState.Loop;
    this.animController.SetTrigger("loop");
  }

  public void UnitEndAnimationEnd() => this.animState = FavorabilityRatingEffect.AnimState.Loop;

  public void SkillAnimationEnd()
  {
    this.animState = FavorabilityRatingEffect.AnimState.End;
    this.animController.SetTrigger("skill_loop");
  }

  public void AnimationEnd()
  {
    if (this.endAction == null)
      return;
    this.endAction();
  }

  public enum AnimationType
  {
    None = -1, // 0xFFFFFFFF
    SkillRelease = 0,
    SkillFrameRelease = 1,
    Max = 2,
  }

  private enum AnimState
  {
    Start,
    Loop,
    End,
    Finish,
  }
}
