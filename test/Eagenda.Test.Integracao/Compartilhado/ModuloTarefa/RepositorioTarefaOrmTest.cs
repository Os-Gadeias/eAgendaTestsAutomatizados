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
    [TestMethod]
    public void UltimoItemConcluido_AtualizaAPorcentagem_Para100_Porcento()
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
        tarefaAtualizada.AlterarConclusaoItem(itemTarefa2.Id, true);
        tarefaAtualizada.AlterarConclusaoItem(itemTarefa3.Id, true);
        tarefaAtualizada.AlterarConclusaoItem(itemTarefa4.Id, true);

        repositorioTarefa = new(dbContext);

        repositorioTarefa.Cadastrar(tarefa);
        dbContext.ChangeTracker.Clear();

        repositorioTarefa.Editar(tarefa.Id, tarefaAtualizada);
        dbContext.ChangeTracker.Clear();

        Tarefa? tarefaEditada = repositorioTarefa.SelecionarPorId(tarefa.Id);

        Assert.IsNotNull(tarefaEditada);
        Assert.AreEqual(100, tarefaEditada.PercentualConcluido);
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
        tarefa.AdicionarItem(itemTarefa2);
        tarefa.AdicionarItem(itemTarefa3);
        tarefa.AdicionarItem(itemTarefa4);


        Tarefa tarefaAtualizada = new("Lavar o Cachorro", PrioridadeTarefa.Alta);
        tarefaAtualizada.AdicionarItem(itemTarefa1);
        tarefaAtualizada.AdicionarItem(itemTarefa2);
        tarefaAtualizada.AdicionarItem(itemTarefa3);
        tarefaAtualizada.AdicionarItem(itemTarefa4);

        tarefaAtualizada.AlterarConclusaoItem(itemTarefa1.Id, true);
        tarefaAtualizada.AlterarConclusaoItem(itemTarefa2.Id, true);
        tarefaAtualizada.AlterarConclusaoItem(itemTarefa3.Id, true);
        tarefaAtualizada.AlterarConclusaoItem(itemTarefa4.Id, true);

        tarefaAtualizada.AlterarConclusaoItem(itemTarefa4.Id, false);

        repositorioTarefa = new(dbContext);

        repositorioTarefa.Cadastrar(tarefa);
        dbContext.ChangeTracker.Clear();

        repositorioTarefa.Editar(tarefa.Id, tarefaAtualizada);
        dbContext.ChangeTracker.Clear();

        Tarefa? tarefaEditada = repositorioTarefa.SelecionarPorId(tarefa.Id);

        Assert.IsNotNull(tarefaEditada);
        Assert.AreEqual(75, tarefaEditada.PercentualConcluido);
    }
    [TestMethod]
    public void AlterarTitulo_DeTarefa_PersisteNaEdicao()
    {
        Tarefa tarefa = new("Lavar o Cachorro", PrioridadeTarefa.Alta);
        ItemTarefa itemTarefa1 = new("Pegar o Shampoo");
        tarefa.AdicionarItem(itemTarefa1);

        repositorioTarefa.Cadastrar(tarefa);

        Tarefa tarefaAtualizada = new("Lavar o Cachorro", PrioridadeTarefa.Alta);
        ItemTarefa itemAtualizado = new("Passar Perfume");
        tarefaAtualizada.AdicionarItem(itemAtualizado);

        repositorioTarefa = new(dbContext);

        repositorioTarefa.Editar(tarefa.Id, tarefaAtualizada);
        dbContext.ChangeTracker.Clear();

        Tarefa? tarefaEditada = repositorioTarefa.SelecionarPorId(tarefa.Id);
        Assert.IsNotNull(tarefaEditada);
        Assert.AreEqual("Passar Perfume", tarefaEditada.Itens.First().Titulo);
    }
    [TestMethod]
    public void ListagemDeTarefas_RetornaItens_DaTarefa()
    {
        Tarefa tarefa = new("Lavar o Cachorro", PrioridadeTarefa.Alta);

        ItemTarefa item1 = new("Pegar o Shampoo");
        tarefa.AdicionarItem(item1);

        ItemTarefa item2 = new("Pegar o Condicionador");
        tarefa.AdicionarItem(item2);

        repositorioTarefa = new(dbContext);

        repositorioTarefa.Cadastrar(tarefa);
        dbContext.ChangeTracker.Clear();

        Tarefa? tarefaSelecionada = repositorioTarefa.SelecionarPorId(tarefa.Id);

        Assert.IsNotNull(tarefaSelecionada);
        Assert.AreEqual("Pegar o Condicionador", tarefaSelecionada.Itens[0].Titulo);
        Assert.AreEqual("Pegar o Shampoo", tarefaSelecionada.Itens[1].Titulo);
    }
    [TestMethod]
    public void RemoverItem_Atualiza_Porcentagem()
    {
        Tarefa tarefa = new("Lavar o Cachorro", PrioridadeTarefa.Alta);
        ItemTarefa itemTarefa1 = new("Pegar o Shampoo");
        ItemTarefa itemTarefa2 = new("Pegar o Condicionador");
        ItemTarefa itemTarefa3 = new("Secar o Cachorro");
        ItemTarefa itemTarefa4 = new("Passar Perfume");

        tarefa.AdicionarItem(itemTarefa1);
        tarefa.AlterarConclusaoItem(itemTarefa1.Id, true);
        tarefa.AdicionarItem(itemTarefa2);
        tarefa.AdicionarItem(itemTarefa3);
        tarefa.AdicionarItem(itemTarefa4);

        repositorioTarefa.Cadastrar(tarefa);

        Tarefa tarefaAtualizada = new("Lavar o Cachorro", PrioridadeTarefa.Alta);
        tarefaAtualizada.AdicionarItem(itemTarefa1);
        tarefaAtualizada.AlterarConclusaoItem(itemTarefa1.Id, true);
        tarefaAtualizada.AdicionarItem(itemTarefa2);
        tarefaAtualizada.AdicionarItem(itemTarefa3);
        tarefaAtualizada.AdicionarItem(itemTarefa4);

        tarefaAtualizada.RemoverItem(itemTarefa4.Id);

        bool conseguiuEditar = repositorioTarefa.Editar(tarefa.Id, tarefaAtualizada);

        Assert.IsTrue(conseguiuEditar);
        Assert.HasCount(3, tarefa.Itens);
        Assert.AreEqual(33, tarefa.PercentualConcluido);
    }
    [TestMethod]
    public void RemoverUltimoITem_Atualiza_PorcentagemParaZero()
    {
        Tarefa tarefa = new("Lavar o Cachorro", PrioridadeTarefa.Alta);
        ItemTarefa itemTarefa1 = new("Pegar o Shampoo");

        tarefa.AdicionarItem(itemTarefa1);
        tarefa.AlterarConclusaoItem(itemTarefa1.Id, true);

        repositorioTarefa.Cadastrar(tarefa);

        Tarefa tarefaAtualizada = new("Lavar o Cachorro", PrioridadeTarefa.Alta);
        tarefaAtualizada.AdicionarItem(itemTarefa1);
        tarefaAtualizada.AlterarConclusaoItem(itemTarefa1.Id, true);

        tarefaAtualizada.RemoverItem(itemTarefa1.Id);

        bool conseguiuEditar = repositorioTarefa.Editar(tarefa.Id, tarefaAtualizada);

        Assert.IsTrue(conseguiuEditar);
        Assert.HasCount(0, tarefa.Itens);
        Assert.AreEqual(0, tarefa.PercentualConcluido);
    }
    [TestMethod]
    public void Cadastrar_Tarefa_ComDadosValidos_NaoRetornaErros()
    {
        Tarefa tarefa = new("Lavar o cachorro", PrioridadeTarefa.Alta);

        repositorioTarefa.Cadastrar(tarefa);
        dbContext.ChangeTracker.Clear();

        Tarefa? tarefaSelecionada = repositorioTarefa.SelecionarPorId(tarefa.Id);

        Assert.IsNotNull(tarefaSelecionada);
        Assert.AreEqual("Lavar o cachorro", tarefaSelecionada.Titulo);
        Assert.AreEqual(PrioridadeTarefa.Alta, tarefaSelecionada.Prioridade);
    }
    [TestMethod]
    public void TarefaCriada_ComDataAtual_StatusPendente_SemPercentual_SemDataDeConclusao()
    {
        Tarefa tarefa = new("Lavar o cachorro", PrioridadeTarefa.Alta);

        repositorioTarefa.Cadastrar(tarefa);
        dbContext.ChangeTracker.Clear();

        Tarefa? tarefaSelecionada = repositorioTarefa.SelecionarPorId(tarefa.Id);

        Assert.IsNotNull(tarefaSelecionada);
        Assert.AreEqual("Lavar o cachorro", tarefaSelecionada.Titulo);
        Assert.AreEqual(PrioridadeTarefa.Alta, tarefaSelecionada.Prioridade);
        Assert.IsFalse(tarefa.Concluida);
        Assert.AreEqual(0, tarefa.PercentualConcluido);
        Assert.AreEqual(DateTime.Today, tarefa.DataCriacao);
        Assert.IsNull(tarefa.DataConclusao);
    }
    [TestMethod]
    public void Tarefa_Cadastrada_Com_Os_Dois_Itens_Vinculados_E_Percentual_Em_0()
    {
        Tarefa tarefa = new("Lavar o cachorro", PrioridadeTarefa.Alta);
        ItemTarefa item1 = new("Pegar Shampoo");
        ItemTarefa item2 = new("Pegar Condicionador");
        tarefa.AdicionarItem(item1);
        tarefa.AdicionarItem(item2);

        repositorioTarefa.Cadastrar(tarefa);
        dbContext.ChangeTracker.Clear();

        Tarefa? tarefaSelecionada = repositorioTarefa.SelecionarPorId(tarefa.Id);

        Assert.IsNotNull(tarefaSelecionada);
        Assert.HasCount(2, tarefaSelecionada.Itens);
        Assert.AreEqual(0, tarefaSelecionada.PercentualConcluido);
    }
    [TestMethod]
    public void TarefaConcluida_AlteraDataDeConclusao_E_Status_PersisteNoBanco()
    {
        Tarefa tarefa = new("Lavar o cachorro", PrioridadeTarefa.Alta);

        tarefa.AlterarConclusaoManual(true);

        repositorioTarefa.Cadastrar(tarefa);
        dbContext.ChangeTracker.Clear();

        Tarefa? tarefaSelecionada = repositorioTarefa.SelecionarPorId(tarefa.Id);

        Assert.IsTrue(tarefaSelecionada!.Concluida);
        Assert.AreEqual(DateTime.Today, tarefaSelecionada.DataConclusao);
        Assert.AreEqual(100, tarefaSelecionada.PercentualConcluido);
    }

}
