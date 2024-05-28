// Decompiled with JetBrains decompiler
// Type: Unit00493Menu
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
public class Unit00493Menu : BackButtonMenuBase
{
  [SerializeField]
  protected UILabel TxtDetaildescription;
  [SerializeField]
  protected UILabel TxtOwnednumber;
  [SerializeField]
  protected UILabel TxtTitle;
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
  private GameObject newflagSprite;
  [SerializeField]
  private GameObject btnBG;

  public IEnumerator Init(UnitUnit MaterialEvolution, bool NewFlag, bool isGacha)
  {
    this.TxtTitle.SetText(MaterialEvolution.name);
    this.TxtDetaildescription.SetText(MaterialEvolution.description);
    UnitMaterialQuestInfo materialQuestInfo = ((IEnumerable<UnitMaterialQuestInfo>) MasterData.UnitMaterialQuestInfoList).SingleOrDefault<UnitMaterialQuestInfo>((Func<UnitMaterialQuestInfo, bool>) (x => x.unit_id == MaterialEvolution.ID));
    if (materialQuestInfo == null)
      this.TxtDropQuestName.SetText("");
    else if (Object.op_Inequality((Object) this.TxtDropQuestName, (Object) null))
      this.TxtDropQuestName.SetText(materialQuestInfo.short_desc);
    PlayerMaterialUnit playerMaterialUnit = ((IEnumerable<PlayerMaterialUnit>) SMManager.Get<PlayerMaterialUnit[]>()).FirstOrDefault<PlayerMaterialUnit>((Func<PlayerMaterialUnit, bool>) (x => x.unit.ID == MaterialEvolution.ID));
    int num = 0;
    if (playerMaterialUnit != null)
      num = playerMaterialUnit.quantity;
    this.TxtOwnednumber.SetTextLocalize(num);
    Future<Sprite> targetSprite = MaterialEvolution.LoadSpriteMedium();
    IEnumerator e = targetSprite.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.mainSprite.sprite2D = targetSprite.Result;
    GameObject[] self = new GameObject[4]
    {
      this.IconUnification,
      this.IconEvolution,
      this.IconRevival,
      this.IconExperience
    };
    int index = -1;
    if (MaterialEvolution.IsTougouUnit)
      index = 0;
    else if (MaterialEvolution.IsSinkaUnit)
      index = 1;
    else if (MaterialEvolution.IsTenseiUnit)
      index = 2;
    else if (MaterialEvolution.is_exp_material)
      index = 3;
    ((IEnumerable<GameObject>) self).ToggleOnceEx(index);
    if (!NewFlag && Object.op_Inequality((Object) this.newflagSprite, (Object) null))
      Object.Destroy((Object) this.newflagSprite.gameObject);
    if (Object.op_Inequality((Object) this.btnBG, (Object) null))
      this.btnBG.SetActive(isGacha);
  }

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  public override void onBackButton() => this.IbtnBack();
}
