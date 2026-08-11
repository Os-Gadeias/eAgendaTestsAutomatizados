using eAgenda.Aplicacao.Modulos.ModuloTarefa;
using eAgenda.Dominio.Modulos.ModuloTarefa;
using FluentResults;
using Moq;

namespace Eagenda.Test.Unitario.Modulos.ModuloTarefa;

[TestClass]
public sealed class ServicoTarefaTest
{
    [TestMethod]
    public void Cadastrar_Tarefa_ComDadosValidos_NaoRetornaErros()
    {
        Mock<IRepositorioTarefa> repositorioTarefa = new();

        ServicoTarefa servicoTarefa = new(repositorioTarefa.Object);

        Result resultado = servicoTarefa.Cadastrar(new("lavar o cachorro", PrioridadeTarefa.Alta));

        Assert.IsTrue(resultado.IsSuccess);
        repositorioTarefa.Verify(r => r.Cadastrar(It.IsAny<Tarefa>()), Times.Once);
    }
    [TestMethod]
    public void TarefaCriada_ComDataAtual_StatusPendente_SemPercentual_SemDataDeConclusao()
    {
        Mock<IRepositorioTarefa> repositorioTarefa = new();

        ServicoTarefa servicoTarefa = new(repositorioTarefa.Object);

        Tarefa? tarefaSelecionada = null!;

        repositorioTarefa.Setup(r => r.Cadastrar(It.IsAny<Tarefa>())).Callback<Tarefa>(t => tarefaSelecionada = t);

        Result resultado = servicoTarefa.Cadastrar(new("lavar o cachorro", PrioridadeTarefa.Alta));

        Assert.IsTrue(resultado.IsSuccess);
        repositorioTarefa.Verify(r => r.Cadastrar(It.IsAny<Tarefa>()), Times.Once);
        Assert.IsFalse(tarefaSelecionada.Concluida);
        Assert.AreEqual(0, tarefaSelecionada.PercentualConcluido);
        Assert.AreEqual(DateTime.Today, tarefaSelecionada.DataCriacao);
        Assert.IsNull(tarefaSelecionada.DataConclusao);
    }
    [TestMethod]
    public void TarefaCadastrada_SemTitulo_RetornaErro()
    {
        Mock<IRepositorioTarefa> repositorioTarefa = new();

        ServicoTarefa servicoTarefa = new(repositorioTarefa.Object);

        Result resultado = servicoTarefa.Cadastrar(new(string.Empty, PrioridadeTarefa.Alta));

        Assert.IsTrue(resultado.IsFailed);
        Assert.AreEqual("O Campo \"Título\" é obrigatório.", resultado.Errors.First().Message);
        repositorioTarefa.Verify(r => r.Cadastrar(It.IsAny<Tarefa>()), Times.Never);
    }
    [TestMethod]
    public void TarefaCadastrada_SemPrioridade_RetornaErro()
    {
        Mock<IRepositorioTarefa> repositorioTarefa = new();

        ServicoTarefa servicoTarefa = new(repositorioTarefa.Object);

        Result resultado = servicoTarefa.Cadastrar(new("Lavar o Cachorro", (PrioridadeTarefa)5));

        Assert.IsTrue(resultado.IsFailed);
        Assert.AreEqual("O campo \"Prioridade\" deve ser preenchido.", resultado.Errors.First().Message);
        repositorioTarefa.Verify(r => r.Cadastrar(It.IsAny<Tarefa>()), Times.Never);
    }
}
