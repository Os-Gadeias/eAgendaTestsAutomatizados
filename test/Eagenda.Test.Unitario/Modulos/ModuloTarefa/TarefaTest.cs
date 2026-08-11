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
}
