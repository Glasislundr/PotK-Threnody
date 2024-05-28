// Decompiled with JetBrains decompiler
// Type: UnitSortAndFilterGroupButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System;
using UnityEngine;

#nullable disable
public class UnitSortAndFilterGroupButton : UnitSortAndFilterButton
{
  [SerializeField]
  private GameObject slcSortFilterTextBG;
  [SerializeField]
  private UILabel txtLabel;
  [SerializeField]
  private SpriteSelectDirect spriteSelectDirect;
  public bool isSelected;
  private Action<UnitGroupHead, int, bool> onSwitch;
  private UnitGroupHead groupType;
  private int groupID;

  public UnitGroupHead GroupType => this.groupType;

  public int GroupID => this.groupID;

  protected override void Awake() => base.Awake();

  protected override void Update()
  {
  }

  public void Init(
    UnitSortAndFilter menu,
    UnitGroupHead type,
    int id,
    string text,
    string spriteName)
  {
    this.Menu = menu;
    this.onSwitch = (Action<UnitGroupHead, int, bool>) null;
    this.init(type, id, text, spriteName);
  }

  public void Init(
    Action<UnitGroupHead, int, bool> eventSwitch,
    UnitGroupHead type,
    int id,
    string text,
    string spriteName)
  {
    this.Menu = (UnitSortAndFilter) null;
    this.onSwitch = eventSwitch;
    this.init(type, id, text, spriteName);
  }

  private void init(UnitGroupHead type, int id, string text, string spriteName)
  {
    this.groupType = type;
    this.groupID = id;
    this.txtLabel.SetTextLocalize(text);
    this.spriteSelectDirect.SetSpriteName<string>(spriteName);
    if (type != UnitGroupHead.group_all)
      return;
    this.slcSortFilterTextBG.SetActive(false);
  }

  public override void TextColorGray(bool flag)
  {
    Color color = Color.gray;
    if (flag)
      color = Color.white;
    ((UIWidget) this.txtLabel).color = color;
  }

  public override void PressButton()
  {
    if (this.onSwitch != null)
      this.onSwitch(this.groupType, this.groupID, !this.isSelected);
    else if (this.isSelected)
      this.Menu.RemoveGroupInfo(this.groupType, this.groupID);
    else
      this.Menu.AddGroupInfo(this.groupType, this.groupID);
  }
}
