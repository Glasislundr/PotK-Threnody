// Decompiled with JetBrains decompiler
// Type: Story0090Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using UnityEngine;

#nullable disable
public class Story0090Menu : BackButtonMenuBase
{
  [SerializeField]
  protected UILabel TxtTitle;
  [SerializeField]
  private UIButton lbtnMainStory_LostRagnarok;
  [SerializeField]
  private UIButton ibtnMainStory;
  [SerializeField]
  private UIButton ibtnCharaStory;
  [SerializeField]
  private UIButton ibtnEventStory;
  [SerializeField]
  private UIButton ibtnCombiStory;
  [SerializeField]
  private UIScrollView scroll;
  [SerializeField]
  private UIGrid grid;

  public virtual void Foreground()
  {
  }

  public virtual void VScrollBar()
  {
  }

  public void Init()
  {
    ((Component) ((Component) this.ibtnCombiStory).transform.parent).gameObject.SetActive(Player.Current.IsCombiQuest());
    this.grid.Reposition();
    if (!Player.Current.IsCombiQuest())
      return;
    this.scroll.ResetPosition();
    this.scroll.UpdateScrollbars(true);
  }

  public override void onBackButton() => this.IbtnBack();

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().clearStack();
    Singleton<NGSceneManager>.GetInstance().changeScene("guide011_1", false);
  }

  public virtual void IbtnMainStory_EverAfter()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().changeScene("story009_1", false, (object) 7);
  }

  public virtual void IbtnMainStory_IntegralNoah()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().changeScene("story009_1", false, (object) 6);
  }

  public virtual void IbtnMainStory_LostRagnarok()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().changeScene("story009_1", false, (object) 4);
  }

  public virtual void IbtnMainstory()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().changeScene("story009_1", false, (object) 1);
  }

  public virtual void IbtnSeastory()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().changeScene("story009_13", false);
  }

  public void IbtnCharacterstory()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().changeScene("story009_5", false, (object) true);
  }

  public void IbtnEventstory()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().changeScene("story009_8", false, (object) true);
  }

  public void IbtnMovieLibrary()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().changeScene("story009_9", false);
  }

  public void IbtnCombi()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().changeScene("story009_10", false);
  }
}
