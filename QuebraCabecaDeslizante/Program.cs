using QuebraCabecaDeslizante.Domain;
using System.Diagnostics;

int[,] pecasInicial = new int[,]
{
    {2, 8, 3},
    {1, 6, 4},
    {7, 0, 5}
};

int[,] pecasObjetivo = new int[,]
{
    {1, 2, 3},
    {8, 0, 4},
    {7, 6, 5}
};

Stopwatch stopwatch = new Stopwatch();
stopwatch.Start();

Process process = Process.GetCurrentProcess();
long memoryBefore = process.PrivateMemorySize64;

Tabuleiro tabuleiro = new Tabuleiro(pecasInicial, pecasObjetivo);

// bool encontrouSolucao = tabuleiro.BuscarEmLargura();

Func<Tabuleiro, int> heuristicFunction = (state) => {
    int distanciaManhattan = 0;
    int size = state.Pecas.GetLength(0);

    for (int i = 0; i < size; i++)
    {
        for (int j = 0; j < size; j++)
        {
            int valorPeca = state.Pecas[i, j];
            if (valorPeca != 0)
            {
                int linhaDesejada = (valorPeca - 1) / size;
                int colunaDesejada = (valorPeca - 1) % size;

                distanciaManhattan += Math.Abs(i - linhaDesejada) + Math.Abs(j - colunaDesejada);
            }
        }
    }

    return distanciaManhattan;
};

Tabuleiro resultado = tabuleiro.BuscaMelhorEscolha(tabuleiro, heuristicFunction);

// if (resultado != null)
// {
//     Console.WriteLine("Solução encontrada:");
//     resultado.ImprimirTabuleiro();
// }
// else
// {
//     Console.WriteLine("Não foi possível encontrar uma solução.");
// }

stopwatch.Stop();
Console.WriteLine("Tempo decorrido: " + stopwatch.ElapsedMilliseconds + " milissegundos");

process = Process.GetCurrentProcess();
long memoryAfter = process.PrivateMemorySize64;
long memoryUsed = memoryAfter - memoryBefore;
Console.WriteLine("Uso de memória: " + memoryUsed + " bytes");

Console.ReadLine();