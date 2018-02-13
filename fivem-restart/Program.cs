using LibGit2Sharp;
using System;
using System.Diagnostics;
using System.IO;

namespace fivem_restart
{
    class Program
    {
        public static string currentPath = Directory.GetCurrentDirectory();
        public static string logFile = "CitizenFX.log";

        static void Main(string[] args)
        {
            Console.Title = "FiveM server";
            Console.WriteLine("FiveM FXServer launcher developed by the SCRP team");
            Console.WriteLine("v1.0.0");

            CheckGit();

            if (File.Exists(logFile)) {
                File.Delete(logFile);
            }

            // LaunchServer();

            Console.ReadKey();
        }

        private static void LaunchServer()
        {
            //fxserver.exe
            //@"+set citizen_dir " + currentPath + @"\citizen\ +exec server.cfg"
        }

        private static void CheckGit()
        {
            Console.WriteLine("Fetching all scripts from git . . .");
            foreach (string path in Directory.GetFiles(currentPath + @"\resources", "README.md", SearchOption.AllDirectories))
            {
                PullProject(path);
            }
        }

        private static void LaunchProcess(string fileName, string arguments)
        {
            var processInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                FileName = fileName,
                Arguments = arguments
            };

            using (var process = Process.Start(processInfo))
            {
                process.WaitForExit();
            }
        }

        private static void PullProject(string directoryPath)
        {

            try {
                using (var repo = new Repository(Path.GetDirectoryName(directoryPath))) {
                    PullOptions options = new PullOptions();
                    options.FetchOptions = new FetchOptions();

                    Commands.Pull(repo, new Signature(new Identity("SCRP0", ""), new DateTimeOffset(DateTime.Now)), options);
                }
            } catch (RepositoryNotFoundException) {
                //Console.WriteLine();
            } catch (Exception ex) {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
