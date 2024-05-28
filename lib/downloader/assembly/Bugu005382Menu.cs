// Decompiled with JetBrains decompiler
// Type: Bugu005382Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Bugu005382Menu : BackButtonMenuBase
{
  [SerializeField]
  private GameObject[] Dir1_linkgears;
  [SerializeField]
  private GameObject[] Dir2_linkgears;
  [SerializeField]
  private GameObject dir_Bugu_Form01;
  [SerializeField]
  private GameObject dir_Bugu_Form02;
  [SerializeField]
  public UIButton yesButton;
  [SerializeField]
  private UILabel lblTitle;
  [SerializeField]
  private UILabel lblDisprict;
  private const float LINK_WIDTH = 91f;
  private const float LINK_HEIGHT = 109f;
  private const float LINK_DEFWIDTH = 114f;
  private const float LINK_DEFHEIGHT = 136f;
  private int addNum;
  private const bool NUM_ODD = false;
  private const bool NUM_EVEN = true;

  public IEnumerator ChangeSprite(List<GameCore.ItemInfo> gears)
  {
    List<GameCore.ItemInfo> rarityWarningGear = gears.Where<GameCore.ItemInfo>((Func<GameCore.ItemInfo, bool>) (x => x.gear.rarity.index >= 3)).ToList<GameCore.ItemInfo>();
    Future<GameObject> prefabF = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = prefabF.Result;
    if (rarityWarningGear.Count % 2 == 0)
    {
      e = this.SetIcon(rarityWarningGear, result, true);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      e = this.SetIcon(rarityWarningGear, result, false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  private IEnumerator SetIcon(List<GameCore.ItemInfo> gears, GameObject iconPrefab, bool evenFlag)
  {
    if (gears.Count == 1)
      this.addNum = 2;
    if (gears.Count == 2 || gears.Count == 3)
      this.addNum = 1;
    if (gears.Count == 4 || gears.Count == 5)
      this.addNum = 0;
    foreach (GameCore.ItemInfo gear in gears)
    {
      GameObject gameObject = !evenFlag ? iconPrefab.Clone(this.Dir1_linkgears[this.addNum].transform) : iconPrefab.Clone(this.Dir2_linkgears[this.addNum].transform);
      float num1 = 0.7982456f;
      float num2 = 0.8014706f;
      gameObject.transform.localScale = new Vector3(num1, num2);
      IEnumerator e = gameObject.GetComponent<ItemIcon>().InitByItemInfo(gear);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      ++this.addNum;
    }
    this.addNum = 0;
  }

  public void ChangeText0058()
  {
    this.lblTitle.text = this.lblTitle.text.Replace(Consts.GetInstance().GEAR_0052_COMPOSITE_TITLE, Consts.GetInstance().GEAR_0052_PAKUPAKU_TITLE);
    this.lblDisprict.text = this.lblDisprict.text.Replace(Consts.GetInstance().GEAR_0052_COMPOSITE, Consts.GetInstance().GEAR_0052_PAKUPAKU);
  }

  public void ChangeText00510()
  {
    this.lblTitle.SetTextLocalize(Consts.GetInstance().GEAR_00510_RECIPE_TITLE);
    this.lblDisprict.SetTextLocalize(Consts.GetInstance().GEAR_00510_PECIPE);
  }

  public void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnNo();
}
