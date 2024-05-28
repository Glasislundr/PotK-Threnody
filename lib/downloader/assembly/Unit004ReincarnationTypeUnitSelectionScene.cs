// Decompiled with JetBrains decompiler
// Type: Unit004ReincarnationTypeUnitSelectionScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Unit004ReincarnationTypeUnitSelectionScene : NGSceneBase
{
  public Unit004ReincarnationTypeUnitSelectionMenu menu;
  [SerializeField]
  private UIScrollView ScrollView;
  private bool isInit = true;

  public override IEnumerator onInitSceneAsync()
  {
    this.isInit = true;
    yield break;
  }

  public static void changeScene(bool stack, UnitTypeTicket ticket)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_Reincarnation_Type_UnitSelection", (stack ? 1 : 0) != 0, (object) ticket);
  }

  public IEnumerator onStartSceneAsync(UnitTypeTicket ticket)
  {
    if (!this.isInit)
    {
      yield return (object) this.reloadMenu();
    }
    else
    {
      yield return (object) this.initMenu(ticket);
      this.isInit = false;
    }
  }

  private IEnumerator initMenu(UnitTypeTicket ticket)
  {
    this.menu.SetIconType(UnitMenuBase.IconType.Normal);
    yield return (object) this.menu.Init(SMManager.Get<Player>(), SMManager.Get<PlayerUnit[]>(), false, ticket);
  }

  private IEnumerator reloadMenu()
  {
    yield return (object) this.menu.reload(SMManager.Get<PlayerUnit[]>());
  }

  public override void onEndScene()
  {
    Persist.sortOrder.Flush();
    UnitIcon.ClearCache();
  }

  public void IbtnSort() => this.menu.IbtnSort();

  public void IbtnBack() => this.menu.IbtnBack();

  protected virtual void OnEnable()
  {
    if (!this.ScrollView.isDragging)
      return;
    this.ScrollView.Press(false);
  }
}
