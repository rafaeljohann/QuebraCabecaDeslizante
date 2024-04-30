using QuebraCabecaDeslizante.Domain;

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

Tabuleiro tabuleiro = new Tabuleiro(pecasInicial, pecasObjetivo);

bool encontrouSolucao = tabuleiro.BuscarEmLargura();

if (!encontrouSolucao)
{
    Console.WriteLine("Não foi possível encontrar uma solução.");
}

Console.ReadLine();