// Decompiled with JetBrains decompiler
// Type: ExtraSkillIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ExtraSkillIcon : IconPrefabBase
{
  public static readonly int Width = 123;
  public static readonly int Height = 147;
  public static readonly int ColumnValue = 5;
  public static readonly int RowValue = 8;
  public static readonly int RowScreenValue = 5;
  public static readonly int ScreenValue = ExtraSkillIcon.ColumnValue * ExtraSkillIcon.RowScreenValue;
  public static readonly int MaxValue = ExtraSkillIcon.ColumnValue * ExtraSkillIcon.RowValue;
  private static readonly int SelectedIndexFontSize = 22;
  [SerializeField]
  private GameObject removeObject;
  [SerializeField]
  private GameObject skillObject;
  [SerializeField]
  private UILabel txtSkillLV;
  [SerializeField]
  private UI2DSprite dynExtraSkill;
  [SerializeField]
  private GameObject forbattle;
  [SerializeField]
  private GameObject favorite;
  [SerializeField]
  private GameObject selectedBack;
  [SerializeField]
  private GameObject selectedCheck;
  [SerializeField]
  private GameObject selectedNum;
  [SerializeField]
  private GameObject slcSelected;
  [SerializeField]
  private Sprite[] selectNumSprite;
  [SerializeField]
  private GameObject objSkillGetInfo;
  private Action<InventoryExtraSkill> clickAction;
  private Action clickActionNoArg;
  private InventoryExtraSkill invExtraSkill;

  public bool ForBattle
  {
    get
    {
      return Object.op_Inequality((Object) this.forbattle, (Object) null) && this.forbattle.activeSelf;
    }
    set
    {
      if (!Object.op_Inequality((Object) this.forbattle, (Object) null))
        return;
      this.forbattle.SetActive(value);
    }
  }

  public bool Favorite
  {
    get => Object.op_Inequality((Object) this.favorite, (Object) null) && this.favorite.activeSelf;
    set
    {
      if (!Object.op_Inequality((Object) this.favorite, (Object) null))
        return;
      this.favorite.SetActive(value);
    }
  }

  public bool IsSelected
  {
    get
    {
      return Object.op_Inequality((Object) this.selectedBack, (Object) null) && this.selectedBack.activeSelf;
    }
  }

  public Action<InventoryExtraSkill> ClickAction
  {
    get => this.clickAction;
    set => this.clickAction = value;
  }

  public InventoryExtraSkill InvExtraSkill => this.invExtraSkill;

  public override bool Gray
  {
    get => this.gray;
    set
    {
      if (this.gray == value)
        return;
      this.gray = value;
      NGTween.playTweens(((Component) this).GetComponentsInChildren<UITweener>(true), NGTween.Kind.GRAYOUT, !value);
    }
  }

  public void InitByRemoveButton(Action<InventoryExtraSkill> btnAct = null)
  {
    this.removeObject.SetActive(true);
    this.skillObject.SetActive(false);
    this.ClickAction = btnAct;
    this.clickActionNoArg = (Action) null;
  }

  public IEnumerator Init(
    InventoryExtraSkill invSkill,
    Dictionary<int, Sprite> spriteCache,
    Action<InventoryExtraSkill> btnAct = null)
  {
    this.setActiveSkillGetInfo(false);
    this.removeObject.SetActive(false);
    this.skillObject.SetActive(true);
    this.invExtraSkill = invSkill;
    this.ClickAction = btnAct;
    this.clickActionNoArg = (Action) null;
    this.txtSkillLV.SetTextLocalize(Consts.Format(Consts.GetInstance().extra_skillI_thumb_skill_level_text, (IDictionary) new Hashtable()
    {
      {
        (object) "level",
        (object) this.invExtraSkill.level
      },
      {
        (object) "max",
        (object) this.invExtraSkill.GetMaxLevel()
      }
    }));
    BattleskillSkill masterData = this.invExtraSkill.skill.masterData;
    Sprite sprite;
    if (spriteCache.TryGetValue(masterData.ID, out sprite))
    {
      this.dynExtraSkill.sprite2D = sprite;
    }
    else
    {
      Future<Sprite> spriteF = masterData.LoadBattleSkillIcon();
      IEnumerator e = spriteF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.dynExtraSkill.sprite2D = spriteF.Result;
      spriteCache[masterData.ID] = spriteF.Result;
      spriteF = (Future<Sprite>) null;
    }
  }

  public void InitByCache(
    InventoryExtraSkill invSkill,
    Dictionary<int, Sprite> spriteCache,
    Action<InventoryExtraSkill> btnAct = null)
  {
    this.setActiveSkillGetInfo(false);
    this.removeObject.SetActive(false);
    this.skillObject.SetActive(true);
    this.invExtraSkill = invSkill;
    this.ClickAction = btnAct;
    this.clickActionNoArg = (Action) null;
    this.txtSkillLV.SetTextLocalize(Consts.Format(Consts.GetInstance().extra_skillI_thumb_skill_level_text, (IDictionary) new Hashtable()
    {
      {
        (object) "level",
        (object) this.invExtraSkill.level
      },
      {
        (object) "max",
        (object) this.invExtraSkill.GetMaxLevel()
      }
    }));
    this.dynExtraSkill.sprite2D = spriteCache[this.invExtraSkill.skill.masterData.ID];
  }

  public IEnumerator Init(int skillId, Dictionary<int, Sprite> spriteCache, Action<int> btnAct = null)
  {
    this.setActiveSkillGetInfo(false);
    this.removeObject.SetActive(false);
    this.skillObject.SetActive(true);
    this.ClickAction = (Action<InventoryExtraSkill>) null;
    this.clickActionNoArg = btnAct != null ? (Action) (() => btnAct(skillId)) : (Action) null;
    this.txtSkillLV.SetTextLocalize(Consts.Format(Consts.GetInstance().extra_skillI_thumb_skill_level_text, (IDictionary) new Hashtable()
    {
      {
        (object) "level",
        (object) 1
      },
      {
        (object) "max",
        (object) 1
      }
    }));
    Sprite result;
    BattleskillSkill battleskillSkill;
    if (!spriteCache.TryGetValue(skillId, out result) && MasterData.BattleskillSkill.TryGetValue(skillId, out battleskillSkill))
    {
      Future<Sprite> loader = battleskillSkill.LoadBattleSkillIcon();
      IEnumerator e = loader.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      result = loader.Result;
      spriteCache.Add(skillId, result);
      loader = (Future<Sprite>) null;
    }
    this.dynExtraSkill.sprite2D = result;
  }

  public Rect GetDrawRect()
  {
    float num1 = 0.0f;
    float num2 = 0.0f;
    float num3 = 0.0f;
    float num4 = 0.0f;
    bool flag = true;
    foreach (Transform child in this.skillObject.transform.GetChildren())
    {
      GameObject gameObject = ((Component) child).gameObject;
      if (gameObject.activeSelf)
      {
        UIWidget component = gameObject.GetComponent<UIWidget>();
        if (!Object.op_Equality((Object) component, (Object) null) && ((Behaviour) component).enabled && (component is UI2DSprite || component is UISprite))
        {
          Vector2 vector2_1 = Vector2.op_Implicit(child.localPosition);
          Vector2 vector2_2 = Vector2.op_Implicit(child.localScale);
          Vector2 pivotOffset = component.pivotOffset;
          float num5 = vector2_1.x - pivotOffset.x * (float) component.width * vector2_2.x;
          float num6 = vector2_1.y - pivotOffset.y * (float) component.height * vector2_2.y;
          float num7 = num5 + (float) component.width * vector2_2.x;
          float num8 = num6 + (float) component.height * vector2_2.y;
          if (flag)
          {
            flag = false;
            num1 = num5;
            num2 = num6;
            num3 = num7;
            num4 = num8;
          }
          else
          {
            if ((double) num1 > (double) num5)
              num1 = num5;
            if ((double) num2 > (double) num6)
              num2 = num6;
            if ((double) num3 < (double) num7)
              num3 = num7;
            if ((double) num4 < (double) num8)
              num4 = num8;
          }
        }
      }
    }
    float num9 = num3 - num1;
    float num10 = num4 - num2;
    return new Rect(num1 + num9 * 0.5f, num2 + num10 * 0.5f, num9, num10);
  }

  public void Deselect()
  {
    if (Object.op_Inequality((Object) this.selectedBack, (Object) null))
      this.selectedBack.SetActive(false);
    if (Object.op_Inequality((Object) this.selectedCheck, (Object) null))
      this.selectedCheck.SetActive(false);
    if (Object.op_Inequality((Object) this.selectedNum, (Object) null))
      this.selectedNum.SetActive(false);
    if (!Object.op_Inequality((Object) this.slcSelected, (Object) null))
      return;
    this.slcSelected.SetActive(false);
  }

  public void SelectByCheckIcon()
  {
    if (Object.op_Inequality((Object) this.selectedBack, (Object) null))
      this.selectedBack.SetActive(true);
    if (Object.op_Inequality((Object) this.selectedCheck, (Object) null))
      this.selectedCheck.SetActive(true);
    if (Object.op_Inequality((Object) this.selectedNum, (Object) null))
      this.selectedNum.SetActive(false);
    if (!Object.op_Inequality((Object) this.slcSelected, (Object) null))
      return;
    this.slcSelected.SetActive(false);
  }

  public void SelectByCheck2Icon()
  {
    if (Object.op_Inequality((Object) this.selectedBack, (Object) null))
      this.selectedBack.SetActive(false);
    if (Object.op_Inequality((Object) this.selectedCheck, (Object) null))
      this.selectedCheck.SetActive(false);
    if (Object.op_Inequality((Object) this.selectedNum, (Object) null))
      this.selectedNum.SetActive(false);
    if (!Object.op_Inequality((Object) this.slcSelected, (Object) null))
      return;
    this.slcSelected.SetActive(true);
  }

  public void Select(int selectCount)
  {
    if (Object.op_Inequality((Object) this.selectedBack, (Object) null))
      this.selectedBack.SetActive(true);
    if (Object.op_Inequality((Object) this.selectedCheck, (Object) null))
      this.selectedCheck.SetActive(false);
    if (Object.op_Inequality((Object) this.selectedNum, (Object) null))
      this.selectedNum.SetActive(true);
    if (Object.op_Inequality((Object) this.slcSelected, (Object) null))
      this.slcSelected.SetActive(false);
    UI2DSprite component = this.selectedNum.GetComponent<UI2DSprite>();
    Rect textureRect = this.selectNumSprite[selectCount].textureRect;
    ((UIWidget) component).SetDimensions((int) ((Rect) ref textureRect).width, ExtraSkillIcon.SelectedIndexFontSize);
    component.sprite2D = this.selectNumSprite[selectCount];
  }

  public void onClick()
  {
    if (this.ClickAction != null)
      this.ClickAction(this.invExtraSkill);
    if (this.clickActionNoArg == null)
      return;
    this.clickActionNoArg();
  }

  public void setActiveSkillGetInfo(bool bEnable)
  {
    if (!Object.op_Inequality((Object) this.objSkillGetInfo, (Object) null))
      return;
    this.objSkillGetInfo.SetActive(bEnable);
  }
}
