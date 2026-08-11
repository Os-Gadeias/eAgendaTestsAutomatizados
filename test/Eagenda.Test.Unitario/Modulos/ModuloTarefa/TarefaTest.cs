using eAgenda.Dominio.Modulos.ModuloTarefa;

namespace Eagenda.Test.Unitario.Modulos.ModuloTarefa;

[TestClass]
public sealed class TarefaTest
{
    [TestMethod]
    public void Cadastrar_Tarefa_ComDadosValidos_NaoRetornaErros()
    {
        Tarefa tarefa = new("Lavar o cachorro", PrioridadeTarefa.Alta);

        List<string> erros = tarefa.Validar();

        Assert.HasCount(0, erros);
    }
    [TestMethod]
    public void TarefaCriada_ComDataAtual_StatusPendente_SemPercentual_SemDataDeConclusao()
    {
        Tarefa tarefa = new("Lavar o cachorro", PrioridadeTarefa.Alta);

        Assert.IsFalse(tarefa.Concluida);
        Assert.AreEqual(0, tarefa.PercentualConcluido);
        Assert.AreEqual(DateTime.Today, tarefa.DataCriacao);
        Assert.IsNull(tarefa.DataConclusao);
    }
    [TestMethod]
    public void Tarefa_Cadastrada_Com_Os_Dois_Itens_Vinculados_E_Percentual_Em_0()
    {
        Tarefa tarefa = new("Lavar o cachorro", PrioridadeTarefa.Alta);
        ItemTarefa item1 = new("Pegar Shampoo");
        ItemTarefa item2 = new("Pegar Condicionador");
        tarefa.AdicionarItem(item1);
        tarefa.AdicionarItem(item2);

        Assert.HasCount(2, tarefa.Itens);
        Assert.AreEqual(0, tarefa.PercentualConcluido);
    }
    [TestMethod]
    public void TarefaCadastrada_SemTitulo_RetornaErro()
    {
        Tarefa tarefa = new(string.Empty, PrioridadeTarefa.Baixa);

        List<string> erros = tarefa.Validar();

        Assert.HasCount(1, erros);
        Assert.AreEqual("O Campo \"Título\" é obrigatório.", erros.First());
    }
    [TestMethod]
    public void TarefaCadastrada_SemPrioridade_RetornaErro()
    {
        Tarefa tarefa = new("lavar o cachorro", (PrioridadeTarefa)5);

        List<string> erros = tarefa.Validar();

        Assert.HasCount(1, erros);
        Assert.AreEqual("O campo \"Prioridade\" deve ser preenchido.", erros.First());
    }
    [TestMethod]
    public void TarefaCadastrada_ComTituloPequeno()
    {
        Tarefa tarefa = new("a", PrioridadeTarefa.Alta);

        List<string> erros = tarefa.Validar();

        Assert.HasCount(1, erros);
        Assert.AreEqual("O campo \"Título\" deve conter entre 2 e 100 caracteres.", erros.First());
    }
    [TestMethod]
    public void TarefaCadastrada_ComTitulo100_NaoRetornaErro()
    {
        Tarefa tarefa = new(new string('a', 100), PrioridadeTarefa.Alta);

        List<string> erros = tarefa.Validar();

        Assert.HasCount(0, erros);
    }
    [TestMethod]
    public void TarefaCadastrada_ComTitulo101_RetornaErro()
    {
        Tarefa tarefa = new(new string('a', 101), PrioridadeTarefa.Alta);

        List<string> erros = tarefa.Validar();

        Assert.HasCount(1, erros);
        Assert.AreEqual("O campo \"Título\" deve conter entre 2 e 100 caracteres.", erros.First());
    }
    [TestMethod]
    public void TarefaConcluida_AlteraDataDeConclusao_E_Status()
    {
        Tarefa tarefa = new("Lavar o cachorro", PrioridadeTarefa.Alta);

        tarefa.AlterarConclusaoManual(true);

        Assert.IsTrue(tarefa.Concluida);
        Assert.AreEqual(DateTime.Today, tarefa.DataConclusao);
        Assert.AreEqual(100, tarefa.PercentualConcluido);
    }
    [TestMethod]
    public void ReAbrirTarefaConcluida_AlteraDataDeConclusaoParaVazia_E_Status()
    {
        Tarefa tarefa = new("Lavar o cachorro", PrioridadeTarefa.Alta);

        tarefa.AlterarConclusaoManual(true);
        tarefa.AlterarConclusaoManual(false);

        Assert.IsFalse(tarefa.Concluida);
        Assert.IsNull(tarefa.DataConclusao);
        Assert.AreEqual(0, tarefa.PercentualConcluido);
    }
}
