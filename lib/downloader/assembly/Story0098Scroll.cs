// Decompiled with JetBrains decompiler
// Type: Story0098Scroll
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class Story0098Scroll : BannerBase
{
  public IEnumerator InitScroll(QuestExtraS extra)
  {
    Story0098Scroll story0098Scroll = this;
    string path = extra.seek_type != QuestExtra.SeekType.L ? BannerBase.GetSpriteIdlePathQuest(extra, extra.quest_m_QuestExtraM, QuestExtra.SeekType.M) : BannerBase.GetSpriteIdlePathQuest(extra, extra.quest_l_QuestExtraL, QuestExtra.SeekType.L);
    Future<Texture2D> futureIdle = Singleton<ResourceManager>.GetInstance().Load<Texture2D>(path);
    IEnumerator e = futureIdle.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Texture2D result = futureIdle.Result;
    if (Object.op_Inequality((Object) result, (Object) null))
    {
      Sprite sprite = Sprite.Create(result, new Rect(0.0f, 0.0f, (float) ((Texture) result).width, (float) ((Texture) result).height), new Vector2(0.5f, 0.5f), 1f, 100U, (SpriteMeshType) 0);
      ((Object) sprite).name = ((Object) result).name;
      story0098Scroll.IdleSprite.sprite2D = sprite;
    }
  }

  public IEnumerator InitScroll(TowerPlaybackStory story)
  {
    Story0098Scroll story0098Scroll = this;
    string spriteIdlePath = BannerBase.GetSpriteIdlePath(story.banner_id, BannerBase.Type.tower);
    Future<Texture2D> futureIdle = Singleton<ResourceManager>.GetInstance().Load<Texture2D>(spriteIdlePath);
    IEnumerator e = futureIdle.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Texture2D result = futureIdle.Result;
    if (Object.op_Inequality((Object) result, (Object) null))
    {
      Sprite sprite = Sprite.Create(result, new Rect(0.0f, 0.0f, (float) ((Texture) result).width, (float) ((Texture) result).height), new Vector2(0.5f, 0.5f), 1f, 100U, (SpriteMeshType) 0);
      ((Object) sprite).name = ((Object) result).name;
      story0098Scroll.IdleSprite.sprite2D = sprite;
    }
  }

  public IEnumerator InitScroll(RaidPlaybackStory story)
  {
    Story0098Scroll story0098Scroll = this;
    string spriteIdlePath = BannerBase.GetSpriteIdlePath(MasterData.GuildRaidPeriod[story.period_id].banner_id, BannerBase.Type.raid);
    Future<Texture2D> futureIdle = Singleton<ResourceManager>.GetInstance().Load<Texture2D>(spriteIdlePath);
    IEnumerator e = futureIdle.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Texture2D result = futureIdle.Result;
    if (Object.op_Inequality((Object) result, (Object) null))
    {
      Sprite sprite = Sprite.Create(result, new Rect(0.0f, 0.0f, (float) ((Texture) result).width, (float) ((Texture) result).height), new Vector2(0.5f, 0.5f), 1f, 100U, (SpriteMeshType) 0);
      ((Object) sprite).name = ((Object) result).name;
      story0098Scroll.IdleSprite.sprite2D = sprite;
    }
  }
}
