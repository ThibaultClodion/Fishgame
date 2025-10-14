using System;
using System.IO;
using UnityEngine;

public class Bestiary
{
    private const string bestiaryPath = "Bestiary";

    public static void NewCatch(FishData fishData)
    {
        BestiaryEntry entry = GetBestiaryEntry(fishData.Type);

        if(entry == null)
        {
            entry = new BestiaryEntry();
            entry.MaxSize = fishData.Length * fishData.Width;
            entry.MinSize = fishData.Length * fishData.Width;
            entry.NbCatched = 1;

            string json = JsonUtility.ToJson(entry, true);

            if(!Directory.Exists(GetBestiaryDirectory()))
                Directory.CreateDirectory(GetBestiaryDirectory());

            File.WriteAllText(GetFishTypePath(fishData.Type), json);
        }
        else
        {
            entry.MaxSize = Mathf.Max(entry.MaxSize, fishData.Length * fishData.Width);
            entry.MinSize = Mathf.Min(entry.MinSize, fishData.Length * fishData.Width);
            entry.NbCatched += 1;

            string json = JsonUtility.ToJson(entry, true);
            File.WriteAllText(GetFishTypePath(fishData.Type), json);
        }
    }

    public static BestiaryEntry GetBestiaryEntry(FishType type)
    {
        string path = GetFishTypePath(type);

        if (!File.Exists(path))
        {
            return null;
        }
        else
        {
            string json = File.ReadAllText(path);
            BestiaryEntry entry = JsonUtility.FromJson<BestiaryEntry>(json);

            return entry;
        }
    }

    private static string GetBestiaryDirectory()
    {
        return Path.Combine(Application.persistentDataPath, bestiaryPath);
    }

    private static string GetFishTypePath(FishType type)
    {
        return Path.Combine(GetBestiaryDirectory(), type.name + ".json");
    }
}

[Serializable]
public class BestiaryEntry
{
    public float MaxSize;
    public float MinSize;
    public int NbCatched;
}
