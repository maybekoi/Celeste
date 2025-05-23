// Decompiled with JetBrains decompiler
// Type: Celeste.UserIO
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using Monocle;
using System;
using System.Collections;
using System.IO;
using System.Xml.Serialization;

namespace Celeste
{

    public static class UserIO
    {
      public const string SaveDataTitle = "Celeste Save Data";
      private const string SavePath = "Saves";
      private const string BackupPath = "Backups";
      private const string Extension = ".celeste";
      private static bool savingInternal;
      private static bool savingFile;
      private static bool savingSettings;
      private static byte[] savingFileData;
      private static byte[] savingSettingsData;

      private static string GetHandle(string name) => Path.Combine("Saves", name + ".celeste");

      private static string GetBackupHandle(string name) => Path.Combine("Backups", name + ".celeste");

      public static bool Open(UserIO.Mode mode) => true;

      public static bool Save<T>(string path, byte[] data) where T : class
      {
        string handle = UserIO.GetHandle(path);
        bool flag = false;
        try
        {
          string backupHandle = UserIO.GetBackupHandle(path);
          DirectoryInfo directory1 = new FileInfo(handle).Directory;
          if (!directory1.Exists)
            directory1.Create();
          DirectoryInfo directory2 = new FileInfo(backupHandle).Directory;
          if (!directory2.Exists)
            directory2.Create();
          using (FileStream fileStream = File.Open(backupHandle, FileMode.Create, FileAccess.Write))
            fileStream.Write(data, 0, data.Length);
          if ((object) UserIO.Load<T>(path, true) != null)
          {
            File.Copy(backupHandle, handle, true);
            flag = true;
          }
        }
        catch (Exception ex)
        {
          Console.WriteLine("ERROR: " + ex.ToString());
          ErrorLog.Write(ex);
        }
        if (!flag)
          Console.WriteLine("Save Failed");
        return flag;
      }

      public static T Load<T>(string path, bool backup = false) where T : class
      {
        string path1 = !backup ? UserIO.GetHandle(path) : UserIO.GetBackupHandle(path);
        T obj = default (T);
        try
        {
          if (File.Exists(path1))
          {
            using (FileStream fileStream = File.OpenRead(path1))
              obj = UserIO.Deserialize<T>((Stream) fileStream);
          }
        }
        catch (Exception ex)
        {
          Console.WriteLine("ERROR: " + ex.ToString());
          ErrorLog.Write(ex);
        }
        return obj;
      }

      private static T Deserialize<T>(Stream stream) where T : class
      {
        return (T) new XmlSerializer(typeof (T)).Deserialize(stream);
      }

      public static bool Exists(string path) => File.Exists(UserIO.GetHandle(path));

      public static bool Delete(string path)
      {
        string handle = UserIO.GetHandle(path);
        if (!File.Exists(handle))
          return false;
        File.Delete(handle);
        return true;
      }

      public static void Close()
      {
      }

      public static byte[] Serialize<T>(T instance)
      {
        using (MemoryStream memoryStream = new MemoryStream())
        {
          new XmlSerializer(typeof (T)).Serialize((Stream) memoryStream, (object) instance);
          return memoryStream.ToArray();
        }
      }

      public static bool Saving { get; private set; }

      public static bool SavingResult { get; private set; }

      public static void SaveHandler(bool file, bool settings)
      {
        if (UserIO.Saving)
          return;
        UserIO.Saving = true;
        Celeste.SaveRoutine = new Coroutine(UserIO.SaveRoutine(file, settings));
      }

      private static IEnumerator SaveRoutine(bool file, bool settings)
      {
        UserIO.savingFile = file;
        UserIO.savingSettings = settings;
        FileErrorOverlay menu;
        do
        {
          if (UserIO.savingFile)
          {
            SaveData.Instance.BeforeSave();
            UserIO.savingFileData = UserIO.Serialize<SaveData>(SaveData.Instance);
          }
          if (UserIO.savingSettings)
            UserIO.savingSettingsData = UserIO.Serialize<Settings>(Settings.Instance);
          UserIO.savingInternal = true;
          UserIO.SavingResult = false;
          RunThread.Start(new Action(UserIO.SaveThread), "USER_IO");
          SaveLoadIcon.Show(Engine.Scene);
          while (UserIO.savingInternal)
            yield return (object) null;
          SaveLoadIcon.Hide();
          if (!UserIO.SavingResult)
          {
            menu = new FileErrorOverlay(FileErrorOverlay.Error.Save);
            while (menu.Open)
              yield return (object) null;
          }
          else
            goto label_14;
        }
        while (menu.TryAgain);
        menu = (FileErrorOverlay) null;
    label_14:
        UserIO.Saving = false;
        Celeste.SaveRoutine = (Coroutine) null;
      }

      private static void SaveThread()
      {
        UserIO.SavingResult = false;
        if (UserIO.Open(UserIO.Mode.Write))
        {
          UserIO.SavingResult = true;
          if (UserIO.savingFile)
            UserIO.SavingResult &= UserIO.Save<SaveData>(SaveData.GetFilename(), UserIO.savingFileData);
          if (UserIO.savingSettings)
            UserIO.SavingResult &= UserIO.Save<Settings>("settings", UserIO.savingSettingsData);
          UserIO.Close();
        }
        UserIO.savingInternal = false;
      }

      public enum Mode
      {
        Read,
        Write,
      }
    }
}
