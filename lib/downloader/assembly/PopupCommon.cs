// Decompiled with JetBrains decompiler
// Type: PopupCommon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class PopupCommon : BackButtonMonoBehaiviour
{
  [SerializeField]
  private UILabel title;
  [SerializeField]
  private UILabel message;
  public UIButton OK;
  private Action callback;
  public const string common_prefab_path = "Prefabs/popup_common";

  public void OnOk()
  {
    if (this.callback != null)
      this.callback();
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  public void IbtnNo() => this.OnOk();

  public override void onBackButton() => this.IbtnNo();

  public static IEnumerator ShowUserPolicyDissent(
    string title,
    string message,
    Transform parent,
    Action callback = null)
  {
    GameObject self = PopupCommon.LoadPrefab();
    callback = callback ?? new Action(PopupCommon.defaultCallback);
    GameObject obj = self.Clone(parent);
    UIWidget component1 = obj.GetComponent<UIWidget>();
    UIButton component2 = ((Component) obj.transform.Find("dir_button/ibtn_Popup_Ok")).gameObject.GetComponent<UIButton>();
    component1.depth = 200;
    obj.SetActive(false);
    obj.SetActive(true);
    PopupCommon component3 = obj.GetComponent<PopupCommon>();
    component3.Init(title, message, callback ?? new Action(PopupCommon.defaultCallback));
    ((Behaviour) component3).enabled = false;
    component2.onClick.Clear();
    EventDelegate.Set(component2.onClick, (EventDelegate.Callback) (() =>
    {
      if (callback != null)
        callback();
      Object.Destroy((Object) obj);
    }));
    yield break;
  }

  public static GameObject LoadPrefab() => Resources.Load<GameObject>("Prefabs/popup_common");

  public void Init(string title, string message, Action callback = null)
  {
    this.title.SetText(title);
    this.message.SetText(message);
    this.callback = callback;
  }

  public static IEnumerator Show(string title, string message, Action callback = null)
  {
    if (!Object.op_Equality((Object) Singleton<PopupManager>.GetInstance(), (Object) null))
    {
      GameObject prefab = PopupCommon.LoadPrefab();
      Singleton<PopupManager>.GetInstance().open(prefab).GetComponent<PopupCommon>().Init(title, message, callback ?? new Action(PopupCommon.defaultCallback));
      yield break;
    }
  }

  public static void defaultCallback() => Debug.Log((object) "popup close");
}
