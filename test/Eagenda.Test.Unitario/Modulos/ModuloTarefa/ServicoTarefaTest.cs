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
}
