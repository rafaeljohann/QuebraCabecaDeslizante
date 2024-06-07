using QuebraCabecaDeslizante.Domain;
using System.Diagnostics;

Stopwatch stopwatch = new Stopwatch();
stopwatch.Start();

Process process = Process.GetCurrentProcess();
long memoryBefore = process.PrivateMemorySize64;

int[,] pecasInicial = new int[,]
{
{2, 3, 4}, {8, 1, 5}, {7, 0, 6}
};

int[,] pecasObjetivo = new int[,]
{
    {1, 2, 3},
    {8, 0, 4},
    {7, 6, 5}
};

Func<Tabuleiro, int> heuristicFunction = (state) => {
    int misplacedPieces = 0;
    int size = state.Pecas.GetLength(0);
    int[,] objetivo = state.PecasObjetivo;

    for (int i = 0; i < size; i++)
    {
        for (int j = 0; j < size; j++)
        {
            if (state.Pecas[i, j] != objetivo[i, j] && state.Pecas[i, j] != 0)
            {
                misplacedPieces++;
            }
        }
    }

    return misplacedPieces;
};

Tabuleiro tabuleiro = new Tabuleiro(pecasInicial, pecasObjetivo);

//   bool encontrouSolucao = tabuleiro.BuscarEmLargura();
 var resultado = tabuleiro.BuscarEmLargura();
//  var resultado = tabuleiro.BuscaMelhorEscolha(heuristicFunction);

stopwatch.Stop();
Console.WriteLine("Tempo decorrido: " + stopwatch.ElapsedMilliseconds + " milissegundos");

process = Process.GetCurrentProcess();
long memoryAfter = process.PrivateMemorySize64;
long memoryUsed = memoryAfter - memoryBefore;
Console.WriteLine("Uso de memória: " + memoryUsed + " bytes");

Console.ReadLine();