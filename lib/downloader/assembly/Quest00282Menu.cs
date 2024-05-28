// Decompiled with JetBrains decompiler
// Type: Quest00282Menu
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
public class Quest00282Menu : BackButtonMenuBase
{
  public UIButton[] elementBtn;
  public NGxScroll scroll;
  private GameObject FriendListPrefab;
  private Dictionary<Helper, int> sortFriends;
  private CommonElement currentElement = CommonElement.none;
  private Dictionary<CommonElement, List<Helper>> elementDic = new Dictionary<CommonElement, List<Helper>>();
  private PlayerStoryQuestS story_quest;
  private PlayerExtraQuestS extra_quest;
  private PlayerCharacterQuestS char_quest;
  private PlayerQuestSConverter harmony_quest;
  private PlayerSeaQuestS sea_quest;
  private bool enabledDetail = true;
  private bool backSceneSelected = true;
  private Action<PlayerHelper> onSetHelper_;
  private int element_index;
  private List<Quest00282FriendManager> friendManagerList = new List<Quest00282FriendManager>();
  private static CommonElement lastSelectElement = CommonElement.none;
  private static int lastSelectIndex = 0;

  public void setEventSetHelper(Action<PlayerHelper> eventSetHelper)
  {
    this.onSetHelper_ = eventSetHelper;
  }

  private void SetElementBtnState(int index)
  {
    if (this.elementBtn == null)
      return;
    this.element_index = index;
    for (int index1 = 0; index1 < this.elementBtn.Length; ++index1)
    {
      SpreadColorButton component = ((Component) this.elementBtn[index1]).gameObject.GetComponent<SpreadColorButton>();
      if (index1 != index)
      {
        component.SetColor(Color.gray);
        ((UIWidget) ((Component) this.elementBtn[index1]).gameObject.GetComponentInChildren<UITexture>(true)).color = Color.gray;
      }
      else
      {
        component.SetColor(Color.white);
        ((UIWidget) ((Component) this.elementBtn[index1]).gameObject.GetComponentInChildren<UITexture>(true)).color = Color.white;
      }
    }
  }

  public void SetEnableOtherElementBtn(bool isEnabled)
  {
    if (this.elementBtn == null)
      return;
    for (int index = 0; index < this.elementBtn.Length; ++index)
    {
      if (this.element_index != index)
        ((UIButtonColor) this.elementBtn[index]).isEnabled = isEnabled;
    }
  }

  public void SetEnableFriendBtn(bool isEnabled)
  {
    foreach (Quest00282FriendManager friendManager in this.friendManagerList)
      friendManager.SetEnableButton(isEnabled);
  }

