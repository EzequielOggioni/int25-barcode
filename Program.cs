using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var item in Directory.GetFiles($@"{AppDomain.CurrentDomain.BaseDirectory}\test"))
            {

            
            Console.WriteLine( Buscar(item, 30, 70, 5, 10));

            }

            Console.ReadLine();

        }

        private static string Buscar(string imagen, float desdeX, float hastaX, float desdeY, float hastaY)
        {
            Dictionary< string, int> valores = new Dictionary<string, int>();
//            StreamWriter sw = new StreamWriter(@"c:\arc2.txt");
            Bitmap bmp = new Bitmap(imagen);
            for (int i = (int)(bmp.Height * desdeY / 100); i < (int)(bmp.Height * hastaY / 100); i++)
            {
                string texto = "";
                for (int j = (int)(bmp.Width * desdeX / 100); j < (int)(bmp.Width * hastaX / 100); j++)
                {

                    if (bmp.GetPixel(j, i).GetBrightness() > 0.4)
                    {
                        texto += "W";
                    }
                    else
                    {
                        texto += "n";
                    }


                }
                texto = AnalizaLinea(texto);
                if (texto != "" && texto != "err")
                {
                    if (valores.ContainsKey(texto))
                    {
                        valores[texto]++;
                    }else
                    valores.Add(texto,0);
//                    sw.WriteLine(texto);
                }


            }
            //  sw.Close();

            return valores.FirstOrDefault(x => x.Value == valores.Values.Max()).Key; ;
        }

        private static string AnalizaLinea(string linea)
        {
            if (linea.Count<char>((letra) => letra == 'W') > 50 && linea.Count<char>((letra) => letra == 'n') > 50)
            {
                string retorno = "";
                string valor = "";
                var sal = Regex.Matches(linea, @"((([n]){1,8}([W]){1,8}){20,40})");
                foreach (var item in sal)
                {
                    retorno = $"{retorno}{item.ToString()}";
                    valor = Regex.Replace(Regex.Replace(retorno, "((n){4,8}|(W){4,8})", "1"), "((n){1,}|(W){1,})", "0");

                }

                // return retorno + "~" + valor + "->" + CalcularNumero(valor);
                 return CalcularNumero(valor);
            }
            else
            {
                return "";
            }
        }
        private static string CalcularNumero(string codigo)
        {
            string salida = "";
            if ((codigo.Length - 8) % 10 != 0) return "err";
            if (codigo.Substring(0, 4) == "0000" && codigo.Substring(codigo.Length - 4, 4) == "1001")
            {
                for (int i = 0; i < codigo.Length / 10; i++)
                {

                    salida += CalcularPar(codigo.Substring((i * 10) + 4, 10));

                }
                return salida;

            }
            else
            {
                return "err";
            }


        }

        private static string CalcularPar(string codigo)
        {
            int valor1 = 0;
            int valor2 = 0;

            valor1 += int.Parse(codigo[0].ToString()) * 1;
            valor1 += int.Parse(codigo[2].ToString()) * 2;
            valor1 += int.Parse(codigo[4].ToString()) * 4;
            valor1 += int.Parse(codigo[6].ToString()) * 7;
                                                  
            valor2 += int.Parse(codigo[1].ToString()) * 1;
            valor2 += int.Parse(codigo[3].ToString()) * 2;
            valor2 += int.Parse(codigo[5].ToString()) * 4;
            valor2 += int.Parse(codigo[7].ToString()) * 7;

            return (valor1 == 11 ? "0" : valor1.ToString()) + (valor2 == 11 ? "0" : valor2.ToString());

        }

    }

}
