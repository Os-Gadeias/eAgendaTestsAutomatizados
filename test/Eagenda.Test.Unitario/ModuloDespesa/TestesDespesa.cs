using eAgenda.Dominio.Modulos.ModuloCategoria;
using eAgenda.Dominio.Modulos.ModuloDespesa;

[TestClass]
public sealed class TestesDespesa
{
    [TestMethod]
    public void Validar_SemDescricao_DeveRetornar_ErroCorrespondente()
    {
        Categoria categoria = new Categoria("Carro");
        Despesa despesa = new Despesa(null, DateTime.Now, 11, FormaPagamento.AVista, new List<Categoria> { categoria });

        List<string> erros = despesa.Validar();

        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Descrição\" deve conter entre 2 e 100 caracteres.",
            erros.First()
        );
    }

    [TestMethod]
    public void Validar_PoucasLetrasDescricao_DeveRetornar_ErroCorrespondente()
    {
        Categoria categoria = new Categoria("Carro");
        Despesa despesa = new Despesa("A", DateTime.Now, 11, FormaPagamento.AVista, new List<Categoria> { categoria });

        List<string> erros = despesa.Validar();

        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Descrição\" deve conter mais que 2 caracteres!",
            erros.First()
        );
    }

    [TestMethod]
    public void Validar_MuitasLetrasDescricao_DeveRetornar_ErroCorrespondente()
    {
        Categoria categoria = new Categoria("Carro");
        Despesa despesa = new Despesa(new string('A', 101), DateTime.Now, 11, FormaPagamento.AVista, new List<Categoria> { categoria });

        List<string> erros = despesa.Validar();

        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Descrição\" deve conter no maximo 100 caracteres!",
            erros.First()
        );
    }

    [TestMethod]
    public void Validar_SemDataOcorrencia_DeveRetornar_ErroCorrespondente()
    {
        Categoria categoria = new Categoria("Carro");
        Despesa despesa = new Despesa(new string('A', 22), new DateTime(), 11, FormaPagamento.AVista, new List<Categoria> { categoria });

        List<string> erros = despesa.Validar();

        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Data de Ocorrência\" deve ser preenchido.",
            erros.First()
        );
    }

    [TestMethod]
    public void Validar_SemValor_DeveRetornar_ErroCorrespondente()
    {
        Categoria categoria = new Categoria("Carro");
        Despesa despesa = new Despesa(new string('A', 22), DateTime.Now, 0, FormaPagamento.AVista, new List<Categoria> { categoria });

        List<string> erros = despesa.Validar();

        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Valor\" deve ser maior que zero.",
            erros.First()
        );
    }

    [TestMethod]
    public void Validar_ValorNegativo_DeveRetornar_ErroCorrespondente()
    {
        Categoria categoria = new Categoria("Carro");
        Despesa despesa = new Despesa(new string('A', 22), DateTime.Now, -1, FormaPagamento.AVista, new List<Categoria> { categoria });

        List<string> erros = despesa.Validar();

        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Valor\" n pode ser negativo",
            erros.First()
        );
    }

    [TestMethod]
    public void Validar_SemFormaPagamento_DeveRetornar_ErroCorrespondente()
    {
        Categoria categoria = new Categoria("Carro");
        Despesa despesa = new Despesa(new string('A', 22), DateTime.Now, 22, (FormaPagamento)(-1), new List<Categoria> { categoria });

        List<string> erros = despesa.Validar();

        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Forma de Pagamento\" deve ser preenchido.",
            erros.First()
        );
    }

    [TestMethod]
    public void Validar_SemCategoria_DeveRetornar_ErroCorrespondente()
    {
        // Categoria categoria = new Categoria(null);
        Despesa despesa = new Despesa(new string('A', 22), DateTime.Now, 22, new FormaPagamento(), new List<Categoria>());

        List<string> erros = despesa.Validar();

        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "Selecione ao menos uma categoria.",
            erros.First()
        );
    }
}
