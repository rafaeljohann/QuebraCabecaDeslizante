namespace QuebraCabecaDeslizante.Domain
{
    public class Tabuleiro
    {
        public int[,] Pecas { get; init; }
        public int[,] PecasObjetivo { get; init; }
        public int H { get; set; }

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

        public bool BuscarEmLargura()
        {
            Queue<(Tabuleiro tabuleiro, int passos)> fila = new Queue<(Tabuleiro, int)>();
            HashSet<Tabuleiro> visitados = new HashSet<Tabuleiro>();

            fila.Enqueue((this, 0));
            visitados.Add(this);

            while (fila.Count > 0)
            {
                (Tabuleiro atual, int passos) = fila.Dequeue();
                Console.WriteLine($"Passo {passos}:");
                atual.ImprimirTabuleiro();
                Console.WriteLine();

                if (atual.EhEstadoObjetivo())
                {
                    Console.WriteLine($"Chegou ao estado objetivo em {passos} passos.");
                    return true;
                }

                foreach (Tabuleiro sucessor in atual.ObterJogadasSucessoras())
                {
                    if (!visitados.Contains(sucessor))
                    {
                        fila.Enqueue((sucessor, passos + 1));
                        visitados.Add(sucessor);
                    }else {
                        Console.WriteLine("Aqui");
                    }
                }
            }

            Console.WriteLine("Não foi possível encontrar o estado objetivo.");
            return false;
        }

    public Tabuleiro Buscar(Tabuleiro initial, Func<Tabuleiro, int> evaluationFunction)
    {
        var abertos = new List<Tabuleiro> { initial };
        var fechados = new HashSet<Tabuleiro>();

        while (abertos.Count > 0)
        {
            var X = abertos.First(); // Retira o estado mais à esquerda de abertos
            abertos.Remove(X);

            if (X.EhEstadoObjetivo())
                return X;

            fechados.Add(X);

            foreach (var filho in X.ObterJogadasSucessoras())
            {
                if (!fechados.Contains(filho))
                {
                    filho.H = evaluationFunction(filho); // Atribui ao filho um valor heurístico

                    var existingOpen = abertos.FirstOrDefault(f => f.Equals(filho));
                    if (existingOpen == null)
                    {
                        abertos.Add(filho); // Adiciona o filho a abertos
                    }
                    else if (filho.H < existingOpen.H)
                    {
                        abertos.Remove(existingOpen);
                        abertos.Add(filho); // Atualiza o valor heurístico se um caminho mais curto foi encontrado
                    }
                }
            }

            abertos = abertos.OrderBy(f => f.H).ToList(); // Reordena estados em aberto pelo mérito heurístico
        }

        return null; // Retorna FALHA
    }

        public Tabuleiro BuscaMelhorEscolha(Tabuleiro initial, Func<Tabuleiro, int> evaluationFunction)
        {
            var abertos = new List<Tabuleiro> { initial };
            var fechados = new HashSet<Tabuleiro>();

            while (abertos.Count > 0)
            {
                var X = abertos.First(); // Retira o estado mais à esquerda de abertos
                abertos.Remove(X);

                if (X.EhEstadoObjetivo())
                    return X;

                fechados.Add(X);

                foreach (var filho in X.ObterJogadasSucessoras())
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

                abertos = abertos.OrderBy(f => f.H).ToList(); // Reordena estados em aberto pelo mérito heurístico
            }

            return null; // Retorna FALHA
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