using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace dotnet_code_challenge.DataService
{
    public static class ReadFileName
    {

        public static List<string> ReadFileNames(string dataFolderPath, string type)
        {
            try
            {
                var fileNames = Directory.GetFiles(dataFolderPath, "*." + type);
                if(fileNames != null)
                    return fileNames.ToList<string>();
            }
            catch (Exception ex)
            {
                //log details
            }

            return null;
        }
    }
}
