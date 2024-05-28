// Decompiled with JetBrains decompiler
// Type: Gacha0063Scene
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
public class Gacha0063Scene : NGSceneBase
{
  private const int GachaModuleMax = 3;
  [SerializeField]
  private Gacha0063Menu gacha0063Menu;
  [SerializeField]
  private NGHorizontalScrollParts scrollParts;
  [SerializeField]
  private UICenterOnChild centerOnChild;
  [SerializeField]
  private UIScrollView scrollView;
  [SerializeField]
  private GameObject dotContainer;
  [SerializeField]
  private UIGrid dotGrid;
  public MasterDataTable.GachaType gachaType;
  protected Modified<GachaModule[]> gachaModule;
  public bool apiUpdate;
  public DateTime serverTime;
  private Dictionary<int, Transform> gachas = new Dictionary<int, Transform>();
  private bool update;
  private bool seEnable;
  private bool apiError;
  private bool duringRetry;
  private bool gachaMaintenance;
  private bool isConnected;
  private Coroutine connectGachaUpdate;
  private List<Gacha0063hindicator> gacha0063hindicatorList;
  private int scrollSelected;
  private int prefabCount;
  private GameObject kisekiPrefab;
  private GameObject pointPrefab;
  private GameObject ticketListPrefab;
  private GameObject ticketPrefab;
  private GameObject detailPopup;
  private GameObject dotContainerPrefab;
  public GameObject dirPanel;
  public GameObject dirPanelSpecial;
  private Vector2 beforeCurrentGachaScrollPosition;
  private bool isFirst_ = true;

  public UIScrollView ScrollView => this.scrollView;

