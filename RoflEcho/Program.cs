using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace RoflEcho
{
    static class Program
    {
        static readonly string ReplayArgsPath = Path.Combine(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName), "ReplayArgs.txt");

        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please play some replay to fetch command line arguments.");
                Console.WriteLine("リプレイの実行引数を取得します。何らかのリプレイを再生してください。");
                var replayArgs = FetchReplayArgs();

                File.WriteAllText(ReplayArgsPath, replayArgs);

                Console.WriteLine();
                Console.WriteLine("Successfully fetched.");
                Console.WriteLine("リプレイの実行引数を取得出来ました。");
                Console.WriteLine(replayArgs);

                Console.WriteLine();
                Console.WriteLine("Now you can play rofl files by dropping file on this executable or via file association.");
                Console.WriteLine("rofl ファイルをこの exe にドラッグするか関連付けをすることで再生が可能です。");

                Console.WriteLine();
                Console.WriteLine("Press any key to quit.");
                Console.WriteLine("終了するには何かのキーを押してください。");
                Console.ReadKey();
            }
            else if (args.Length == 1)
            {
                if (!File.Exists(ReplayArgsPath))
                {
                    Console.WriteLine("There's no replay command line arguments file. Please run without rofl file first.");
                    Console.WriteLine("リプレイの実行引数がありません。先に単体で起動してください。");
                    Console.WriteLine();
                    Console.WriteLine("Press any key to quit.");
                    Console.WriteLine("終了するには何かのキーを押してください。");
                    Console.ReadKey();
                    return;
                }

                var replayArgs = new ReplayArgs(File.ReadAllText(ReplayArgsPath))
                {
                    ReplayPath = new FileInfo(args[0])
                };
                replayArgs.ExecuteReplay();
            }
            else
            {
                Console.WriteLine("Too many arguments. Please drop only one single rofl file.");
                Console.WriteLine("引数が多すぎます。ファイルは一つだけにしてください。");
                Console.WriteLine();
                Console.WriteLine("Press any key to quit.");
                Console.WriteLine("終了するには何かのキーを押してください。");
                Console.ReadKey();
            }
        }

        static string FetchReplayArgs()
        {
            while (true)
            {
                var (replayArgs, e) = ReplayArgsFetcher.Fetch();
                if (e == null)
                {
                    return replayArgs;
                }

                switch (e)
                {
                    case ClientCantGetArgumentsException _:
                    {
                        Console.WriteLine("Couldn't fetch correct replay arguments from the client.");
                        Console.WriteLine("クライアントから正しい引数を取得出来ませんでした。");
                        break;
                    }

                    case ClientNotFoundException _:
                    {
                        Console.WriteLine("Couldn't find the client. Please play some replay.");
                        Console.WriteLine("クライアントを見つけられませんでした。リプレイを再生してください。");
                        break;
                    }
                }

                Thread.Sleep(2000);
            }
        }
    }
}
