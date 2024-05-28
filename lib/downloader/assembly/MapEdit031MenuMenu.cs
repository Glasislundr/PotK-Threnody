// Decompiled with JetBrains decompiler
// Type: MapEdit031MenuMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class MapEdit031MenuMenu : MapEditMenuBase
{
  [SerializeField]
  private UIButton btnReset_;
  [SerializeField]
  private GuildTownMapScroll mapCheck_;
  [SerializeField]
  private UILabel txtMapName_;
  private UIWidget widgetMapCheck_;
  private int currentStage_;
  private List<Tuple<int, int>> lstPosition_;

  public override MapEdit031TopMenu.EditState editState_ => MapEdit031TopMenu.EditState.Menu;

  protected override IEnumerator initializeAsync()
  {
    MapEdit031MenuMenu mapEdit031MenuMenu = this;
    mapEdit031MenuMenu.widgetMapCheck_ = ((Component) mapEdit031MenuMenu.mapCheck_).GetComponent<UIWidget>();
    mapEdit031MenuMenu.currentStage_ = mapEdit031MenuMenu.topMenu_.data_.stage_.ID;
    List<Tuple<int, int>> list = mapEdit031MenuMenu.sortPositions(((IEnumerable<PlayerGuildTownSlotPosition>) mapEdit031MenuMenu.topMenu_.data_.originalPosition_).Select<PlayerGuildTownSlotPosition, Tuple<int, int>>((Func<PlayerGuildTownSlotPosition, Tuple<int, int>>) (p => new Tuple<int, int>(p.x, p.y)))).ToList<Tuple<int, int>>();
    IEnumerator e = mapEdit031MenuMenu.doResetInformation(list);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  protected override void onEnable()
  {
    this.ui3DEvent_.isEnabled_ = false;
    ((UIButtonColor) this.btnReset_).isEnabled = this.topMenu_.data_.dicOrnament_.Count > 0;
    List<Tuple<int, int>> list = this.sortPositions(this.topMenu_.data_.dicOrnament_.Where<KeyValuePair<int, MapEditOrnament>>((Func<KeyValuePair<int, MapEditOrnament>, bool>) (kv => kv.Value.hasLocation_)).Select<KeyValuePair<int, MapEditOrnament>, Tuple<int, int>>((Func<KeyValuePair<int, MapEditOrnament>, Tuple<int, int>>) (kv => new Tuple<int, int>(kv.Value.column_ + 1, kv.Value.row_ + 1)))).ToList<Tuple<int, int>>();
    if (this.currentStage_ == this.topMenu_.data_.stage_.ID && !this.checkModifiedPositions(list))
      return;
    this.currentStage_ = this.topMenu_.data_.stage_.ID;
    this.lstPosition_ = list;
    this.StartCoroutine(this.doResetInformationWithEffect());
  }

  protected override void onDisable()
  {
  }

  public override void onBackButton() => this.topMenu_.onClickedMenuClose();

  public void onClickedResetLayout()
  {
    if (this.waitAndSet())
      return;
    this.StartCoroutine(this.doConfirmResetLayout());
  }

  private IEnumerator doConfirmResetLayout()
  {
    MapEdit031MenuMenu mapEdit031MenuMenu = this;
    Consts instance = Consts.GetInstance();
    bool bWait = true;
    bool bOk = false;
    ModalWindow.ShowYesNo(instance.MAPEDIT_031_TITLE_CONFIRM_RESETLAYOUT, instance.MAPEDIT_031_MESSAGE_CONFIRM_RESETLAYOUT, (Action) (() =>
    {
      bWait = false;
      bOk = true;
    }), (Action) (() => bWait = false));
    while (bWait)
      yield return (object) null;
    if (bOk)
    {
      ((UIButtonColor) mapEdit031MenuMenu.btnReset_).isEnabled = false;
      mapEdit031MenuMenu.topMenu_.returnStorageAll();
      mapEdit031MenuMenu.lstPosition_ = (List<Tuple<int, int>>) null;
      IEnumerator e = mapEdit031MenuMenu.doResetInformationWithEffect();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
      mapEdit031MenuMenu.clearWait();
  }

  private IEnumerable<Tuple<int, int>> sortPositions(IEnumerable<Tuple<int, int>> lstPos)
  {
    return (IEnumerable<Tuple<int, int>>) lstPos.OrderBy<Tuple<int, int>, int>((Func<Tuple<int, int>, int>) (tp => tp.Item1)).ThenBy<Tuple<int, int>, int>((Func<Tuple<int, int>, int>) (tp => tp.Item2));
  }

  private bool checkModifiedPositions(List<Tuple<int, int>> lstPos)
  {
    if (this.lstPosition_ == null)
      return true;
    int count = this.lstPosition_.Count;
    if (count != lstPos.Count)
      return true;
    for (int index = 0; index < count; ++index)
    {
      if (!lstPos[index].Equals((object) this.lstPosition_[index]))
        return true;
    }
    return false;
  }

  private IEnumerator doResetInformationWithEffect()
  {
    ((UIRect) this.widgetMapCheck_).alpha = 0.0f;
    IEnumerator e = this.doResetInformation(this.lstPosition_);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    TweenAlpha.Begin(((Component) this.widgetMapCheck_).gameObject, 0.5f, 1f);
  }

  private IEnumerator doResetInformation(List<Tuple<int, int>> lstPosition)
  {
    MapEdit031MenuMenu mapEdit031MenuMenu = this;
    mapEdit031MenuMenu.txtMapName_.SetTextLocalize(mapEdit031MenuMenu.topMenu_.data_.mapTown_.name);
    mapEdit031MenuMenu.lstPosition_ = lstPosition;
    IEnumerator e = mapEdit031MenuMenu.mapCheck_.InitializeAsync(mapEdit031MenuMenu.currentStage_, lstPosition);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
