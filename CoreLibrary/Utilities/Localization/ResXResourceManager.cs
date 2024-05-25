using System.Collections;
using System.Resources.NetStandard;
using CoreLibrary.Utilities.Localization.Model;

namespace CoreLibrary.Utilities.Localization;

public static class ResXResourceManager
{
    public static List<ResXReaderResponseModel> ReadResxFile(List<string> pathList)
    {
        List<ResXReaderResponseModel> l = new();

        for (int i = 0; i < pathList.Count; i++)
        {
            using ResXResourceReader resxReader = new ResXResourceReader(pathList[i]);

            switch (i)
            {
                case 0:
                {
                    foreach (DictionaryEntry entry in resxReader)
                    {
                        string? key = entry.Key.ToString();
                        string? value = entry.Value?.ToString();

                        l.Add(new ResXReaderResponseModel
                        {
                            Key = key,
                            Item = new ResXReaderItemModel
                            {
                                Value1 = value
                            }
                        });
                    }

                    break;
                }
                case 1:
                {
                    foreach (DictionaryEntry entry in resxReader)
                    {
                        string? key = entry.Key.ToString();
                        string? value = entry.Value?.ToString();

                        var data = l.FirstOrDefault(c => c.Key == key);
                        
                        if (data is not null)
                        {
                            l.Remove(data);
                            
                            data.Item.Value2 = value;
                            
                            l.Add(data);
                        }
                    }

                    break;
                }
            }
        }

        return l;
    }

    public static void WriteResxItem(string path, string key, string value)
    {
        ResXResourceReader resources = new ResXResourceReader(path);
        Hashtable resourceDict = new Hashtable();

        foreach (DictionaryEntry entry in resources)
        {
            resourceDict.Add(entry.Key, entry.Value);
        }

        resources.Close();

        resourceDict[key] = value;

        using ResXResourceWriter resxWriter = new ResXResourceWriter(path);

        foreach (DictionaryEntry entry in resourceDict)
        {
            resxWriter.AddResource(entry.Key.ToString()!, entry.Value);
        }
    }
}