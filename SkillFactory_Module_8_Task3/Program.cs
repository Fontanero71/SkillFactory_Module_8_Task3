using System;
using System.IO;

namespace ReadMyFile
{
    class Program
    {
        public static void Main()
        {

            string DirPath = "";
            long totalVol;
            long totalVolafterCleaning;

            while (!Directory.Exists(DirPath))
            {
                Console.WriteLine("Enter a directory to succeed:"); //Вводим путь директория
                DirPath = Console.ReadLine();
            }

            DirectoryInfo myDir = new DirectoryInfo(DirPath);


            try
            {
                DirectoryInfo[] diArr = myDir.GetDirectories(); // Получаем вложенные директории
                FileInfo[] fiArr = myDir.GetFiles(); // Получаем файлы в директории

                totalVol = Root_Total_Value(myDir,0); //Вызываем метод подсчета объема файлов во всех директориях
                Console.WriteLine("Directory volume before cleaning - " + totalVol + " bytes");
                Console.ReadKey();

                DirDel(diArr); //Вызываем метод удаления директориев
                FileDel(fiArr); //Вызываем метод удаления файлов

                totalVolafterCleaning = Root_Total_Value(myDir, 0);  //Вызываем метод подсчета объема файлов во всех директориях
                Console.WriteLine("Directory volume after cleaning - " + totalVolafterCleaning + " bytes");


            }
            catch (Exception ex)
            {
                Console.WriteLine("Enter error"); //Вывод сообщения о некорректном вводе
                Console.WriteLine(ex.ToString()); //Детализация
            }
            finally
            {
                Console.WriteLine("Thanks for the job");
            }
        }

        static void DirDel(DirectoryInfo[] vs) //Метод удаления директориев
        {
            double duration;
            int dirDel = 0;

            for (int i = 0; i < vs.Length; i++) //Проходим по списку директориев
            {
                duration = (DateTime.Now - vs[i].LastAccessTime).TotalMinutes; //Получаем время последнего доступа (использования) директория
                if (duration > 30)                                             //Если прошло больше 30 мин - удаляем
                {
                    Console.WriteLine($"{vs[i].Name} Directory deleting");
                    vs[i].Delete(true);
                    dirDel++;
                }
            }
            Console.WriteLine(dirDel + " directories were deleted");
        }
        static void FileDel(FileInfo[] fl)  //Метод удаления файлов
        {
            double duration;
            int fileDel = 0;

            for (int i = 0; i < fl.Length; i++)  //Проходим по списку файлов
            {
                duration = (DateTime.Now - fl[i].LastAccessTime).TotalMinutes; //Получаем время последнего доступа (использования) файла
                if (duration > 30)                                             //Если прошло больше 30 мин - удаляем
                {
                    Console.WriteLine($"{fl[i].Name} File deleting");
                    fl[i].Delete();
                    fileDel++;
                }
            }
            Console.WriteLine(fileDel + " files were deleted");
        }

        //

        static long Root_Total_Value(DirectoryInfo dir, long r_vol) //Метод вычисления объема файлов в условно корневом директории
        {
            long r_sum = r_vol;
            FileInfo[] r_fileArr = dir.GetFiles();
            for (int i = 0; i < r_fileArr.Length; i++)
            {
                //Console.WriteLine("File " + r_fileArr[i].Name + " Volume " + r_fileArr[i].Length + " bytes");
                r_sum += r_fileArr[i].Length;
            }

            //Console.WriteLine("Files of the directory - " + dir.FullName);
            //Console.WriteLine("Volume - " + r_sum + " bytes");

            DirectoryInfo[] dirArr = dir.GetDirectories();
            r_sum += Total_Value(dirArr, r_sum); //Вызываем Метод вычесления объема файлов в поддиректориях

            return r_sum;
        }

        //
        static long Total_Value(DirectoryInfo[] workDir, long vol) //Метод вычесления объема файлов в поддиректориях
        {
            long sum = vol;

            for (int i = 0; i < workDir.Length; i++)
            {

                FileInfo[] fileArr = workDir[i].GetFiles();
                for (int j = 0; j < fileArr.Length; j++)
                {
                    //Console.WriteLine("File " + fileArr[j].Name + " Volume " + fileArr[j].Length + " bytes");
                    sum += fileArr[j].Length;
                }
                //Console.WriteLine("Files of the directory " + workDir[i].FullName);
                //Console.WriteLine("Volume " + sum + " bytes");
                sum += Total_Value(workDir[i].GetDirectories(), sum); //Вызываем Метод вычесления объема файлов в поддиректориях
            }
            return sum;
        }

    }
}