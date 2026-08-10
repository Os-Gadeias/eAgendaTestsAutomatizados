using eAgenda.Aplicacao.Modulos.ModuloTarefa;
using eAgenda.Dominio.Modulos.ModuloTarefa;
using FluentResults;
using Moq;

namespace Eagenda.Test.Unitario.Modulos.ModuloItensDeTarefa;

[TestClass]
public class ServicoItensDeTarfeaTest
{
    [TestMethod]
    public void Add_Item_A_TarefaExistente_NaoRetornaErros()
    {
        Mock<IRepositorioTarefa> repositorioTarefa = new();

        ServicoTarefa servicoTarefa = new(repositorioTarefa.Object);

        Tarefa tarefa = new("lavar o cachorro", PrioridadeTarefa.Alta);

        repositorioTarefa.Setup(r => r.SelecionarPorId(It.IsAny<Guid>())).Returns(tarefa);
        repositorioTarefa.Setup(r => r.Editar(It.IsAny<Guid>(), It.IsAny<Tarefa>())).Returns(true);

        Result resultado = servicoTarefa.AdicionarItem(new(tarefa.Id, "Pegar Shampoo"));

        Assert.IsTrue(resultado.IsSuccess);
        Assert.IsNotNull(tarefa.Itens.First());
        Assert.AreEqual("Pegar Shampoo", tarefa.Itens.First().Titulo);
        repositorioTarefa.Verify(r => r.Editar(It.IsAny<Guid>(), It.IsAny<Tarefa>()), Times.Once);
    }
    [TestMethod]
    public void Add_Item_A_TarefaExistente_SemTitulo_RetornaErros()
    {
        Mock<IRepositorioTarefa> repositorioTarefa = new();

        ServicoTarefa servicoTarefa = new(repositorioTarefa.Object);

        Tarefa tarefa = new("lavar o cachorro", PrioridadeTarefa.Alta);

        repositorioTarefa.Setup(r => r.SelecionarPorId(It.IsAny<Guid>())).Returns(tarefa);
        repositorioTarefa.Setup(r => r.Editar(It.IsAny<Guid>(), It.IsAny<Tarefa>())).Returns(false);

        Result resultado = servicoTarefa.AdicionarItem(new(tarefa.Id, string.Empty));

        Assert.IsTrue(resultado.IsFailed);
        Assert.HasCount(0, tarefa.Itens);
        Assert.Contains("O campo \"Título\" do Item Tarefa é obrigatório!", resultado.Errors.First().Message);
        repositorioTarefa.Verify(r => r.Editar(It.IsAny<Guid>(), It.IsAny<Tarefa>()), Times.Never);
    }
    [TestMethod]
    public void AdicionarItem_SemTarefa_Vinculada_RetornaErro()
    {
        Mock<IRepositorioTarefa> repositorioTarefa = new();

        ServicoTarefa servicoTarefa = new(repositorioTarefa.Object);

        Tarefa tarefa = new("lavar o cachorro", PrioridadeTarefa.Alta);

        repositorioTarefa.Setup(r => r.SelecionarPorId(It.IsAny<Guid>())).Returns<Tarefa?>(null!);
        repositorioTarefa.Setup(r => r.Editar(It.IsAny<Guid>(), It.IsAny<Tarefa>())).Returns(false);

        Result resultado = servicoTarefa.AdicionarItem(new(tarefa.Id, string.Empty));

        Assert.IsTrue(resultado.IsFailed);
        Assert.Contains("Tarefa não encontrada.", resultado.Errors.First().Message);
        repositorioTarefa.Verify(r => r.Editar(It.IsAny<Guid>(), It.IsAny<Tarefa>()), Times.Never);
    }
    [TestMethod]
    public void ListaComItens_AtualizaAPorcentagem_DeAcordoCom_OsItensConcluidos()
    {
        Mock<IRepositorioTarefa> repositorioTarefa = new();

        ServicoTarefa servicoTarefa = new(repositorioTarefa.Object);

        Tarefa tarefa = new("Lavar o Cachorro", PrioridadeTarefa.Alta);

        repositorioTarefa.Setup(r => r.SelecionarPorId(It.IsAny<Guid>())).Returns(tarefa);
        repositorioTarefa.Setup(r => r.Editar(It.IsAny<Guid>(), It.IsAny<Tarefa>())).Returns(true);

        servicoTarefa.AdicionarItem(new(tarefa.Id, "Pegar o Shampoo"));
        servicoTarefa.AdicionarItem(new(tarefa.Id, "Pegar o Condicionador"));
        servicoTarefa.AdicionarItem(new(tarefa.Id, "Secar o Cachorro"));
        servicoTarefa.AdicionarItem(new(tarefa.Id, "Passar Perfume"));

        servicoTarefa.AlterarConclusaoItem(new(tarefa.Id, tarefa.Itens.First().Id, true));

        Assert.AreEqual(25, tarefa.PercentualConcluido);
        Assert.IsTrue(tarefa.Itens.First().Concluido);
        repositorioTarefa.Verify(r => r.Editar(It.IsAny<Guid>(), It.IsAny<Tarefa>()), Times.AtLeast(5));
    }
    [TestMethod]
    public void UltimoItemConcluido_AtualizaAPorcentagem_Para100_Porcento()
    {
        Mock<IRepositorioTarefa> repositorioTarefa = new();

        ServicoTarefa servicoTarefa = new(repositorioTarefa.Object);

        Tarefa tarefa = new("Lavar o Cachorro", PrioridadeTarefa.Alta);

        repositorioTarefa.Setup(r => r.SelecionarPorId(It.IsAny<Guid>())).Returns(tarefa);
        repositorioTarefa.Setup(r => r.Editar(It.IsAny<Guid>(), It.IsAny<Tarefa>())).Returns(true);

        servicoTarefa.AdicionarItem(new(tarefa.Id, "Pegar o Shampoo"));
        servicoTarefa.AdicionarItem(new(tarefa.Id, "Pegar o Condicionador"));
        servicoTarefa.AdicionarItem(new(tarefa.Id, "Secar o Cachorro"));
        servicoTarefa.AdicionarItem(new(tarefa.Id, "Passar Perfume"));

        servicoTarefa.AlterarConclusaoItem(new(tarefa.Id, tarefa.Itens[0].Id, true));
        servicoTarefa.AlterarConclusaoItem(new(tarefa.Id, tarefa.Itens[1].Id, true));
        servicoTarefa.AlterarConclusaoItem(new(tarefa.Id, tarefa.Itens[2].Id, true));
        servicoTarefa.AlterarConclusaoItem(new(tarefa.Id, tarefa.Itens[3].Id, true));

        Assert.AreEqual(100, tarefa.PercentualConcluido);
        Assert.IsTrue(tarefa.Itens[0].Concluido);
        Assert.IsTrue(tarefa.Itens[1].Concluido);
        Assert.IsTrue(tarefa.Itens[2].Concluido);
        Assert.IsTrue(tarefa.Itens[3].Concluido);
    }
    [TestMethod]
    public void ReabrirItemConcluido_AtualizaAPorcentagem_Para75_Porcento()
    {
        Mock<IRepositorioTarefa> repositorioTarefa = new();

        ServicoTarefa servicoTarefa = new(repositorioTarefa.Object);

        Tarefa tarefa = new("Lavar o Cachorro", PrioridadeTarefa.Alta);

        repositorioTarefa.Setup(r => r.SelecionarPorId(It.IsAny<Guid>())).Returns(tarefa);
        repositorioTarefa.Setup(r => r.Editar(It.IsAny<Guid>(), It.IsAny<Tarefa>())).Returns(true);

        servicoTarefa.AdicionarItem(new(tarefa.Id, "Pegar o Shampoo"));
        servicoTarefa.AdicionarItem(new(tarefa.Id, "Pegar o Condicionador"));
        servicoTarefa.AdicionarItem(new(tarefa.Id, "Secar o Cachorro"));
        servicoTarefa.AdicionarItem(new(tarefa.Id, "Passar Perfume"));

        servicoTarefa.AlterarConclusaoItem(new(tarefa.Id, tarefa.Itens[0].Id, true));
        servicoTarefa.AlterarConclusaoItem(new(tarefa.Id, tarefa.Itens[1].Id, true));
        servicoTarefa.AlterarConclusaoItem(new(tarefa.Id, tarefa.Itens[2].Id, true));
        servicoTarefa.AlterarConclusaoItem(new(tarefa.Id, tarefa.Itens[3].Id, true));

        servicoTarefa.AlterarConclusaoItem(new(tarefa.Id, tarefa.Itens[3].Id, false));

        Assert.AreEqual(75, tarefa.PercentualConcluido);
        Assert.IsTrue(tarefa.Itens[0].Concluido);
        Assert.IsTrue(tarefa.Itens[1].Concluido);
        Assert.IsTrue(tarefa.Itens[2].Concluido);
        Assert.IsFalse(tarefa.Itens[3].Concluido);
    }
}
