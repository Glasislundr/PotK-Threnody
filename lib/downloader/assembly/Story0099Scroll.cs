// Decompiled with JetBrains decompiler
// Type: Story0099Scroll
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Story0099Scroll : MonoBehaviour
{
  public Story0099Scroll.MovieType movieType;
  public Story0099Menu menu;
  [SerializeField]
  private UILabel TxtChapter;
  private int movieID;

  public void IbtnMoviePlay()
  {
    if (!this.menu.onStartMovie())
      return;
    switch (this.movieType)
    {
      case Story0099Scroll.MovieType.OPENING:
        this.StartCoroutine(this.menu.PlayOpeningMovie());
        break;
      case Story0099Scroll.MovieType.TUTORIAL1:
        this.StartCoroutine(this.menu.PlayTutorialMovie1());
        break;
      case Story0099Scroll.MovieType.TUTORIAL2:
        this.StartCoroutine(this.menu.PlayTutorialMovie2());
        break;
      case Story0099Scroll.MovieType.STORY:
        this.StartCoroutine(this.menu.PlayStoryQuestMovie(this.movieID));
        break;
    }
  }

  public void Init(Story0099Menu menu, string title, Story0099Scroll.MovieType type, int movieID)
  {
    this.menu = menu;
    this.movieType = type;
    this.movieID = movieID;
    this.TxtChapter.SetTextLocalize(title);
  }

  public enum MovieType
  {
    OPENING,
    TUTORIAL1,
    TUTORIAL2,
    STORY,
  }
}
