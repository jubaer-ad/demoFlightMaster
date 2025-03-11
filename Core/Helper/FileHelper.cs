using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helper
{
    public abstract class FileHelper
    {
        private static string GeneratePath(string apiName, string logType)
        {
            var basePath = Environment.CurrentDirectory;
            return Path.Combine(basePath, "Logs", apiName, logType);
        }
        public static async void JsonFileSaveAsync(string fileName, string jsonString, string apiName, string logType)
        {
            string path = GeneratePath(apiName, logType);

            try
            {
                if (Directory.Exists(path))
                {
                    path += @"\" + fileName + ".json";
                    await File.WriteAllTextAsync(path, jsonString);
                }
                else
                {
                    Directory.CreateDirectory(path);
                    path += @"\" + fileName + ".json";
                    await File.WriteAllTextAsync(path, jsonString);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(ex);
                    if (Directory.Exists(path))
                    {
                        path += @"\" + fileName + ".json";
                        await File.WriteAllTextAsync(path, jsonString);
                    }
                    else
                    {

                        Directory.CreateDirectory(path);
                        path += @"\" + fileName + ".json";
                        await File.WriteAllTextAsync(path, jsonString);
                    }

                }
                catch (Exception)
                {

                }

            }
        }
        public static async Task<string> JsonFileRetriveAsync(string fileName, string apiName, string logType)
        {
            string jsonString = string.Empty;
            try
            {
                string path = GeneratePath(apiName, logType);

                if (Directory.Exists(path))
                {
                    path += $"\\{fileName + ".json"}";
                    jsonString = await File.ReadAllTextAsync(path);
                }
            }
            catch (Exception)
            {

            }
            return jsonString;

        }
    }
}
