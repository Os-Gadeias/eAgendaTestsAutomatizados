using eAgenda.Aplicacao.Modulos.ModuloTarefa;
using eAgenda.Dominio.Modulos.ModuloTarefa;
using FluentResults;
using Moq;

namespace Eagenda.Test.Unitario.Modulos.ModuloItensDeTarefa;

[TestClass]
public class ServicoItensDeTarefaTest
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
    [TestMethod]
    public void ListagemDeTarefas_RetornaItens_DaTarefa()
    {
        Mock<IRepositorioTarefa> repositorioTarefa = new();

        ServicoTarefa servicoTarefa = new(repositorioTarefa.Object);

        Tarefa tarefa = new("Lavar o Cachorro", PrioridadeTarefa.Alta);

        ItemTarefa item1 = new("Pegar o Shampoo");
        tarefa.AdicionarItem(item1);

        ItemTarefa item2 = new("Pegar o Condicionador");
        tarefa.AdicionarItem(item2);

        repositorioTarefa.Setup(r => r.SelecionarPorId(It.IsAny<Guid>())).Returns(tarefa);

        Result<DetalhesTarefaDto> resultado = servicoTarefa.SelecionarPorId(tarefa.Id);

        DetalhesTarefaDto tarefaSelecionada = resultado.Value;

        Assert.IsTrue(resultado.IsSuccess);
        Assert.IsNotNull(resultado.Value);
        Assert.AreEqual("Pegar o Shampoo", tarefaSelecionada.Itens[0].Titulo);
        Assert.AreEqual("Pegar o Condicionador", tarefaSelecionada.Itens[1].Titulo);
    }
    [TestMethod]
    public void RemoverItem_Atualiza_Porcentagem()
    {
        Mock<IRepositorioTarefa> repositorioTarefa = new();
        ServicoTarefa servicoTarefa = new(repositorioTarefa.Object);

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

        repositorioTarefa.Setup(r => r.SelecionarPorId(It.IsAny<Guid>())).Returns(tarefa);

        Result resultado = servicoTarefa.RemoverItem(new(tarefa.Id, itemTarefa4.Id));

        Assert.IsTrue(resultado.IsSuccess);
        Assert.HasCount(3, tarefa.Itens);
        Assert.AreEqual(33, tarefa.PercentualConcluido);
    }
    [TestMethod]
    public void Remover_UltimoItem_DaLista_ZeraAPorcentagem()
    {
        Mock<IRepositorioTarefa> repositorioTarefa = new();
        ServicoTarefa servicoTarefa = new(repositorioTarefa.Object);

        Tarefa tarefa = new("Lavar o Cachorro", PrioridadeTarefa.Alta);
        ItemTarefa itemTarefa1 = new("Pegar o Shampoo");
        tarefa.AdicionarItem(itemTarefa1);

        repositorioTarefa.Setup(r => r.SelecionarPorId(It.IsAny<Guid>())).Returns(tarefa);

        Result resultado = servicoTarefa.RemoverItem(new(tarefa.Id, itemTarefa1.Id));

        Assert.HasCount(0, tarefa.Itens);
        Assert.AreEqual(0, tarefa.PercentualConcluido);
        Assert.IsFalse(tarefa.Concluida);
    }
}
