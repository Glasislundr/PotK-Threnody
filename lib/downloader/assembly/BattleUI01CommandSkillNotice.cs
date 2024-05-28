// Decompiled with JetBrains decompiler
// Type: BattleUI01CommandSkillNotice
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class BattleUI01CommandSkillNotice : MonoBehaviour
{
  [SerializeField]
  private UILabel mSkillNameLbl;
  [SerializeField]
  private UILabel mOwnEffectLbl;
  [SerializeField]
  private UILabel mOpponentEffectLbl;
  [SerializeField]
  private GameObject mOwnObj;
  [SerializeField]
  private GameObject mOpponentObj;
  [SerializeField]
  private UIGrid mGrid;
  private float ViewWait = 7f;
  private IEnumerator mTask;

  public bool isView => this.mTask != null;

  public void PlayView(BL.Skill skill, bool isPlayer = false, float viewWait = 7f)
  {
    this.ViewWait = viewWait;
    if (this.mTask != null)
      this.StopCoroutine(this.mTask);
    ((UIRect) ((Component) this).GetComponent<UIWidget>()).alpha = 0.0f;
    this.mSkillNameLbl.SetTextLocalize(skill.name);
    this.SetEffectDescription(skill, isPlayer);
    this.mTask = this.View();
    this.StartCoroutine(this.mTask);
  }

  private IEnumerator View()
  {
    BattleUI01CommandSkillNotice commandSkillNotice = this;
    UITweener[] tweeners = NGTween.findTweenersGroup(((Component) commandSkillNotice).gameObject, 0, true);
    foreach (UITweener uiTweener in tweeners)
      uiTweener.PlayForward();
    yield return (object) new WaitForSeconds(commandSkillNotice.ViewWait);
    foreach (UITweener uiTweener in tweeners)
      uiTweener.PlayReverse();
    commandSkillNotice.mTask = (IEnumerator) null;
  }

  private void SetEffectDescription(BL.Skill skill, bool isPlayer)
  {
    if (isPlayer)
    {
      string text1 = !string.IsNullOrEmpty(skill.skill.viewShortDescriptionEnemy) ? skill.skill.viewShortDescriptionEnemy : skill.skill.shortDescriptionEnemy;
      if (string.IsNullOrEmpty(text1))
      {
        this.mOwnEffectLbl.SetTextLocalize(string.Empty);
        this.mOwnObj.SetActive(false);
      }
      else
      {
        this.mOwnEffectLbl.SetTextLocalize(text1);
        this.mOwnObj.SetActive(true);
      }
      string text2 = !string.IsNullOrEmpty(skill.skill.viewShortDescription) ? skill.skill.viewShortDescription : skill.skill.shortDescription;
      if (string.IsNullOrEmpty(text2))
      {
        this.mOpponentEffectLbl.SetTextLocalize(string.Empty);
        this.mOpponentObj.SetActive(false);
      }
      else
      {
        this.mOpponentEffectLbl.SetTextLocalize(text2);
        this.mOpponentObj.SetActive(true);
      }
    }
    else
    {
      string text3 = !string.IsNullOrEmpty(skill.skill.viewShortDescription) ? skill.skill.viewShortDescription : skill.skill.shortDescription;
      if (string.IsNullOrEmpty(text3))
      {
        this.mOwnEffectLbl.SetTextLocalize(string.Empty);
        this.mOwnObj.SetActive(false);
      }
      else
      {
        this.mOwnEffectLbl.SetTextLocalize(text3);
        this.mOwnObj.SetActive(true);
      }
      string text4 = !string.IsNullOrEmpty(skill.skill.viewShortDescriptionEnemy) ? skill.skill.viewShortDescriptionEnemy : skill.skill.shortDescriptionEnemy;
      if (string.IsNullOrEmpty(text4))
      {
        this.mOpponentEffectLbl.SetTextLocalize(string.Empty);
        this.mOpponentObj.SetActive(false);
      }
      else
      {
        this.mOpponentEffectLbl.SetTextLocalize(text4);
        this.mOpponentObj.SetActive(true);
      }
    }
    this.mGrid.Reposition();
  }
}
