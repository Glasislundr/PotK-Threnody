// Decompiled with JetBrains decompiler
// Type: PopupCommonOkTitle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class PopupCommonOkTitle : MonoBehaviour
{
  [SerializeField]
  private UILabel title;
  [SerializeField]
  private UILabel message;
  private Action yes;
  private Action no;
  private static string prefab_path = "Prefabs/popup_common_ok_title";

  public void OnYes()
  {
    this.yes();
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  public void OnNo()
  {
    this.no();
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  public static void Show(string title, string message, Action yes = null, Action no = null)
  {
    GameObject prefab = Resources.Load<GameObject>(PopupCommonOkTitle.prefab_path);
    PopupCommonOkTitle component = Singleton<PopupManager>.GetInstance().open(prefab).GetComponent<PopupCommonOkTitle>();
    component.title.SetText(title);
    component.message.SetText(message);
    component.yes = yes ?? new Action(PopupCommonOkTitle.defaultCallback);
    component.no = no ?? new Action(PopupCommonOkTitle.defaultCallback);
  }

  public static void defaultCallback() => Debug.Log((object) "popup close");
}
