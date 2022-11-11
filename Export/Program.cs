// See https://aka.ms/new-console-template for more information


using Salvini.IoTDB;
using System.IO.Compression;
using System.Text;

var usage = @"Apache IoTDB 数据导出工具 v(1.13.22)

--host=127.0.0.1
--port=6667
--user=root
--pwd=admin#123
--database=kylin
--zip=true|false
--start=2022-11-01
--end=2022-11-10 
--point=

调用示例如上, 去掉'--'将参数写入export.ini即可直接执行

";
Console.Title = "Apache IoTDB 数据导出工具 v(1.13.22)";
Console.OutputEncoding = Encoding.UTF8;
Console.ForegroundColor = ConsoleColor.Magenta;
Console.WriteLine(usage);
Console.ResetColor();

var startTime = DateTime.Today.AddDays(-1);
var endTime = DateTime.Today;
var host = "127.0.0.1";
var user = "root";
var pwd = "admin#123";
var port = "6667";
var database = "kylin";
var zip = true;
List<string> points = null;

if (File.Exists("export.ini"))
{
    var ini = File.ReadAllLines("export.ini", Encoding.UTF8);
    host = ini.FirstOrDefault(x => x.StartsWith("host="))?[5..] ?? host;
    port = ini.FirstOrDefault(x => x.StartsWith("port="))?[5..] ?? port;
    user = ini.FirstOrDefault(x => x.StartsWith("user="))?[5..] ?? user;
    pwd = ini.FirstOrDefault(x => x.StartsWith("pwd="))?[4..] ?? pwd;
    database = ini.FirstOrDefault(x => x.StartsWith("database="))?[9..] ?? database;
    zip = bool.Parse(ini.FirstOrDefault(x => x.StartsWith("zip="))?[4..] ?? zip.ToString());
    startTime = DateTime.Parse(ini.FirstOrDefault(x => x.StartsWith("start="))?[6..] ?? startTime.ToString("yyyy-MM-dd HH:mm:ss"));
    endTime = DateTime.Parse(ini.FirstOrDefault(x => x.StartsWith("end="))?[4..] ?? endTime.ToString("yyyy-MM-dd HH:mm:ss"));
    points = ini.FirstOrDefault(x => x.StartsWith("point="))?[6..].Split(',').ToList();
}

host = args.FirstOrDefault(x => x.StartsWith("--host="))?[7..] ?? host;
port = args.FirstOrDefault(x => x.StartsWith("--port="))?[7..] ?? port;
user = args.FirstOrDefault(x => x.StartsWith("--user="))?[7..] ?? user;
pwd = args.FirstOrDefault(x => x.StartsWith("--pwd="))?[6..] ?? pwd;
database = args.FirstOrDefault(x => x.StartsWith("--database="))?[11..] ?? database;
zip = bool.Parse(args.FirstOrDefault(x => x.StartsWith("--zip="))?[6..] ?? zip.ToString());
startTime = DateTime.Parse(args.FirstOrDefault(x => x.StartsWith("--start="))?[8..] ?? startTime.ToString("yyyy-MM-dd HH:mm:ss"));
endTime = DateTime.Parse(args.FirstOrDefault(x => x.StartsWith("--end="))?[6..] ?? endTime.ToString("yyyy-MM-dd HH:mm:ss"));
points = args.FirstOrDefault(x => x.StartsWith("--point="))?[8..].Split(',').ToList() ?? points;

var client = new TimeSeriesClient($"iotdb://{user}:{pwd}@{host}:{port}/admin?database={database}");
client.DataToCsvAsync(startTime, endTime, points).Wait();
if (zip)
{
    var target = $"iotdb_export_{startTime:yyyy-MM-dd}_{endTime:yyyy-MM-dd}.zip";
    if (File.Exists(target)) File.Delete(target);

    Console.ForegroundColor = ConsoleColor.Magenta;
    Console.Write("正在压缩数据目录,请等待.");
    Task.Run(async () =>
    {
        while (Directory.Exists("csv"))
        {
            await Task.Delay(256);
            Console.Write('.');
        };
        Console.WriteLine();
        Console.WriteLine($"数据文件：{target}");
        Console.ResetColor();
    });

    ZipFile.CreateFromDirectory("csv", target);
    Directory.Delete("csv", true);
}
Thread.Sleep(2000);