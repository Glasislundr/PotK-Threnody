// Decompiled with JetBrains decompiler
// Type: BattleSkillIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class BattleSkillIcon : IconPrefabBase
{
  public UI2DSprite iconSprite;
  public GameObject unLockIcon;
  public GameObject skillNeedLvIcon;
  public UILabel skillNeedRankTxt;
  public UILabel skillNeedLvTxt;
  public int skillID;

  public IEnumerator Init(BattleskillSkill skill)
  {
    if (Object.op_Implicit((Object) this.unLockIcon))
      this.unLockIcon.SetActive(false);
    this.skillID = skill.ID;
    Future<Sprite> ft = skill.skill_type != BattleskillSkillType.call ? skill.LoadBattleSkillIcon() : skill.LoadCallSkillIcon();
    IEnumerator e = ft.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.iconSprite.sprite2D = ft.Result;
  }

  public IEnumerator Init(BattleFuncs.InvestSkill s)
  {
    if (Object.op_Implicit((Object) this.unLockIcon))
      this.unLockIcon.SetActive(false);
    this.skillID = s.skill.ID;
    Future<Sprite> ft = s.skill.LoadBattleSkillIcon(s);
    IEnumerator e = ft.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.iconSprite.sprite2D = ft.Result;
  }

  public void Init(Sprite sprite)
  {
    if (Object.op_Implicit((Object) this.unLockIcon))
      this.unLockIcon.SetActive(false);
    this.iconSprite.sprite2D = sprite;
  }

  public void SetDepth(int value)
  {
    ((Component) this).GetComponent<UIWidget>().depth = value;
    ((UIWidget) this.iconSprite).depth = value;
  }

  public void DisableNeedDisp()
  {
    ((UIWidget) this.iconSprite).color = Color.white;
    if (Object.op_Inequality((Object) this.skillNeedRankTxt, (Object) null))
      ((Component) this.skillNeedRankTxt).gameObject.SetActive(false);
    if (!Object.op_Inequality((Object) this.skillNeedLvIcon, (Object) null))
      return;
    this.skillNeedLvIcon.SetActive(false);
  }

  public void EnableNeedLvIcon(int needLv)
  {
    ((UIWidget) this.iconSprite).color = new Color(0.3f, 0.3f, 0.3f);
    if (Object.op_Inequality((Object) this.skillNeedLvIcon, (Object) null))
      this.skillNeedLvIcon.SetActive(true);
    if (!Object.op_Inequality((Object) this.skillNeedLvTxt, (Object) null))
      return;
    this.skillNeedLvTxt.SetTextLocalize(needLv.ToString());
  }

  public void EnableNeedRankIcon(int needRank)
  {
    if (needRank > 0)
    {
      ((UIWidget) this.iconSprite).color = new Color(0.3f, 0.3f, 0.3f);
      if (!Object.op_Inequality((Object) this.skillNeedRankTxt, (Object) null))
        return;
      ((Component) this.skillNeedRankTxt).gameObject.SetActive(true);
      this.skillNeedRankTxt.SetTextLocalize(Consts.Format(Consts.GetInstance().GEAR_RANK_TXT, (IDictionary) new Hashtable()
      {
        {
          (object) "rank",
          (object) needRank
        }
      }));
    }
    else
    {
      ((UIWidget) this.iconSprite).color = new Color(1f, 1f, 1f);
      if (!Object.op_Inequality((Object) this.skillNeedRankTxt, (Object) null))
        return;
      ((Component) this.skillNeedRankTxt).gameObject.SetActive(false);
    }
  }
}
