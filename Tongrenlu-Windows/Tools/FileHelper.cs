using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tongrenlu_Windows.Tools
{
    public class FileHelper
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(FileHelper));

        public static string readFileToString(string path)
        {
            if (!File.Exists(path))
            {
                return null;
            }
            
            FileStream fs = null;
            StreamReader sr = null;
            try
            {
                fs = new FileStream(path, FileMode.Open);
                sr = new StreamReader(fs);

                string text = sr.ReadToEnd();
                return text;
            }
            catch(Exception ex)
            {
                log.Error(ex.Message, ex);
                return null;
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }

        public static void WriteStringToFile(string path, string text)
        {
            FileStream fs = null;
            StreamWriter sw = null;
            try
            {
                fs = new FileStream(path, FileMode.Create);
                sw = new StreamWriter(fs);
                sw.WriteLine(text);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                }
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }
    }
}
