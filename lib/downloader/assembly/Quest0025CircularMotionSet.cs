// Decompiled with JetBrains decompiler
// Type: Quest0025CircularMotionSet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Quest0025CircularMotionSet : MonoBehaviour
{
  [SerializeField]
  protected Transform CenterTarget;
  [SerializeField]
  protected GameObject Director;
  [SerializeField]
  private List<Transform> MotionTargets = new List<Transform>();
  [SerializeField]
  private UIPanel mPanel;
  [SerializeField]
  private Transform DragArea;
  [HideInInspector]
  protected float radius;
  [HideInInspector]
  protected Vector3 CenterPosition;
  protected const float Displace = -100f;
  private int MotionTargetQuantity;
  private float SpaceAngle = 70f;
  private const int TARGET_HEIGHT = 130;
  private const int StartTweenGroup = 12;
  private const int EndTweenGroup = 13;
  public int SpringSpeed = 15;
  private Transform NextCenterObject;
  private bool ChangeCenterObj;
  private bool SetNextCenterObj;
  public int Tolerance = 2;
  private float Goal;
  private float[] rate;
  private const int TEXTURE_FULL_SIZE = 585;
  public BGChange bgchange;
  private jogDialSE jog;
  [SerializeField]
  private UIScrollView scroll;
  public bool Hard;
  public Quest0025CircularMotionSet.CirculCondition condition;

  public virtual void SetValue()
  {
    Transform transform = this.Director.transform;
    this.CenterPosition = Vector3.op_Addition(transform.localPosition, transform.parent.localPosition);
    this.CenterPosition.x += -100f;
    this.radius = (float) this.Director.GetComponent<UIWidget>().width;
    this.scroll.disableDragIfFits = this.MotionTargets.Count <= 1;
    this.condition = Quest0025CircularMotionSet.CirculCondition.START;
    this.ChangeCenterObj = false;
    this.SetNextCenterObj = false;
    ((Component) this.mPanel).transform.localPosition = new Vector3(0.0f, 0.0f);
    this.mPanel.clipOffset = new Vector2(0.0f, 0.0f);
    this.jog = ((Component) this).gameObject.GetOrAddComponent<jogDialSE>();
    int? nullable1 = this.MotionTargets.FirstIndexOrNull<Transform>((Func<Transform, bool>) (x => Object.op_Equality((Object) x, (Object) this.CenterTarget)));
    this.NextCenterObject = this.MotionTargets[nullable1.Value];
    for (int index = 0; index < this.MotionTargets.Count; ++index)
    {
      int num1 = index;
      int? nullable2 = nullable1;
      float num2 = (float) (nullable2.HasValue ? new int?(num1 - nullable2.GetValueOrDefault()) : new int?()).Value * this.SpaceAngle;
      int num3 = 0;
      while ((double) num2 > (double) this.SpaceAngle)
      {
        num2 -= this.SpaceAngle;
        ++num3;
      }
      while ((double) num2 < -(double) this.SpaceAngle)
      {
        num2 += this.SpaceAngle;
        ++num3;
      }
      float num4 = this.radius * Mathf.Cos(num2 * ((float) Math.PI / 180f)) + (float) (-1000 * num3);
      float num5 = this.radius * Mathf.Sin(num2 * ((float) Math.PI / 180f)) * (float) (num3 + 1);
      Vector2 frompos = new Vector2((float) ((double) num4 + (double) this.CenterPosition.x - 1000.0), -num5 + this.CenterPosition.y);
      Vector2 topos = new Vector2(num4 + this.CenterPosition.x, -num5 + this.CenterPosition.y);
      ((IEnumerable<TweenPosition>) ((Component) this.MotionTargets[index]).GetComponents<TweenPosition>()).ForEach<TweenPosition>((Action<TweenPosition>) (x =>
      {
        if (((UITweener) x).tweenGroup == 12)
        {
          x.from = Vector2.op_Implicit(frompos);
          x.to = Vector2.op_Implicit(topos);
        }
        else
        {
          if (((UITweener) x).tweenGroup != 13)
            return;
          x.from = Vector2.op_Implicit(topos);
          x.to = Vector2.op_Implicit(frompos);
        }
      }));
    }
    this.MotionTargets.ForEach((Action<Transform>) (x =>
    {
      Quest0025SlidePanelDragged component = ((Component) x).gameObject.GetComponent<Quest0025SlidePanelDragged>();
      component.drag = this;
      if (Object.op_Equality((Object) x, (Object) this.CenterTarget))
      {
        ((Behaviour) ((Component) x).GetComponent<UIButton>()).enabled = true;
        ((Component) component.SlcOpen).gameObject.SetActive(true);
      }
      else
      {
        ((Behaviour) ((Component) x).GetComponent<UIButton>()).enabled = false;
        ((Component) component.SlcOpen).gameObject.SetActive(true);
      }
    }));
    this.MotionTargets.Where<Transform>((Func<Transform, bool>) (x => !((Component) x).GetComponent<Quest0025SlidePanelDragged>().Release)).ForEach<Transform>((Action<Transform>) (x => ((IEnumerable<UISprite>) ((Component) x).GetComponentsInChildren<UISprite>()).ForEach<UISprite>((Action<UISprite>) (y => ((UIWidget) y).color = Color.gray))));
    ((Component) this.DragArea).gameObject.GetComponent<Quest0025SlidePanelDragged>().drag = this;
    this.DragArea.localPosition = new Vector3(this.DragArea.localPosition.x, this.CenterPosition.y);
    this.mPanel.baseClipRegion = new Vector4(this.mPanel.baseClipRegion.x, this.CenterPosition.y, 1000f, (float) (130 * this.MotionTargets.Count * 3));
  }

  public void SetCenter(Transform target) => this.CenterTarget = target;

  private void Update()
  {
    if (this.condition == Quest0025CircularMotionSet.CirculCondition.START)
    {
      this.Rating();
      float panelY = this.mPanel.baseClipRegion.y + this.mPanel.clipOffset.y;
      this.MotionTargets.ForEach((Action<Transform>) (x => ((Component) x).GetComponent<Quest0025SlidePanelDragged>().PanelEffect(panelY)));
    }
    if (this.condition == Quest0025CircularMotionSet.CirculCondition.DRAG || this.condition == Quest0025CircularMotionSet.CirculCondition.RELEASE)
      this.XposCalculation();
    if (this.condition == Quest0025CircularMotionSet.CirculCondition.RELEASE && this.SetNextCenterObj)
      this.CenterNearHear();
    if (this.condition != Quest0025CircularMotionSet.CirculCondition.DRAG && this.ChangeCenterObj)
      this.SpringValue();
    if (this.condition == Quest0025CircularMotionSet.CirculCondition.SET)
      this.SetAndKeepCenter();
    if (this.condition == Quest0025CircularMotionSet.CirculCondition.DRAG || this.condition == Quest0025CircularMotionSet.CirculCondition.RELEASE)
      this.jogSE();
    float axis = Input.GetAxis("Mouse ScrollWheel");
    if ((double) axis == 0.0)
      return;
    this.ScrollWheel(axis);
  }

  private void ScrollWheel(float d)
  {
    float nPosY = 50f;
    if ((double) d > 0.0)
      this.StartCoroutine(this.ScrollSmooth(nPosY));
    else
      this.StartCoroutine(this.ScrollSmooth(-nPosY));
  }

  private IEnumerator ScrollSmooth(float nPosY)
  {
    float duration = 0.15f;
    float y = ((Component) this.mPanel).transform.localPosition.y;
    float targetPosY = y + nPosY;
    this.condition = Quest0025CircularMotionSet.CirculCondition.DRAG;
    float startPosY = y;
    for (float t = 0.0f; (double) t < (double) duration; t += Time.deltaTime)
    {
      float num = (float) (int) Mathf.Lerp(startPosY, targetPosY, t / duration);
      ((Component) this.mPanel).transform.localPosition = new Vector3(((Component) this.mPanel).transform.localPosition.x, num);
      this.mPanel.clipOffset = new Vector2(this.mPanel.clipOffset.x, -num);
      this.XposCalculation();
      yield return (object) null;
    }
    this.SetNextCenterObj = true;
    this.condition = Quest0025CircularMotionSet.CirculCondition.RELEASE;
  }

  private void jogSE()
  {
    foreach (Component motionTarget in this.MotionTargets)
    {
      Quest0025SlidePanelDragged component1 = motionTarget.GetComponent<Quest0025SlidePanelDragged>();
      Transform transform = ((Component) component1.SlcOpen).transform;
      if (Object.op_Inequality((Object) transform, (Object) null) && ((Component) transform).gameObject.activeInHierarchy)
      {
        UISprite component2 = ((Component) transform).gameObject.GetComponent<UISprite>();
        if (Object.op_Inequality((Object) component2, (Object) null) && ((UIWidget) component2).width != 0)
        {
          if (((UIWidget) component2).width >= 585 && !component1.jogPlayed)
          {
            if (Object.op_Inequality((Object) this.jog, (Object) null))
              this.jog.playSE();
            component1.jogPlayed = true;
          }
          else if ((((UIWidget) component2).width < 585 || !component1.jogPlayed) && ((UIWidget) component2).width != 0 && ((UIWidget) component2).width != 585)
            component1.jogPlayed = false;
        }
      }
    }
  }

  public void XposCalculation()
  {
    float radius = this.radius;
    float num1 = this.mPanel.clipOffset.y * -1f;
    foreach (Transform motionTarget in this.MotionTargets)
    {
      float num2 = this.CenterPosition.y - (motionTarget.localPosition.y + num1);
      Vector3 localPosition = motionTarget.localPosition;
      localPosition.x = (double) radius >= (double) Mathf.Abs(num2) ? Mathf.Sqrt((float) ((double) radius * (double) radius - (double) num2 * (double) num2)) + this.CenterPosition.x : -1000f;
      motionTarget.localPosition = localPosition;
    }
    this.Rating();
    float panelY = this.mPanel.baseClipRegion.y + this.mPanel.clipOffset.y;
    this.MotionTargets.ForEach((Action<Transform>) (x => ((Component) x).GetComponent<Quest0025SlidePanelDragged>().PanelEffect(panelY)));
    Vector3 localPosition1 = this.DragArea.localPosition;
    this.DragArea.localPosition = new Vector3(localPosition1.x, panelY, localPosition1.z);
    this.MotionTargets.ForEach((Action<Transform>) (x =>
    {
      TweenPosition tweenPosition = ((IEnumerable<TweenPosition>) ((Component) x).GetComponents<TweenPosition>()).Where<TweenPosition>((Func<TweenPosition, bool>) (y => ((UITweener) y).tweenGroup == 13)).First<TweenPosition>();
      tweenPosition.from = new Vector3(((Component) x).transform.localPosition.x, x.localPosition.y);
      tweenPosition.to = new Vector3(-1000f, x.localPosition.y);
    }));
  }

  public void Rating()
  {
    this.rate = new float[this.MotionTargets.Count];
    for (int index = 0; index < this.MotionTargets.Count; ++index)
    {
      this.rate[index] = Mathf.Abs(this.MotionTargets[index].localPosition.x / (this.CenterPosition.x - -100f)) - 1f;
      this.rate[index] = (double) this.rate[index] > 1.0 ? 1f : this.rate[index];
    }
  }

  public void CenterNearHear()
  {
    float num = this.mPanel.clipOffset.y * -1f;
    int index1 = 0;
    for (int index2 = 0; index2 < this.MotionTargets.Count; ++index2)
    {
      if ((double) this.rate[index2] <= (double) this.rate[index1])
        index1 = index2;
    }
    if ((double) this.rate[index1] >= 1.0)
      index1 = (double) num < 0.0 ? 0 : index1;
    this.NextCenterObject = this.MotionTargets[index1];
    this.SetNextCenterObj = false;
    this.ChangeCenterObj = true;
  }

  public void SpringValue()
  {
    if (!Singleton<NGSceneManager>.GetInstance().isSceneInitialized)
      return;
    float num1 = this.mPanel.clipOffset.y * -1f;
    float num2 = this.CenterPosition.y - this.NextCenterObject.localPosition.y;
    float num3 = num2 - num1;
    Transform transform = ((Component) this.mPanel).transform;
    transform.localPosition = Vector3.op_Addition(transform.localPosition, new Vector3(0.0f, num3 * (float) this.SpringSpeed * Time.deltaTime));
    this.mPanel.clipOffset = new Vector2(((Component) this.mPanel).transform.localPosition.x * -1f, ((Component) this.mPanel).transform.localPosition.y * -1f);
    if ((int) ((Component) this.mPanel).transform.localPosition.y > (int) num2 + this.Tolerance || (int) ((Component) this.mPanel).transform.localPosition.y <= (int) num2 - this.Tolerance)
      return;
    this.ChangeCenterObj = false;
    this.Goal = (float) (int) num2;
    this.SetAndKeepCenter();
    this.XposCalculation();
    this.condition = Quest0025CircularMotionSet.CirculCondition.SET;
    this.MotionTargets.ForEach((Action<Transform>) (x =>
    {
      Quest0025SlidePanelDragged component = ((Component) x).GetComponent<Quest0025SlidePanelDragged>();
      if (Object.op_Equality((Object) this.NextCenterObject, (Object) x))
      {
        ((Behaviour) ((Component) x).GetComponent<UIButton>()).enabled = component.Release;
        ((Component) component.SlcOpen).gameObject.SetActive(true);
      }
      else
      {
        ((Behaviour) ((Component) x).GetComponent<UIButton>()).enabled = false;
        ((Component) component.SlcOpen).gameObject.SetActive(false);
      }
    }));
  }

  public void SetAndKeepCenter()
  {
    ((Component) this.mPanel).transform.localPosition = new Vector3(((Component) this.mPanel).transform.localPosition.x, this.Goal);
    this.mPanel.clipOffset = new Vector2(((Component) this.mPanel).transform.localPosition.x * -1f, ((Component) this.mPanel).transform.localPosition.y * -1f);
  }

  private IEnumerator setBackGroundPrefab()
  {
    string path = string.Format("Prefabs/Quest/Story/BG/L/{0}/prefab", (object) 1);
    Future<GameObject> prefabF = !Singleton<ResourceManager>.GetInstance().Contains(path) ? Res.Prefabs.Quest.Story.BG.L._1.prefab.Load<GameObject>() : Singleton<ResourceManager>.GetInstance().Load<GameObject>(path);
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return (object) prefabF.Result;
    e = (IEnumerator) null;
  }

  public void onPress()
  {
    this.condition = Quest0025CircularMotionSet.CirculCondition.DRAG;
    this.MotionTargets.ForEach((Action<Transform>) (x =>
    {
      Quest0025SlidePanelDragged component = ((Component) x).GetComponent<Quest0025SlidePanelDragged>();
      component.DisableSlcPressedEffect();
      if (Object.op_Equality((Object) this.NextCenterObject, (Object) x))
        component.EnableSlcPressedEffect();
      ((Component) component.SlcOpen).gameObject.SetActive(true);
    }));
  }

  public void onRelease()
  {
    this.condition = Quest0025CircularMotionSet.CirculCondition.RELEASE;
    this.SetNextCenterObj = true;
    this.MotionTargets.ForEach((Action<Transform>) (x => ((Component) x).GetComponent<Quest0025SlidePanelDragged>().DisableSlcPressedEffect()));
  }

  public void End()
  {
    this.condition = Quest0025CircularMotionSet.CirculCondition.START;
    List<UITweener> EndTween = new List<UITweener>();
    this.MotionTargets.ForEach((Action<Transform>) (x => EndTween.Add((UITweener) ((IEnumerable<TweenPosition>) ((Component) x).GetComponents<TweenPosition>()).Where<TweenPosition>((Func<TweenPosition, bool>) (y => ((UITweener) y).tweenGroup == 13)).First<TweenPosition>())));
    EndTween.ForEach((Action<UITweener>) (x =>
    {
      x.ResetToBeginning();
      x.PlayForward();
    }));
    EndTween.Clear();
    EndTween = (List<UITweener>) null;
  }

  public enum CirculCondition
  {
    START,
    DRAG,
    RELEASE,
    SET,
  }
}
