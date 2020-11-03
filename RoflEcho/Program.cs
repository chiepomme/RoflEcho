using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace RoflEcho
{
    static class Program
    {
        static readonly string ReplayArgsPath = Path.Combine(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName), "ReplayArgs.txt");

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Fetch();
            }
            else if (args.Length == 1)
            {
                if (!File.Exists(ReplayArgsPath))
                {
                    Fetch();
                }

                if (!args[0].ToLower().EndsWith(".rofl"))
                {
                    Console.WriteLine("This is not a replay file(.rofl)");
                    Console.WriteLine("����� rofl �t�@�C���ł͂���܂���B");
                    Finish();
                    return;
                }

                Play(args[0]);
            }
            else
            {
                Console.WriteLine("Too many arguments. Please drop only one single rofl file.");
                Console.WriteLine("�������������܂��B�t�@�C���͈�����ɂ��Ă��������B");
                Finish();
            }
        }

        static void Fetch()
        {
            Console.WriteLine("Please play some replay to fetch command line arguments.");
            Console.WriteLine("���v���C�̎��s�������擾���܂��B���炩�̃��v���C���Đ����Ă��������B");

            var replayArgs = FetchReplayArgs();
            var stringifiedArgs = ReplayArgsStringifier.Stringify(replayArgs, withExecutable: true);
            File.WriteAllText(ReplayArgsPath, stringifiedArgs);

            Console.WriteLine();
            Console.WriteLine("Successfully fetched.");
            Console.WriteLine("���v���C�̎��s�������擾�o���܂����B");
            Console.WriteLine(stringifiedArgs);

            Console.WriteLine();
            Console.WriteLine("Now you can play rofl files by dropping file on this executable or via file association.");
            Console.WriteLine("����� rofl �t�@�C�������� exe �Ƀh���b�O���邩�֘A�t�������邱�ƂōĐ����\�ł��B");
            Console.WriteLine();
        }

        static ReplayArgs FetchReplayArgs()
        {
            while (true)
            {
                var process = ClientProcessFinder.Find();
                if (process == null || process.HasExited)
                {
                    Console.WriteLine("Couldn't find the client. Please play some replay.");
                    Console.WriteLine("�N���C�A���g�v���Z�X���������܂���ł����B���v���C���Đ����Ă��������B");
                    Thread.Sleep(2000);
                    continue;
                }

                var replayArgs = ReplayArgsFetcher.Fetch(process);
                if (replayArgs == null)
                {
                    Console.WriteLine("Couldn't fetch correct replay arguments from the client.");
                    Console.WriteLine("�N���C�A���g���琳�����������擾�o���܂���ł����B");
                    Thread.Sleep(2000);
                    continue;
                }

                return replayArgs;
            }
        }

        static void Play(string replayPath)
        {
            while (true)
            {
                var process = ClientProcessFinder.Find();
                if (process == null)
                {
                    break;
                }

                Console.WriteLine("The client has been already running. Please close it first.");
                Console.WriteLine("�N���C�A���g�v���Z�X�����ɋN�����Ă��܂��B��ɏI�����Ă��������B");
                Thread.Sleep(2000);
                continue;
            }

            var replayArgs = ReplayArgsParser.Parse(File.ReadAllText(ReplayArgsPath));
            replayArgs.ReplayFile = new FileInfo(replayPath);
            ReplayPlayer.Play(replayArgs);
        }

        static void Finish()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to quit.");
            Console.WriteLine("�I������ɂ͉����̃L�[�������Ă��������B");
            Console.ReadKey();
        }
    }
}
