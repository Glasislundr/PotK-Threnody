// Decompiled with JetBrains decompiler
// Type: Unit004UnitMaterialsListScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Unit004UnitMaterialsListScene : NGSceneBase
{
  private static readonly string DefaultName = "unit004_UnitMaterials_List";
  private Unit00491Menu unit00491Menu;
  private Unit00414Menu unit00414Menu;
  public UIButton HimeMaterialEvolutionBtn;
  public UIButton bottomSell;
  public UILabel textTitle;
  [SerializeField]
  private float waitEndLoading_ = 2f;
  private Unit004UnitMaterialsListScene.PAGETYPE pageType;

  public override IEnumerator onInitSceneAsync()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Unit004UnitMaterialsListScene materialsListScene = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated method
    materialsListScene.HimeMaterialEvolutionBtn.onClick.Add(new EventDelegate(new EventDelegate.Callback(materialsListScene.\u003ConInitSceneAsync\u003Eb__9_0)));
    materialsListScene.bottomSell.onClick.Add(new EventDelegate((EventDelegate.Callback) (() => Unit00468Scene.changeScene00410(false, Unit00410Menu.FromType.MaterialList))));
    return false;
  }

  public virtual IEnumerator onStartSceneAsync()
  {
    Unit004UnitMaterialsListScene materialsListScene = this;
    if (Object.op_Equality((Object) materialsListScene.unit00491Menu, (Object) null))
      materialsListScene.unit00491Menu = ((Component) materialsListScene).gameObject.GetComponent<Unit00491Menu>();
    if (Object.op_Equality((Object) materialsListScene.unit00414Menu, (Object) null))
      materialsListScene.unit00414Menu = ((Component) materialsListScene).gameObject.GetComponent<Unit00414Menu>();
    Player player = SMManager.Get<Player>();
    SMManager.Get<PlayerUnit[]>();
    PlayerMaterialUnit[] playerMaterialUnits = SMManager.Get<PlayerMaterialUnit[]>();
    if (Singleton<NGGameDataManager>.GetInstance().IsColosseum)
      Singleton<CommonRoot>.GetInstance().SetFooterEnable(false);
    ((Component) materialsListScene.HimeMaterialEvolutionBtn).gameObject.SetActive(true);
    materialsListScene.SetTitle();
    yield return (object) ServerTime.WaitSync();
    if (materialsListScene.pageType == Unit004UnitMaterialsListScene.PAGETYPE.Unit00414Menu)
    {
      yield return (object) materialsListScene.changeMenu((UnitMenuBase) materialsListScene.unit00414Menu);
      materialsListScene.pageType = Unit004UnitMaterialsListScene.PAGETYPE.Unit00414Menu;
      materialsListScene.unit00414Menu.SetIconType(UnitMenuBase.IconType.Normal);
      yield return (object) materialsListScene.unit00414Menu.Init(player, playerMaterialUnits, true);
    }
    else
      materialsListScene.StartCoroutine(materialsListScene.OnHimeMaterialEvolutionButton());
  }

  private IEnumerator OnHimeMaterialEvolutionButton()
  {
    Player player = SMManager.Get<Player>();
    PlayerUnit[] playerUnits = SMManager.Get<PlayerUnit[]>();
    PlayerMaterialUnit[] playerMaterialUnits = SMManager.Get<PlayerMaterialUnit[]>();
    yield return (object) this.changeMenu((UnitMenuBase) this.unit00491Menu);
    this.pageType = Unit004UnitMaterialsListScene.PAGETYPE.Unit00491Menu;
    this.unit00491Menu.EnableTouch();
    yield return (object) this.unit00491Menu.Init(player, playerUnits, playerMaterialUnits, false, Unit00491Menu.Mode.Evolution);
    ((Component) this.HimeMaterialEvolutionBtn).gameObject.SetActive(false);
  }

  private IEnumerator changeMenu(UnitMenuBase menu)
  {
    ((Behaviour) this.unit00414Menu).enabled = false;
    ((Behaviour) this.unit00491Menu).enabled = false;
    ((Component) this.bottomSell).gameObject.SetActive(true);
    ((Behaviour) menu).enabled = true;
    this.SetTitle();
    menu.SetIconType(UnitMenuBase.IconType.Normal);
    IEnumerator e = menu.Initialize();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator SetSeaBgm()
  {
    Unit004UnitMaterialsListScene materialsListScene = this;
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    SeaHomeMap seaHomeMap = ((IEnumerable<SeaHomeMap>) MasterData.SeaHomeMapList).ActiveSeaHomeMap(ServerTime.NowAppTimeAddDelta());
    if (seaHomeMap != null && !string.IsNullOrEmpty(seaHomeMap.bgm_cuesheet_name) && !string.IsNullOrEmpty(seaHomeMap.bgm_cue_name))
    {
      materialsListScene.bgmFile = seaHomeMap.bgm_cuesheet_name;
      materialsListScene.bgmName = seaHomeMap.bgm_cue_name;
    }
  }

  public static void changeScene(bool isStack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_UnitMaterials_List", isStack);
  }

  public void onStartScene() => this.StartCoroutine(this.doWaitEndLoading());

  private IEnumerator doWaitEndLoading()
  {
    yield return (object) new WaitForSeconds(this.waitEndLoading_);
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  private void SetTitle()
  {
    if (((Behaviour) this.unit00491Menu).enabled)
      this.textTitle.SetTextLocalize(this.unit00491Menu.Title);
    else
      this.textTitle.SetTextLocalize(this.unit00414Menu.Title);
  }

  public void IbtnBack()
  {
    if (((Behaviour) this.unit00491Menu).enabled)
    {
      this.pageType = Unit004UnitMaterialsListScene.PAGETYPE.Unit00414Menu;
      this.StartCoroutine(this.onStartSceneAsync());
    }
    else
      this.unit00491Menu.IbtnBack();
  }

  private enum PAGETYPE
  {
    Unit00414Menu,
    Unit00491Menu,
  }
}
