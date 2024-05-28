// Decompiled with JetBrains decompiler
// Type: LayoutDebug
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class LayoutDebug : NGMenuBase
{
  public GUISkin guiSkin;
  private int maxScene;
  private int currentScene;
  private string sceneTitle;
  private string sceneName;
  [SerializeField]
  private string[] scenes = new string[72]
  {
    "bugu005_2",
    "bugu005_3",
    "bugu005_3_10",
    "friend008_1",
    "friend008_6",
    "friend008_11",
    "friend008_17",
    "friend008_18",
    "gacha006_5",
    "gacha006_9",
    "gacha006_11",
    "gacha999_5_1a",
    "gacha999_6_1a",
    "guide011_1",
    "guide011_2",
    "guide011_2_1",
    "guide011_3",
    "guide011_3_1",
    "guide011_4",
    "guide011_4_1",
    "invite013_1",
    "invite013_4",
    "mypage001_7",
    "mypage001_7_2",
    "mypage001_8_1",
    "quest002_2",
    "quest002_4",
    "quest002_8",
    "quest002_8_2",
    "quest002_10",
    "quest002_10_3",
    "quest002_10a",
    "quest002_14",
    "quest002_15",
    "quest002_15_a",
    "quest002_17",
    "quest002_19",
    "quest002_21",
    "quest999_5_1",
    "quest999_6_1",
    "serial014_1",
    "serial014_4",
    "setting010_1",
    "setting010_2",
    "shop007_1",
    "shop007_4",
    "shop007_4_1",
    "shop007_6",
    "shop007_7_1",
    "shop007_8",
    "shop007_9",
    "shop007_10",
    "shop007_11",
    "shop007_12",
    "shop999_8_1",
    "shop999_9_1",
    "startup000_12",
    "startup000_17",
    "story009_0",
    "story009_1",
    "story009_2",
    "story009_6",
    "story009_6_a",
    "transfer012_1",
    "unit004_4_1",
    "unit004_4_3",
    "unit004_6",
    "unit004_6_8",
    "unit004_8_3",
    "unit004_8_6",
    "unit004_9_3",
    "unit004_9_9"
  };

  private void Start()
  {
    this.maxScene = this.scenes.Length;
    this.currentScene = 0;
    this.sceneName = "";
    this.sceneTitle = "(0/" + (object) this.maxScene + ") null";
    Debug.Log((object) ("Number of registered scenes are " + (object) this.maxScene + "."));
  }

  private void Update()
  {
  }

  private void UpdateSelection()
  {
    if (this.currentScene < 1)
      this.currentScene = this.scenes.Length;
    else if (this.currentScene > this.scenes.Length)
      this.currentScene = 1;
    this.sceneName = this.scenes[this.currentScene - 1];
    this.sceneTitle = "(" + (object) this.currentScene + "/" + (object) this.maxScene + ") " + this.sceneName;
    Debug.Log((object) ("switch to " + this.sceneTitle));
    this.changeScene(this.sceneName);
  }

  private void OnGUI()
  {
    GUILayout.Space((float) (int) ((double) Screen.height * 0.8));
    GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
    GUILayout.Space((float) (int) ((double) Screen.width * 0.3));
    if (GUILayout.Button("<<<", new GUILayoutOption[2]
    {
      GUILayout.MinWidth((float) (int) ((double) Screen.width * 0.2)),
      GUILayout.MinHeight((float) (int) ((double) Screen.height * 0.1))
    }))
    {
      --this.currentScene;
      this.UpdateSelection();
    }
    if (GUILayout.Button(">>>", new GUILayoutOption[2]
    {
      GUILayout.MinWidth((float) (int) ((double) Screen.width * 0.2)),
      GUILayout.MinHeight((float) (int) ((double) Screen.height * 0.1))
    }))
    {
      ++this.currentScene;
      this.UpdateSelection();
    }
    GUILayout.EndHorizontal();
    GUILayout.Label(this.sceneTitle, this.guiSkin.label, Array.Empty<GUILayoutOption>());
  }
}
