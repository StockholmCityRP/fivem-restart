using LibGit2Sharp;
using System;
using System.Diagnostics;
using System.IO;

namespace fivem_restart
{
    /*
    fivem-restart - Auto restart script for FiveM
    Copyright (C) 2018 SCRP team

    This program Is free software: you can redistribute it And/Or modify
    it under the terms Of the GNU General Public License As published by
    the Free Software Foundation, either version 3 Of the License, Or
    (at your option) any later version.

    This program Is distributed In the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty Of
    MERCHANTABILITY Or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License For more details.
    
    You should have received a copy Of the GNU General Public License
    along with this program.  If Not, see <http://www.gnu.org/licenses/>.
    */

    class Program
    {
        public static string currentPath = Directory.GetCurrentDirectory();
        public static string logFile = "CitizenFX.log";
        public static string fxserverName = "FXServer.exe";

        static void Main(string[] args)
        {
            Console.Title = "FiveM server";
            Console.WriteLine("fivem-restart v1.0.0");

            if (!File.Exists(fxserverName)) {
                Console.WriteLine("FXServer was not found, we cannot run the program! Press any key to exit the program.");
                Console.ReadKey();
                Environment.Exit(1);
            }

            CheckGit();

            if (File.Exists(logFile)) {
                Console.WriteLine("fivem-restart: removed old log file");
                File.Delete(logFile);
            }

            LaunchServer();
        }

        private static void LaunchServer()
        {
            Console.WriteLine("fivem-restart: starting fxserver . . .");
            LaunchProcess(fxserverName, @"+set citizen_dir " + currentPath + @"\citizen\ +exec server.cfg");
        }

        private static void CheckGit()
        {
            Console.WriteLine("fivem-restart: fetching all scripts from git");
            foreach (string path in Directory.GetFiles(currentPath + @"\resources", "README.md", SearchOption.AllDirectories)) {
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

                    Commands.Pull(repo, new Signature(new Identity("SCRP0", "scrpbot@gmail.com"), new DateTimeOffset(DateTime.Now)), options);
                }
            } catch (RepositoryNotFoundException) {
                //Console.WriteLine();
            } catch (Exception ex) {
                Console.WriteLine(directoryPath);
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
