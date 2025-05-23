// Decompiled with JetBrains decompiler
// Type: Monocle.VirtualTexture
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;

namespace Monocle
{

    public class VirtualTexture : VirtualAsset
    {
      private const int ByteArraySize = 524288 /*0x080000*/;
      private const int ByteArrayCheckSize = 524256;
      internal static readonly byte[] buffer = new byte[67108864 /*0x04000000*/];
      internal static readonly byte[] bytes = new byte[524288 /*0x080000*/];
      public Texture2D Texture;
      private Color color;

      public string Path { get; private set; }

      public bool IsDisposed
      {
        get
        {
          return this.Texture == null || this.Texture.IsDisposed || this.Texture.GraphicsDevice.IsDisposed;
        }
      }

      internal VirtualTexture(string path)
      {
        this.Name = this.Path = path;
        this.Reload();
      }

      internal VirtualTexture(string name, int width, int height, Color color)
      {
        this.Name = name;
        this.Width = width;
        this.Height = height;
        this.color = color;
        this.Reload();
      }

      internal override void Unload()
      {
        if (this.Texture != null && !this.Texture.IsDisposed)
          this.Texture.Dispose();
        this.Texture = (Texture2D) null;
      }

      internal override unsafe void Reload()
      {
        this.Unload();
        if (string.IsNullOrEmpty(this.Path))
        {
          this.Texture = new Texture2D(Engine.Instance.GraphicsDevice, this.Width, this.Height);
          Color[] data = new Color[this.Width * this.Height];
          fixed (Color* colorPtr = data)
          {
            for (int index = 0; index < data.Length; ++index)
              colorPtr[index] = this.color;
          }
          this.Texture.SetData<Color>(data);
        }
        else
        {
          switch (System.IO.Path.GetExtension(this.Path))
          {
            case ".data":
              using (FileStream fileStream = File.OpenRead(System.IO.Path.Combine(Engine.ContentDirectory, this.Path)))
              {
                fileStream.Read(VirtualTexture.bytes, 0, 524288 /*0x080000*/);
                int startIndex = 0;
                int int32_1 = BitConverter.ToInt32(VirtualTexture.bytes, startIndex);
                int int32_2 = BitConverter.ToInt32(VirtualTexture.bytes, startIndex + 4);
                bool flag = VirtualTexture.bytes[startIndex + 8] == (byte) 1;
                int index1 = startIndex + 9;
                int elementCount = int32_1 * int32_2 * 4;
                int index2 = 0;
                fixed (byte* numPtr1 = VirtualTexture.bytes)
                  fixed (byte* numPtr2 = VirtualTexture.buffer)
                  {
                    while (index2 < elementCount)
                    {
                      int num1 = (int) numPtr1[index1] * 4;
                      if (flag)
                      {
                        byte num2 = numPtr1[index1 + 1];
                        if (num2 > (byte) 0)
                        {
                          numPtr2[index2] = numPtr1[index1 + 4];
                          numPtr2[index2 + 1] = numPtr1[index1 + 3];
                          numPtr2[index2 + 2] = numPtr1[index1 + 2];
                          numPtr2[index2 + 3] = num2;
                          index1 += 5;
                        }
                        else
                        {
                          numPtr2[index2] = (byte) 0;
                          numPtr2[index2 + 1] = (byte) 0;
                          numPtr2[index2 + 2] = (byte) 0;
                          numPtr2[index2 + 3] = (byte) 0;
                          index1 += 2;
                        }
                      }
                      else
                      {
                        numPtr2[index2] = numPtr1[index1 + 3];
                        numPtr2[index2 + 1] = numPtr1[index1 + 2];
                        numPtr2[index2 + 2] = numPtr1[index1 + 1];
                        numPtr2[index2 + 3] = byte.MaxValue;
                        index1 += 4;
                      }
                      if (num1 > 4)
                      {
                        int index3 = index2 + 4;
                        for (int index4 = index2 + num1; index3 < index4; index3 += 4)
                        {
                          numPtr2[index3] = numPtr2[index2];
                          numPtr2[index3 + 1] = numPtr2[index2 + 1];
                          numPtr2[index3 + 2] = numPtr2[index2 + 2];
                          numPtr2[index3 + 3] = numPtr2[index2 + 3];
                        }
                      }
                      index2 += num1;
                      if (index1 > 524256)
                      {
                        int offset = 524288 /*0x080000*/ - index1;
                        for (int index5 = 0; index5 < offset; ++index5)
                          numPtr1[index5] = numPtr1[index1 + index5];
                        fileStream.Read(VirtualTexture.bytes, offset, 524288 /*0x080000*/ - offset);
                        index1 = 0;
                      }
                    }
                  }
                this.Texture = new Texture2D(Engine.Graphics.GraphicsDevice, int32_1, int32_2);
                this.Texture.SetData<byte>(VirtualTexture.buffer, 0, elementCount);
                break;
              }
            case ".png":
              using (FileStream fileStream = File.OpenRead(System.IO.Path.Combine(Engine.ContentDirectory, this.Path)))
                this.Texture = Texture2D.FromStream(Engine.Graphics.GraphicsDevice, (Stream) fileStream);
              int elementCount1 = this.Texture.Width * this.Texture.Height;
              Color[] data = new Color[elementCount1];
              this.Texture.GetData<Color>(data, 0, elementCount1);
              fixed (Color* colorPtr = data)
              {
                for (int index = 0; index < elementCount1; ++index)
                {
                  colorPtr[index].R = (byte) ((double) colorPtr[index].R * ((double) colorPtr[index].A / (double) byte.MaxValue));
                  colorPtr[index].G = (byte) ((double) colorPtr[index].G * ((double) colorPtr[index].A / (double) byte.MaxValue));
                  colorPtr[index].B = (byte) ((double) colorPtr[index].B * ((double) colorPtr[index].A / (double) byte.MaxValue));
                }
              }
              this.Texture.SetData<Color>(data, 0, elementCount1);
              break;
            case ".xnb":
              this.Texture = Engine.Instance.Content.Load<Texture2D>(this.Path.Replace(".xnb", ""));
              break;
            default:
              using (FileStream fileStream = File.OpenRead(System.IO.Path.Combine(Engine.ContentDirectory, this.Path)))
              {
                this.Texture = Texture2D.FromStream(Engine.Graphics.GraphicsDevice, (Stream) fileStream);
                break;
              }
          }
          this.Width = this.Texture.Width;
          this.Height = this.Texture.Height;
        }
      }

      public override void Dispose()
      {
        this.Unload();
        this.Texture = (Texture2D) null;
        VirtualContent.Remove((VirtualAsset) this);
      }
    }
}
