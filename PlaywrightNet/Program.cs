
// .NET information
using Microsoft.Playwright;
using PlaywrightNet;
using System.Net;
using System.Runtime.InteropServices;
using static System.Console;

//add argument when calling docker run or include commandLineArgs in launchSettings.json.
//Or just leave as it always try to install
if (args.Length > 0 && args[0] == "-i")
    PerformInitialPlaywrightInstall();

WriteLine(RuntimeInformation.FrameworkDescription);

// OS information
const string OSRel = "/etc/os-release";
if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) &&
    File.Exists(OSRel))
{
    const string PrettyName = "PRETTY_NAME";
    foreach (string line in File.ReadAllLines(OSRel))
    {
        if (line.StartsWith(PrettyName))
        {
            ReadOnlySpan<char> value = line.AsSpan()[(PrettyName.Length + 2)..^1];
            WriteLine(value.ToString());
            break;
        }
    }
}
else
{
    WriteLine(RuntimeInformation.OSDescription);
}

WriteLine();

const long Mebi = 1024 * 1024;
const long Gibi = Mebi * 1024;
GCMemoryInfo gcInfo = GC.GetGCMemoryInfo();
long totalMemoryBytes = gcInfo.TotalAvailableMemoryBytes;

// Environment information
WriteLine($"{nameof(Environment.UserName)}: {Environment.UserName}");
WriteLine($"{nameof(RuntimeInformation.OSArchitecture)}: {RuntimeInformation.OSArchitecture}");
WriteLine($"{nameof(Environment.ProcessorCount)}: {Environment.ProcessorCount}");
WriteLine($"{nameof(GCMemoryInfo.TotalAvailableMemoryBytes)}: {totalMemoryBytes} ({GetInBestUnit(totalMemoryBytes)})");
WriteLine($"HostName : {Dns.GetHostName()}");

PlayTest.BrowseAsync().GetAwaiter().GetResult();

//Console.Read();

string GetInBestUnit(long size)
{
    if (size < Mebi)
    {
        return $"{size} bytes";
    }
    else if (size < Gibi)
    {
        double mebibytes = (double)(size / Mebi);
        return $"{mebibytes:F} MiB";
    }
    else
    {
        double gibibytes = (double)(size / Gibi);
        return $"{gibibytes:F} GiB";
    }
}

static void PerformInitialPlaywrightInstall()
{
    var exitCode = Microsoft.Playwright.Program.Main(new[] { "install", "--with-deps", "chromium" });
    if (exitCode != 0)
    {
        throw new Exception($"Playwright exited with code {exitCode}");
    }
}
