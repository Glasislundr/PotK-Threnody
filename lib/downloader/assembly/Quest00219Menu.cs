// Decompiled with JetBrains decompiler
// Type: Quest00219Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/QuestExtra/M_Menu")]
public class Quest00219Menu : BackButtonMenuBase
{
  public UIScrollView scrollview;
  public UIGrid grid;
  public UI2DSprite EventSprite;
  public bool isSideQuest;
  [Header("AnchorTopControl")]
  [Tooltip("EventSpriteの有無に合わせてScrollViewのTop位置を調整")]
  [SerializeField]
  protected UIWidget widgetScrollViewTop;
  [SerializeField]
  protected int whenOnEventSprite;
  [SerializeField]
  protected int whenOffEventSprige;
  protected DateTime serverTime;
  private HashSet<int> emphasis_;
  private List<Coroutine> loadingCoroutineList = new List<Coroutine>();
  private const int maxDefaultSetupBanner = 5;
  private int setupBannerCount;
  private int focusSId_;
  private QuestExtraL headerInfo_;
  private int player_lv;
  private Queue<Tuple<IEnumerator, Quest00217Scroll.Parameter>> lateAddItems_;

  public bool IncludingKeyGate { get; set; }

  private PlayerExtraQuestS[] ExtraData { get; set; }

  private List<QuestExtraTimetableNotice> Notices { get; set; }

  private void OnDestroy()
  {
    foreach (Coroutine loadingCoroutine in this.loadingCoroutineList)
    {
      if (loadingCoroutine != null)
        this.StopCoroutine(loadingCoroutine);
    }
    this.loadingCoroutineList.Clear();
  }

  protected string LoadSpriteEvent(int LId)
  {
    string path = "Prefabs/Banners/ExtraQuest/L/" + LId.ToString() + "/Specialquest_Story";
    return Singleton<ResourceManager>.GetInstance().Contains(path) ? path : "Prefabs/Banners/ExtraQuest/L/4/Specialquest_Story";
  }

