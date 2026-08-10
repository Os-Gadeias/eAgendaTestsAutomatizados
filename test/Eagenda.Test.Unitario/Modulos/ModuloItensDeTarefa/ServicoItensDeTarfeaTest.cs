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
}
