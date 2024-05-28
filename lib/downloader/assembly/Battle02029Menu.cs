// Decompiled with JetBrains decompiler
// Type: Battle02029Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Battle02029Menu : NGMenuBase
{
  [SerializeField]
  protected UI2DSprite srcSkill;
  [SerializeField]
  protected UI2DSprite srcSkillBefore;
  [SerializeField]
  protected UILabel txtSkillName;
  [SerializeField]
  protected UILabel txtSkillNameBefore;
  [SerializeField]
  protected UILabel txtSkillDescription;
  [SerializeField]
  protected UILabel txtSkillDescriptionBefore;
  [SerializeField]
  protected GameObject charaParent;
  [SerializeField]
  protected UILabel txtCharacterName;
  [SerializeField]
  private UISprite slcNewSkillTxt;
  [SerializeField]
  private GameObject skillIcon;
  [SerializeField]
  private GameObject skillIconBefore;
  [SerializeField]
  private GameObject skillIconBase;
  [SerializeField]
  private UIWidget elementIconParent;
  [SerializeField]
  private UIWidget elementIconParentBefore;
  private bool isSkip;
  private bool isRunning = true;
  private Action onCallback;
  private UITweener[] tweens;

  public IEnumerator InitForEvolution(
    int unit_id,
    int skill_id,
    int skill_id_before,
    bool sea_effect = false)
  {
    this.skillIconBefore.SetActive(false);
    ((Component) this.elementIconParentBefore).gameObject.SetActive(false);
    this.skillIconBase.SetActive(false);
    ((Component) this.elementIconParent).gameObject.SetActive(false);
    IEnumerator e = this.setSkill(skill_id_before, this.skillIconBefore, this.srcSkillBefore, this.txtSkillNameBefore, this.txtSkillDescriptionBefore, this.elementIconParentBefore);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.setSkill(skill_id, this.skillIcon, this.srcSkill, this.txtSkillName, this.txtSkillDescription, this.elementIconParent);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.txtCharacterName.SetTextLocalize(MasterData.UnitUnit[unit_id].name);
    e = this.SetCharacterViewUnit(MasterData.UnitUnit[unit_id]);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.Play(sea_effect);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator Init(int unit_id, int skill_id, bool sea_effect = false)
  {
    this.skillIconBase.SetActive(false);
    ((Component) this.elementIconParent).gameObject.SetActive(false);
    IEnumerator e = this.setSkill(skill_id, this.skillIcon, this.srcSkill, this.txtSkillName, this.txtSkillDescription, this.elementIconParent);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.txtCharacterName.SetTextLocalize(MasterData.UnitUnit[unit_id].name);
    e = this.SetCharacterViewUnit(MasterData.UnitUnit[unit_id]);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.Play(sea_effect);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator InitLeaderSkill(int unit_id, int skill_id)
  {
    this.SetLeaderSkill(skill_id, this.txtSkillName, this.txtSkillDescription);
    this.txtCharacterName.SetTextLocalize(MasterData.UnitUnit[unit_id].name);
    IEnumerator e = this.SetCharacterViewUnit(MasterData.UnitUnit[unit_id]);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.Play();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator setSkill(
    int skill_id,
    GameObject _skillIcon,
    UI2DSprite _srcSkill,
    UILabel _txtSkillName,
    UILabel _txtSkillDescription,
    UIWidget _elementIconParent)
  {
    _skillIcon.SetActive(false);
    BattleskillSkill skill = MasterData.BattleskillSkill[skill_id];
    IEnumerator e;
    if (skill.skill_type != BattleskillSkillType.magic)
    {
      Future<Sprite> skillIconSprite = skill.LoadBattleSkillIcon();
      e = skillIconSprite.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      _srcSkill.sprite2D = skillIconSprite.Result;
      _skillIcon.SetActive(true);
      this.skillIconBase.SetActive(true);
      skillIconSprite = (Future<Sprite>) null;
    }
    else
    {
      Future<GameObject> elementLoader = Res.Icons.CommonElementIcon.Load<GameObject>();
      e = elementLoader.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject gameObject = elementLoader.Result.Clone(((Component) _elementIconParent).transform);
      UI2DSprite component1 = gameObject.GetComponent<UI2DSprite>();
      ((UIWidget) component1).depth = _elementIconParent.depth + 1;
      CommonElementIcon component2 = gameObject.GetComponent<CommonElementIcon>();
      component2.Init(skill.element);
      ((UIWidget) component1).width = ((Texture) component2.iconSprite.sprite2D.texture).width;
      ((UIWidget) component1).height = ((Texture) component2.iconSprite.sprite2D.texture).height;
      ((Component) _elementIconParent).gameObject.SetActive(true);
      elementLoader = (Future<GameObject>) null;
    }
    _txtSkillName.SetTextLocalize(skill.name);
    _txtSkillDescription.SetTextLocalize(skill.description);
    yield return (object) null;
  }

  public void SetLeaderSkill(int skill_id, UILabel _txtSkillName, UILabel _txtSkillDescription)
  {
    BattleskillSkill battleskillSkill = MasterData.BattleskillSkill[skill_id];
    if (battleskillSkill == null)
      return;
    _txtSkillName.SetTextLocalize(battleskillSkill.name);
    _txtSkillDescription.SetTextLocalize(battleskillSkill.description);
  }

  private IEnumerator Play(bool sea_effect = false)
  {
    Battle02029Menu battle02029Menu = this;
    if (!sea_effect)
      yield return (object) new WaitForSeconds(1f);
    else
      yield return (object) new WaitForSeconds(0.02f);
    battle02029Menu.tweens = ((Component) ((Component) battle02029Menu).transform).GetComponentsInChildren<UITweener>();
    ((Component) battle02029Menu).gameObject.SetActive(true);
    foreach (UITweener tween in battle02029Menu.tweens)
    {
      tween.ResetToBeginning();
      tween.PlayForward();
    }
    UITweener[] uiTweenerArray = battle02029Menu.tweens;
    for (int index = 0; index < uiTweenerArray.Length; ++index)
    {
      UITweener tween = uiTweenerArray[index];
      while (!battle02029Menu.isSkip && ((Behaviour) tween).enabled)
        yield return (object) null;
      tween = (UITweener) null;
    }
    uiTweenerArray = (UITweener[]) null;
    battle02029Menu.isRunning = false;
  }

  private IEnumerator SetCharacterViewUnit(UnitUnit unit)
  {
    Future<GameObject> future = unit.LoadMypage();
    IEnumerator e = future.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject texObj = future.Result.Clone(this.charaParent.transform);
    e = unit.SetLargeSpriteSnap(texObj, 4);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = unit.SetLargeSpriteWithMask(texObj, Res.GUI._020_19_1_sozai.mask_Chara.Load<Texture2D>(), 0, -146, 36);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void IbtnScreen()
  {
    if (this.isRunning && !this.isSkip)
      this.isSkip = true;
    else
      this.StartCoroutine(this.toClose());
  }

  private IEnumerator toClose()
  {
    while (this.isRunning)
      yield return (object) null;
    if (this.onCallback != null)
      this.onCallback();
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  public void SetCallback(Action callback) => this.onCallback = callback;
}
