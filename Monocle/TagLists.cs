// Decompiled with JetBrains decompiler
// Type: Monocle.TagLists
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using System.Collections.Generic;

namespace Monocle
{

    public class TagLists
    {
      private List<Entity>[] lists;
      private bool[] unsorted;
      private bool areAnyUnsorted;

      internal TagLists()
      {
        this.lists = new List<Entity>[BitTag.TotalTags];
        this.unsorted = new bool[BitTag.TotalTags];
        for (int index = 0; index < this.lists.Length; ++index)
          this.lists[index] = new List<Entity>();
      }

      public List<Entity> this[int index] => this.lists[index];

      internal void MarkUnsorted(int index)
      {
        this.areAnyUnsorted = true;
        this.unsorted[index] = true;
      }

      internal void UpdateLists()
      {
        if (!this.areAnyUnsorted)
          return;
        for (int index = 0; index < this.lists.Length; ++index)
        {
          if (this.unsorted[index])
          {
            this.lists[index].Sort(EntityList.CompareDepth);
            this.unsorted[index] = false;
          }
        }
        this.areAnyUnsorted = false;
      }

      internal void EntityAdded(Entity entity)
      {
        for (int index = 0; index < BitTag.TotalTags; ++index)
        {
          if (entity.TagCheck(1 << index))
          {
            this[index].Add(entity);
            this.areAnyUnsorted = true;
            this.unsorted[index] = true;
          }
        }
      }

      internal void EntityRemoved(Entity entity)
      {
        for (int index = 0; index < BitTag.TotalTags; ++index)
        {
          if (entity.TagCheck(1 << index))
            this.lists[index].Remove(entity);
        }
      }
    }
}
