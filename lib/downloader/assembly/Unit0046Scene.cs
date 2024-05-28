// Decompiled with JetBrains decompiler
// Type: Unit0046Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/Unit0046/Scene")]
public class Unit0046Scene : NGSceneBase
{
  public static bool isQuestEdit;
  public Unit0046Menu menu;
  private GameObject itemIcon;

  public static void changeScene(
    bool stack,
    QuestLimitationBase[] limitations = null,
    string limitedDescription = null,
    bool isFromMypage = false)
  {
    if (limitations != null && limitations.Length != 0)
    {
      if (Unit0046Scene.isSeaScene())
        Singleton<NGSceneManager>.GetInstance().changeScene("unit004_6_0822_sea", (stack ? 1 : 0) != 0, (object) new Unit0046Scene.LimitationData(limitations, limitedDescription), (object) (Unit0046Scene.From) (isFromMypage ? 3 : 0));
      else
        Singleton<NGSceneManager>.GetInstance().changeScene("unit004_6_0822", (stack ? 1 : 0) != 0, (object) new Unit0046Scene.LimitationData(limitations, limitedDescription), (object) Unit0046Scene.From.Normal);
    }
    else if (Unit0046Scene.isSeaScene())
      Singleton<NGSceneManager>.GetInstance().changeScene("unit004_6_0822_sea", (stack ? 1 : 0) != 0, (object) (Unit0046Scene.From) (isFromMypage ? 3 : 0));
    else
      Singleton<NGSceneManager>.GetInstance().changeScene("unit004_6_0822", (stack ? 1 : 0) != 0, (object) Unit0046Scene.From.Normal);
  }

  private static bool isSeaScene()
  {
    if ((Singleton<NGGameDataManager>.GetInstance().IsSea || Singleton<NGSceneManager>.GetInstance().IsSeaByChangeScene()) && !Singleton<NGGameDataManager>.GetInstance().QuestType.HasValue)
      return true;
    CommonQuestType? questType = Singleton<NGGameDataManager>.GetInstance().QuestType;
    CommonQuestType commonQuestType = CommonQuestType.Sea;
    return questType.GetValueOrDefault() == commonQuestType & questType.HasValue;
  }

  public static void changeScene(bool stack, ColosseumUtility.Info colosseumInfo)
  {
    if (Singleton<NGGameDataManager>.GetInstance().IsSea || Singleton<NGSceneManager>.GetInstance().IsSeaByChangeScene())
      Singleton<NGSceneManager>.GetInstance().changeScene("unit004_6_0822_sea", (stack ? 1 : 0) != 0, (object) colosseumInfo);
    else
      Singleton<NGSceneManager>.GetInstance().changeScene("unit004_6_0822", (stack ? 1 : 0) != 0, (object) colosseumInfo);
  }

  public static void changeSceneVersus(bool stack)
  {
    if (Singleton<NGGameDataManager>.GetInstance().IsSea)
      Singleton<NGSceneManager>.GetInstance().changeScene("unit004_6_0822_sea", (stack ? 1 : 0) != 0, (object) Unit0046Scene.From.Versus);
    else
      Singleton<NGSceneManager>.GetInstance().changeScene("unit004_6_0822", (stack ? 1 : 0) != 0, (object) Unit0046Scene.From.Versus);
  }

