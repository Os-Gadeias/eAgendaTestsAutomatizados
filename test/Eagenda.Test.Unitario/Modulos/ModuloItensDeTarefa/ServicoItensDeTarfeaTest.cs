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
}
