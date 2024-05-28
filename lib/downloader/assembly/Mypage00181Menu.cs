// Decompiled with JetBrains decompiler
// Type: Mypage00181Menu
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
public class Mypage00181Menu : BackButtonMenuBase
{
  private GameObject informationListPrefab;
  private GameObject informationListLPrefab;
  private GameObject informationListLImagePrefab;
  [SerializeField]
  private List<Mypage00181TabPage> tabPages = new List<Mypage00181TabPage>();
  public bool initFlag;

  public IEnumerator onInitMenuAsync()
  {
    Mypage00181Menu mypage00181Menu = this;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    mypage00181Menu.initFlag = true;
    IEnumerator e = mypage00181Menu.LoadPrefab();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    mypage00181Menu.Initialize();
    yield return (object) mypage00181Menu.StartCoroutine(mypage00181Menu.CreateTabScroll());
    mypage00181Menu.SetActiveTab(Mypage00181Menu.CategoryType.Notice);
    mypage00181Menu.initFlag = false;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  private IEnumerator LoadPrefab()
  {
    Future<GameObject> dir_Infomation_List = Res.Prefabs.mypage001_8_1.dir_Infomation_List.Load<GameObject>();
    IEnumerator e = dir_Infomation_List.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.informationListPrefab = dir_Infomation_List.Result;
    Future<GameObject> dir_Infomation_List_L = Res.Prefabs.mypage001_8_1.dir_Infomation_List_L.Load<GameObject>();
    e = dir_Infomation_List_L.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.informationListLPrefab = dir_Infomation_List_L.Result;
    Future<GameObject> dir_Infomation_List_L_Image_Only = Res.Prefabs.mypage001_8_1.dir_Infomation_List_L_Image_Only.Load<GameObject>();
    e = dir_Infomation_List_L_Image_Only.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.informationListLImagePrefab = dir_Infomation_List_L_Image_Only.Result;
  }

  private void Initialize() => this.InitTab();

  private IEnumerator CreateTabScroll()
  {
    Mypage00181Menu menu = this;
    foreach (Mypage00181TabPage tabPage in menu.tabPages)
    {
      Mypage00181TabPage tab = tabPage;
      OfficialInformationArticle[] array = ((IEnumerable<OfficialInformationArticle>) Singleton<NGGameDataManager>.GetInstance().officialInfos).Where<OfficialInformationArticle>((Func<OfficialInformationArticle, bool>) (x => x.category_id == tab.category_id)).ToArray<OfficialInformationArticle>();
      IEnumerator e = ((Component) tab).GetComponent<Mypage00181TabPage>().CreateScroll(array, (NGMenuBase) menu);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  private void InitTab()
  {
    foreach (Component tabPage in this.tabPages)
      tabPage.GetComponent<Mypage00181TabPage>().Init(this, this.informationListPrefab, this.informationListLPrefab, this.informationListLImagePrefab);
  }

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet() || Singleton<NGGameDataManager>.GetInstance().InfoOrLoginBonusJump())
      return;
    Singleton<NGSceneManager>.GetInstance().destoryNonStackScenes();
    Singleton<NGSceneManager>.GetInstance().destroyCurrentScene();
    if (Singleton<NGGameDataManager>.GetInstance().IsSea)
    {
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      Sea030HomeScene.ChangeScene(false);
    }
    else
      MypageScene.ChangeScene();
  }

  public override void onBackButton() => this.IbtnBack();

  public void OnPushPageTab(Mypage00181Menu.CategoryType category) => this.SetActiveTab(category);

  private void SetActiveTab(Mypage00181Menu.CategoryType category)
  {
    foreach (Mypage00181TabPage tabPage in this.tabPages)
    {
      if (tabPage.categoryType == category)
        tabPage.SetPageActive(true);
      else
        tabPage.SetPageActive(false);
    }
  }

  public IEnumerator onStartMenuAsync()
  {
    IEnumerator e = this.CheckNew();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator CheckNew()
  {
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    foreach (Mypage00181TabPage tabPage in this.tabPages)
    {
      GameObject tabBadge = tabPage.tabBadge;
      tabBadge.SetActive(false);
      foreach (GameObject gameObject in tabPage.scrollContainer.GetComponent<Mypage00181ScrollContainer>().Scroll.Arr)
      {
        Mypage00181ScrollParts component = gameObject.GetComponent<Mypage00181ScrollParts>();
        component.SetBadge(component.article, tabBadge);
      }
    }
  }

  public enum CategoryType
  {
    Notice = 1,
    GachaEvent = 2,
    Important = 3,
    Bug = 4,
  }
}