  private IEnumerator OnClickElementBtn(CommonElement type, int index)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Quest00282Menu quest00282Menu = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    if (quest00282Menu.currentElement == type)
      return false;
    quest00282Menu.StartCoroutine(quest00282Menu.ChangeElement(type, index));
    return false;
  }

  private IEnumerator ChangeElement(CommonElement type, int index)
  {
    Quest00282Menu quest00282Menu = this;
    quest00282Menu.currentElement = type;
    quest00282Menu.SetElementBtnState(index);
    quest00282Menu.SetEnableOtherElementBtn(false);
    quest00282Menu.SetEnableFriendBtn(false);
    ((Behaviour) ((Component) quest00282Menu.scroll.scrollView).gameObject.GetComponent<UIPanel>()).enabled = false;
    quest00282Menu.scroll.Clear();
    quest00282Menu.friendManagerList.Clear();
    IEnumerator coroutine;
    if (quest00282Menu.currentElement != CommonElement.none)
    {
      coroutine = quest00282Menu.GetCurrentElementFriendList(quest00282Menu.currentElement);
      yield return (object) quest00282Menu.StartCoroutine(coroutine);
      Dictionary<Helper, int> current = (Dictionary<Helper, int>) coroutine.Current;
      quest00282Menu.StartCoroutine(quest00282Menu.InitFriendList(current));
      coroutine = (IEnumerator) null;
    }
    else if (quest00282Menu.sortFriends != null)
    {
      quest00282Menu.StartCoroutine(quest00282Menu.InitFriendList(quest00282Menu.sortFriends));
    }
    else
    {
      coroutine = quest00282Menu.RefreshFavoriteMember();
      while (coroutine.MoveNext())
        yield return coroutine.Current;
      coroutine = (IEnumerator) null;
      quest00282Menu.StartCoroutine(quest00282Menu.InitFriendList(quest00282Menu.sortFriends));
    }
    if (!Singleton<NGGameDataManager>.GetInstance().IsSea)
    {
      Quest00282Menu.lastSelectElement = type;
      Quest00282Menu.lastSelectIndex = index;
    }
  }

  public IEnumerator InitPlayerDecks(
    PlayerStoryQuestS story_quest,
    PlayerExtraQuestS extra_quest,
    PlayerCharacterQuestS char_quest,
    PlayerQuestSConverter harmony_quest,
    PlayerSeaQuestS sea_quest,
    bool enabledDetail = true,
    bool backSceneSelected = true)
  {
    Quest00282Menu quest00282Menu = this;
    quest00282Menu.scroll.Clear();
    if (!Singleton<NGGameDataManager>.GetInstance().IsSea)
    {
      for (int index = 0; index < quest00282Menu.elementBtn.Length; ++index)
        ((Component) quest00282Menu.elementBtn[index]).gameObject.SetActive(true);
      // ISSUE: reference to a compiler-generated method
      EventDelegate.Set(quest00282Menu.elementBtn[0].onClick, new EventDelegate.Callback(quest00282Menu.\u003CInitPlayerDecks\u003Eb__24_0));
      // ISSUE: reference to a compiler-generated method
      EventDelegate.Set(quest00282Menu.elementBtn[1].onClick, new EventDelegate.Callback(quest00282Menu.\u003CInitPlayerDecks\u003Eb__24_1));
      // ISSUE: reference to a compiler-generated method
      EventDelegate.Set(quest00282Menu.elementBtn[2].onClick, new EventDelegate.Callback(quest00282Menu.\u003CInitPlayerDecks\u003Eb__24_2));
      // ISSUE: reference to a compiler-generated method
      EventDelegate.Set(quest00282Menu.elementBtn[3].onClick, new EventDelegate.Callback(quest00282Menu.\u003CInitPlayerDecks\u003Eb__24_3));
      // ISSUE: reference to a compiler-generated method
      EventDelegate.Set(quest00282Menu.elementBtn[4].onClick, new EventDelegate.Callback(quest00282Menu.\u003CInitPlayerDecks\u003Eb__24_4));
      // ISSUE: reference to a compiler-generated method
      EventDelegate.Set(quest00282Menu.elementBtn[5].onClick, new EventDelegate.Callback(quest00282Menu.\u003CInitPlayerDecks\u003Eb__24_5));
      // ISSUE: reference to a compiler-generated method
      EventDelegate.Set(quest00282Menu.elementBtn[6].onClick, new EventDelegate.Callback(quest00282Menu.\u003CInitPlayerDecks\u003Eb__24_6));
    }
    else if (quest00282Menu.elementBtn != null)
    {
      for (int index = 0; index < quest00282Menu.elementBtn.Length; ++index)
        ((Component) quest00282Menu.elementBtn[index]).gameObject.SetActive(false);
    }
    Future<GameObject> prefabF = Singleton<NGGameDataManager>.GetInstance().IsSea ? Res.Prefabs.quest002_8_2_sea.list_sea.Load<GameObject>() : Res.Prefabs.quest002_8_2.list.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    quest00282Menu.FriendListPrefab = prefabF.Result;
    quest00282Menu.story_quest = story_quest;
    quest00282Menu.extra_quest = extra_quest;
    quest00282Menu.char_quest = char_quest;
    quest00282Menu.harmony_quest = harmony_quest;
    quest00282Menu.sea_quest = sea_quest;
    quest00282Menu.enabledDetail = enabledDetail;
    quest00282Menu.backSceneSelected = backSceneSelected;
    if (Singleton<NGGameDataManager>.GetInstance().IsSea)
      quest00282Menu.StartCoroutine(quest00282Menu.ChangeElement(CommonElement.none, 0));
    else
      quest00282Menu.StartCoroutine(quest00282Menu.ChangeElement(Quest00282Menu.lastSelectElement, Quest00282Menu.lastSelectIndex));
  }

  private IEnumerator RefreshFavoriteMember()
  {
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1, true);
    List<Helper> helper2 = new List<Helper>();
    IEnumerator e1;
    if (Singleton<NGGameDataManager>.GetInstance().IsSea)
    {
      ((IEnumerable<SeaPlayerHelper>) SMManager.Get<SeaPlayerHelper[]>()).ToList<SeaPlayerHelper>().ForEach((Action<SeaPlayerHelper>) (x => helper2.Add(new Helper(x))));
    }
    else
    {
      Future<WebAPI.Response.PlayerHelpers> helpers2 = WebAPI.PlayerHelpers(0, (Action<WebAPI.Response.UserError>) (e => WebAPI.DefaultUserErrorCallback(e)));
      e1 = helpers2.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (helpers2.Result == null)
        Debug.LogError((object) "helpers.Result is null");
      ((IEnumerable<PlayerHelper>) SMManager.Get<PlayerHelper[]>()).ToList<PlayerHelper>().ForEach((Action<PlayerHelper>) (x => helper2.Add(new Helper(x))));
      helpers2 = (Future<WebAPI.Response.PlayerHelpers>) null;
    }
    Helper[] helpers = helper2.ToArray();
    e1 = ServerTime.WaitSync();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    DateTime dateTime = ServerTime.NowAppTime();
    Dictionary<Helper, int> dictionary = new Dictionary<Helper, int>();
    this.sortFriends = (Dictionary<Helper, int>) null;
    foreach (Helper key in helpers)
    {
      TimeSpan timeSpan = dateTime - key.target_player_last_signed_in_at;
      dictionary.Add(key, (int) timeSpan.TotalSeconds);
    }
    if (this.existGuildMember(dictionary))
    {
      this.sortFriends = dictionary;
    }
    else
    {
      string[] favoriteFriends = Singleton<NGGameDataManager>.GetInstance().favoriteFriends;
      this.sortFriends = dictionary.OrderByDescending<KeyValuePair<Helper, int>, bool>((Func<KeyValuePair<Helper, int>, bool>) (x => x.Key.is_friend && ((IEnumerable<string>) favoriteFriends).Contains<string>(x.Key.target_player_id))).ThenByDescending<KeyValuePair<Helper, int>, bool>((Func<KeyValuePair<Helper, int>, bool>) (x => x.Key.is_friend)).ThenByDescending<KeyValuePair<Helper, int>, int>((Func<KeyValuePair<Helper, int>, int>) (x => x.Key.leader_unit_level)).ThenByDescending<KeyValuePair<Helper, int>, int>((Func<KeyValuePair<Helper, int>, int>) (x => x.Key.level)).ThenBy<KeyValuePair<Helper, int>, int>((Func<KeyValuePair<Helper, int>, int>) (x => x.Value)).ToDictionary<KeyValuePair<Helper, int>, Helper, int>((Func<KeyValuePair<Helper, int>, Helper>) (x => x.Key), (Func<KeyValuePair<Helper, int>, int>) (x => x.Value));
    }
  }

  private IEnumerator RefreshHelperData(CommonElement element)
  {
    Future<WebAPI.Response.PlayerHelpers> helpers = WebAPI.PlayerHelpers((int) element, (Action<WebAPI.Response.UserError>) (e => WebAPI.DefaultUserErrorCallback(e)));
    IEnumerator e1 = helpers.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (helpers.Result == null)
      Debug.LogError((object) "helpers.Result is null");
    List<Helper> helperList = new List<Helper>();
    for (int index = 0; index < helpers.Result.player_helpers.Length; ++index)
      helperList.Add(new Helper(helpers.Result.player_helpers[index]));
    this.elementDic.Add(element, helperList);
  }

  private IEnumerator GetCurrentElementFriendList(CommonElement element)
  {
    Quest00282Menu quest00282Menu = this;
    if (!quest00282Menu.elementDic.ContainsKey(element))
    {
      Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1, true);
      yield return (object) quest00282Menu.StartCoroutine(quest00282Menu.RefreshHelperData(element));
    }
    Dictionary<Helper, int> currentElementFriend = new Dictionary<Helper, int>();
    if (quest00282Menu.elementDic.ContainsKey(element))
    {
      DateTime dateTime = ServerTime.NowAppTime();
      foreach (Helper key in quest00282Menu.elementDic[element])
        currentElementFriend.Add(key, (int) (dateTime - key.target_player_last_signed_in_at).TotalSeconds);
    }
    yield return (object) currentElementFriend;
  }

  private IEnumerator InitFriendList(Dictionary<Helper, int> sortList)
  {
    Quest00282Menu menu = this;
    foreach (KeyValuePair<Helper, int> sort in sortList)
    {
      GameObject gameObject = Object.Instantiate<GameObject>(menu.FriendListPrefab);
      menu.scroll.Add(gameObject);
      PlayerUnit byUnitunit = PlayerUnit.create_by_unitunit(sort.Key);
      sort.Key.leader_unit = byUnitunit;
      Quest00282FriendManager friendManager = gameObject.GetComponent<Quest00282FriendManager>();
      yield return (object) menu.StartCoroutine(friendManager.InitFriendList(menu, sort.Key, menu.story_quest, menu.extra_quest, menu.char_quest, menu.harmony_quest, menu.sea_quest, ServerTime.NowAppTime(), menu.onSetHelper_, menu.enabledDetail, menu.backSceneSelected));
      menu.friendManagerList.Add(friendManager);
      friendManager = (Quest00282FriendManager) null;
    }
    GameObject gameObject1 = Object.Instantiate<GameObject>(menu.FriendListPrefab);
    menu.scroll.Add(gameObject1);
    Quest00282FriendManager component = gameObject1.GetComponent<Quest00282FriendManager>();
    component.FriendNone(menu, menu.story_quest, menu.extra_quest, menu.char_quest, menu.harmony_quest, menu.sea_quest, menu.onSetHelper_, menu.backSceneSelected);
    menu.friendManagerList.Add(component);
    menu.scroll.grid.Reposition();
    menu.scroll.scrollView.ResetPosition();
    ((Behaviour) ((Component) menu.scroll.scrollView).gameObject.GetComponent<UIPanel>()).enabled = true;
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
    menu.SetEnableOtherElementBtn(true);
    menu.SetEnableFriendBtn(true);
  }

  private bool existGuildMember(Dictionary<Helper, int> dic)
  {
    return !dic.Where<KeyValuePair<Helper, int>>((Func<KeyValuePair<Helper, int>, bool>) (x => x.Key.is_guild_member)).FirstOrDefault<KeyValuePair<Helper, int>>().Equals((object) new KeyValuePair<Helper, int>());
  }

  public virtual void Foreground() => Debug.Log((object) "click default event Foreground");

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  public override void onBackButton() => this.IbtnBack();

  public virtual void IbtnDecide() => Debug.Log((object) "click default event IbtnDecide");

  public virtual void VScrollBar() => Debug.Log((object) "click default event VScrollBar");

  public IEnumerator onEndScene()
  {
    this.elementDic.Clear();
    yield return (object) null;
  }
}
