using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SQLite;
using System.Data;
using System.Threading;
namespace ChromeLogCatch
{    
    class ChromeLog
    {
        public string lastlog;
        public string chromeHistoryFile;
        public string tempPath;
        public List<string> result_url;
        public ChromeLog()
        {            
            chromeHistoryFile = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            chromeHistoryFile += @"\Google\Chrome\User Data\Default\History";
            tempPath = Path.GetTempPath();
            lastlog = last_visited_time();

        }
        public void getChromeLog()
        {
            SQLiteConnection conn;
            SQLiteCommand cmd;
            SQLiteDataReader rdr;
            while (true)
            {
                result_url = new List<string>();
                string str_json;                
                File.Copy(chromeHistoryFile, tempPath + "/chrome.history", true);
                if (File.Exists(chromeHistoryFile))
                {
                    conn = new SQLiteConnection("Data Source=" + tempPath + "/chrome.history" + ";Version=3;New=False;Compress=True;");
                    conn.Open();
                    string sql = "select url,title,last_visit_time AS timestmp,datetime(last_visit_time / 1000000 + (strftime('%s', '1601-01-01')), 'unixepoch', 'localtime') AS last_visit_time from urls where last_visit_time > " + lastlog;
                    cmd = new SQLiteCommand(sql, conn);
                    rdr = cmd.ExecuteReader();                    
                    while (rdr.Read())
                    {
                        str_json = "{\"url\":\"" + rdr["url"] + "\",\"title\":\"" + rdr["title"] + "\",\"last_visit_time\":\"" + rdr["last_visit_time"] + "\"}";
                        result_url.Add(str_json);
                        Console.WriteLine(str_json);
                        lastlog = rdr["timestmp"].ToString();
                    }
                    if(result_url.Count !=0)
                        saveLastTime(lastlog);
                    rdr.Close();                    

                }
                Thread.Sleep(3000);
            }
            
        }
        public string last_visited_time()
        {            
            if(!File.Exists(tempPath + "/lastime.chrome"))
            {
                return "1";
            }
            StreamReader sr = new StreamReader(tempPath + "/lastime.chrome");
            string result = sr.ReadLine();
            sr.Close();
            Console.WriteLine("Read lasttime: "+result);
            return result;
            
        }
        public void saveLastTime(string lasttime)
        {
            if (!File.Exists(tempPath + "/lastime.chrome"))
            {
                File.Create(tempPath + "/lastime.chrome");
            }
            StreamWriter sw = new StreamWriter(tempPath + "/lastime.chrome");
            sw.Write(lasttime);
            sw.Close();
            Console.WriteLine("Write lasttime: " + lasttime);
        }
    }
    
}
