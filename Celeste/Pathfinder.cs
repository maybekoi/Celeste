// Decompiled with JetBrains decompiler
// Type: Celeste.Pathfinder
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using Microsoft.Xna.Framework;
using Monocle;
using System;
using System.Collections.Generic;

namespace Celeste
{

    public class Pathfinder
    {
      private static readonly Point[] directions = new Point[4]
      {
        new Point(1, 0),
        new Point(0, 1),
        new Point(-1, 0),
        new Point(0, -1)
      };
      private const int MapSize = 200;
      private Level level;
      private Pathfinder.Tile[,] map;
      private List<Point> active = new List<Point>();
      private Pathfinder.PointMapComparer comparer;
      public bool DebugRenderEnabled;
      private List<Vector2> lastPath;
      private Point debugLastStart;
      private Point debugLastEnd;

      public Pathfinder(Level level)
      {
        this.level = level;
        this.map = new Pathfinder.Tile[200, 200];
        this.comparer = new Pathfinder.PointMapComparer(this.map);
      }

      public bool Find(
        ref List<Vector2> path,
        Vector2 from,
        Vector2 to,
        bool fewerTurns = true,
        bool logging = false)
      {
        this.lastPath = (List<Vector2>) null;
        int num1 = this.level.Bounds.Left / 8;
        int num2 = this.level.Bounds.Top / 8;
        int num3 = this.level.Bounds.Width / 8;
        int num4 = this.level.Bounds.Height / 8;
        Point levelSolidOffset = this.level.LevelSolidOffset;
        for (int index1 = 0; index1 < num3; ++index1)
        {
          for (int index2 = 0; index2 < num4; ++index2)
          {
            this.map[index1, index2].Solid = this.level.SolidsData[index1 + levelSolidOffset.X, index2 + levelSolidOffset.Y] != '0';
            this.map[index1, index2].Cost = int.MaxValue;
            this.map[index1, index2].Parent = new Point?();
          }
        }
        foreach (Entity entity in this.level.Tracker.GetEntities<Solid>())
        {
          if (entity.Collidable && entity.Collider is Hitbox)
          {
            int num5 = (int) Math.Floor((double) entity.Left / 8.0);
            for (int index3 = (int) Math.Ceiling((double) entity.Right / 8.0); num5 < index3; ++num5)
            {
              int num6 = (int) Math.Floor((double) entity.Top / 8.0);
              for (int index4 = (int) Math.Ceiling((double) entity.Bottom / 8.0); num6 < index4; ++num6)
              {
                int index5 = num5 - num1;
                int index6 = num6 - num2;
                if (index5 >= 0 && index6 >= 0 && index5 < num3 && index6 < num4)
                  this.map[index5, index6].Solid = true;
              }
            }
          }
        }
        Point point1 = this.debugLastStart = new Point((int) Math.Floor((double) from.X / 8.0) - num1, (int) Math.Floor((double) from.Y / 8.0) - num2);
        Point point2 = this.debugLastEnd = new Point((int) Math.Floor((double) to.X / 8.0) - num1, (int) Math.Floor((double) to.Y / 8.0) - num2);
        if (point1.X < 0 || point1.Y < 0 || point1.X >= num3 || point1.Y >= num4 || point2.X < 0 || point2.Y < 0 || point2.X >= num3 || point2.Y >= num4)
        {
          if (logging)
            Calc.Log((object) "PF: FAILED - Start or End outside the level bounds");
          return false;
        }
        if (this.map[point1.X, point1.Y].Solid)
        {
          if (logging)
            Calc.Log((object) "PF: FAILED - Start inside a solid");
          return false;
        }
        if (this.map[point2.X, point2.Y].Solid)
        {
          if (logging)
            Calc.Log((object) "PF: FAILED - End inside a solid");
          return false;
        }
        this.active.Clear();
        this.active.Add(point1);
        this.map[point1.X, point1.Y].Cost = 0;
        bool flag = false;
        while (this.active.Count > 0 && !flag)
        {
          Point point3 = this.active[this.active.Count - 1];
          this.active.RemoveAt(this.active.Count - 1);
          for (int index7 = 0; index7 < 4; ++index7)
          {
            Point point4 = new Point(Pathfinder.directions[index7].X, Pathfinder.directions[index7].Y);
            Point point5 = new Point(point3.X + point4.X, point3.Y + point4.Y);
            int num7 = 1;
            if (point5.X >= 0 && point5.Y >= 0 && point5.X < num3 && point5.Y < num4 && !this.map[point5.X, point5.Y].Solid)
            {
              for (int index8 = 0; index8 < 4; ++index8)
              {
                Point point6 = new Point(point5.X + Pathfinder.directions[index8].X, point5.Y + Pathfinder.directions[index8].Y);
                if (point6.X >= 0 && point6.Y >= 0 && point6.X < num3 && point6.Y < num4 && this.map[point6.X, point6.Y].Solid)
                {
                  num7 = 7;
                  break;
                }
              }
              if (fewerTurns && this.map[point3.X, point3.Y].Parent.HasValue && point5.X != this.map[point3.X, point3.Y].Parent.Value.X && point5.Y != this.map[point3.X, point3.Y].Parent.Value.Y)
                num7 += 4;
              int cost = this.map[point3.X, point3.Y].Cost;
              if (point4.Y != 0)
                num7 += (int) ((double) cost * 0.5);
              int num8 = cost + num7;
              if (this.map[point5.X, point5.Y].Cost > num8)
              {
                this.map[point5.X, point5.Y].Cost = num8;
                this.map[point5.X, point5.Y].Parent = new Point?(point3);
                int index9 = this.active.BinarySearch(point5, (IComparer<Point>) this.comparer);
                if (index9 < 0)
                  index9 = ~index9;
                this.active.Insert(index9, point5);
                if (point5 == point2)
                {
                  flag = true;
                  break;
                }
              }
            }
          }
        }
        if (!flag)
        {
          if (logging)
            Calc.Log((object) "PF: FAILED - ran out of active nodes, can't find ending");
          return false;
        }
        path.Clear();
        Point point7 = point2;
        int num9;
        for (num9 = 0; point7 != point1 && num9++ < 1000; point7 = this.map[point7.X, point7.Y].Parent.Value)
          path.Add(new Vector2((float) point7.X + 0.5f, (float) point7.Y + 0.5f) * 8f + this.level.LevelOffset);
        if (num9 >= 1000)
        {
          Console.WriteLine("WARNING: Pathfinder 'succeeded' but then was unable to work out its path?");
          return false;
        }
        for (int index = 1; index < path.Count - 1 && path.Count > 2; ++index)
        {
          if ((double) path[index].X == (double) path[index - 1].X && (double) path[index].X == (double) path[index + 1].X || (double) path[index].Y == (double) path[index - 1].Y && (double) path[index].Y == (double) path[index + 1].Y)
          {
            path.RemoveAt(index);
            --index;
          }
        }
        path.Reverse();
        this.lastPath = path;
        if (logging)
          Calc.Log((object) "PF: SUCCESS");
        return true;
      }

