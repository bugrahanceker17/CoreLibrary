﻿
namespace CoreLibrary.Utilities.ObjectStorage.Base
{
    public class Storage
    {
        protected delegate bool HasFile(string pathOrContainerName, string fileName);

        protected async Task<string> FileRenameAsync(string pathOrContainerName, string fileName, HasFile hasFileMethod, bool first = true)
        {
            string newFileName = await Task.Run(async () =>
            {
                string extension = Path.GetExtension(fileName);
                string newFileName = string.Empty;
                if (first)
                {
                    string oldName = Path.GetFileNameWithoutExtension(fileName);
                    newFileName = $"{CharacterRegulatory(oldName)}{extension}";
                }
                else
                {
                    newFileName = fileName;
                    int indexNo1 = newFileName.IndexOf("-");
                    if (indexNo1 == -1)
                        newFileName = $"{Path.GetFileNameWithoutExtension(newFileName)}-2{extension}";
                    else
                    {
                        int lastIndex = 0;
                        while (true)
                        {
                            lastIndex = indexNo1;
                            indexNo1 = newFileName.IndexOf("-", indexNo1 + 1);
                            if (indexNo1 == -1)
                            {
                                indexNo1 = lastIndex;
                                break;
                            }
                        }

                        int indexNo2 = newFileName.IndexOf(".");
                        string fileNo = newFileName.Substring(indexNo1 + 1, indexNo2 - indexNo1 - 1);

                        if (int.TryParse(fileNo, out int _fileNo))
                        {
                            _fileNo++;
                            newFileName = newFileName.Remove(indexNo1 + 1, indexNo2 - indexNo1 - 1)
                                .Insert(indexNo1 + 1, _fileNo.ToString());
                        }
                        else
                            newFileName = $"{Path.GetFileNameWithoutExtension(newFileName)}-2{extension}";
                    }
                }

                if (hasFileMethod(pathOrContainerName, newFileName))
                    return await FileRenameAsync(pathOrContainerName, newFileName, hasFileMethod, false);
                else
                    return newFileName;
            });

            return newFileName;
        }

        private static string CharacterRegulatory(string name)
            => name.Replace("\"", "")
                .Replace("!", "")
                .Replace("'", "")
                .Replace("^", "")
                .Replace("+", "")
                .Replace("%", "")
                .Replace("&", "")
                .Replace("/", "")
                .Replace("(", "")
                .Replace(")", "")
                .Replace("=", "")
                .Replace("?", "")
                .Replace("_", "")
                .Replace(" ", "-")
                .Replace("@", "")
                .Replace("€", "")
                .Replace("¨", "")
                .Replace("~", "")
                .Replace(",", "")
                .Replace(";", "")
                .Replace(":", "")
                .Replace(".", "-")
                .Replace("Ö", "o")
                .Replace("ö", "o")
                .Replace("Ü", "u")
                .Replace("ü", "u")
                .Replace("ı", "i")
                .Replace("İ", "i")
                .Replace("ğ", "g")
                .Replace("Ğ", "g")
                .Replace("æ", "")
                .Replace("ß", "")
                .Replace("â", "a")
                .Replace("î", "i")
                .Replace("ş", "s")
                .Replace("Ş", "s")
                .Replace("Ç", "c")
                .Replace("ç", "c")
                .Replace("<", "")
                .Replace(">", "")
                .Replace("|", "");
    }
}