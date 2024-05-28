// Decompiled with JetBrains decompiler
// Type: PopupTitleBack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using UnityEngine;
using UnityEngine.SceneManagement;

#nullable disable
public class PopupTitleBack : BackButtonMonoBehaiviour
{
  [SerializeField]
  private UIButton ibtnYes;
  [SerializeField]
  private UIButton ibtnNo;
  [SerializeField]
  private UILabel title;
  [SerializeField]
  private UILabel description;

  public void Init()
  {
    if (Object.op_Inequality((Object) ((Component) this).GetComponent<UIWidget>(), (Object) null))
      ((UIRect) ((Component) this).GetComponent<UIWidget>()).alpha = 0.0f;
    Consts instance = Consts.GetInstance();
    this.title.SetText(instance.titleback_title);
    this.description.SetText(instance.titleback_text);
  }

  public void IbtnYes()
  {
    NGSceneManager instance = Singleton<NGSceneManager>.GetInstance();
    Singleton<NGGameDataManager>.GetInstance().refreshHomeHome = true;
    if (Object.op_Inequality((Object) instance, (Object) null))
      instance.RestartGame();
    else
      SceneManager.LoadScene("startup000_6");
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public void IbtnNo() => Singleton<PopupManager>.GetInstance().onDismiss();

  public override void onBackButton() => this.IbtnNo();
}
