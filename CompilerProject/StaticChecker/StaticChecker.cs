using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticChecker
{
    public class StaticChecker
    {
        static void Main(string[] args)
        {
            int stillInProgram = 1;


            while (stillInProgram == 1)
            {
                string documentNameOrPath, path;
                List<String> lines = new List<String>();

                Console.WriteLine("Digite o nome do arquivo ou caminho completo: ");
                documentNameOrPath = Console.ReadLine();

                var splittedName = documentNameOrPath.Split('\\');


                //"inteligência" para saber se foi informado um path completo ou apenas o nome do documento.
                if (splittedName.Length > 1)
                {
                    path = documentNameOrPath;
                }
                else
                {
                    path = documentNameOrPath + ".201";
                }

                try
                {
                    using (StreamReader sr = new StreamReader(@path))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            lines.Add(line);
                        }

                        foreach (var str in lines)
                        {
                            Console.WriteLine(str);
                        }
                        Console.ReadKey();
                    }
                }
                catch
                {
                    Console.WriteLine("Nenhum arquivo com esse nome foi encontrado. Verifique se o nome do arquivo ou caminho dele estão corretos e tente novamente.");
                    Console.ReadKey();
                }

                Console.WriteLine("Deseja testar com outro arquivo?");
                Console.WriteLine("1. Sim");
                Console.WriteLine("2. Não");
                stillInProgram = Convert.ToInt32(Console.ReadLine());
            }
            Console.WriteLine("Programa finalizado. Pressione ENTER para continuar");
            Console.ReadKey();
        }
    }
}
