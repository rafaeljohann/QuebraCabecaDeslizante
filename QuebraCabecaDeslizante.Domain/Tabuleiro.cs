namespace QuebraCabecaDeslizante.Domain
{
    public class Tabuleiro
    {
        public int[,] Pecas { get; init; }
        public int[,] PecasObjetivo { get; init; }

        public Tabuleiro(int[,] pecas, int[,] pecasObjetivo)
        {
            Pecas = pecas;
            PecasObjetivo = pecasObjetivo;
        }

        public List<Tabuleiro> ObterJogadasSucessoras()
        {
            List<Tabuleiro> movimentos = new List<Tabuleiro>();
            int tamanho = Pecas.GetLength(0);

            int i = 0, j = 0;
            for (int linha = 0; linha < tamanho; linha++)
            {
                for (int coluna = 0; coluna < tamanho; coluna++)
                {
                    if (Pecas[linha, coluna] == 0)
                    {
                        i = linha;
                        j = coluna;
                        break;
                    }
                }
            }

            if (i > 0) // CIMA
                movimentos.Add(CriarNovoTabuleiro(i, j, i - 1, j));
            if (i < tamanho - 1) // BAIXO
                movimentos.Add(CriarNovoTabuleiro(i, j, i + 1, j));
            if (j > 0) // ESQUERDA
                movimentos.Add(CriarNovoTabuleiro(i, j, i, j - 1));
            if (j < tamanho - 1) // DIREITA
                movimentos.Add(CriarNovoTabuleiro(i, j, i, j + 1));

            return movimentos;
        }

        private Tabuleiro CriarNovoTabuleiro(
            int i, int j, int novaPosicaoI, int novaPosicaoJ)
        {
            int tamanho = Pecas.GetLength(0);
            int[,] novoPecas = new int[tamanho, tamanho];
            Array.Copy(Pecas, novoPecas, Pecas.Length);
            int temp = novoPecas[i, j];
            novoPecas[i, j] = novoPecas[novaPosicaoI, novaPosicaoJ];
            novoPecas[novaPosicaoI, novaPosicaoJ] = temp;
            return new Tabuleiro(novoPecas, PecasObjetivo);
        }

        public bool EhEstadoObjetivo()
        {
            int tamanho = Pecas.GetLength(0);
            for (int linha = 0; linha < tamanho; linha++)
            {
                for (int coluna = 0; coluna < tamanho; coluna++)
                {
                    if (Pecas[linha, coluna] != PecasObjetivo[linha, coluna])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void ImprimirTabuleiro()
        {
            int tamanho = Pecas.GetLength(0);
            for (int linha = 0; linha < tamanho; linha++)
            {
                for (int coluna = 0; coluna < tamanho; coluna++)
                {
                    Console.Write(Pecas[linha, coluna] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}