using eAgenda.Dominio.Modulos.ModuloTarefa;

namespace Eagenda.Test.Integracao.Compartilhado.ModuloTarefa;

[TestClass]
public class RepositorioTarefaOrmTest : RepositorioOrmTestBase
{
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



        Tarefa tarefaAtualizada = new("Lavar o Cachorro", PrioridadeTarefa.Alta);
        tarefaAtualizada.AdicionarItem(itemTarefa1);
        tarefaAtualizada.AdicionarItem(itemTarefa2);
        tarefaAtualizada.AdicionarItem(itemTarefa3);
        tarefaAtualizada.AdicionarItem(itemTarefa4);

        tarefaAtualizada.AlterarConclusaoItem(itemTarefa1.Id, true);

        repositorioTarefa = new(dbContext);

        repositorioTarefa.Cadastrar(tarefa);
        dbContext.ChangeTracker.Clear();

        repositorioTarefa.Editar(tarefa.Id, tarefaAtualizada);
        dbContext.ChangeTracker.Clear();

        Tarefa? tarefaEditada = repositorioTarefa.SelecionarPorId(tarefa.Id);

        Assert.IsNotNull(tarefaEditada);
        Assert.AreEqual(25, tarefaEditada.PercentualConcluido);
    }
}
