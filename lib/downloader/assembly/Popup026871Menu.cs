// Decompiled with JetBrains decompiler
// Type: Popup026871Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class Popup026871Menu : BackButtonMonoBehaiviour
{
  [SerializeField]
  private UILabel txtTitle;
  [SerializeField]
  private UILabel txtDescription1;
  [SerializeField]
  private UILabel txtDescription2;

  public void Init(int battleCnt)
  {
    Consts instance = Consts.GetInstance();
    this.txtTitle.SetText(instance.VERSUS_0026871POPUP_TITLE);
    this.txtDescription1.SetText(instance.VERSUS_0026871POPUP_DESCRIPTION1);
    this.txtDescription2.SetText(Consts.Format(instance.VERSUS_0026871POPUP_DESCRIPTION2, (IDictionary) new Hashtable()
    {
      {
        (object) "cnt",
        (object) battleCnt.ToLocalizeNumberText()
      }
    }));
  }

  public void IbtnOK()
  {
    Singleton<PopupManager>.GetInstance().onDismiss();
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Singleton<NGGameDataManager>.GetInstance().StartCoroutine(this.SceneUpdate());
  }

  private IEnumerator SceneUpdate()
  {
    GameObject gameObject = GameObject.Find("Versus02610Versus02610Scene UI Root");
    if (Object.op_Inequality((Object) gameObject, (Object) null))
    {
      gameObject.GetComponent<Versus02610Menu>().StartSceneUpdate();
      yield break;
    }
  }

  public void IbtnNo() => this.IbtnOK();

  public override void onBackButton() => this.IbtnNo();
}
