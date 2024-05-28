// Decompiled with JetBrains decompiler
// Type: BacklogManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

#nullable disable
public class BacklogManager : MonoBehaviour
{
  [SerializeField]
  private GameObject message_obj;
  public GameObject scroll_view_obj;
  public GameObject bg_obj;
  public GameObject logContainer;
  public BoxCollider skipBtn;
  public BoxCollider logBtn;
  [SerializeField]
  private List<BoxCollider> colliders_;
  public GameObject closeBtn;
  public GameObject logHeader;
  private List<string> log_data = new List<string>();
  private List<GameObject> clone_data = new List<GameObject>();
  private UIScrollView scroll_view;
  private int scroll_height;
  private Action endAction;
  private readonly string patternBBCode = "(\\[[0-9a-f]{6}\\])|(\\[\\-\\])|(\\[b\\])|(\\[\\/b\\])|(\\[i\\])|(\\[\\/i\\])|(\\[u\\])|(\\[\\/u\\])|(\\[s\\])|(\\[\\/s\\])|(\\[sub\\])|(\\[\\/sub\\])|(\\[sup\\])|(\\[\\/sup\\])|(\\[url=.*?\\])|(\\[\\/url\\])";

  private void Start() => this.StartCoroutine(this.Initialize());

  private void Update()
  {
    if (!this.logContainer.activeSelf || !Input.GetKeyUp((KeyCode) 27))
      return;
    this.endAction();
  }

  private IEnumerator Initialize()
  {
    Future<GameObject> futureC = Res.Prefabs.story009_3.BacklogMsg.Load<GameObject>();
    IEnumerator e = futureC.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.message_obj = futureC.Result.Clone(this.scroll_view_obj.transform);
    this.scroll_view = this.scroll_view_obj.GetComponentInChildren<UIScrollView>();
    this.logContainer.SetActive(false);
    this.closeBtn.SetActive(false);
    this.logHeader.SetActive(false);
    this.bg_obj.SetActive(false);
    ((Collider) this.skipBtn).enabled = true;
    ((Collider) this.logBtn).enabled = true;
    if (this.colliders_ != null)
      this.colliders_.ForEach((Action<BoxCollider>) (c => ((Collider) c).enabled = true));
    this.scroll_height = (int) this.scroll_view.panel.height;
  }

  public void Add(string name, string msg)
  {
    if (msg == "")
      return;
    string str1 = this.NormalizeBBCode(name);
    string str2 = this.NormalizeBBCode(msg);
    this.log_data.Add("[" + (this.log_data.Count % 2 == 0 ? "FFFFFF" : "FFFFCC") + "]" + str1 + "\n" + str2 + "\n\n");
  }

  public void StartBacklog(Action endAction)
  {
    this.End();
    this.endAction = endAction;
    this.logContainer.SetActive(true);
    this.closeBtn.SetActive(true);
    this.logHeader.SetActive(true);
    ((Collider) this.skipBtn).enabled = false;
    ((Collider) this.logBtn).enabled = false;
    if (this.colliders_ != null)
      this.colliders_.ForEach((Action<BoxCollider>) (c => ((Collider) c).enabled = false));
    int num1 = this.scroll_height / 2;
    int num2 = 0;
    foreach (string str in this.log_data)
    {
      int crlf = this.GetCRLF(str);
      GameObject gameObject = Object.Instantiate<GameObject>(this.message_obj, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
      UILabel component = gameObject.GetComponent<UILabel>();
      int num3 = (component.fontSize + component.spacingY) * crlf;
      component.SetText(str);
      ((UIWidget) component).height = num3;
      gameObject.transform.parent = this.scroll_view_obj.transform;
      gameObject.transform.localPosition = new Vector3(-3f, (float) (num1 - num3 / 2), 0.0f);
      gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
      this.clone_data.Add(gameObject);
      num1 -= num3;
      num2 += num3;
    }
    this.scroll_view.ResetPosition();
    if (num2 >= this.scroll_height)
    {
      float y = ((Component) this.scroll_view).GetComponent<UIPanel>().GetViewSize().y;
      this.scroll_view.MoveRelative(new Vector3(0.0f, (float) num2 - y, 0.0f));
    }
    this.bg_obj.SetActive(true);
  }

  private string NormalizeBBCode(string input)
  {
    string replacement = "";
    return Regex.Replace(input, this.patternBBCode, replacement);
  }

  public void End()
  {
    foreach (Object @object in this.clone_data)
      Object.Destroy(@object);
    this.clone_data.Clear();
    this.logContainer.SetActive(false);
    this.closeBtn.SetActive(false);
    this.logHeader.SetActive(false);
    this.bg_obj.SetActive(false);
    ((Collider) this.skipBtn).enabled = true;
    ((Collider) this.logBtn).enabled = true;
    if (this.colliders_ == null)
      return;
    this.colliders_.ForEach((Action<BoxCollider>) (c => ((Collider) c).enabled = true));
  }

  public void BacklogDestroy()
  {
    this.log_data.Clear();
    this.End();
  }

  public bool IsEnable() => this.bg_obj.activeSelf;

  private int GetCRLF(string s)
  {
    return s.Split(new char[1]{ '\n' }, StringSplitOptions.None).Length;
  }
}
