// Decompiled with JetBrains decompiler
// Type: Monocle.RendererList
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using System.Collections.Generic;

namespace Monocle
{

    public class RendererList
    {
      public List<Renderer> Renderers;
      private List<Renderer> adding;
      private List<Renderer> removing;
      private Scene scene;

      internal RendererList(Scene scene)
      {
        this.scene = scene;
        this.Renderers = new List<Renderer>();
        this.adding = new List<Renderer>();
        this.removing = new List<Renderer>();
      }

      internal void UpdateLists()
      {
        if (this.adding.Count > 0)
        {
          foreach (Renderer renderer in this.adding)
            this.Renderers.Add(renderer);
        }
        this.adding.Clear();
        if (this.removing.Count > 0)
        {
          foreach (Renderer renderer in this.removing)
            this.Renderers.Remove(renderer);
        }
        this.removing.Clear();
      }

      internal void Update()
      {
        foreach (Renderer renderer in this.Renderers)
          renderer.Update(this.scene);
      }

      internal void BeforeRender()
      {
        for (int index = 0; index < this.Renderers.Count; ++index)
        {
          if (this.Renderers[index].Visible)
          {
            Draw.Renderer = this.Renderers[index];
            this.Renderers[index].BeforeRender(this.scene);
          }
        }
      }

      internal void Render()
      {
        for (int index = 0; index < this.Renderers.Count; ++index)
        {
          if (this.Renderers[index].Visible)
          {
            Draw.Renderer = this.Renderers[index];
            this.Renderers[index].Render(this.scene);
          }
        }
      }

      internal void AfterRender()
      {
        for (int index = 0; index < this.Renderers.Count; ++index)
        {
          if (this.Renderers[index].Visible)
          {
            Draw.Renderer = this.Renderers[index];
            this.Renderers[index].AfterRender(this.scene);
          }
        }
      }

      public void MoveToFront(Renderer renderer)
      {
        this.Renderers.Remove(renderer);
        this.Renderers.Add(renderer);
      }

      public void Add(Renderer renderer) => this.adding.Add(renderer);

      public void Remove(Renderer renderer) => this.removing.Add(renderer);
    }
}
