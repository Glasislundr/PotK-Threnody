// Decompiled with JetBrains decompiler
// Type: Story00191Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Story00191Scene : NGSceneBase
{
  [SerializeField]
  private NGxScroll ScrollContainer;
  [SerializeField]
  private Story00191Menu menu;
  private bool flg_init = true;
  [SerializeField]
  private GameObject IbtnCopyRight;
  [SerializeField]
  private GameObject IbtnColabo;
  [SerializeField]
  private GameObject IbtnMigrate;
  [SerializeField]
  private GameObject IbtnHintsAndTips;
  [SerializeField]
  private GameObject AchievementsButton;
  [SerializeField]
  private UISprite AchievementsBtnAndroidSprite;

  public override IEnumerator onInitSceneAsync()
  {
    this.AchievementsButton.SetActive(false);
    this.IbtnHintsAndTips.SetActive(false);
    this.menu.showBtnCredit(false);
    this.menu.InitAsync();
    this.ScrollContainer.ResolvePosition();
    IEnumerator e = this.menu.onInitSceneAsync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void onStartScene()
  {
    if (this.flg_init)
    {
      this.ScrollContainer.ResolvePosition();
      this.flg_init = false;
    }
    this.menu.Init();
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }
}
