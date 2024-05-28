// Decompiled with JetBrains decompiler
// Type: Unit004ReincarnationTypeMenu
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
public class Unit004ReincarnationTypeMenu : BackButtonMenuBase
{
  [SerializeField]
  private UILabel txtTitle;
  [SerializeField]
  private GameObject ticketIconBase;
  private Unit00499ReincarnationChange ReinCarnationChangeCtrl;
  [SerializeField]
  protected Unit00499UnitStatus beforeUnit;
  private Unit00499UnitStatus afterUnit;
  [SerializeField]
  private Transform afterUnitBase;
  private UnitTypeTicket ticket;
  private PlayerUnit playerUnitBefore;
  private PlayerUnit playerUnitAfter;

  public virtual IEnumerator Init(
    UnitTypeTicket ticket,
    PlayerUnit playerUnitBefore,
    PlayerUnit playerUnitAfter)
  {
    this.ticket = ticket;
    this.playerUnitBefore = playerUnitBefore;
    this.playerUnitAfter = playerUnitAfter;
    this.txtTitle.SetTextLocalize(Consts.Format(Consts.GetInstance().UNIT_004_REINCARNATION_TYPE_SCENE_TITLE, (IDictionary) new Hashtable()
    {
      {
        (object) "UnitName",
        (object) playerUnitBefore.unit.name
      }
    }));
    Future<GameObject> ldicon = Res.Icons.UniqueIconPrefab.Load<GameObject>();
    yield return (object) ldicon.Wait();
    GameObject result1 = ldicon.Result;
    if (Object.op_Inequality((Object) result1, (Object) null))
    {
      UniqueIcons ticketIcon = result1.Clone(this.ticketIconBase.transform).GetComponent<UniqueIcons>();
      yield return (object) ticketIcon.SetReincarnationTypeTicket(ticket.ID, ticket.cost);
      ticketIcon.LabelActivated = false;
      ticketIcon.BackGroundActivated = false;
      ticketIcon = (UniqueIcons) null;
    }
    ldicon = (Future<GameObject>) null;
    ldicon = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    yield return (object) ldicon.Wait();
    GameObject result2 = ldicon.Result;
    yield return (object) this.InitPlayer(playerUnitBefore, playerUnitAfter, result2);
    ldicon = (Future<GameObject>) null;
  }

