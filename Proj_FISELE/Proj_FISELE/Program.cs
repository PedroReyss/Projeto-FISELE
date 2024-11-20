using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace Program
{
    internal class Init
    {
        static void Main(string[] args)
        {
            if (!File.Exists("salute.txt"))
            {
                File.Create("salute.txt");
            }
            Console.CursorVisible = false;
            Console.WriteLine("\n\tPressione qualquer tecla para iniciar");
            Console.ReadKey();
            Menu.Main_Menu();
        }
    }
    public class Menu
    {
        public static short menu_function; //0 = y/n, 1 = menu
        static string tip;
        static bool tipped;
        public static void Main_Menu()
        {
            Thread.Sleep(100);
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(" ____  ____  _  _  ____      ____   __   ____  __   \n" +
                              "(  __)(  __)( \\/ )(  _ \\ ___(  __) / _\\ (  _ \\(  )  \n" +
                              " ) _)  ) _)  )  (  ) __/(___)) _) /    \\ ) __// (_/\\\n" +
                              "(__)  (____)(_/\\_)(__)      (____)\\_/\\_/(__)  \\____/");
            Console.ResetColor();
            if (!tipped)
                Tips();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n\t" + tip);
            Console.ResetColor();
            menu_function = 1;
            Select();
        }
        static void Tips()
        {
            List<string> tips = File.ReadAllLines("salute.txt").ToList();
            int listCount = tips.Count;
            Random rand = new Random();
            while (listCount > 1)
            {
                listCount--;
                int swap = rand.Next(listCount + 1);
                string text = tips[swap];
                tips[swap] = tips[listCount];
                tips[listCount] = text;
            }
            tip = tips[0];
            tipped = true;
            return;
        }
        public static void Select()
        {
            string[] options = null;
            switch (menu_function)
            {
                case 0:
                    options = new string[] { "Sim\t", "Não\t" };
                    break;

                case 1:
                    options = new string[] { "Eletricidade\t", "Física\t", "Sair\t" };
                    break;
            }
            ConsoleKey key;
            short selected = new short();
            do
            {
                for (int i = 0; i < options.Length; i++)
                {
                    Console.SetCursorPosition(16, 8 + i);
                    if (i == selected)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    Console.Write($"{options[i]}");
                    Console.ResetColor();
                }
                key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.DownArrow:
                        {
                            if (selected < options.Length - 1)
                            {
                                selected++;
                            }
                            break;
                        }
                    case ConsoleKey.UpArrow:
                        {
                            if (selected > 0)
                            {
                                selected--;
                            }
                            break;
                        }
                }
            } while (key != ConsoleKey.Enter);
            tipped = false;
            if (menu_function == 0)
            {
                switch (selected)
                {
                    case 0:
                        return;

                    case 1:
                        Main_Menu();
                        break;
                }
            }
            else if (menu_function == 1)
            {
                switch (selected)
                {
                    case 0:
                        Eletrica.Mesh();
                        break;

                    case 1:
                        Fisica.Cannon();
                        break;

                    case 2:
                        Exit();
                        break;
                }
            }
        }
        static void Exit()
        {
            Console.Clear();
            Console.WriteLine("\n\n\n\n\n\n\t\tTem certeza que deseja sair?");
            menu_function = 0;
            Select();
            Environment.Exit(0);
        }
    }
    public class Fisica
    {
        private static bool exception_caught;
        public static void Cannon()
        {
            float xPonto, yPonto, theta;
            double v0, vX, g = 9.8, thetaRad, thetaMin, formulaTopo,
                   formulaBaixo, aFuncao, bFuncao, tempoContato, xApice;
            string etapaContato = null;

            Console.Clear();
            float[] temp = Coords();
            xPonto = temp[0];
            yPonto = temp[1];
            thetaMin = ((Math.Atan2(yPonto, xPonto)) * 180) / Math.PI;
            Console.Clear();
            theta = Angle(Math.Round(thetaMin, 3));

            thetaRad = Convert.ToDouble((theta * Math.PI) / 180);
            formulaTopo = g * (Math.Pow(xPonto, 2)) * (1 + Math.Pow(Math.Tan(thetaRad), 2));
            formulaBaixo = (yPonto - (xPonto * Math.Tan(thetaRad))) * 2;
            v0 = Math.Sqrt(-(formulaTopo / formulaBaixo));
            aFuncao = ((yPonto - (Math.Tan(thetaRad) * xPonto)) / Math.Pow(xPonto, 2));
            bFuncao = Math.Tan(thetaRad);
            vX = Math.Cos(thetaRad) * v0;
            tempoContato = (xPonto / vX);
            xApice = -(bFuncao / (2 * aFuncao));
            if (Math.Round(xPonto, 1) < Math.Round(xApice, 1))
            {
                etapaContato = "em movimento ascendente";
            }
            else if (Math.Round(xPonto, 1) > Math.Round(xApice, 1))
            {
                etapaContato = "em movimento descendente";
            }
            else if (Math.Round(xPonto, 1) == Math.Round(xApice, 1))
            {
                etapaContato = "aproximadamente no topo da parábola";
            }

            Console.Clear();
            Console.Write("\n\tDistância horizontal: ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(xPonto + "m");
            Console.ResetColor();
            Console.Write("\tDistância vertical: ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(yPonto + "m");
            Console.ResetColor();
            Console.Write("\n\tA função da parábola é: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"y = {aFuncao}x² + {bFuncao}x");
            Console.ResetColor();
            Console.Write("\n\tA velocidade de lançamento inicial é: ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"{Math.Round(v0, 3)}m/s");
            Console.ResetColor();
            Console.Write("\n\tO tempo gasto até o projétil atingir o alvo é: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"{Math.Round(tempoContato, 3)}s");
            Console.ResetColor();
            Console.Write($"\n\tO projétil atinge o alvo {etapaContato}");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n\n\tDeseja imprimir o gráfico na tela?");
            Menu.menu_function = 0;
            Menu.Select();
            Graph(aFuncao, bFuncao);
        }
        private static float[] Coords()
        {
            Console.Clear();
            float[] temp = new float[2];
            for (int i = 0; i < 2; i++)
            {
                switch (i)
                {
                    case 0:
                        Console.WriteLine("\n\tDigite a distância horizontal do ponto (m)");
                        Console.Write("\n\tDistância: ");
                        break;

                    case 1:
                        Console.WriteLine("\n\n\n\tDigite a distância vertical do ponto (m)");
                        Console.Write("\n\tDistância: ");
                        break;
                }
                try
                {
                    temp[i] = float.Parse(User_Input.Input());
                }
                catch (Exception e)
                {
                    Console.WriteLine($"\n\tErro: {e.Message}");
                    Console.WriteLine("\n\n\n\t\tPressione qualquer tecla para voltar");
                    Console.ReadKey();
                    Menu.Main_Menu();
                }
            }
            return temp;
        }
        private static float Angle(double min_angle)
        {
            float temp = new float();
            Console.WriteLine($"\n\tO ângulo de lançamento fornecido deve estar entre {min_angle}° e 90°");
            Console.Write("\tForneça o ângulo de lançamento em graus: ");
            do
            {
                try
                {
                    temp = float.Parse(User_Input.Input());
                }
                catch (Exception e)
                {
                    Console.WriteLine($"\n\tErro: {e.Message}");
                    Console.WriteLine("\n\n\n\t\tPressione qualquer tecla para voltar");
                    Console.ReadKey();
                    Menu.Main_Menu();
                }
                if (temp < min_angle | temp > 90)
                {
                    Console.Clear();
                    Console.WriteLine($"\n\tValor inválido, o ângulo deve estar entre {min_angle}° e 90°");
                    Console.Write("\n\tDigite um valor possível: ");
                }
            } while (temp < min_angle || temp > 90);
            return temp;
        }
        private static void Graph(double a, double b)
        {
            {
                Console.Clear();
                double aFuncao = a, bFuncao = b, xMax;
                int x = 0, y, xMaxGrafico, yMaxGrafico, xGrafico = 0, yGrafico = 0, redutor = 0, comprimentoValor;
                xMax = (Math.Floor(-bFuncao / aFuncao));
                xMaxGrafico = Convert.ToInt32(xMax);
                yMaxGrafico = Convert.ToInt32(Math.Round((-Math.Pow(bFuncao, 2) / (4 * aFuncao))));
                int yMax = yMaxGrafico;
                while (xMaxGrafico > 112 || yMaxGrafico > 26)
                {
                    xMaxGrafico = Convert.ToInt32(xMaxGrafico / 1.1);
                    yMaxGrafico = Convert.ToInt32(yMaxGrafico / 1.1);
                    redutor++;
                }
                xMaxGrafico++;
                yMaxGrafico++;
                int[] ordenada = new int[yMaxGrafico];
                bool[,] grafico = new bool[Convert.ToInt32(xMaxGrafico), Convert.ToInt32(yMaxGrafico)];
                do
                {
                    y = Convert.ToInt32(Math.Floor((Math.Pow(x, 2) * aFuncao) + (bFuncao * x)));
                    if (redutor == 0)
                    {
                        xGrafico = x;
                        yGrafico = y;
                    }
                    else
                    {
                        xGrafico = Convert.ToInt32(Math.Floor(x / (Math.Pow(1.1, redutor))));
                        yGrafico = Convert.ToInt32(Math.Floor(y / (Math.Pow(1.1, redutor))));
                    }
                    ordenada[yGrafico] = y;
                    if (ordenada[yGrafico] == 0)
                    {
                        ordenada[yGrafico]++;
                    }
                    grafico[(xGrafico), (yGrafico)] = true;
                    x++;
                } while (x <= xMax);
                x = 0;
                y = (yMaxGrafico - 1);
                do
                {
                    if (ordenada[y] != 0)
                    {
                        comprimentoValor = Convert.ToString(ordenada[y]).Length;
                        while (comprimentoValor < Convert.ToString(yMax).Length)
                        {
                            Console.Write(" ");
                            comprimentoValor++;
                        }
                        Console.Write($"{ordenada[y]}| ");
                        do
                        {
                            if (grafico[x, y])
                            {
                                Console.Write("*");
                            }
                            else if (!grafico[x, y])
                            {
                                Console.Write(" ");
                            }
                            x++;
                        } while (x < xMaxGrafico);
                        Console.WriteLine();
                        x = 0;
                    }
                    y--;
                } while (y >= 0);
                comprimentoValor = 0;
                while (comprimentoValor < Convert.ToString(yMax).Length)
                {
                    Console.Write(" ");
                    comprimentoValor++;
                }
                Console.Write("0 ");
                x = 0;
                while (x < xMaxGrafico)
                {
                    Console.Write("─");
                    x++;
                }
                Console.Write(xMax);
                Console.WriteLine("\n\n\n\tPressione qualquer tecla para voltar");
                Console.ReadKey(true);
                Menu.Main_Menu();
            }
        }
    }
    public class Eletrica
    {
        private static bool exception_caught;
        static List<int> ListarPares(int[,] pares, int qtI, int nM)
        {
            List<int> listaPares = new List<int>();
            for (int i = 0; i < qtI; i++)
            {
                if (pares[i, 0] == nM)
                {
                    listaPares.Add(i);
                }
            }
            return listaPares;
        }

        static double Determinante(int dMatriz, double[,] matriz)
        {
            double det = 1;
            if (dMatriz == 2)
            {
                det = (matriz[0, 0] * matriz[1, 1]) - (matriz[0, 1] * matriz[1, 0]);
            }
            else
            {
                short[] tempLin = new short[dMatriz];
                for (int a = 0; a < dMatriz; a++)
                {
                    if (matriz[0, 0] == 0)
                    {
                        if (matriz[a, 0] != 0)
                        {
                            for (int i = 0; i < dMatriz; i++)
                            {
                                tempLin[i] = (short)-(matriz[0, i]);
                            }
                            for (int i = 0; i < dMatriz; i++)
                            {
                                matriz[0, i] = matriz[a, i];
                                matriz[a, i] = tempLin[i];
                            }
                        }
                    }
                }
                double[,] temp = new double[dMatriz, dMatriz];
                for (int i = 0; i < dMatriz; i++)
                {
                    for (int j = 0; j < dMatriz; j++)
                    {
                        temp[i, j] = matriz[i, j];
                    }
                }
                for (int i = 0; i < dMatriz; i++)
                {
                    for (int j = i; j < dMatriz; j++)
                    {
                        double fator = temp[j, i] / temp[i, i];
                        for (int k = i; k < dMatriz; k++)
                        {
                            if (j != i)
                            {
                                temp[j, k] -= fator * temp[i, k];
                            }
                        }
                    }
                }
                for (int i = 0; i < dMatriz; i++)
                {
                    det *= temp[i, i];
                }
            }
            return det;
        }
        public static void Mesh()
        {
            Console.Clear();
            string temp;
            int qtdMalhas = new int();
            try
            {
                Console.Write("\n\t\tDigite o número de malhas: ");
                temp = User_Input.Input();
                qtdMalhas = int.Parse(temp);
            }
            catch (Exception e)
            {
                Console.Write($"\n\tErro: {e.Message}");
                Console.WriteLine("\n\n\n\t\tPressione qualquer tecla para voltar");
                Console.ReadKey();
                Menu.Main_Menu();
            }
            Console.Clear();
            double[] tensaoMalhas = new double[qtdMalhas];
            for (int i = 0; i < qtdMalhas; i++)
            {
                try
                {
                    Console.Clear();
                    Console.Write($"\n\tDigite a soma das tensões da malha {i + 1}: ");
                    temp = User_Input.Input();
                    temp.Replace(',', '.');
                    tensaoMalhas[i] = double.Parse(temp);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"\n\tErro: {e.Message}");
                    Console.WriteLine("\n\n\n\t\tPressione qualquer tecla para voltar");
                    Console.ReadKey();
                    Menu.Main_Menu();
                }
            }
            Console.Clear();
            Console.Write("\n\tDigite os valores das resistências dos resistores ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("exclusivos ");
            Console.ResetColor();
            Console.Write("de cada malha separados por ponto e vírgula (;)");
            double[] resMalhas = new double[qtdMalhas];                             
            List<double> resistoresMalhas = new List<double>();
            int[] qtdResistoresMalhas = new int[qtdMalhas];

            for (int i = 0; i < qtdMalhas; i++)
            {
                try
                {
                    Console.SetCursorPosition(5, 3);
                    Console.Write($"\tMalha {i + 1}");
                    Console.Write("\n\tResistores                                                                     ");
                    Console.SetCursorPosition(18, 4);
                    Console.Write(": ");
                    temp = User_Input.Input();
                    double[] resistores = Array.ConvertAll(temp.Split(';'), double.Parse);
                    resistoresMalhas.AddRange(resistores);
                    resMalhas[i] = resistores.Sum();
                    qtdResistoresMalhas[i] += resistores.Length;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"\n\tErro: {e.Message}");
                    Console.WriteLine("\n\n\n\t\tPressione qualquer tecla para voltar");
                    Console.ReadKey();
                    Menu.Main_Menu();
                }
            }
            int qtdIntersec = 0;
            try
            {
                Console.Clear();
                Console.Write("\n\tDigite o número de trechos de intersecção entre malhas: ");
                temp = User_Input.Input();
                qtdIntersec = int.Parse(temp);
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n\tErro: {e.Message}");
                Console.WriteLine("\n\n\n\t\tPressione qualquer tecla para voltar");
                Console.ReadKey();
                Menu.Main_Menu();
            }
            int[,] intersec = new int[qtdIntersec, 2];
            double[] resIntersec = new double[qtdIntersec];
            List<double> resistoresIntersec = new List<double>();
            int[] qtdResistoresIntersec = new int[qtdIntersec];
            for (int i = 0; i < qtdIntersec; i++)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("\n\tDigite os números dos pares de malhas que formam intersecção, separados por ponto e vírgula (;)");
                    Console.Write($"\n\t{i + 1}ª intersecção: ");
                    temp = User_Input.Input();
                    string[] par = temp.Split(';');
                    intersec[i, 0] = int.Parse(par[0]) - 1;
                    intersec[i, 1] = int.Parse(par[1]) - 1;
                    Console.WriteLine($"\n\tDigite os valores de resistência dos resistores da intersecção {i + 1}");
                    Console.Write("\n\tResistências: ");
                    temp = User_Input.Input();
                    double[] resistores = Array.ConvertAll(temp.Split(';'), double.Parse);
                    resistoresIntersec.AddRange(resistores);
                    resIntersec[i] = resistores.Sum();
                    qtdResistoresIntersec[i] += resistores.Length;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"\n\tErro: {e.Message}");
                    Console.WriteLine("\n\n\n\t\tPressione qualquer tecla para voltar");
                    Console.ReadKey();
                    Menu.Main_Menu();
                }
            }
            int[,] intersecMirr = new int[qtdIntersec, 2];
            for (int i = 0; i < qtdIntersec; i++)
            {
                intersecMirr[i, 0] = intersec[i, 1];
                intersecMirr[i, 1] = intersec[i, 0];
            }
            double[] resTotal = new double[qtdMalhas];
            for (int numMalha = 0; numMalha < qtdMalhas; numMalha++)
            {
                List<int> listaPares = ListarPares(intersec, qtdIntersec, numMalha);
                List<int> listaParesMirr = ListarPares(intersecMirr, qtdIntersec, numMalha);
                double resTotalMalha = resMalhas[numMalha];
                foreach (int i in listaPares)
                {
                    resTotalMalha += resIntersec[i];
                }
                foreach (int i in listaParesMirr)
                {
                    resTotalMalha += resIntersec[i];
                }
                resTotal[numMalha] = resTotalMalha;
            }
            double[,] matrizSistema = new double[qtdMalhas, qtdMalhas];
            for (int numMalha = 0; numMalha < qtdMalhas; numMalha++)
            {
                List<int> listaPares = ListarPares(intersec, qtdIntersec, numMalha);
                List<int> listaParesMirr = ListarPares(intersecMirr, qtdIntersec, numMalha);
                for (int a = 0; a < qtdMalhas; a++)
                {
                    if (a == numMalha)
                    {
                        matrizSistema[numMalha, a] = resTotal[a];
                    }
                }
                foreach (int i in listaPares)
                {
                    matrizSistema[numMalha, intersec[i, 1]] = -resIntersec[i];
                }
                foreach (int i in listaParesMirr)
                {
                    matrizSistema[numMalha, intersecMirr[i, 1]] = -resIntersec[i];
                }
            }
            double determinante = Determinante(qtdMalhas, matrizSistema);
            double[] correntesMalhas = new double[qtdMalhas];
            for (int i = 0; i < qtdMalhas; i++)
            {
                double[,] matrizTrocada = new double[qtdMalhas, qtdMalhas];
                for (int j = 0; j < qtdMalhas; j++)
                {
                    for (int k = 0; k < qtdMalhas; k++)
                    {
                        matrizTrocada[j, k] = matrizSistema[j, k];
                    }
                }
                for (int j = 0; j < qtdMalhas; j++)
                {
                    matrizTrocada[j, i] = tensaoMalhas[j];
                }
                correntesMalhas[i] = Determinante(qtdMalhas, matrizTrocada) / determinante;
            }
            double[] correntesIntersec = new double[qtdIntersec];
            for (int i = 0; i < qtdIntersec; i++)
            {
                if (correntesMalhas[intersec[i, 0]] > correntesMalhas[intersec[i, 1]])
                {
                    correntesIntersec[i] = correntesMalhas[intersec[i, 0]] - correntesMalhas[intersec[i, 1]];
                }
                else
                {
                    correntesIntersec[i] = correntesMalhas[intersec[i, 1]] - correntesMalhas[intersec[i, 0]];
                }
            }
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n\tCORRENTES DAS MALHAS");
            Console.ResetColor();
            for (int i = 0; i < qtdMalhas; i++)
            {
                Console.WriteLine($"\tMalha {i + 1}: {correntesMalhas[i]} A");
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n\tCORRENTES DAS INTERSECÇÕES");
            Console.ResetColor();
            for (int i = 0; i < qtdIntersec; i++)
            {
                Console.WriteLine($"\tInterseção {intersec[i, 0] + 1}-{intersec[i, 1] + 1}: {correntesIntersec[i]} A");
            }
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("\n\tQUEDAS DE TENSÂO & RESISTORES EXCLUSIVOS");
            Console.ResetColor();
            int indice = 0;
            for (int i = 0; i < qtdMalhas; i++)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\tMALHA " + (i + 1));
                Console.ResetColor();
                for (int j = indice; j < indice + qtdResistoresMalhas[i]; j++)
                {
                    double tensaoResistor = correntesMalhas[i] * resistoresMalhas[j];
                    Console.WriteLine($"\tVR{j - indice + 1} ({resistoresMalhas[j]} Ω): {tensaoResistor} V");
                }
                indice += qtdResistoresMalhas[i];
            }
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("\n\tRESISTORES DE INTERSECÇÕES");
            Console.ResetColor();
            indice = 0;
            for (int i = 0; i < qtdIntersec; i++)
            {
                Console.WriteLine($"\tINTERSECÇÃO {intersec[i, 0] + 1}-{intersec[i, 1] + 1}");
                for (int j = indice; j < indice + qtdResistoresIntersec[i]; j++)
                {
                    double tensaoResistor = correntesIntersec[i] * resistoresIntersec[j];
                    Console.WriteLine($"\tVR{j - indice + 1} ({resistoresIntersec[j]} Ω): {tensaoResistor} V");
                }
                indice += qtdResistoresIntersec[i];
            }
            Console.Write("\n\n\n\t\tPressione");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(" ENTER ");
            Console.ResetColor();
            Console.Write("para voltar");
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
            } while (key.Key != ConsoleKey.Enter);
            Menu.Main_Menu();
        }
    }
    public class User_Input
    {
        private static string valid_input = "0123456789.;-";
        public static string Input()
        {
            StringBuilder input = new StringBuilder(16);
            ConsoleKeyInfo key;
            bool invalid_input = false;
            int x_base = Console.CursorLeft;
            int y_base = Console.CursorTop;
            do
            {
                int x, y;
                Thread.Sleep(100);
                if (string.IsNullOrEmpty(input.ToString()) || Console.CursorLeft < x_base)
                {
                    x = x_base;
                    y = y_base;
                }
                else
                {
                    x = Console.CursorLeft;
                    y = Console.CursorTop;
                }
                key = Console.ReadKey(true);
                for (int j = 0; j < valid_input.Length; j++)
                {
                    if (key.KeyChar != valid_input[j] && key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Backspace)
                    {
                        Console.ResetColor();
                        invalid_input = true;
                        Console.SetCursorPosition(5, y + 1);
                        Console.Write("\n\tA entrada deve ser um número     ");
                        Console.SetCursorPosition(x, y);
                    }
                    else
                    {
                        Console.ResetColor();
                        invalid_input = false;
                        Console.SetCursorPosition(5, y + 1);
                        Console.Write("\n\tPressione ESC para voltar ao menu");
                        Console.SetCursorPosition(x, y);
                        break;
                    }
                }
                Console.ForegroundColor = ConsoleColor.Magenta;
                if (key.Key == ConsoleKey.Backspace && !string.IsNullOrEmpty(input.ToString()))
                {
                    if (input.Length == 1)
                    {
                        Console.SetCursorPosition(x_base, y);
                        Console.Write(" ");
                        Console.SetCursorPosition(x_base, y);
                        input.Clear();
                    }
                    else if (input.Length > 1)
                    {
                        input.Remove(input.Length - 1, 1);
                        Console.SetCursorPosition(x - 1, y);
                        Console.Write(" ");
                        Console.SetCursorPosition(x - 1, y);
                    }
                }
                else if (!invalid_input && input.Length < 16)
                {
                    input.Append(key.KeyChar);
                    Console.Write(key.KeyChar);
                }
                else if (key.Key == ConsoleKey.Escape)
                {
                    Menu.Main_Menu();
                }
            } while (key.Key != ConsoleKey.Enter || string.IsNullOrEmpty(input.ToString()));
            Console.ResetColor();
            Console.SetCursorPosition(5, y_base + 1);
            Console.Write("                                     ");
            return input.ToString();
        }
    }
}