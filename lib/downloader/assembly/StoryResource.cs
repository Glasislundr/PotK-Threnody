// Decompiled with JetBrains decompiler
// Type: StoryResource
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using GameCore.LispCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UniLinq;
using UnityEngine;

#nullable disable
public class StoryResource
{
  private Dictionary<int, StoryResource.UnitResource> unitResources = new Dictionary<int, StoryResource.UnitResource>();

  public IEnumerator Run(ReadOnlyCollection<StoryBlock> xs)
  {
    StoryResource storyResource = this;
    // ISSUE: reference to a compiler-generated method
    foreach (Cons cons in xs.SelectMany<StoryBlock, Cons>(new Func<StoryBlock, IEnumerable<Cons>>(storyResource.\u003CRun\u003Eb__2_0)))
    {
      IEnumerator e = storyResource.dispatch(cons);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public GameObject GetCharacterPrefab(int character_id) => this.unitResource(character_id).Prefab;

  public Sprite GetLargeTexture(int character_id) => this.unitResource(character_id).Large;

  private StoryResource.UnitResource unitResource(int character_id)
  {
    if (!this.unitResources.ContainsKey(character_id))
      Debug.LogError((object) ("character id not found = " + (object) character_id));
    return this.unitResources[character_id];
  }

  private List<Cons> flatten(Cons cons)
  {
    List<Cons> consList = new List<Cons>();
    for (; cons != null; cons = cons.cdr as Cons)
    {
      if (cons.car is Cons car)
        consList.Add(car);
    }
    return consList;
  }

  private IEnumerator dispatch(Cons cons)
  {
    if (!(cons.car is string car))
      Debug.LogError((object) ("invalid car or car. cons = " + (object) cons));
    else if (cons.cdr is Cons cdr1 && (car == "chara" || car == "body" || car == "entry"))
    {
      Decimal? car1 = cdr1.car as Decimal?;
      if (!car1.HasValue)
      {
        Debug.LogError((object) ("invalid arguments. cons = " + (object) cons));
      }
      else
      {
        int character_id = Decimal.ToInt32(car1.Value);
        if (!this.unitResources.ContainsKey(character_id))
        {
          int resource_id = character_id;
          MasterDataTable.UnitJob unitJob = (MasterDataTable.UnitJob) null;
          UnitExtensionStory unitExtensionStory = (UnitExtensionStory) null;
          Cons cdr = cdr1.cdr as Cons;
          if (car == "entry")
          {
            if (cdr == null)
            {
              Debug.LogError((object) ("invalid arguments. cons = " + (object) cons));
              yield break;
            }
            else
            {
              car1 = cdr.car as Decimal?;
              if (!car1.HasValue)
              {
                Debug.LogError((object) ("invalid arguments. cons = " + (object) cons));
                yield break;
              }
              else
              {
                character_id = Decimal.ToInt32(car1.Value);
                cdr = cdr.cdr as Cons;
              }
            }
          }
          Decimal? nullable = new Decimal?();
          Decimal? car2;
          if (cdr != null && (car2 = cdr.car as Decimal?).HasValue)
          {
            int job_id = Decimal.ToInt32(car2.Value);
            if (!MasterData.UnitJob.TryGetValue(job_id, out unitJob) && (unitExtensionStory = Array.Find<UnitExtensionStory>(MasterData.UnitExtensionStoryList, (Predicate<UnitExtensionStory>) (x => x.unit == character_id && x.job_metamor_id == job_id))) == null)
              Debug.LogError((object) string.Format("invalid jobOrMetamorID LispCommand:\"{0} {1} {2}\"", (object) car, (object) character_id, (object) job_id));
          }
          Future<GameObject> fGameObject;
          Future<Sprite> fLarge;
          if (character_id <= 999)
          {
            fGameObject = MobUnits.LoadStory(character_id);
            fLarge = MobUnits.LoadSpriteLarge(character_id);
          }
          else
          {
            UnitUnit unitUnit;
            if (MasterData.UnitUnit.TryGetValue(character_id, out unitUnit))
            {
              fGameObject = unitUnit.LoadStory();
              fLarge = unitJob == null ? (unitExtensionStory == null ? (!unitUnit.ExistSpriteStory() ? unitUnit.LoadSpriteLarge() : unitUnit.LoadSpriteStory()) : (!unitUnit.ExistSpriteStory() ? unitUnit.LoadSpriteLarge(unitExtensionStory.job_metamor_id, 1f) : unitUnit.LoadSpriteStory(unitExtensionStory.job_metamor_id))) : (!unitUnit.ExistSpriteStory() ? unitUnit.LoadSpriteLarge(unitJob.ID, 1f) : unitUnit.LoadSpriteStory(unitJob.ID));
            }
            else
            {
              Debug.LogError((object) ("invalid character id " + (object) character_id));
              yield break;
            }
          }
          IEnumerator e = fGameObject.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          e = fLarge.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          if (fGameObject.HasResult && fLarge.HasResult)
            this.unitResources[resource_id] = new StoryResource.UnitResource(fGameObject.Result, fLarge.Result);
          else
            Debug.LogError((object) ("Can't load resource. character id " + (object) character_id));
          fGameObject = (Future<GameObject>) null;
          fLarge = (Future<Sprite>) null;
        }
      }
    }
  }

  private class UnitResource
  {
    public readonly GameObject Prefab;
    public readonly Sprite Large;

    public UnitResource(GameObject prefab, Sprite large)
    {
      this.Prefab = prefab;
      this.Large = large;
    }
  }
}
