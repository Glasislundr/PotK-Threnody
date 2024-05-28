// Decompiled with JetBrains decompiler
// Type: BattleResultBuguSkillGet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class BattleResultBuguSkillGet : MonoBehaviour
{
  private bool mIsAdd;
  private Action mOnClick;
  [SerializeField]
  private GameObject dyn_Bugu_Thum;
  [SerializeField]
  private GameObject dyn_SkillDetail;
  [SerializeField]
  private Animator anime;
  [SerializeField]
  private UILabel skillNameLabel;
  [SerializeField]
  private UILabel skillDescriptionLabel;
  [SerializeField]
  private Transform skillIconRoot;
  [SerializeField]
  private Transform skillProperty1Root;
  [SerializeField]
  private Transform skillProperty2Root;
  [SerializeField]
  private UILabel txt_skillLv;
  [SerializeField]
  private UISprite slcTitleSkillMasterEffect;
  [SerializeField]
  private UISprite slcTitleSubSkillMaster;
  [SerializeField]
  private UISprite slcTitleSkillMaster;

  public IEnumerator Init(bool isAdd, GameCore.ItemInfo gear, GearGearSkill skill)
  {
    this.mIsAdd = isAdd;
    Future<GameObject> itemIconF = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
    IEnumerator e = itemIconF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = itemIconF.Result.CloneAndGetComponent<ItemIcon>(this.dyn_Bugu_Thum).InitByItemInfo(gear);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<GameObject> battleSkillIconF = Res.Prefabs.BattleSkillIcon._battleSkillIcon.Load<GameObject>();
    e = battleSkillIconF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = battleSkillIconF.Result.CloneAndGetComponent<BattleSkillIcon>(((Component) this.skillIconRoot).gameObject).Init(skill.skill);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<GameObject> skillGenreIconF = Res.Icons.SkillGenreIcon.Load<GameObject>();
    e = skillGenreIconF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    skillGenreIconF.Result.CloneAndGetComponent<SkillGenreIcon>(((Component) this.skillProperty1Root).gameObject).Init(skill.skill.genre1);
    skillGenreIconF.Result.CloneAndGetComponent<SkillGenreIcon>(((Component) this.skillProperty2Root).gameObject).Init(skill.skill.genre2);
    this.skillNameLabel.SetTextLocalize(skill.skill.name);
    this.skillDescriptionLabel.SetTextLocalize(skill.skill.description);
    this.txt_skillLv.SetText(skill.skill_level.ToString() + "/" + (object) skill.skill.upper_level);
  }

  public IEnumerator InitReisou(bool isAdd, PlayerItem reisou, GearReisouSkill skill)
  {
    this.mIsAdd = isAdd;
    Future<GameObject> itemIconF = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
    IEnumerator e = itemIconF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = itemIconF.Result.CloneAndGetComponent<ItemIcon>(this.dyn_Bugu_Thum).InitByPlayerItem(reisou);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<GameObject> battleSkillIconF = Res.Prefabs.BattleSkillIcon._battleSkillIcon.Load<GameObject>();
    e = battleSkillIconF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = battleSkillIconF.Result.CloneAndGetComponent<BattleSkillIcon>(((Component) this.skillIconRoot).gameObject).Init(skill.skill);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<GameObject> skillGenreIconF = Res.Icons.SkillGenreIcon.Load<GameObject>();
    e = skillGenreIconF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    skillGenreIconF.Result.CloneAndGetComponent<SkillGenreIcon>(((Component) this.skillProperty1Root).gameObject).Init(skill.skill.genre1);
    skillGenreIconF.Result.CloneAndGetComponent<SkillGenreIcon>(((Component) this.skillProperty2Root).gameObject).Init(skill.skill.genre2);
    this.skillNameLabel.SetTextLocalize(skill.skill.name);
    this.skillDescriptionLabel.SetTextLocalize(skill.skill.description);
    this.txt_skillLv.SetText(skill.skill_level.ToString() + "/" + (object) skill.skill.upper_level);
    Future<GameObject> atlasGameObjectF = Singleton<ResourceManager>.GetInstance().Load<GameObject>("GUI/020-29_add_sozai/020-29_sozai_add_prefab");
    e = atlasGameObjectF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.slcTitleSkillMasterEffect.atlas = atlasGameObjectF.Result.GetComponent<UIAtlas>();
    this.slcTitleSkillMasterEffect.spriteName = "slc_Title_ReisouSkill_Master.png__GUI__020-29_sozai__020-29_sozai_prefab";
    atlasGameObjectF = (Future<GameObject>) null;
    this.slcTitleSubSkillMaster.spriteName = "slc_SubTitle_Skill_Master.png__GUI__020-29_sozai__020-29_sozai_prefab";
    atlasGameObjectF = Singleton<ResourceManager>.GetInstance().Load<GameObject>("GUI/020-29_sozai/020-29_sozai_prefab");
    e = atlasGameObjectF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.slcTitleSkillMaster.atlas = atlasGameObjectF.Result.GetComponent<UIAtlas>();
    this.slcTitleSkillMaster.spriteName = "slc_Title_ReisouSkill_Master.png__GUI__020-29_sozai__020-29_sozai_prefab";
    atlasGameObjectF = (Future<GameObject>) null;
  }

  public void doStart()
  {
    string str = "Bugu_NewSkill_Update";
    if (this.mIsAdd)
      str = "Bugu_NewSkill";
    this.anime.Play(str);
  }

  public void onClick()
  {
    this.mOnClick();
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  public void SetCallback(Action f) => this.mOnClick = f;
}
