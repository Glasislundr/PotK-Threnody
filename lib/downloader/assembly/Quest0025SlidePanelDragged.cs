// Decompiled with JetBrains decompiler
// Type: Quest0025SlidePanelDragged
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Quest0025SlidePanelDragged : MonoBehaviour
{
  [SerializeField]
  private UISprite slcPressedEffect;
  public Quest0025CircularMotionSet drag;
  public UISprite SlcOpen;
  private Vector2 defaultsize;
  private const float FULLOPEN = 20f;
  private const float OPENSTART = 45f;
  public int Lid;
  public bool Release;
  public bool jogPlayed = true;
  private BGChange bgChange;
  private Quest00240723Menu.StoryMode storyMode = Quest00240723Menu.StoryMode.None;
  private bool isChangingScene;

  public void EnableSlcPressedEffect()
  {
    if (!this.Release)
      return;
    ((Component) this.slcPressedEffect).gameObject.SetActive(true);
  }

  public void DisableSlcPressedEffect()
  {
    ((Component) this.slcPressedEffect).gameObject.SetActive(false);
  }

  private void OnPress(bool isDown)
  {
    if (isDown)
      this.drag.onPress();
    else
      this.drag.onRelease();
  }

  private void Start()
  {
    if (Object.op_Inequality((Object) this.SlcOpen, (Object) null))
      this.defaultsize = ((UIWidget) this.SlcOpen).localSize;
    if (this.Lid >= 36)
      this.storyMode = Quest00240723Menu.StoryMode.IntegralNoah;
    else if (this.Lid >= 19)
      this.storyMode = Quest00240723Menu.StoryMode.LostRagnarok;
    else
      this.storyMode = Quest00240723Menu.StoryMode.Heaven;
  }

  public void Initialize(BGChange bgChange)
  {
    this.bgChange = bgChange;
    this.isChangingScene = false;
  }

  public void PanelEffect(float panelY)
  {
    float num1 = (float) (((double) Mathf.Abs(Mathf.Abs(((Component) this).transform.localPosition.y) - Mathf.Abs(panelY)) - 20.0) / 25.0);
    float num2 = 1f - ((double) num1 > 1.0 ? 1f : ((double) num1 < 0.0 ? 0.0f : num1));
    this.Fade((float) ((double) num2 / 2.0 + 0.5));
    this.Expand((float) ((double) num2 / 2.0 + 0.5));
  }

  private void Fade(float rato)
  {
    if ((double) rato <= 0.5)
      rato = 0.0f;
    Color color = ((UIWidget) this.SlcOpen).color;
    color.a = rato;
    ((UIWidget) this.SlcOpen).color = color;
  }

  private void Expand(float rato)
  {
    GameObject gameObject = ((Component) this.SlcOpen).gameObject;
    if (Object.op_Equality((Object) gameObject, (Object) null))
      return;
    Vector3 vector3;
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3).\u002Ector(rato, rato, 1f);
    gameObject.transform.localScale = vector3;
  }

  public void ChangeScene()
  {
    if (this.isChangingScene)
      return;
    this.StartCoroutine(this.execChangeScene());
  }

  private IEnumerator execChangeScene()
  {
    Quest0025SlidePanelDragged slidePanelDragged = this;
    slidePanelDragged.isChangingScene = true;
    IEnumerator e = slidePanelDragged.bgChange.BGprefabCreateForQuest0025(slidePanelDragged.Lid, slidePanelDragged.storyMode);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().getBackgroundComponent<QuestBG>().CloudAnim(false);
    slidePanelDragged.StartCoroutine(slidePanelDragged.drag.bgchange.asyncBgAnim(QuestBG.AnimApply.MyPage, 1f));
    Quest00240723Scene.ChangeScene0024(false, slidePanelDragged.Lid, slidePanelDragged.drag.Hard, false);
  }

  public Vector3 GetSpriteObjectScale()
  {
    if (!this.Release)
      return Vector3.zero;
    GameObject gameObject = ((Component) this.SlcOpen).gameObject;
    return Object.op_Equality((Object) gameObject, (Object) null) ? Vector3.zero : gameObject.transform.localScale;
  }
}