  public IEnumerator InitPlayer(
    PlayerUnit playerUnitBefore,
    PlayerUnit playerUnitAfter,
    GameObject unitIconPrefab)
  {
    if (Object.op_Equality((Object) this.afterUnit, (Object) null))
    {
      Future<GameObject> prefabF = new ResourceObject("Prefabs/unit004_Reincarnation_Type/dir_Status_Reincarnation_Type_After").Load<GameObject>();
      yield return (object) prefabF.Wait();
      this.afterUnit = prefabF.Result.Clone(this.afterUnitBase).GetComponent<Unit00499UnitStatus>();
      prefabF = (Future<GameObject>) null;
    }
    Func<PlayerUnit, GameObject, bool, IEnumerator> initUnitIcon = (Func<PlayerUnit, GameObject, bool, IEnumerator>) ((pu, go, before) =>
    {
      foreach (Component component in go.transform)
        Object.Destroy((Object) component.gameObject);
      UnitIcon component1 = unitIconPrefab.CloneAndGetComponent<UnitIcon>(go.transform);
      component1.RarityCenter();
      ((Collider) component1.buttonBoxCollider).enabled = true;
      ((Behaviour) component1.Button).enabled = true;
      PlayerUnit[] units = new PlayerUnit[1]
      {
        before ? playerUnitBefore : playerUnitAfter
      };
      return this.InitUnitIcon(component1, pu, units, before);
    });
    IEnumerator e = initUnitIcon(playerUnitBefore, this.beforeUnit.linkUnit, true);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = initUnitIcon(playerUnitAfter, this.afterUnit.linkUnit, false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.beforeUnit.SetStatusText(playerUnitBefore);
    this.afterUnit.SetStatusText(playerUnitAfter);
  }

  private IEnumerator InitUnitIcon(
    UnitIcon unitIcon,
    PlayerUnit unit,
    PlayerUnit[] units,
    bool before)
  {
    IEnumerator e;
    if (before)
    {
      e = unitIcon.SetPlayerUnit(unit, units, (PlayerUnit) null, true, false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      e = unitIcon.SetPlayerUnitReincarnationType(unit, units, isMaterial: true);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    unitIcon.onClick = (Action<UnitIconBase>) (icon =>
    {
      if (before)
        Unit0042Scene.changeScene(true, icon.PlayerUnit, units, true);
      else
        Unit0042Scene.changeSceneReincarnationTypeUnit(true, icon.PlayerUnit, units, true);
    });
  }

  private IEnumerator openPopupTypeConfirm()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Unit004ReincarnationTypeMenu reincarnationTypeMenu = this;
    Future<GameObject> prefabF;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      Singleton<PopupManager>.GetInstance().open(prefabF.Result).GetComponent<Popup004ReincarnationTypeMenu>().Init(reincarnationTypeMenu.playerUnitBefore.unit_type.name, reincarnationTypeMenu.playerUnitAfter.unit_type.name, new Action(reincarnationTypeMenu.OnReincarnation));
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    prefabF = new ResourceObject("Prefabs/unit004_Reincarnation_Type/popup_004_Reincarnation_Type__anim_popup01").Load<GameObject>();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) prefabF.Wait();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private void OnReincarnation() => this.StartCoroutine(this.ReincarnationType());

  public IEnumerator ReincarnationType()
  {
    Unit004ReincarnationTypeMenu reincarnationTypeMenu = this;
    if (!Singleton<CommonRoot>.GetInstance().isLoading)
    {
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      Future<WebAPI.Response.UnittypeticketSpend> paramF = WebAPI.UnittypeticketSpend(reincarnationTypeMenu.playerUnitBefore.id, reincarnationTypeMenu.ticket.ID, reincarnationTypeMenu.playerUnitAfter.unit_type.ID, (Action<WebAPI.Response.UserError>) (error =>
      {
        WebAPI.DefaultUserErrorCallback(error);
        MypageScene.ChangeSceneOnError();
      }));
      yield return (object) paramF.Wait();
      if (paramF.Result != null)
      {
        Singleton<NGGameDataManager>.GetInstance().corps_player_unit_ids = new HashSet<int>((IEnumerable<int>) paramF.Result.corps_player_unit_ids);
        yield return (object) OnDemandDownload.WaitLoadHasUnitResource(false);
        // ISSUE: reference to a compiler-generated method
        PlayerUnit playerUnit = ((IEnumerable<PlayerUnit>) SMManager.Get<PlayerUnit[]>()).FirstOrDefault<PlayerUnit>(new Func<PlayerUnit, bool>(reincarnationTypeMenu.\u003CReincarnationType\u003Eb__14_1));
        unit00497Scene.ChangeScene(false, new PrincesEvolutionParam()
        {
          materiaqlUnits = new List<PlayerUnit>(),
          is_new = false,
          baseUnit = reincarnationTypeMenu.playerUnitBefore,
          resultUnit = playerUnit,
          mode = Unit00499Scene.Mode.ReincarnationType
        });
        Singleton<NGSceneManager>.GetInstance().destroyScene("unit004_Reincarnation_Type");
        paramF = (Future<WebAPI.Response.UnittypeticketSpend>) null;
      }
    }
  }

  public override void onBackButton() => this.IbtnBack();

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
    Singleton<PopupManager>.GetInstance().closeAll();
  }

  public virtual void IbtnTrans()
  {
    if (this.IsPush)
      return;
    this.StartCoroutine(this.IbtnTransAsync());
  }

  private IEnumerator IbtnTransAsync()
  {
    if (this.playerUnitBefore.tower_is_entry || this.playerUnitBefore.corps_is_entry)
    {
      bool isRejected = false;
      yield return (object) PopupManager.StartTowerEntryUnitWarningPopupProc((Action<bool>) (selected => isRejected = !selected));
      if (isRejected)
        yield break;
    }
    yield return (object) this.openPopupTypeConfirm();
  }
}
