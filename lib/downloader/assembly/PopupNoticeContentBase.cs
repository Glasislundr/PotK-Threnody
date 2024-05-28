// Decompiled with JetBrains decompiler
// Type: PopupNoticeContentBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public abstract class PopupNoticeContentBase : MonoBehaviour
{
  public abstract IEnumerator LoadContent(OfficialInfoPopupSchema contentData);

  public abstract void Refresh();
}
