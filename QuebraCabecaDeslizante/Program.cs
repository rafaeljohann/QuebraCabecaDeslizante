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

// bool encontrouSolucao = tabuleiro.BuscarEmLargura();

Func<Tabuleiro, int> heuristicFunction = (state) => {
    int distanciaManhattan = 0;
    int size = state.Pecas.GetLength(0);

    for (int i = 0; i < size; i++)
    {
        for (int j = 0; j < size; j++)
        {
            int valorPeca = state.Pecas[i, j];
            if (valorPeca != 0) // Ignora a peça vazia
            {
                // Calcula a posição desejada (linha e coluna) da peça com base no seu valor
                int linhaDesejada = (valorPeca - 1) / size;
                int colunaDesejada = (valorPeca - 1) % size;

                // Calcula a distância Manhattan entre a posição atual e a posição desejada
                distanciaManhattan += Math.Abs(i - linhaDesejada) + Math.Abs(j - colunaDesejada);
            }
        }
    }

    return distanciaManhattan;
};

Tabuleiro resultado = tabuleiro.BuscaMelhorEscolha(tabuleiro, heuristicFunction);

if (resultado != null)
{
    Console.WriteLine("Solução encontrada:");
    resultado.ImprimirTabuleiro();
}
else
{
    Console.WriteLine("Não foi possível encontrar uma solução.");
}

Console.ReadLine();