      public void Render()
      {
        Rectangle bounds;
        for (int index1 = 0; index1 < 200; ++index1)
        {
          for (int index2 = 0; index2 < 200; ++index2)
          {
            if (this.map[index1, index2].Solid)
            {
              bounds = this.level.Bounds;
              double x = (double) (bounds.Left + index1 * 8);
              bounds = this.level.Bounds;
              double y = (double) (bounds.Top + index2 * 8);
              Color color = Color.Red * 0.25f;
              Draw.Rect((float) x, (float) y, 8f, 8f, color);
            }
          }
        }
        if (this.lastPath != null)
        {
          Vector2 start = this.lastPath[0];
          for (int index = 1; index < this.lastPath.Count; ++index)
          {
            Vector2 end = this.lastPath[index];
            Draw.Line(start, end, Color.Red);
            Draw.Rect(start.X - 2f, start.Y - 2f, 4f, 4f, Color.Red);
            start = end;
          }
          Draw.Rect(start.X - 2f, start.Y - 2f, 4f, 4f, Color.Red);
        }
        bounds = this.level.Bounds;
        double x1 = (double) (bounds.Left + this.debugLastStart.X * 8 + 2);
        bounds = this.level.Bounds;
        double y1 = (double) (bounds.Top + this.debugLastStart.Y * 8 + 2);
        Color green1 = Color.Green;
        Draw.Rect((float) x1, (float) y1, 4f, 4f, green1);
        bounds = this.level.Bounds;
        double x2 = (double) (bounds.Left + this.debugLastEnd.X * 8 + 2);
        bounds = this.level.Bounds;
        double y2 = (double) (bounds.Top + this.debugLastEnd.Y * 8 + 2);
        Color green2 = Color.Green;
        Draw.Rect((float) x2, (float) y2, 4f, 4f, green2);
      }

      private struct Tile
      {
        public bool Solid;
        public int Cost;
        public Point? Parent;
      }

      private class PointMapComparer : IComparer<Point>
      {
        private Pathfinder.Tile[,] map;

        public PointMapComparer(Pathfinder.Tile[,] map) => this.map = map;

        public int Compare(Point a, Point b) => this.map[b.X, b.Y].Cost - this.map[a.X, a.Y].Cost;
      }
    }
}
