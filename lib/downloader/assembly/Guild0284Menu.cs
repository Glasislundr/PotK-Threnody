// Decompiled with JetBrains decompiler
// Type: Guild0284Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Guild0284Menu : BackButtonMenuBase
{
  private GuildDisplayEmblem[] emblems;
  private const int UNKNOWN_ID = 99999;
  private int curEmblemID;
  [SerializeField]
  private NGxScroll scroll;
  private GameObject scrollObj;
  private GameObject guild02841Popup;
  private GameObject guildTitleSortPopup;
  [SerializeField]
  private UIGrid grid;
  [SerializeField]
  private UIScrollView scrollview;
  [SerializeField]
  private UILabel txtTitle;
  [SerializeField]
  private UI2DSprite currentTitle;
  [SerializeField]
  private GameObject[] DisplayOrderSprites;

  public GameObject Guild02841Popup => this.guild02841Popup;

  public IEnumerator InitializeAsync()
  {
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    IEnumerator e = this.ResourceLoad();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.EmblemsUpdate();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (this.emblems == null)
    {
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      MypageScene.ChangeSceneOnError();
    }
    else
    {
      e = this.ListSorting();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = this.CreateSprite();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    }
  }

  public void Initialize()
  {
    this.txtTitle.SetTextLocalize(Consts.GetInstance().GUILD_28_4_MENU_TITLE);
  }

  public IEnumerator EndSceneAsync()
  {
    if (Persist.guildSetting.Exists)
    {
      IEnumerator e = ServerTime.WaitSync();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GuildUtil.setTimeTitleAppear(ServerTime.NowAppTime());
      GuildUtil.setBadgeState(GuildUtil.GuildBadgeInfoType.newTitle, false);
      Persist.guildSetting.Flush();
    }
  }

  private IEnumerator ResourceLoad()
  {
    Future<GameObject> fgObj;
    IEnumerator e;
    if (Object.op_Equality((Object) this.scrollObj, (Object) null))
    {
      fgObj = Res.Prefabs.guild028_4.guild_title_list.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.scrollObj = fgObj.Result;
      fgObj = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.guildTitleSortPopup, (Object) null))
    {
      fgObj = Res.Prefabs.popup.popup_028_guild_title_sort__anim_popup01.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.guildTitleSortPopup = fgObj.Result;
      fgObj = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.guild02841Popup, (Object) null))
    {
      fgObj = Res.Prefabs.popup.popup_028_4_1__anim_popup01.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.guild02841Popup = fgObj.Result;
      fgObj = (Future<GameObject>) null;
    }
  }

  private IEnumerator ScrollInit(GuildDisplayEmblem emblem)
  {
    Guild0284Menu baseMenu = this;
    GameObject gameObject = baseMenu.scrollObj.Clone(((Component) baseMenu.grid).transform);
    bool isNew = false;
    if (emblem.is_enabled && Persist.guildSetting.Exists)
    {
      DateTime timeTitleAppear = GuildUtil.getTimeTitleAppear();
      DateTime? createdAt = emblem.created_at;
      if ((createdAt.HasValue ? (timeTitleAppear < createdAt.GetValueOrDefault() ? 1 : 0) : 0) != 0)
        isNew = true;
    }
    IEnumerator e = gameObject.GetComponent<GuildTItleScroll>().Init(emblem.is_enabled, emblem.unit.ID, emblem.unit.rarity_GuildEmblemRarity, emblem.unit.description, emblem.in_use, isNew, emblem.is_enabled ? emblem.created_at : new DateTime?(), new Action(baseMenu.EmblemsScrollUpdate), baseMenu);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator SortPopup()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Guild0284Menu guild0284Menu = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    GameObject guildTitleSortPopup = guild0284Menu.guildTitleSortPopup;
    if (Object.op_Inequality((Object) guildTitleSortPopup.GetComponent<UIWidget>(), (Object) null))
      ((UIRect) guildTitleSortPopup.GetComponent<UIWidget>()).alpha = 0.0f;
    Singleton<PopupManager>.GetInstance().open(guildTitleSortPopup).GetComponent<Guild0284TitleSortPopup>().Init(new Action(guild0284Menu.ListSort));
    return false;
  }

  private IEnumerator EmblemsUpdate()
  {
    Future<WebAPI.Response.GuildEmblemIndex> emblemData = WebAPI.GuildEmblemIndex((Action<WebAPI.Response.UserError>) (e => WebAPI.DefaultUserErrorCallback(e)));
    IEnumerator e1 = emblemData.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (emblemData.Result == null)
    {
      this.emblems = (GuildDisplayEmblem[]) null;
    }
    else
    {
      this.emblems = ((IEnumerable<GuildDisplayEmblem>) emblemData.Result.emblems).Where<GuildDisplayEmblem>((Func<GuildDisplayEmblem, bool>) (x => x.unit != null)).ToArray<GuildDisplayEmblem>();
      IEnumerable<GuildDisplayEmblem> source = ((IEnumerable<GuildDisplayEmblem>) this.emblems).Where<GuildDisplayEmblem>((Func<GuildDisplayEmblem, bool>) (x => x.in_use));
      this.curEmblemID = source.Count<GuildDisplayEmblem>() <= 0 ? 99999 : source.First<GuildDisplayEmblem>().unit.ID;
    }
  }

  private void ListSort() => this.StartCoroutine(this.ListSorting());

  private void DisplayOrderSpriteSetting(int type)
  {
    ((IEnumerable<GameObject>) this.DisplayOrderSprites).ForEach<GameObject>((Action<GameObject>) (x => x.SetActive(false)));
    this.DisplayOrderSprites[type].SetActive(true);
  }

  private IEnumerator ListSorting()
  {
    foreach (Component component in ((Component) this.grid).transform)
      Object.Destroy((Object) component.gameObject);
    int titleSortCategory = GuildUtil.getTitleSortCategory();
    this.DisplayOrderSpriteSetting(titleSortCategory);
    switch (titleSortCategory)
    {
      case 0:
        this.emblems = ((IEnumerable<GuildDisplayEmblem>) this.emblems).OrderBy<GuildDisplayEmblem, int>((Func<GuildDisplayEmblem, int>) (x => x.unit.ID)).ToArray<GuildDisplayEmblem>();
        break;
      case 1:
        this.emblems = ((IEnumerable<GuildDisplayEmblem>) this.emblems).OrderByDescending<GuildDisplayEmblem, DateTime?>((Func<GuildDisplayEmblem, DateTime?>) (x => x.created_at)).ThenBy<GuildDisplayEmblem, int>((Func<GuildDisplayEmblem, int>) (x => x.unit.ID)).ToArray<GuildDisplayEmblem>();
        break;
      case 2:
        this.emblems = ((IEnumerable<GuildDisplayEmblem>) this.emblems).OrderByDescending<GuildDisplayEmblem, int>((Func<GuildDisplayEmblem, int>) (x => x.unit.rarity_GuildEmblemRarity)).ThenBy<GuildDisplayEmblem, int>((Func<GuildDisplayEmblem, int>) (x => x.unit.ID)).ToArray<GuildDisplayEmblem>();
        break;
    }
    GuildDisplayEmblem[] guildDisplayEmblemArray = this.emblems;
    for (int index = 0; index < guildDisplayEmblemArray.Length; ++index)
    {
      IEnumerator e = this.ScrollInit(guildDisplayEmblemArray[index]);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    guildDisplayEmblemArray = (GuildDisplayEmblem[]) null;
    ((Component) this.grid).gameObject.SetActive(true);
    this.grid.Reposition();
    this.scrollview.ResetPosition();
  }

  private IEnumerator CreateSprite()
  {
    Future<Sprite> sprF = EmblemUtility.LoadGuildEmblemSprite(this.curEmblemID);
    IEnumerator e = sprF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.currentTitle.sprite2D = sprF.Result;
  }

  private void EmblemsScrollUpdate() => this.StartCoroutine(this.InitializeAsync());

  public void onSortButton() => this.StartCoroutine(this.SortPopup());

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }
}
