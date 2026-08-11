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
    [TestMethod]
    public void TarefaConcluida_AlteraDataDeConclusao_E_Status()
    {
        Mock<IRepositorioTarefa> repositorioTarefa = new();

        ServicoTarefa servicoTarefa = new(repositorioTarefa.Object);
        Tarefa tarefa = new("Lavar o cachorro", PrioridadeTarefa.Alta);

        repositorioTarefa.Setup(r => r.SelecionarPorId(It.IsAny<Guid>())).Returns(tarefa);

        servicoTarefa.AlterarConclusao(new(tarefa.Id, true));

        Assert.IsTrue(tarefa.Concluida);
        Assert.AreEqual(DateTime.Today, tarefa.DataConclusao);
        Assert.AreEqual(100, tarefa.PercentualConcluido);
    }
    [TestMethod]
    public void ReAbrirTarefaConcluida_AlteraDataDeConclusaoParaVazia_E_Status()
    {
        Mock<IRepositorioTarefa> repositorioTarefa = new();

        ServicoTarefa servicoTarefa = new(repositorioTarefa.Object);
        Tarefa tarefa = new("Lavar o cachorro", PrioridadeTarefa.Alta);

        repositorioTarefa.Setup(r => r.SelecionarPorId(It.IsAny<Guid>())).Returns(tarefa);

        servicoTarefa.AlterarConclusao(new(tarefa.Id, true));
        servicoTarefa.AlterarConclusao(new(tarefa.Id, false));

        Assert.IsFalse(tarefa.Concluida);
        Assert.IsNull(tarefa.DataConclusao);
        Assert.AreEqual(0, tarefa.PercentualConcluido);
    }
    [TestMethod]
    public void EditarTarefa_PersisteDados()
    {
        Mock<IRepositorioTarefa> repositorioTarefa = new();

        ServicoTarefa servicoTarefa = new(repositorioTarefa.Object);
        Tarefa tarefa = new("Lavar o cachorro", PrioridadeTarefa.Alta);

        repositorioTarefa.Setup(r => r.Editar(It.IsAny<Guid>(), It.IsAny<Tarefa>())).Returns(true);
        repositorioTarefa.Setup(r => r.SelecionarPorId(It.IsAny<Guid>())).Returns(tarefa);

        servicoTarefa.Editar(new(tarefa.Id, "Lavar o Gato", PrioridadeTarefa.Baixa));

        Assert.AreEqual("Lavar o Gato", tarefa.Titulo);
        Assert.AreEqual(PrioridadeTarefa.Baixa, tarefa.Prioridade);
    }
    [TestMethod]
    public void ExcluirTarefaApaga_Registro_E_Itens()
    {
        Mock<IRepositorioTarefa> repositorioTarefa = new();

        ServicoTarefa servicoTarefa = new(repositorioTarefa.Object);
        Tarefa tarefa = new("Lavar o cachorro", PrioridadeTarefa.Alta);

        repositorioTarefa.Setup(r => r.Excluir(It.IsAny<Guid>())).Returns(true);
        repositorioTarefa.Setup(r => r.SelecionarPorId(It.IsAny<Guid>())).Returns(tarefa);

        Result resultado = servicoTarefa.Excluir(tarefa.Id);

        Assert.IsTrue(resultado.IsSuccess);
        repositorioTarefa.Verify(r => r.Excluir(It.IsAny<Guid>()), Times.Once);
    }
    [TestMethod]
    public void SelecionarTodosRetorna_Tarefas_EmAberto_E_Concluidas()
    {
        Mock<IRepositorioTarefa> repositorioTarefa = new();

        ServicoTarefa servicoTarefa = new(repositorioTarefa.Object);

        Tarefa tarefa1 = new("Lavar o cachorro", PrioridadeTarefa.Alta);
        tarefa1.AlterarConclusaoManual(true);

        Tarefa tarefa2 = new("Lavar o Gato", PrioridadeTarefa.Normal);

        repositorioTarefa.Setup(r => r.SelecionarTodos()).Returns([tarefa1, tarefa2]);

        List<ListarTarefasDto> dtos = servicoTarefa.SelecionarTodos();

        Assert.HasCount(2, dtos);
        Assert.IsTrue(dtos[0].Concluida);
        Assert.IsFalse(dtos[1].Concluida);
    }
    [TestMethod]
    public void SelecionarTodosRetorna_Tarefas_Pendentes()
    {
        Mock<IRepositorioTarefa> repositorioTarefa = new();

        ServicoTarefa servicoTarefa = new(repositorioTarefa.Object);

        Tarefa tarefa1 = new("Lavar o cachorro", PrioridadeTarefa.Alta);
        tarefa1.AlterarConclusaoManual(true);

        Tarefa tarefa2 = new("Lavar o Gato", PrioridadeTarefa.Normal);

        repositorioTarefa.Setup(r => r.SelecionarTodos()).Returns([tarefa1, tarefa2]);

        List<ListarTarefasDto> dtos = servicoTarefa.SelecionarTodos("Pendentes");

        Assert.HasCount(1, dtos);
        Assert.IsFalse(dtos[0].Concluida);
    }
    [TestMethod]
    public void SelecionarTodosRetorna_Tarefas_EmAberto()
    {
        Mock<IRepositorioTarefa> repositorioTarefa = new();

        ServicoTarefa servicoTarefa = new(repositorioTarefa.Object);

        Tarefa tarefa1 = new("Lavar o cachorro", PrioridadeTarefa.Alta);
        tarefa1.AlterarConclusaoManual(true);

        Tarefa tarefa2 = new("Lavar o Gato", PrioridadeTarefa.Normal);

        repositorioTarefa.Setup(r => r.SelecionarTodos()).Returns([tarefa1, tarefa2]);

        List<ListarTarefasDto> dtos = servicoTarefa.SelecionarTodos("Concluidas");

        Assert.HasCount(1, dtos);
        Assert.IsTrue(dtos[0].Concluida);
    }
    [TestMethod]
    public void SelecionarTarefa_RetornaListaDeItens()
    {
        Tarefa tarefa1 = new("Lavar o cachorro", PrioridadeTarefa.Alta);
        ItemTarefa item = new("Pegar Shampoo");
        ItemTarefa item2 = new("Pegar Creme");

        tarefa1.AdicionarItem(item);
        tarefa1.AdicionarItem(item2);

        Mock<IRepositorioTarefa> repositorioTarefa = new();

        ServicoTarefa servicoTarefa = new(repositorioTarefa.Object);

        repositorioTarefa.Setup(r => r.SelecionarTodos()).Returns([tarefa1]);

        List<ListarTarefasDto> dtos = servicoTarefa.SelecionarTodos();

        Assert.HasCount(2, dtos.First().Itens);
        Assert.AreEqual("Pegar Shampoo", dtos[0].Itens[0].Titulo);
        Assert.AreEqual("Pegar Creme", dtos[0].Itens[1].Titulo);
    }
}
