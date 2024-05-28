// Decompiled with JetBrains decompiler
// Type: QuestBG
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class QuestBG : MonoBehaviour
{
  public GameObject current;
  public UITweener tweener;
  public GameObject[] states;
  public GameObject current_xl;
  public GameObject current_l;
  public ParticleSystem cloudanimRev;
  public ParticleSystem cloudanimFow;
  public float blackTweenTime = 1.8f;
  public bool bgChange;
  public int bgnum = 1;
  public string bgName = "";
  private int activeIndex;
  public QuestBG.QuestPosition currentPos;
  public string namePrefab;

  private void Awake()
  {
    if (Object.op_Inequality((Object) this.current_xl, (Object) null))
    {
      this.Current = this.current_xl;
      this.changeActive(true);
    }
    else
    {
      this.current = this.current_l;
      this.changeActive(false);
    }
    this.Toggle(QuestBG.AnimApply.LMainPostion, 0.0f);
  }

  public GameObject Current
  {
    get => this.current;
    set => this.current = value;
  }

  public bool DiffStageLId
  {
    get
    {
      int lId = Persist.lastsortie.Data.l_id;
      int result = 0;
      if (int.TryParse(this.namePrefab, out result))
        return lId != result;
      Debug.LogWarning((object) "クエスト背景PrefabのnamePrefabに数字が入ってない");
      return true;
    }
  }

  public void changeActive(bool xl_isActive)
  {
    if (Object.op_Equality((Object) this.current_xl, (Object) null) || Object.op_Equality((Object) this.current_l, (Object) null))
      return;
    UI2DSprite component1 = this.current_l.GetComponent<UI2DSprite>();
    UI2DSprite component2 = this.current_xl.GetComponent<UI2DSprite>();
    if (xl_isActive)
    {
      ((UIRect) component1).alpha = 0.0f;
      ((UIWidget) component1).depth = 1;
      ((UIRect) component2).alpha = 1f;
      ((UIWidget) component2).depth = 10;
    }
    else
    {
      ((UIRect) component1).alpha = 1f;
      ((UIWidget) component1).depth = 10;
      ((UIRect) component2).alpha = 0.0f;
      ((UIWidget) component2).depth = 1;
    }
  }

  public void CloudAnim(bool reverse)
  {
    if (reverse)
    {
      if (Object.op_Inequality((Object) this.cloudanimRev, (Object) null))
      {
        ((Component) this.cloudanimRev).gameObject.SetActive(true);
        this.cloudanimRev.Play();
      }
    }
    else if (Object.op_Inequality((Object) this.cloudanimFow, (Object) null))
    {
      ((Component) this.cloudanimFow).gameObject.SetActive(true);
      this.cloudanimFow.Play();
    }
    Singleton<NGSoundManager>.GetInstance().playSE("SE_0050");
  }

  public int GetActiveIndex() => this.activeIndex;

  public int GetLength() => this.states.Length;

  public void Toggle(QuestBG.AnimApply num) => this.Toggle(num, this.tweener.duration);

  public void Toggle(QuestBG.AnimApply num, float duration)
  {
    int index = (int) num;
    this.activeIndex = index;
    TweenPosition orAddComponent1 = this.current.GetOrAddComponent<TweenPosition>();
    TweenScale orAddComponent2 = this.current.GetOrAddComponent<TweenScale>();
    if (this.states.Length == 0)
      return;
    GameObject state = this.states[index];
    orAddComponent1.from = this.current.transform.localPosition;
    orAddComponent1.to = state.transform.localPosition;
    orAddComponent2.from = this.current.transform.localScale;
    orAddComponent2.to = state.transform.localScale;
    ((UITweener) orAddComponent1).duration = this.tweener.duration;
    if ((double) duration == 0.0)
      ((UITweener) orAddComponent1).duration = 0.0f;
    ((UITweener) orAddComponent1).delay = this.tweener.delay;
    ((UITweener) orAddComponent1).animationCurve = this.tweener.animationCurve;
    ((UITweener) orAddComponent2).duration = this.tweener.duration;
    if ((double) duration == 0.0)
      ((UITweener) orAddComponent2).duration = 0.0f;
    ((UITweener) orAddComponent2).delay = this.tweener.delay;
    ((UITweener) orAddComponent2).animationCurve = this.tweener.animationCurve;
    ((UITweener) orAddComponent1).ResetToBeginning();
    ((UITweener) orAddComponent1).PlayForward();
    ((UITweener) orAddComponent2).ResetToBeginning();
    ((UITweener) orAddComponent2).PlayForward();
  }

  public void ClickTweenV(bool TweenToS, bool durationJudge)
  {
    TweenColor orAddComponent = this.current.GetOrAddComponent<TweenColor>();
    orAddComponent.from = Color.Lerp(Color.white, Color.gray, 0.0f);
    orAddComponent.to = Color.Lerp(Color.white, Color.gray, 1f);
    float num = durationJudge ? 0.0f : this.blackTweenTime;
    ((UITweener) orAddComponent).duration = num;
    if (TweenToS)
      ((UITweener) orAddComponent).PlayForward();
    else
      ((UITweener) orAddComponent).PlayReverse();
  }

  public void Darken(bool toHard, bool quick)
  {
    if (Object.op_Equality((Object) this.current, (Object) null))
      return;
    if (Object.op_Equality((Object) this.current, (Object) this.current_xl))
      this.current = this.current_l;
    TweenColor orAddComponent = this.current.gameObject.GetOrAddComponent<TweenColor>();
    float num1 = 0.0f;
    float num2 = 0.0f;
    if (toHard)
      num1 = 0.5f;
    else
      num2 = 0.5f;
    if (quick)
      ((UITweener) orAddComponent).duration = 0.0f;
    else
      ((UITweener) orAddComponent).duration = 0.5f;
    orAddComponent.to = Color.Lerp(Color.white, Color.black, num1);
    orAddComponent.from = Color.Lerp(Color.white, Color.black, num2);
    ((UITweener) orAddComponent).ResetToBeginning();
    ((UITweener) orAddComponent).PlayForward();
  }

  public enum AnimApply
  {
    Button1,
    Button2,
    Button3,
    Button4,
    Button5,
    MyPage,
    LMainPostion,
    XLMainPositon,
  }

  public enum QuestPosition
  {
    Story,
    Chapter,
  }
}
