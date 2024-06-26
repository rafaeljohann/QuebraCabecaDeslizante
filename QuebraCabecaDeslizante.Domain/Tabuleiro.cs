namespace QuebraCabecaDeslizante.Domain
{
    public class Tabuleiro
    {
        public int[,] Pecas { get; init; }
        public int[,] PecasObjetivo { get; init; }
        public int H { get; set; }

        public List<Tabuleiro> CaminhoPercorrido { get; set; } = new List<Tabuleiro>();

        public Tabuleiro(int[,] pecas, int[,] pecasObjetivo)
        {
            Pecas = pecas;
            PecasObjetivo = pecasObjetivo;
            CaminhoPercorrido.Add(this);
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
            var novoTabuleiro = new Tabuleiro(novoPecas, PecasObjetivo);
            novoTabuleiro.CaminhoPercorrido.AddRange(this.CaminhoPercorrido);
            return novoTabuleiro;
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

        public bool BuscarEmLargura()
        {
            Console.WriteLine("---INICIANDO BUSCA EM LARGURA---");
            Console.WriteLine("");
            Queue<(Tabuleiro tabuleiro, int passos)> fila = new Queue<(Tabuleiro, int)>();
            HashSet<Tabuleiro> visitados = new HashSet<Tabuleiro>();

            fila.Enqueue((this, 0));
            visitados.Add(this);

            while (fila.Count > 0)
            {
                (Tabuleiro atual, int passos) = fila.Dequeue();

                if (atual.EhEstadoObjetivo())
                {
                    Console.WriteLine($"Chegou ao estado objetivo em {passos} passos.");
                    foreach (var item in atual.CaminhoPercorrido)
                    {
                        int numRows = item.Pecas.GetLength(0);
                        int numCols = item.Pecas.GetLength(1);

                        for (int i = 0; i < numRows; i++)
                        {
                            for (int j = 0; j < numCols; j++)
                            {
                                Console.Write(item.Pecas[i, j] + " ");
                            }
                            Console.WriteLine("");
                        }
                        Console.WriteLine("");
                    }
                    Console.WriteLine($"Chegou ao estado objetivo com custo {atual.CaminhoPercorrido.Count}.");
                    Console.WriteLine("Número de estados visitados: " + visitados.Count);
                    return true;
                }

                foreach (Tabuleiro sucessor in atual.ObterJogadasSucessoras())
                {
                    if (!visitados.Contains(sucessor))
                    {
                        fila.Enqueue((sucessor, passos + 1));
                        visitados.Add(sucessor);
                    }
                }
            }

            Console.WriteLine("Não foi possível encontrar o estado objetivo.");
            return false;
        }


        public Tabuleiro BuscaMelhorEscolha(Func<Tabuleiro, int> evaluationFunction)
        {
            Console.WriteLine("---INICIANDO BUSCA MELHOR ESCOLHA---");
            Console.WriteLine("");
            var abertos = new List<Tabuleiro> { this };
            var fechados = new HashSet<Tabuleiro>();

            while (abertos.Count > 0)
            {
                var X = abertos.First();
                abertos.Remove(X);

                if (X.EhEstadoObjetivo())
                {
                    foreach (var item in X.CaminhoPercorrido)
                    {
                        int numRows = item.Pecas.GetLength(0);
                        int numCols = item.Pecas.GetLength(1);

                        for (int i = 0; i < numRows; i++)
                        {
                            for (int j = 0; j < numCols; j++)
                            {
                                Console.Write(item.Pecas[i, j] + " ");
                            }
                            Console.WriteLine("");
                        }
                        Console.WriteLine("");
                    }
                    Console.WriteLine($"Chegou ao estado objetivo com custo {X.CaminhoPercorrido.Count}.");
                    Console.WriteLine("Número de estados abertos: " + abertos.Count);
                    Console.WriteLine("Número de estados fechados: " + fechados.Count);
                    return X;
                }

                fechados.Add(X);

                foreach (var filho in X.ObterJogadasSucessoras())
                {
                    if (!fechados.Contains(filho))
                    {
                        filho.H = evaluationFunction(filho);
                        var existeAberto = abertos.FirstOrDefault(f => f.Equals(filho));
                        var existeFechado = fechados.FirstOrDefault(f => f.Equals(filho));
                        if (existeFechado is null && existeAberto is null)
                        {
                            abertos.Add(filho);
                        }
                        else if (existeAberto is not null && filho.H < existeAberto.H)
                        {
                            existeAberto.H = filho.H;
                        }
                        else if (existeFechado is not null && filho.H < existeFechado.H)
                        {
                            fechados.Remove(existeFechado);
                            abertos.Add(filho);
                        }
                    }
                }

                abertos = abertos.OrderBy(f => f.H).ToList();
            }

            return null;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Tabuleiro outroTabuleiro = (Tabuleiro)obj;

            int tamanho = Pecas.GetLength(0);
            for (int linha = 0; linha < tamanho; linha++)
            {
                for (int coluna = 0; coluna < tamanho; coluna++)
                {
                    if (Pecas[linha, coluna] != outroTabuleiro.Pecas[linha, coluna])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                int tamanho = Pecas.GetLength(0);
                for (int linha = 0; linha < tamanho; linha++)
                {
                    for (int coluna = 0; coluna < tamanho; coluna++)
                    {
                        hash = hash * 31 + Pecas[linha, coluna];
                    }
                }
                return hash;
            }
        }
    }
}