  public override IEnumerator onInitSceneAsync()
  {
    Gacha0063Scene gacha0063Scene = this;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    gacha0063Scene.gachaModule = SMManager.Observe<GachaModule[]>();
    gacha0063Scene.gachaModule.NotifyChanged();
    if (PerformanceConfig.GetInstance().IsTuningGachaInitialize)
    {
      gacha0063Scene.connectGachaUpdate = gacha0063Scene.StartCoroutine(gacha0063Scene.updateGachaParameter());
      yield return (object) null;
    }
    if (!Singleton<TutorialRoot>.GetInstance().IsTutorialFinish())
    {
      gacha0063Scene.isActiveHeader = false;
      gacha0063Scene.isActiveFooter = false;
      GameObject.Find("ibtn_Back_Depth")?.SetActive(false);
      if (Object.op_Inequality((Object) gacha0063Scene.scrollParts.rightArrow, (Object) null))
      {
        Object.Destroy((Object) gacha0063Scene.scrollParts.rightArrow);
        gacha0063Scene.scrollParts.rightArrow = (GameObject) null;
      }
    }
    IEnumerator e = gacha0063Scene.SetBackGround();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = gacha0063Scene.CreatePrefab();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = gacha0063Scene.gacha0063Menu.CreatePrefab();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync()
  {
    IEnumerator e = this.onStartSceneAsync(0);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(int gachaNumber, bool forceApiUpdate)
  {
    this.apiUpdate |= forceApiUpdate;
    IEnumerator e = this.onStartSceneAsync(gachaNumber);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private bool IsSyncRemote()
  {
    return ((IEnumerable<bool>) new bool[4]
    {
      this.apiUpdate,
      DateTime.Now > Singleton<NGGameDataManager>.GetInstance().lastGachaTime.AddMinutes(10.0),
      DateTime.Now.Hour != Singleton<NGGameDataManager>.GetInstance().lastGachaTime.Hour,
      Singleton<NGGameDataManager>.GetInstance().isChangeHaveGachaTiket()
    }).Any<bool>((Func<bool, bool>) (a => a));
  }

  public IEnumerator onStartSceneAsync(int gachaNumber)
  {
    Gacha0063Scene gacha0063Scene = this;
    if (!gacha0063Scene.isFirst_)
      gachaNumber = Singleton<NGGameDataManager>.GetInstance().currentGachaNumber;
    gacha0063Scene.isFirst_ = false;
    if (gacha0063Scene.gachas.ContainsKey(gachaNumber))
    {
      NGxScroll componentInChildren = ((Component) gacha0063Scene.gachas[gachaNumber]).GetComponentInChildren<NGxScroll>();
      gacha0063Scene.beforeCurrentGachaScrollPosition = Object.op_Inequality((Object) componentInChildren, (Object) null) ? componentInChildren.GetScrollPosition() : Vector2.zero;
    }
    gacha0063Scene.seEnable = false;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    if (!gacha0063Scene.isConnected)
      gacha0063Scene.connectGachaUpdate = gacha0063Scene.StartCoroutine(gacha0063Scene.updateGachaParameter());
    yield return (object) gacha0063Scene.connectGachaUpdate;
    gacha0063Scene.isConnected = false;
    if (gacha0063Scene.gachaMaintenance)
    {
      Consts instance = Consts.GetInstance();
      if (!Singleton<TutorialRoot>.GetInstance().IsTutorialFinish())
        ModalWindow.Show(instance.GACHA_MAINTENANCE_TITLE, instance.GACHA_MAINTENANCE_DESCRIPTION, (Action) (() => Singleton<NGSceneManager>.GetInstance().RestartGame()));
      else
        ModalWindow.Show(instance.GACHA_MAINTENANCE_TITLE, instance.GACHA_MAINTENANCE_DESCRIPTION, (Action) (() => MypageScene.ChangeScene(MypageRootMenu.Mode.STORY)));
    }
    else
    {
      IEnumerator e = ServerTime.WaitSync();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      gacha0063Scene.serverTime = ServerTime.NowAppTimeAddDelta();
      e = OnDemandDownload.WaitLoadUnitResource(((IEnumerable<GachaModule>) SMManager.Get<GachaModule[]>()).SelectMany<GachaModule, GachaModuleNewentity>((Func<GachaModule, IEnumerable<GachaModuleNewentity>>) (x => (IEnumerable<GachaModuleNewentity>) x.newentity)).Where<GachaModuleNewentity>((Func<GachaModuleNewentity, bool>) (x => (x.reward_type_id == 1 || x.reward_type_id == 24) && MasterData.UnitUnit.ContainsKey(x.reward_id))).Select<GachaModuleNewentity, UnitUnit>((Func<GachaModuleNewentity, UnitUnit>) (x => MasterData.UnitUnit[x.reward_id])), false, (IEnumerable<string>) new string[1]
      {
        "unit_large"
      });
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = gacha0063Scene.CreateGachaList(gachaNumber);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (gachaNumber != 0)
        gacha0063Scene.StartPoint(gachaNumber);
      gacha0063Scene.scrollParts.CreateAwakeParts(gacha0063Scene.dotContainerPrefab);
      gacha0063Scene.scrollParts.dotPrefab = (GameObject) null;
      gacha0063Scene.seEnable = true;
      gacha0063Scene.StartCoroutine(gacha0063Scene.WaitScrollSe());
      Singleton<CommonRoot>.GetInstance().setBackground(gacha0063Scene.backgroundPrefab);
      ((Component) gacha0063Scene.centerOnChild).transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
      // ISSUE: method pointer
      gacha0063Scene.centerOnChild.onFinished = new SpringPanel.OnFinished((object) gacha0063Scene, __methodptr(\u003ConStartSceneAsync\u003Eb__38_3));
      gacha0063Scene.centerOnChild.onFinished.Invoke();
      e = gacha0063Scene.gacha0063hindicatorList[gacha0063Scene.scrollParts.selected].TextureSet();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (gacha0063Scene.scrollParts.selected + 1 < gacha0063Scene.gacha0063hindicatorList.Count)
      {
        e = gacha0063Scene.gacha0063hindicatorList[gacha0063Scene.scrollParts.selected + 1].TextureSet();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      if (gacha0063Scene.scrollParts.selected - 1 >= 0)
      {
        e = gacha0063Scene.gacha0063hindicatorList[gacha0063Scene.scrollParts.selected - 1].TextureSet();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      gacha0063Scene.dotGrid.Reposition();
      Persist.lastAccessTime.Data.gachaRootLastAccessTime = DateTime.Now;
      Persist.lastAccessTime.Flush();
      Singleton<CommonRoot>.GetInstance().UpdateFooterGachaButton();
    }
  }

  public IEnumerator updateGachaParameter()
  {
    this.update = false;
    this.apiError = false;
    this.isConnected = true;
    if (this.IsSyncRemote() && Singleton<TutorialRoot>.GetInstance().IsTutorialFinish())
    {
      Future<WebAPI.Response.Gacha> future = WebAPI.Gacha((Action<WebAPI.Response.UserError>) (e =>
      {
        WebAPI.DefaultUserErrorCallback(e);
        MypageScene.ChangeSceneOnError();
      }));
      IEnumerator e1 = future.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (future.Result == null)
      {
        this.apiError = true;
      }
      else
      {
        this.gachaMaintenance = future.Result.gacha_modules.Length == 0;
        if (!this.gachaMaintenance)
        {
          this.duringRetry = future.Result.during_retry_gacha;
          if (this.duringRetry)
          {
            Consts instance = Consts.GetInstance();
            ModalWindow.Show(instance.GACHA_NOT_END_TITLE, instance.GACHA_NOT_END_DESCRIPTION, (Action) (() => Gacha00613Scene.ChangeScene(true, true)));
          }
          else
          {
            if (((IEnumerable<GachaModule>) this.gachaModule.Value).Where<GachaModule>((Func<GachaModule, bool>) (x => x.type == 6)).Count<GachaModule>() > 0)
            {
              Future<WebAPI.Response.GachaG007PanelPanelInfo> panelFuture = WebAPI.GachaG007PanelPanelInfo((Action<WebAPI.Response.UserError>) (e =>
              {
                WebAPI.DefaultUserErrorCallback(e);
                MypageScene.ChangeSceneOnError();
              }));
              e1 = panelFuture.Wait();
              while (e1.MoveNext())
                yield return e1.Current;
              e1 = (IEnumerator) null;
              if (panelFuture.Result == null)
              {
                this.apiError = true;
                yield break;
              }
              else
                panelFuture = (Future<WebAPI.Response.GachaG007PanelPanelInfo>) null;
            }
            Singleton<NGGameDataManager>.GetInstance().lastGachaTime = DateTime.Now;
            this.update = true;
            this.apiUpdate = false;
            future = (Future<WebAPI.Response.Gacha>) null;
          }
        }
      }
    }
  }

  private int GetGachaPageLength(GachaModule[] gachaModules)
  {
    int num = ((IEnumerable<GachaModule>) gachaModules).Count<GachaModule>((Func<GachaModule, bool>) (c => c.type != 4));
    GachaModule gachaModule = ((IEnumerable<GachaModule>) gachaModules).FirstOrDefault<GachaModule>((Func<GachaModule, bool>) (n => n.type == 4));
    int length = gachaModule != null ? gachaModule.gacha.Length : 0;
    return num + length;
  }

  public void onStartScene()
  {
    if (this.apiError || this.duringRetry || this.gachaMaintenance)
      return;
    this.gacha0063hindicatorList.ForEach((Action<Gacha0063hindicator>) (x => x.EndAnim()));
    Singleton<PopupManager>.GetInstance().closeAll();
    this.StartCoroutine(this.waitClearLoading());
    this.gacha0063hindicatorList.ForEach((Action<Gacha0063hindicator>) (x => x.PlayAnim()));
  }

  public void onStartScene(int gacha_type)
  {
    if (this.apiError || this.duringRetry || this.gachaMaintenance)
      return;
    this.gacha0063hindicatorList.ForEach((Action<Gacha0063hindicator>) (x => x.EndAnim()));
    Singleton<PopupManager>.GetInstance().closeAll();
    this.StartCoroutine(this.waitClearLoading());
    this.gacha0063hindicatorList.ForEach((Action<Gacha0063hindicator>) (x => x.PlayAnim()));
  }

  public void onStartScene(int gacha_type, bool forceApiUpdate)
  {
    if (this.apiError || this.duringRetry || this.gachaMaintenance)
      return;
    this.gacha0063hindicatorList.ForEach((Action<Gacha0063hindicator>) (x => x.EndAnim()));
    Singleton<PopupManager>.GetInstance().closeAll();
    this.StartCoroutine(this.waitClearLoading());
    this.gacha0063hindicatorList.ForEach((Action<Gacha0063hindicator>) (x => x.PlayAnim()));
  }

  private IEnumerator waitClearLoading()
  {
    yield return (object) new WaitForEndOfFrame();
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  public override void onEndScene()
  {
    if (this.apiError || this.duringRetry || this.gachaMaintenance)
      return;
    this.seEnable = false;
    this.scrollParts.SeEnable = false;
    this.gacha0063hindicatorList.ForEach((Action<Gacha0063hindicator>) (x => x.EndAnim()));
    this.gacha0063hindicatorList[this.scrollSelected].TextureClear();
    if (this.scrollSelected + 1 < this.gacha0063hindicatorList.Count)
      this.gacha0063hindicatorList[this.scrollSelected + 1].TextureClear();
    if (this.scrollSelected - 1 < 0)
      return;
    this.gacha0063hindicatorList[this.scrollSelected - 1].TextureClear();
  }

  private void StartPoint(int moduleNumber)
  {
    if (!this.gachas.ContainsKey(moduleNumber))
      return;
    this.scrollParts.setItemPosition(this.scrollParts.GetIndex(((Component) this.gachas[moduleNumber]).transform));
    this.scrollParts.setItemPositionQuick(this.scrollParts.GetIndex(((Component) this.gachas[moduleNumber]).transform));
  }

  public void BackPage()
  {
    int index = this.scrollParts.selected - 1;
    if (index < 0)
      return;
    this.centerOnChild.CenterOn(((Component) this.gacha0063hindicatorList[index]).transform);
  }

  public void NextPage()
  {
    int index = this.scrollParts.selected + 1;
    if (index >= this.gacha0063hindicatorList.Count)
      return;
    this.centerOnChild.CenterOn(((Component) this.gacha0063hindicatorList[index]).transform);
  }

  private IEnumerator WaitScrollSe()
  {
    yield return (object) new WaitForSeconds(0.3f);
    this.scrollParts.SeEnable = this.seEnable;
  }

  private IEnumerator CreatePrefab()
  {
    Future<GameObject> prefabF = Res.Prefabs.gacha006_3.hindicator_640_23_kiseki.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.kisekiPrefab = prefabF.Result;
    prefabF = (Future<GameObject>) null;
    prefabF = Res.Prefabs.gacha006_3.hindicator_640_23_point.Load<GameObject>();
    e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.pointPrefab = prefabF.Result;
    prefabF = (Future<GameObject>) null;
    prefabF = Res.Prefabs.gacha006_3.hindicator_640_23_ticket_list.Load<GameObject>();
    e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.ticketListPrefab = prefabF.Result;
    prefabF = (Future<GameObject>) null;
    prefabF = Res.Prefabs.gacha006_3.hindicator_640_23_ticket.Load<GameObject>();
    e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.ticketPrefab = prefabF.Result;
    prefabF = (Future<GameObject>) null;
    if (Object.op_Equality((Object) this.detailPopup, (Object) null))
    {
      prefabF = Res.Prefabs.popup.popup_006_3_1__anim_popup01.Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.detailPopup = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.dirPanel, (Object) null))
    {
      prefabF = Res.Prefabs.gacha006_3.dir_panel.Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.dirPanel = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.dirPanelSpecial, (Object) null))
    {
      prefabF = Res.Prefabs.gacha006_3.dir_panel_special.Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.dirPanelSpecial = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.dotContainerPrefab, (Object) null))
    {
      prefabF = Res.Prefabs.gacha006_3.dotContainer.Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.dotContainerPrefab = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
  }

  private IEnumerator CreateGachaList(int gachaNumber)
  {
    this.scrollParts.destroyParts(false);
    this.gacha0063hindicatorList = new List<Gacha0063hindicator>();
    this.gacha0063hindicatorList.Clear();
    this.gachas.Clear();
    GachaModule[] gachaModuleArray = SMManager.Get<GachaModule[]>();
    if (gachaModuleArray != null)
    {
      bool flag = !Singleton<TutorialRoot>.GetInstance().IsTutorialFinish();
      Dictionary<int, PlayerGachaTicket> ticketDict = flag ? new Dictionary<int, PlayerGachaTicket>() : ((IEnumerable<PlayerGachaTicket>) SMManager.Get<Player>().gacha_tickets).ToDictionary<PlayerGachaTicket, int>((Func<PlayerGachaTicket, int>) (x => x.ticket_id));
      this.prefabCount = 0;
      foreach (GachaModule gachaModule in gachaModuleArray)
      {
        if (!this.IsLimited(gachaModule))
        {
          if (flag)
          {
            this.AddGachaIndicator(this.ticketPrefab, gachaModule);
            break;
          }
          switch (gachaModule.type)
          {
            case 2:
              this.AddGachaIndicator(this.pointPrefab, gachaModule);
              continue;
            case 4:
              if (((IEnumerable<GachaModuleGacha>) gachaModule.gacha).Any<GachaModuleGacha>((Func<GachaModuleGacha, bool>) (x => x.payment_id.HasValue && ticketDict.ContainsKey(x.payment_id.Value) && ticketDict[x.payment_id.Value].quantity > 0)))
              {
                this.AddGachaIndicator(this.ticketListPrefab, gachaModule);
                continue;
              }
              continue;
            default:
              this.AddGachaIndicator(this.kisekiPrefab, gachaModule);
              continue;
          }
        }
      }
      this.scrollParts.resetScrollView();
      this.scrollParts.setItemPosition(0);
      this.scrollParts.setItemPositionQuick(0);
      yield return (object) null;
      IEnumerable<\u003C\u003Ef__AnonymousType12<Gacha0063hindicator, int>> setupGachaItemList = this.gacha0063hindicatorList.Select((gacha, count) => new
      {
        gacha = gacha,
        count = count
      });
      foreach (var data in setupGachaItemList)
      {
        IEnumerator e = data.gacha.Set(this.detailPopup);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      if (PerformanceConfig.GetInstance().IsTuningGachaInitialize && gachaNumber != 0)
      {
        var data = setupGachaItemList.FirstOrDefault(x => x.gacha.GachaNumber == gachaNumber);
        if (data != null)
        {
          NGxScroll componentInChildren = ((Component) ((Component) data.gacha).transform).GetComponentInChildren<NGxScroll>();
          if (Object.op_Inequality((Object) componentInChildren, (Object) null) && Object.op_Inequality((Object) componentInChildren.scrollView, (Object) null))
          {
            double height = (double) componentInChildren.scrollView.panel.height;
            Bounds bounds = componentInChildren.scrollView.bounds;
            double y = (double) ((Bounds) ref bounds).size.y;
            if (height < y)
              componentInChildren.ResolvePosition(this.beforeCurrentGachaScrollPosition);
          }
        }
      }
    }
  }

  private void AddGachaIndicator(GameObject prefab, GachaModule gachaModule)
  {
    bool flag = false;
    if (gachaModule.type == 4)
    {
      if (gachaModule.gacha != null && gachaModule.gacha.Length != 0)
        flag = true;
    }
    else if (gachaModule.gacha != null && gachaModule.gacha.Length != 0 && gachaModule.gacha.Length < 3)
      flag = true;
    if (!flag)
      return;
    Gacha0063hindicator component = this.scrollParts.instantiateParts(prefab, false).GetComponent<Gacha0063hindicator>();
    this.gacha0063Menu.scene = this;
    DateTime rootLastAccessTime = Persist.lastAccessTime.Data.gachaRootLastAccessTime;
    ((IEnumerable<GachaModuleGacha>) gachaModule.gacha).Max<GachaModuleGacha, DateTime?>((Func<GachaModuleGacha, DateTime?>) (x => x.start_at));
    component.InitGachaModuleGacha(this.gacha0063Menu, gachaModule, this.serverTime, this.scrollView, this.prefabCount);
    component.ScrollCenterOnFinished();
    this.gacha0063hindicatorList.Add(component);
    this.gachas[gachaModule.number] = ((Component) component).transform;
    ++this.prefabCount;
  }

  private IEnumerator SetBackGround()
  {
    Gacha0063Scene gacha0063Scene = this;
    Future<GameObject> fBG = Res.Prefabs.BackGround.GachaTopBackground.Load<GameObject>();
    IEnumerator e = fBG.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    gacha0063Scene.backgroundPrefab = fBG.Result;
    ((UIWidget) gacha0063Scene.backgroundPrefab.GetComponent<UI2DSprite>()).color = Color.white;
    Singleton<CommonRoot>.GetInstance().setBackground(gacha0063Scene.backgroundPrefab);
  }

  public bool IsLimited(GachaModule module)
  {
    DateTime? endAt = module.period.end_at;
    DateTime serverTime = this.serverTime;
    TimeSpan? nullable = endAt.HasValue ? new TimeSpan?(endAt.GetValueOrDefault() - serverTime) : new TimeSpan?();
    return module.period.end_at.HasValue && nullable.Value.Milliseconds < 0;
  }

  public GameObject GetTutorialGachaTop()
  {
    return ((Component) this.gacha0063hindicatorList[0]).gameObject;
  }

  public bool IsCenterCheck(int index)
  {
    return !Object.op_Equality((Object) this.scrollParts, (Object) null) && this.scrollParts.selected == index;
  }

  private void Update()
  {
    if (!Object.op_Inequality((Object) this.scrollParts, (Object) null) || Singleton<CommonRoot>.GetInstance().isLoading)
      return;
    if (this.scrollParts.selected >= 0 && this.scrollParts.selected != this.scrollSelected)
      this.GachaTextureSet(this.scrollParts.selected, this.scrollParts.selected > this.scrollSelected);
    this.scrollSelected = this.scrollParts.selected;
  }

  private IEnumerator CashClean()
  {
    GC.Collect();
    GC.WaitForPendingFinalizers();
    Singleton<ResourceManager>.GetInstance().ClearCache();
    AsyncOperation asyncOP = Resources.UnloadUnusedAssets();
    while (!asyncOP.isDone)
      yield return (object) null;
  }

  private void GachaTextureSet(int num, bool forward)
  {
    if (!Singleton<TutorialRoot>.GetInstance().IsTutorialFinish())
      return;
    if (PerformanceConfig.GetInstance().IsGachaLowMemory)
      this.StartCoroutine(this.CashClean());
    if (num + 1 < this.gacha0063hindicatorList.Count & forward)
      this.StartCoroutine(this.gacha0063hindicatorList[num + 1].TextureSet());
    if (num - 1 >= 0 && !forward)
      this.StartCoroutine(this.gacha0063hindicatorList[num - 1].TextureSet());
    if (num + 2 < this.gacha0063hindicatorList.Count && !forward)
      this.gacha0063hindicatorList[num + 2].TextureClear();
    if (num - 2 >= 0 & forward)
      this.gacha0063hindicatorList[num - 2].TextureClear();
    if (num == this.gacha0063hindicatorList.Count - 1 && this.gacha0063hindicatorList[num].GachaModule.type == 4)
      this.dotContainer.SetActive(false);
    else
      this.dotContainer.SetActive(true);
  }
}
