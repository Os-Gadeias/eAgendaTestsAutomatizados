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
    [TestMethod]
    public void Add_Item_A_TarefaExistente_SemTituloNosItens_RetornaErros()
    {
        Tarefa tarefa = new("Lavar o Cachorro", PrioridadeTarefa.Alta);
        ItemTarefa itemTarefa = new(string.Empty);

        tarefa.AdicionarItem(itemTarefa);

        List<string> erros = tarefa.Validar();

        Assert.HasCount(1, erros);
        Assert.AreEqual("O campo \"Título\" do Item Tarefa é obrigatório!", erros.First());
    }
    [TestMethod]
    public void Add_Item_A_TarefaExistente_ComTituloPequeno_RetornaErros()
    {
        Tarefa tarefa = new("Lavar o Cachorro", PrioridadeTarefa.Alta);
        ItemTarefa itemTarefa = new("a");

        tarefa.AdicionarItem(itemTarefa);

        List<string> erros = tarefa.Validar();

        Assert.HasCount(1, erros);
        Assert.AreEqual("O campo \"Título\" do Item Tarefa deve conter entre 2 e 100 caracteres.", erros.First());
    }
    [TestMethod]
    public void Add_Item_A_TarefaExistente_ComTituloGrande_RetornaErros()
    {
        Tarefa tarefa = new("Lavar o Cachorro", PrioridadeTarefa.Alta);
        ItemTarefa itemTarefa = new(new string('s', 101));

        tarefa.AdicionarItem(itemTarefa);

        List<string> erros = tarefa.Validar();

        Assert.HasCount(1, erros);
        Assert.AreEqual("O campo \"Título\" do Item Tarefa deve conter entre 2 e 100 caracteres.", erros.First());
    }
    [TestMethod]
    public void ListaComItens_AtualizaAPorcentagem_DeAcordoCom_OsItensConcluidos()
    {
        Tarefa tarefa = new("Lavar o Cachorro", PrioridadeTarefa.Alta);
        ItemTarefa itemTarefa1 = new("Pegar o Shampoo");
        ItemTarefa itemTarefa2 = new("Pegar o Condicionador");
        ItemTarefa itemTarefa3 = new("Secar o Cachorro");
        ItemTarefa itemTarefa4 = new("Passar Perfume");

        tarefa.AdicionarItem(itemTarefa1);
        tarefa.AdicionarItem(itemTarefa2);
        tarefa.AdicionarItem(itemTarefa3);
        tarefa.AdicionarItem(itemTarefa4);

        tarefa.AlterarConclusaoItem(itemTarefa1.Id, true);

        Assert.AreEqual(25, tarefa.PercentualConcluido);
        Assert.IsTrue(itemTarefa1.Concluido);
    }
    [TestMethod]
    public void UltimoItemConcluido_AtualizaAPorcentagem_Para100_Porcento()
    {
        Tarefa tarefa = new("Lavar o Cachorro", PrioridadeTarefa.Alta);
        ItemTarefa itemTarefa1 = new("Pegar o Shampoo");
        ItemTarefa itemTarefa2 = new("Pegar o Condicionador");
        ItemTarefa itemTarefa3 = new("Secar o Cachorro");
        ItemTarefa itemTarefa4 = new("Passar Perfume");

        tarefa.AdicionarItem(itemTarefa1);
        tarefa.AlterarConclusaoItem(itemTarefa1.Id, true);
        tarefa.AdicionarItem(itemTarefa2);
        tarefa.AlterarConclusaoItem(itemTarefa2.Id, true);
        tarefa.AdicionarItem(itemTarefa3);
        tarefa.AlterarConclusaoItem(itemTarefa3.Id, true);

        tarefa.AdicionarItem(itemTarefa4);
        tarefa.AlterarConclusaoItem(itemTarefa4.Id, true);

        Assert.AreEqual(100, tarefa.PercentualConcluido);
    }
    [TestMethod]
    public void ReabrirItemConcluido_AtualizaAPorcentagem_Para75_Porcento()
    {
        Tarefa tarefa = new("Lavar o Cachorro", PrioridadeTarefa.Alta);
        ItemTarefa itemTarefa1 = new("Pegar o Shampoo");
        ItemTarefa itemTarefa2 = new("Pegar o Condicionador");
        ItemTarefa itemTarefa3 = new("Secar o Cachorro");
        ItemTarefa itemTarefa4 = new("Passar Perfume");

        tarefa.AdicionarItem(itemTarefa1);
        tarefa.AlterarConclusaoItem(itemTarefa1.Id, true);
        tarefa.AdicionarItem(itemTarefa2);
        tarefa.AlterarConclusaoItem(itemTarefa2.Id, true);
        tarefa.AdicionarItem(itemTarefa3);
        tarefa.AlterarConclusaoItem(itemTarefa3.Id, true);
        tarefa.AdicionarItem(itemTarefa4);

        tarefa.AlterarConclusaoItem(itemTarefa4.Id, false);

        Assert.AreEqual(75, tarefa.PercentualConcluido);
    }
}
