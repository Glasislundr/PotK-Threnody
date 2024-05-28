// Decompiled with JetBrains decompiler
// Type: SocialListener
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using gu3;
using SM;
using UnityEngine;

#nullable disable
public class SocialListener : MonoBehaviour, SocialKit.Share.IShareListener
{
  private const string SHARE_TEXT = "ファンキルはプレーした？本格的なシミュレーションRPGが手軽に楽しめてゲーム好きなら絶対オススメだから遊んでみて！招待コード入れてくれたら特典も貰えるよ！";
  private const string SHARE_URL = "https://pk.fg-games.co.jp";

  public void OnSucceeded(SocialKit.Platform platform)
  {
  }

  public void NotInstalled(SocialKit.Platform platform)
  {
    GameObject prefab = PopupCommon.LoadPrefab();
    Singleton<PopupManager>.GetInstance().open(prefab).GetComponent<PopupCommon>().Init("送信エラー", string.Format("{0}対応アプリがインストールされていません", (object) platform.ToString()));
  }

  public static void ShareWithTwitter()
  {
    SocialKit.Share.Send((SocialKit.Platform) 1, string.Format("{0}【招待コード：{1}】{2}", (object) "ファンキルはプレーした？本格的なシミュレーションRPGが手軽に楽しめてゲーム好きなら絶対オススメだから遊んでみて！招待コード入れてくれたら特典も貰えるよ！", (object) SMManager.Get<Player>().short_id, (object) "https://pk.fg-games.co.jp"));
  }
}
