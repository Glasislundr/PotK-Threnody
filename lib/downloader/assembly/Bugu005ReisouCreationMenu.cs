// Decompiled with JetBrains decompiler
// Type: Bugu005ReisouCreationMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Bugu005ReisouCreationMenu : BackButtonMenuBase
{
  [SerializeField]
  private UI2DSprite iconSand;
  [SerializeField]
  private UI2DSprite iconMedal;
  [SerializeField]
  private UILabel txtSand;
  [SerializeField]
  private UILabel txtBattleMedal;
  [SerializeField]
  private NGxScroll2 scroll;
  [SerializeField]
  private GameObject dir_noList;
  protected GameObject reisouRecipePrefab;
  protected int CellWidth = 544;
  protected int CellHeight = 144;
  protected List<GameObject> reisouRecipePopupList;

  public IEnumerator Init()
  {
    if (Object.op_Equality((Object) this.reisouRecipePrefab, (Object) null))
    {
      Future<GameObject> popupPrefabF = new ResourceObject("Prefabs/UnitGUIs/ReisouCreationSkillDetail").Load<GameObject>();
      IEnumerator e = popupPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.reisouRecipePrefab = popupPrefabF.Result;
      popupPrefabF = (Future<GameObject>) null;
    }
    this.updateInventoryText();
    this.reisouRecipePopupList = new List<GameObject>();
    GearReisouChaosCreation[] reisouChaosCreationArray = MasterData.GearReisouChaosCreation.Values;
    for (int index = 0; index < reisouChaosCreationArray.Length; ++index)
      yield return (object) this.createRecipeBar(reisouChaosCreationArray[index]);
    reisouChaosCreationArray = (GearReisouChaosCreation[]) null;
    this.scroll.CreateScrollPoint(this.CellHeight, 10);
    ((Component) this.scroll).gameObject.SetActive(false);
    yield return (object) null;
    ((Component) this.scroll).gameObject.SetActive(true);
    foreach (GameObject reisouRecipePopup in this.reisouRecipePopupList)
      reisouRecipePopup.SetActive(true);
    yield return (object) null;
    this.scroll.ResolvePosition();
    this.scroll.scrollView.UpdatePosition();
    Future<Sprite> spriteF = new ResourceObject("Icons/ChaosJewel_Icon").Load<Sprite>();
    yield return (object) spriteF.Wait();
    this.iconSand.sprite2D = spriteF.Result;
    spriteF = (Future<Sprite>) null;
    spriteF = new ResourceObject("Icons/BattleMedal_Icon").Load<Sprite>();
    yield return (object) spriteF.Wait();
    this.iconMedal.sprite2D = spriteF.Result;
    spriteF = (Future<Sprite>) null;
  }

  protected IEnumerator createRecipeBar(GearReisouChaosCreation recipe)
  {
    Bugu005ReisouCreationMenu reisouCreationMenu = this;
    GameObject go = reisouCreationMenu.reisouRecipePrefab.Clone();
    ReisouCreationSkillDetail component = go.GetComponent<ReisouCreationSkillDetail>();
    go.SetActive(false);
    IEnumerator e = component.Init(recipe, new Action(reisouCreationMenu.updateInventoryText));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    reisouCreationMenu.scroll.AddColumn1(go, reisouCreationMenu.CellWidth, reisouCreationMenu.CellHeight);
    reisouCreationMenu.reisouRecipePopupList.Add(go);
  }

  public void updateInventoryText()
  {
    Player player = SMManager.Get<Player>();
    this.txtSand.SetTextLocalize(player.reisou_jewel);
    this.txtBattleMedal.SetTextLocalize(player.battle_medal);
  }

  public override void onBackButton() => this.IbtnBack();

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().closeAll();
    if (Singleton<NGGameDataManager>.GetInstance().IsColosseum)
    {
      Singleton<NGSceneManager>.GetInstance().destroyCurrentScene();
      Singleton<CommonRoot>.GetInstance().SetFooterEnable(true);
    }
    this.backScene();
  }
}