  public IEnumerator Init(
    Future<GameObject> ListPrefab,
    Future<GameObject> ScrollPrefab,
    PlayerExtraQuestS[] ExtraData,
    int lid,
    int Sid,
    int[] Emphasis,
    QuestExtraTimetableNotice[] Notices)
  {
    ((Component) this.grid).transform.Clear();
    ((Component) this.grid).gameObject.SetActive(false);
    Modified<Player> modified = SMManager.Observe<Player>();
    if (modified != null)
      this.player_lv = modified.Value.level;
    this.ExtraData = ExtraData;
    this.emphasis_ = new HashSet<int>((IEnumerable<int>) Emphasis);
    this.Notices = ((IEnumerable<QuestExtraTimetableNotice>) Notices).ToList<QuestExtraTimetableNotice>();
    PlayerQuestGate[] questGate = SMManager.Get<PlayerQuestGate[]>();
    HashSet<int> idGateS = new HashSet<int>(((IEnumerable<PlayerQuestGate>) questGate).SelectMany<PlayerQuestGate, int>((Func<PlayerQuestGate, IEnumerable<int>>) (s => (IEnumerable<int>) s.quest_ids)));
    PlayerExtraQuestS[] questExtra = SMManager.Get<PlayerExtraQuestS[]>();
    PlayerExtraQuestS[] list = ((IEnumerable<PlayerExtraQuestS>) ((IEnumerable<PlayerExtraQuestS>) this.ExtraData).M(lid, true)).Where<PlayerExtraQuestS>((Func<PlayerExtraQuestS, bool>) (w => !idGateS.Contains(w._quest_extra_s))).ToArray<PlayerExtraQuestS>();
    IEnumerator e;
    if (list.Length == 0)
    {
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Future<GameObject> time_popup = Res.Prefabs.popup.popup_002_23__anim_popup01.Load<GameObject>();
      e = time_popup.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject result = time_popup.Result;
      Singleton<PopupManager>.GetInstance().openAlert(result);
    }
    else
    {
      this.focusSId_ = Sid;
      this.setupBannerCount = 0;
      this.headerInfo_ = MasterData.QuestExtraL[lid];
      if (this.headerInfo_.enabled_header)
      {
        ((UIRect) this.widgetScrollViewTop).topAnchor.absolute = this.whenOnEventSprite;
        ((Component) this.EventSprite).gameObject.SetActive(true);
        Future<Texture2D> futureEvent = Singleton<ResourceManager>.GetInstance().Load<Texture2D>(this.LoadSpriteEvent(this.headerInfo_.banner_image_id.GetValueOrDefault(lid)));
        e = futureEvent.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        Texture2D result = futureEvent.Result;
        Sprite sprite = Sprite.Create(result, new Rect(0.0f, 0.0f, (float) ((Texture) result).width, (float) ((Texture) result).height), new Vector2(0.5f, 0.5f), 1f, 100U, (SpriteMeshType) 0);
        ((Object) sprite).name = ((Object) result).name;
        this.EventSprite.sprite2D = sprite;
        futureEvent = (Future<Texture2D>) null;
      }
      else
      {
        ((UIRect) this.widgetScrollViewTop).topAnchor.absolute = this.whenOffEventSprige;
        ((Component) this.EventSprite).gameObject.SetActive(false);
      }
      e = ServerTime.WaitSync();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.serverTime = ServerTime.NowAppTime();
      this.lateAddItems_ = new Queue<Tuple<IEnumerator, Quest00217Scroll.Parameter>>();
      PlayerExtraQuestS playerExtraQuestS = ((IEnumerable<PlayerExtraQuestS>) list).First<PlayerExtraQuestS>();
      if (BannerBase.PathExist(playerExtraQuestS.quest_extra_s, playerExtraQuestS.quest_extra_s.quest_m_QuestExtraM, QuestExtra.SeekType.M))
      {
        e = this.PutBannerList(ScrollPrefab, list);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      else
      {
        e = this.PutGeneralBtnList(ListPrefab, list);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      e = this.InitQuestGate(lid, questExtra, questGate);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      while (this.lateAddItems_.Any<Tuple<IEnumerator, Quest00217Scroll.Parameter>>())
        yield return (object) this.lateAddItems_.Dequeue().Item1;
      this.lateAddItems_ = (Queue<Tuple<IEnumerator, Quest00217Scroll.Parameter>>) null;
      ((Component) this.grid).gameObject.SetActive(true);
      this.grid.Reposition();
      this.scrollview.ResetPosition();
      Singleton<CommonRoot>.GetInstance().isLoading = false;
    }
  }

  private Quest00217Scroll.Parameter GenerateParam(PlayerExtraQuestS extra)
  {
    Quest00217Scroll.Parameter parameter = new Quest00217Scroll.Parameter();
    QuestExtra.getStatusM(extra.quest_extra_s.quest_m_QuestExtraM, this.ExtraData, this.emphasis_, out parameter.isNew, out parameter.isClear, out parameter.isHighlighting, out parameter.isClearedToday, out parameter.isSkipSortie, out parameter.entryConditionID);
    return parameter;
  }

  private IEnumerator PutBannerList(Future<GameObject> prefab, PlayerExtraQuestS[] list)
  {
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject scrollPrefab = prefab.Result;
    this.grid.cellHeight = 168f;
    PlayerExtraQuestS[] playerExtraQuestSArray = list;
    for (int index = 0; index < playerExtraQuestSArray.Length; ++index)
    {
      PlayerExtraQuestS extra = playerExtraQuestSArray[index];
      Quest00217Scroll.Parameter parameter = this.GenerateParam(extra);
      QuestExtraTimetableNotice extraTimetableNotice = this.Notices.Find((Predicate<QuestExtraTimetableNotice>) (n => n._quest_extra_s == extra._quest_extra_s));
      if (extraTimetableNotice != null && extraTimetableNotice.start_at.HasValue)
      {
        parameter.isNotice = true;
        parameter.startTime = extraTimetableNotice.start_at;
      }
      parameter.setMainData(extra);
      parameter.seek = QuestExtra.SeekType.M;
      if (parameter.isClearedToday)
      {
        this.lateAddItems_.Enqueue(Tuple.Create<IEnumerator, Quest00217Scroll.Parameter>(this.ScrollInit(parameter, scrollPrefab), parameter));
      }
      else
      {
        e = this.ScrollInit(parameter, scrollPrefab);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
    playerExtraQuestSArray = (PlayerExtraQuestS[]) null;
  }

  private IEnumerator PutGeneralBtnList(Future<GameObject> prefab, PlayerExtraQuestS[] list)
  {
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject listPrefab = prefab.Result;
    this.grid.cellHeight = 114f;
    PlayerExtraQuestS[] playerExtraQuestSArray = list;
    for (int index = 0; index < playerExtraQuestSArray.Length; ++index)
    {
      PlayerExtraQuestS extra = playerExtraQuestSArray[index];
      Quest00217Scroll.Parameter parameter = this.GenerateParam(extra);
      if (parameter.isClearedToday)
      {
        this.lateAddItems_.Enqueue(Tuple.Create<IEnumerator, Quest00217Scroll.Parameter>(this.ListInit(extra, listPrefab, parameter.isClear, parameter.isNew, parameter.isClearedToday, parameter.isSkipSortie), parameter));
      }
      else
      {
        e = this.ListInit(extra, listPrefab, parameter.isClear, parameter.isNew, parameter.isClearedToday, parameter.isSkipSortie);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
    playerExtraQuestSArray = (PlayerExtraQuestS[]) null;
  }

  private IEnumerator InitQuestGate(
    int idQuestL,
    PlayerExtraQuestS[] questExtra,
    PlayerQuestGate[] questGates)
  {
    IEnumerable<PlayerExtraQuestS> source = ((IEnumerable<PlayerExtraQuestS>) questExtra).Where<PlayerExtraQuestS>((Func<PlayerExtraQuestS, bool>) (w => w.quest_extra_s != null)).Where<PlayerExtraQuestS>((Func<PlayerExtraQuestS, bool>) (w => w.quest_extra_s.quest_l_QuestExtraL == idQuestL));
    if (source.Count<PlayerExtraQuestS>() != 0)
    {
      List<int> targetIdS = source.Select<PlayerExtraQuestS, int>((Func<PlayerExtraQuestS, int>) (s => s.quest_extra_s.ID)).ToList<int>();
      List<PlayerQuestGate> gates = ((IEnumerable<PlayerQuestGate>) questGates).Where<PlayerQuestGate>((Func<PlayerQuestGate, bool>) (w => ((IEnumerable<int>) w.quest_ids).Any<int>((Func<int, bool>) (a => targetIdS.Contains(a))))).ToList<PlayerQuestGate>();
      if (gates.Count<PlayerQuestGate>() != 0)
      {
        Dictionary<int, bool> inProgressQuests = gates.Aggregate<PlayerQuestGate, Dictionary<int, bool>>(new Dictionary<int, bool>(), (Func<Dictionary<int, bool>, PlayerQuestGate, Dictionary<int, bool>>) ((acc, quest) =>
        {
          if (quest.in_progress)
            acc[quest.quest_key_id] = true;
          return acc;
        }));
        List<int> keyKinds = gates.Distinct<PlayerQuestGate>((IEqualityComparer<PlayerQuestGate>) new LambdaEqualityComparer<PlayerQuestGate>((Func<PlayerQuestGate, PlayerQuestGate, bool>) ((a, b) => a.quest_key_id == b.quest_key_id))).OrderByDescending<PlayerQuestGate, bool>((Func<PlayerQuestGate, bool>) (x => inProgressQuests.ContainsKey(x.quest_key_id))).ThenBy<PlayerQuestGate, int>((Func<PlayerQuestGate, int>) (y => y.quest_key_id)).Select<PlayerQuestGate, int>((Func<PlayerQuestGate, int>) (z => z.quest_key_id)).ToList<int>();
        if (keyKinds.Count<int>() != 0)
        {
          this.IncludingKeyGate = true;
          Future<GameObject> ScrollPrefab = Res.Prefabs.quest002_17_1.scroll.Load<GameObject>();
          IEnumerator e = ScrollPrefab.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          GameObject prefab = ScrollPrefab.Result;
          foreach (int num in keyKinds)
          {
            int keyKind = num;
            e = this.ScrollInit(gates.Where<PlayerQuestGate>((Func<PlayerQuestGate, bool>) (x => x.quest_key_id == keyKind)).ToArray<PlayerQuestGate>(), prefab);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
          }
        }
      }
    }
  }

  private IEnumerator ScrollInit(PlayerQuestGate[] gates, GameObject prefab)
  {
    GameObject gameObject = Object.Instantiate<GameObject>(prefab);
    gameObject.transform.parent = ((Component) this.grid).transform;
    gameObject.transform.localScale = Vector3.one;
    gameObject.transform.localPosition = Vector3.zero;
    IEnumerator e = gameObject.GetComponent<Quest002171Scroll>().InitScroll(gates, this.serverTime, false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public virtual IEnumerator ScrollInit(Quest00217Scroll.Parameter param, GameObject prefab)
  {
    Quest00219Menu quest00219Menu = this;
    GameObject gameObject = Object.Instantiate<GameObject>(prefab);
    gameObject.transform.parent = ((Component) quest00219Menu.grid).transform;
    gameObject.transform.localScale = Vector3.one;
    gameObject.transform.localPosition = Vector3.zero;
    Quest00217Scroll component = gameObject.GetComponent<Quest00217Scroll>();
    if (PerformanceConfig.GetInstance().IsTuningEventTopSetting && quest00219Menu.setupBannerCount >= 5)
    {
      component.Setup(param, quest00219Menu.serverTime);
      Coroutine coroutine = quest00219Menu.StartCoroutine(component.SetAndCreate_BannerSprite());
      quest00219Menu.loadingCoroutineList.Add(coroutine);
    }
    else
    {
      IEnumerator e = component.InitScroll(param, quest00219Menu.serverTime);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    ++quest00219Menu.setupBannerCount;
  }

  public virtual IEnumerator ListInit(
    PlayerExtraQuestS extra,
    GameObject prefab,
    bool isClear,
    bool isNew,
    bool isClearedToday,
    bool isSkipSortie)
  {
    GameObject list = Object.Instantiate<GameObject>(prefab);
    list.transform.parent = ((Component) this.grid).transform;
    list.transform.localScale = Vector3.one;
    list.transform.localPosition = Vector3.zero;
    IEnumerator e = list.GetComponent<Quest00219List>().Init(extra, isClear, isNew, isClearedToday);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (!isSkipSortie)
      EventDelegate.Set(list.GetComponent<Quest00219List>().Dock.onClick, (EventDelegate.Callback) (() => this.ChangeScene00220(extra, list)));
    else
      EventDelegate.Set(list.GetComponent<Quest00219List>().Dock.onClick, (EventDelegate.Callback) (() => this.changeScene002201(extra, list)));
  }

  protected void ChangeScene00220(PlayerExtraQuestS extra, GameObject obj, bool Guerrilla = false)
  {
    this.StartCoroutine(this.QuestTimeCompare(extra, obj, (Action) (() =>
    {
      QuestExtraS questExtraS = extra.quest_extra_s;
      Quest00220Scene.ChangeScene00220(false, questExtraS.quest_l_QuestExtraL, questExtraS.quest_m_QuestExtraM, Guerrilla);
    })));
  }

  private void changeScene002201(PlayerExtraQuestS extra, GameObject obj)
  {
    this.StartCoroutine(this.QuestTimeCompare(extra, obj, (Action) (() =>
    {
      QuestExtraS questExtraS = extra.quest_extra_s;
      Quest002201Scene.changeScene(false, questExtraS.quest_l_QuestExtraL, questExtraS.quest_m_QuestExtraM);
    })));
  }

  public virtual void Foreground() => Debug.Log((object) "click default event Foreground");

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    if (this.headerInfo_ == null)
    {
      this.backScene();
    }
    else
    {
      QuestExtraLL questLl = this.headerInfo_.quest_ll;
      Singleton<NGSceneManager>.GetInstance().destroyCurrentScene();
      if (questLl != null)
        Quest00218Scene.backOrChangeScene(questLl.ID, new int?(this.focusSId_));
      else if (!this.isSideQuest)
        Quest00217Scene.backOrChangeScene(this.headerInfo_.category_QuestExtraCategory);
      else
        Quest002SideStoryScene.backOrChangeScene(this.headerInfo_.category_QuestExtraCategory);
      this.isSideQuest = false;
    }
  }

  public override void onBackButton() => this.IbtnBack();

  public virtual void IbtnEvent() => Debug.Log((object) "click default event IbtnEvent");

  public virtual void VScrollBar() => Debug.Log((object) "click default event VScrollBar");

  private IEnumerator QuestTimeCompare(
    PlayerExtraQuestS StageData,
    GameObject obj,
    Action actChangeScene)
  {
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (ServerTime.NowAppTime() < StageData.today_day_end_at)
    {
      actChangeScene();
    }
    else
    {
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Future<GameObject> time_popup = Res.Prefabs.popup.popup_002_23__anim_popup01.Load<GameObject>();
      e = time_popup.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject result = time_popup.Result;
      Singleton<PopupManager>.GetInstance().openAlert(result);
      time_popup = (Future<GameObject>) null;
    }
  }
}
