// Decompiled with JetBrains decompiler
// Type: Startup00012Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Startup00012Menu : BackButtonMenuBase
{
  [SerializeField]
  protected UILabel TxtTitle;
  [SerializeField]
  private NGxScrollMasonry scrollMasonry;
  [SerializeField]
  private List<GameObject> categoryList;
  public UIPanel panel;
  public UIWidget widget;
  public UITextList textList;
  public GameObject scrollViewObj;
  public GameObject logContainer;
  public GameObject newSpriteObj;
  public int oldDay = -7;
  private UIScrollView scrollView;
  private UILabel label;
  private List<GameObject> cloneList = new List<GameObject>();
  public GameObject scrollBar;
  public bool isContinue = true;
  public bool unRead;
  private bool isScroll;
  [SerializeField]
  private string nextSceneName;
  [SerializeField]
  private UIButton backButton;
  [SerializeField]
  private UILabel StartAtDisplay;
  private string strDate = "   {0}/{1} {2}:{3}";
  private string title;

  protected virtual int GetWidth() => 532;

  public List<GameObject> CategoryList
  {
    get => this.categoryList;
    set => this.categoryList = value;
  }

  public string NextSceneName
  {
    get => this.nextSceneName;
    set => this.nextSceneName = value;
  }

  protected virtual bool DeleteTitle() => false;

  public virtual void IbtnClose()
  {
    if (this.IsPushAndSet() || Singleton<NGGameDataManager>.GetInstance().InfoOrLoginBonusJump())
      return;
    MypageScene.ChangeScene();
  }

  public virtual void IbtnList()
  {
    if (this.IsPushAndSet())
      return;
    this.EndScene();
    Singleton<NGSceneManager>.GetInstance().changeScene(this.NextSceneName, true);
  }

  public IEnumerator InitAsync()
  {
    foreach (GameObject clone in this.cloneList)
    {
      UILabel component1 = clone.GetComponent<UILabel>();
      if (Object.op_Inequality((Object) component1, (Object) null))
        ((UIWidget) component1).SetDirty();
      UI2DSprite component2 = clone.GetComponent<UI2DSprite>();
      if (Object.op_Inequality((Object) component2, (Object) null))
        ((UIWidget) component2).SetDirty();
      clone.SetActive(true);
    }
    ((Behaviour) this.scrollView).enabled = true;
    this.scrollBar.SetActive(true);
    this.scrollView.ResetPosition();
    yield return (object) new WaitForSeconds(0.1f);
    if (this.isScroll)
    {
      ((Behaviour) this.scrollView).enabled = false;
      this.scrollBar.SetActive(false);
    }
    else
    {
      ((Behaviour) this.scrollView).enabled = true;
      this.scrollBar.SetActive(true);
    }
  }

  private void EndScene()
  {
    foreach (Component child in ((Component) this.scrollMasonry.Scroll).transform.GetChildren())
      Object.Destroy((Object) child.gameObject);
    this.scrollMasonry.Reset();
  }

  public IEnumerator InitSceneAsync(OfficialInformationArticle info)
  {
    this.EndScene();
    this.scrollView = this.scrollViewObj.GetComponentInChildren<UIScrollView>();
    this.unRead = false;
    try
    {
      this.unRead = Persist.infoUnRead.Data.GetUnRead(info);
    }
    catch
    {
      Persist.infoUnRead.Delete();
    }
    if (Object.op_Inequality((Object) this.newSpriteObj, (Object) null))
      this.newSpriteObj.SetActive(!this.unRead);
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Persist.infoUnRead.Data.SetUnRead(info, ServerTime.NowAppTime());
    Persist.infoUnRead.Flush();
    switch (info.category_id)
    {
      case 1:
        if (this.CategoryList != null)
        {
          this.CategoryList.ToggleOnce(0);
          break;
        }
        break;
      case 2:
        if (this.CategoryList != null)
        {
          this.CategoryList.ToggleOnce(1);
          break;
        }
        break;
      case 3:
        if (this.CategoryList != null)
        {
          this.CategoryList.ToggleOnce(2);
          break;
        }
        break;
      case 4:
        if (this.CategoryList != null)
        {
          this.CategoryList.ToggleOnce(3);
          break;
        }
        break;
    }
    this.StartAtDisplay.SetTextLocalize(string.Format(this.strDate, (object) string.Format("{0:D2}", (object) info.published_at.Month), (object) string.Format("{0:D2}", (object) info.published_at.Day), (object) string.Format("{0:D2}", (object) info.published_at.Hour), (object) string.Format("{0:D2}", (object) info.published_at.Minute)));
    if (!this.DeleteTitle())
    {
      switch (info.category_id)
      {
        case 1:
          this.title = Consts.GetInstance().INFO_CATEGORY1;
          break;
        case 2:
          this.title = Consts.GetInstance().INFO_CATEGORY2;
          break;
        case 3:
          this.title = Consts.GetInstance().INFO_CATEGORY3;
          break;
        case 4:
          this.title = Consts.GetInstance().INFO_CATEGORY4;
          break;
      }
    }
    e = DetailController.Init(this.scrollMasonry, this.title, info.bodies);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void ChangeDirectBackSceneMode()
  {
    this.backButton.onClick = new List<EventDelegate>()
    {
      new EventDelegate((MonoBehaviour) this, "onBackButton")
    };
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    this.EndScene();
    this.backScene();
  }
}
