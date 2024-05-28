// Decompiled with JetBrains decompiler
// Type: MaterialDetailMenu
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
public class MaterialDetailMenu : DetailMenuBase
{
  [SerializeField]
  protected UILabel TxtDetaildescription;
  [SerializeField]
  protected UILabel TxtOwnednumber;
  [SerializeField]
  protected UILabel TxtDropQuestName;
  [SerializeField]
  private GameObject IconEvolution;
  [SerializeField]
  private GameObject IconUnification;
  [SerializeField]
  private GameObject IconRevival;
  [SerializeField]
  private GameObject IconExperience;
  [SerializeField]
  private UI2DSprite mainSprite;
  [SerializeField]
  private Transform transBottom;

  public override IEnumerator Init(
    Unit0042Menu menu,
    int index,
    PlayerUnit playerUnit,
    int infoIndex,
    bool isLimit,
    bool isMaterial,
    QuestScoreBonusTimetable[] tables,
    UnitBonus[] unitBonus,
    bool isUpdate = true,
    PlayerUnit baseUnit = null)
  {
    MaterialDetailMenu materialDetailMenu = this;
    materialDetailMenu.menu = menu;
    materialDetailMenu.index = index;
    if (Object.op_Equality((Object) materialDetailMenu.transBottom, (Object) null))
      materialDetailMenu.transBottom = ((Component) materialDetailMenu).transform.GetChildInFind("Bottom");
    materialDetailMenu.transBottom.localPosition = new Vector3(0.0f, (float) -(((double) menu.scrollView.scrollView.panel.GetViewSize().y - (double) ((Component) materialDetailMenu.transBottom).GetComponent<UIWidget>().height) / 2.0), 0.0f);
    materialDetailMenu.TxtDetaildescription.text = playerUnit.unit.description;
    UnitMaterialQuestInfo materialQuestInfo = ((IEnumerable<UnitMaterialQuestInfo>) MasterData.UnitMaterialQuestInfoList).SingleOrDefault<UnitMaterialQuestInfo>((Func<UnitMaterialQuestInfo, bool>) (x => x.unit_id == playerUnit.unit.ID));
    if (materialQuestInfo == null)
      materialDetailMenu.TxtDropQuestName.text = "";
    else if (Object.op_Inequality((Object) materialDetailMenu.TxtDropQuestName, (Object) null))
      materialDetailMenu.TxtDropQuestName.text = materialQuestInfo.short_desc;
    PlayerMaterialUnit playerMaterialUnit = ((IEnumerable<PlayerMaterialUnit>) SMManager.Get<PlayerMaterialUnit[]>()).FirstOrDefault<PlayerMaterialUnit>((Func<PlayerMaterialUnit, bool>) (x => x._unit == playerUnit.unit.ID));
    materialDetailMenu.TxtOwnednumber.SetTextLocalize(playerMaterialUnit != null ? playerMaterialUnit.quantity : 0);
    Future<Sprite> targetSprite = playerUnit.unit.LoadSpriteMedium();
    IEnumerator e = targetSprite.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    materialDetailMenu.mainSprite.sprite2D = targetSprite.Result;
    GameObject[] self = new GameObject[4]
    {
      materialDetailMenu.IconUnification,
      materialDetailMenu.IconEvolution,
      materialDetailMenu.IconRevival,
      materialDetailMenu.IconExperience
    };
    UnitUnit unit = playerUnit.unit;
    int index1 = -1;
    if (unit.IsTougouUnit)
      index1 = 0;
    else if (unit.IsSinkaUnit)
      index1 = 1;
    else if (unit.IsTenseiUnit)
      index1 = 2;
    else if (unit.is_exp_material)
      index1 = 3;
    ((IEnumerable<GameObject>) self).ToggleOnceEx(index1);
  }

  public void changeBackScene() => this.backScene();
}
