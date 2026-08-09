using eAgenda.Dominio.Modulos.ModuloTarefa;

namespace Eagenda.Test.Unitario.Modulos.ModuloItensDeTarefa;

[TestClass]
public class ItensDeTarefaTest
{
    [TestMethod]
    public void Add_Item_A_TarefaExistente_NaoRetornaErros()
    {
        Tarefa tarefa = new("Lavar o Cachorro", PrioridadeTarefa.Alta);
        ItemTarefa itemTarefa = new("Pegar o Shampoo");

        tarefa.AdicionarItem(itemTarefa);

        Assert.HasCount(1, tarefa.Itens);
        Assert.AreSame(tarefa.Itens[0], itemTarefa);
        Assert.AreEqual("Pegar o Shampoo", tarefa.Itens[0].Titulo);
    }
}
