// Decompiled with JetBrains decompiler
// Type: Explore033MiniMap
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UniLinq;
using UnityEngine;

#nullable disable
public class Explore033MiniMap : MonoBehaviour
{
  [SerializeField]
  private int mFloorCountPerLabel;
  [SerializeField]
  private GameObject mNowFloorMarker;
  [SerializeField]
  private UILabel[] mFloorLbls;
  [SerializeField]
  private GameObject mLineBase;
  [SerializeField]
  private int mOnSize = 16;
  [SerializeField]
  private int mOffSize = 10;
  private Transform[] mFloorAnchors;

  private void Awake()
  {
    this.mFloorAnchors = this.mLineBase.transform.GetChildren().ToArray<Transform>();
  }

  public void UpdateFloorData()
  {
    int nowFloor = Singleton<ExploreDataManager>.GetInstance().NowFloor;
    this.SetMiniMapFloorLabel(nowFloor);
    this.SetFloorMaker(nowFloor);
  }

  private void SetMiniMapFloorLabel(int floor)
  {
    int num = Mathf.Max(0, floor - this.mFloorCountPerLabel);
    if (num > 0)
      this.mFloorLbls[0].SetTextLocalize(num.ToString() + "階");
    else
      this.mFloorLbls[0].SetTextLocalize("入口");
    this.mFloorLbls[1].SetTextLocalize(floor);
    this.mFloorLbls[2].SetTextLocalize((num + this.mFloorCountPerLabel * 2).ToString() + "階");
  }

  private void SetFloorMaker(int floor)
  {
    int index1 = Mathf.Min(this.mFloorCountPerLabel, floor);
    Vector3 localPosition = this.mNowFloorMarker.transform.localPosition;
    // ISSUE: explicit constructor call
    ((Vector3) ref localPosition).\u002Ector(localPosition.x, this.mFloorAnchors[index1].localPosition.y + this.mLineBase.transform.localPosition.y, localPosition.z);
    this.mNowFloorMarker.transform.localPosition = localPosition;
    for (int index2 = 1; index2 <= this.mFloorCountPerLabel; ++index2)
    {
      bool flag = index2 == index1;
      GameObject gameObject1 = ((Component) this.mFloorAnchors[index2].GetChildInFind("slc_Point_ON")).gameObject;
      GameObject gameObject2 = ((Component) this.mFloorAnchors[index2].GetChildInFind("slc_Point_OFF")).gameObject;
      gameObject1.gameObject.SetActive(flag);
      gameObject2.gameObject.SetActive(!flag);
      if (flag)
      {
        ((UIWidget) gameObject1.GetComponent<UISprite>()).SetDimensions(this.mOnSize, this.mOnSize);
        ((UIWidget) gameObject2.GetComponent<UISprite>()).SetDimensions(this.mOnSize, this.mOnSize);
      }
      else
      {
        ((UIWidget) gameObject1.GetComponent<UISprite>()).SetDimensions(this.mOffSize, this.mOffSize);
        ((UIWidget) gameObject2.GetComponent<UISprite>()).SetDimensions(this.mOffSize, this.mOffSize);
      }
    }
  }
}