  public IEnumerator onStartSceneAsync(Unit0046Scene.From fromScene)
  {
    Unit0046Scene unit0046Scene = this;
    unit0046Scene.menu.fromScene = fromScene;
    if (fromScene == Unit0046Scene.From.Colosseum)
    {
      Singleton<CommonRoot>.GetInstance().SetFooterEnable(false);
      unit0046Scene.menu.isSea = false;
    }
    else
    {
      Unit0046Menu menu = unit0046Scene.menu;
      int num;
      if (!Singleton<NGGameDataManager>.GetInstance().IsSea || Singleton<NGGameDataManager>.GetInstance().QuestType.HasValue)
      {
        CommonQuestType? questType = Singleton<NGGameDataManager>.GetInstance().QuestType;
        CommonQuestType commonQuestType = CommonQuestType.Sea;
        num = questType.GetValueOrDefault() == commonQuestType & questType.HasValue ? 1 : 0;
      }
      else
        num = 1;
      menu.isSea = num != 0;
    }
    Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
    IEnumerator e;
    if (unit0046Scene.menu.isSea)
    {
      e = unit0046Scene.SetSeaBackground();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = unit0046Scene.SetSeaBgm();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    ((UIRect) unit0046Scene.menu.TopObject.GetComponent<UIWidget>()).alpha = 0.0f;
    ((UIRect) unit0046Scene.menu.MiddleObject.GetComponent<UIWidget>()).alpha = 0.0f;
    e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit0046Scene.menu.SevertTime = ServerTime.NowAppTime();
    PlayerDeck[] _playerDecks = unit0046Scene.menu.isSea ? PlayerSeaDeck.convertDeckData(SMManager.Get<PlayerSeaDeck[]>()) : SMManager.Get<PlayerDeck[]>();
    e = unit0046Scene.menu.InitPlayerDecks(_playerDecks);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    PlayerItem[] items = SMManager.Get<PlayerItem[]>();
    items = items.AllBattleSupplies();
    yield return (object) unit0046Scene.LoadItemIcon();
    e = unit0046Scene.menu.InitPlayerItems(items, unit0046Scene.itemIcon);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ((UIRect) unit0046Scene.menu.TopObject.GetComponent<UIWidget>()).alpha = 1f;
    ((UIRect) unit0046Scene.menu.MiddleObject.GetComponent<UIWidget>()).alpha = 1f;
    unit0046Scene.StartCoroutine(unit0046Scene.menu.Init3DModelPrefab());
    Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
  }

  public IEnumerator onStartSceneAsync(ColosseumUtility.Info colosseumInfo)
  {
    this.menu.info = colosseumInfo;
    IEnumerator e = this.onStartSceneAsync(Unit0046Scene.From.Colosseum);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(
    Unit0046Scene.LimitationData limitationData,
    Unit0046Scene.From fromScene)
  {
    this.menu.limitationData = limitationData;
    IEnumerator e = this.onStartSceneAsync(fromScene);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public override IEnumerator onEndSceneAsync()
  {
    this.menu.stop = true;
    while (this.menu.coroutine)
      yield return (object) null;
  }

  public void onStartScene(Unit0046Scene.From fromScene)
  {
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  public void onStartScene(ColosseumUtility.Info colosseumInfo)
  {
    this.onStartScene(Unit0046Scene.From.Colosseum);
  }

  public void onStartScene(
    Unit0046Scene.LimitationData limitationData,
    Unit0046Scene.From fromScene)
  {
    this.onStartScene(fromScene);
  }

  public override void onEndScene()
  {
    this.menu.onEndScene();
    this.menu.stop = true;
  }

  private IEnumerator SetSeaBackground()
  {
    Unit0046Scene unit0046Scene = this;
    Future<GameObject> bgF = new ResourceObject("Prefabs/BackGround/DefaultBackground_sea").Load<GameObject>();
    IEnumerator e = bgF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Inequality((Object) bgF.Result, (Object) null))
      unit0046Scene.backgroundPrefab = bgF.Result;
  }

  private IEnumerator SetSeaBgm()
  {
    Unit0046Scene unit0046Scene = this;
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    SeaHomeMap seaHomeMap = ((IEnumerable<SeaHomeMap>) MasterData.SeaHomeMapList).ActiveSeaHomeMap(ServerTime.NowAppTimeAddDelta());
    if (seaHomeMap != null && !string.IsNullOrEmpty(seaHomeMap.bgm_cuesheet_name) && !string.IsNullOrEmpty(seaHomeMap.bgm_cue_name))
    {
      unit0046Scene.bgmFile = seaHomeMap.bgm_cuesheet_name;
      unit0046Scene.bgmName = seaHomeMap.bgm_cue_name;
    }
  }

  private IEnumerator LoadItemIcon()
  {
    int num;
    if (!Singleton<NGGameDataManager>.GetInstance().IsSea || Singleton<NGGameDataManager>.GetInstance().QuestType.HasValue)
    {
      CommonQuestType? questType = Singleton<NGGameDataManager>.GetInstance().QuestType;
      CommonQuestType commonQuestType = CommonQuestType.Sea;
      num = questType.GetValueOrDefault() == commonQuestType & questType.HasValue ? 1 : 0;
    }
    else
      num = 1;
    Future<GameObject> fItemIcon = num == 0 ? new ResourceObject("Prefabs/ItemIcon/prefab").Load<GameObject>() : new ResourceObject("Prefabs/Sea/ItemIcon/prefab_sea").Load<GameObject>();
    IEnumerator e = fItemIcon.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.itemIcon = fItemIcon.Result;
  }

  public class LimitationData
  {
    public QuestLimitationBase[] limitations_ { get; private set; }

    public string description_ { get; private set; }

    public LimitationData(QuestLimitationBase[] limitations, string description)
    {
      this.limitations_ = limitations;
      this.description_ = description;
    }
  }

  public enum From
  {
    Normal,
    Versus,
    Colosseum,
    Mypage,
  }
}
