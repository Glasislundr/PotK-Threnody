// Decompiled with JetBrains decompiler
// Type: Battle02MenuBase
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
public abstract class Battle02MenuBase : BattleBackButtonMenuBase
{
  public Color mGreen = new Color(0.0f, 0.863f, 0.118f);
  public Color mRed = new Color(0.98f, 0.0f, 0.0f);
  protected BL.BattleModified<BL.Unit> modified;
  private static Dictionary<ulong, Sprite> spriteCache = new Dictionary<ulong, Sprite>();
  private const int SHIFT_METAMOR_ID = 32;

  private static ulong makeSpriteCacheKey(int unitId, int metamorId)
  {
    return ((ulong) metamorId << 32) + (ulong) unitId;
  }

  public static void ClearCache() => Battle02MenuBase.spriteCache.Clear();

  public static IEnumerator LoadIcon(BL.Unit v)
  {
    SkillMetamorphosis metamorphosis = v.metamorphosis;
    int metamorphosisId = metamorphosis != null ? metamorphosis.metamorphosis_id : 0;
    ulong cacheKey = Battle02MenuBase.makeSpriteCacheKey(v.unitId, metamorphosisId);
    if (!Battle02MenuBase.spriteCache.ContainsKey(cacheKey))
    {
      Future<Sprite> fs = v.unit.LoadSpriteThumbnail(metamorphosisId);
      IEnumerator e = fs.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Battle02MenuBase.spriteCache.Add(cacheKey, fs.Result);
    }
  }

  public void CreateUnitSprite(UI2DSprite sp)
  {
    BL.Unit unit = this.modified.value;
    SkillMetamorphosis metamorphosis = unit.metamorphosis;
    int metamorphosisId = metamorphosis != null ? metamorphosis.metamorphosis_id : 0;
    ulong cacheKey = Battle02MenuBase.makeSpriteCacheKey(unit.unitId, metamorphosisId);
    Sprite sprite1;
    if (Battle02MenuBase.spriteCache.TryGetValue(cacheKey, out sprite1))
      sp.sprite2D = sprite1;
    else
      unit.unit.LoadSpriteThumbnail(metamorphosisId).RunOn<Sprite>((MonoBehaviour) this, (Action<Sprite>) (sprite =>
      {
        sp.sprite2D = sprite;
        Battle02MenuBase.spriteCache.Add(cacheKey, sp.sprite2D);
      }));
  }

  public void setParentText(UILabel label, int v)
  {
    if (v >= 0)
    {
      label.SetTextLocalize(string.Concat((object) v));
    }
    else
    {
      if (v >= 0)
        return;
      label.SetTextLocalize(Mathf.Abs(v));
      label.SetText("-" + label.text);
    }
  }

  public void setBDText(UILabel label, int v)
  {
    if (v > 0)
    {
      label.SetTextLocalize(Mathf.Abs(v));
      label.SetText("( +" + label.text + " )");
      ((UIWidget) label).color = this.mGreen;
    }
    else if (v < 0)
    {
      label.SetTextLocalize(Mathf.Abs(v));
      label.SetText("( -" + label.text + " )");
      ((UIWidget) label).color = this.mRed;
    }
    else
      label.SetText(" ");
  }

  public void setBDTextWrraper(UILabel label, int v)
  {
    if (v > 0)
    {
      label.SetTextLocalize(Mathf.Abs(v));
      label.SetText("( +" + label.text + " )");
      ((UIWidget) label).color = this.mGreen;
    }
    else if (v < 0)
    {
      label.SetTextLocalize(Mathf.Abs(v));
      label.SetText("( -" + label.text + " )");
      ((UIWidget) label).color = this.mRed;
    }
    else
      label.SetText(" ");
  }

  public void setColordText(UILabel label, int v, int bd)
  {
    this.setParentText(label, v);
    if (bd > 0)
    {
      ((UIWidget) label).color = this.mGreen;
    }
    else
    {
      if (bd >= 0)
        return;
      ((UIWidget) label).color = this.mRed;
    }
  }

  public void setUnit(BL.Unit unit) => this.modified = BL.Observe<BL.Unit>(unit);

  public BL.Unit getUnit() => this.modified.value;

  public virtual void UpdateData()
  {
  }
}
