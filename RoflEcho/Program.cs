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
                Console.WriteLine("���v���C�̎��s�������擾���܂��B���炩�̃��v���C���Đ����Ă��������B");
                var replayArgs = FetchReplayArgs();

                File.WriteAllText(ReplayArgsPath, replayArgs);

                Console.WriteLine();
                Console.WriteLine("Successfully fetched.");
                Console.WriteLine("���v���C�̎��s�������擾�o���܂����B");
                Console.WriteLine(replayArgs);

                Console.WriteLine();
                Console.WriteLine("Now you can play rofl files by dropping file on this executable or via file association.");
                Console.WriteLine("rofl �t�@�C�������� exe �Ƀh���b�O���邩�֘A�t�������邱�ƂōĐ����\�ł��B");

                Console.WriteLine();
                Console.WriteLine("Press any key to quit.");
                Console.WriteLine("�I������ɂ͉����̃L�[�������Ă��������B");
                Console.ReadKey();
            }
            else if (args.Length == 1)
            {
                if (!File.Exists(ReplayArgsPath))
                {
                    Console.WriteLine("There's no replay command line arguments file. Please run without rofl file first.");
                    Console.WriteLine("���v���C�̎��s����������܂���B��ɒP�̂ŋN�����Ă��������B");
                    Console.WriteLine();
                    Console.WriteLine("Press any key to quit.");
                    Console.WriteLine("�I������ɂ͉����̃L�[�������Ă��������B");
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
                Console.WriteLine("�������������܂��B�t�@�C���͈�����ɂ��Ă��������B");
                Console.WriteLine();
                Console.WriteLine("Press any key to quit.");
                Console.WriteLine("�I������ɂ͉����̃L�[�������Ă��������B");
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
                        Console.WriteLine("�N���C�A���g���琳�����������擾�o���܂���ł����B");
                        break;
                    }

                    case ClientNotFoundException _:
                    {
                        Console.WriteLine("Couldn't find the client. Please play some replay.");
                        Console.WriteLine("�N���C�A���g���������܂���ł����B���v���C���Đ����Ă��������B");
                        break;
                    }
                }

                Thread.Sleep(2000);
            }
        }
    }
}