// Decompiled with JetBrains decompiler
// Type: MovieInputController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
[RequireComponent(typeof (RawImage), typeof (CriManaMovieControllerForUI))]
public class MovieInputController : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
  [SerializeField]
  private WindowsMovieController movieController;
  [HideInInspector]
  public bool enableSkip = true;

  public void OnPointerClick(PointerEventData ped)
  {
    if (!this.enableSkip)
      return;
    this.movieController.StopMovie();
  }
}
