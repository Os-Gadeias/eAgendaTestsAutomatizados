using Eagenda.Test.E2E.Compartilhado;
using Microsoft.Playwright;

namespace Eagenda.Test.E2E.Modulos.ModuloTarefa;

[TestClass]
public sealed class TarefaTest2E2 : E2ETestsBase
{
    [TestMethod]
    public async Task Cadastrar_Tarefa_ComDadosValidos_NaoRetornaErros()
    {
        await Page.GotoAsync(UrlBase + "/Tarefa/Cadastrar");

        await Page.GetByLabel("Título").FillAsync("Lavar o Cachorro");

        await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();

        string rotaAbsoluta = new Uri(Page.Url).AbsolutePath;
        Assert.AreEqual("/Tarefa/Listar", rotaAbsoluta);
        await Expect(Page.GetByText("Lavar o Cachorro")).ToBeVisibleAsync();
        await Expect(Page.GetByText("Normal")).ToBeVisibleAsync();
    }
    [TestMethod]
    public async Task Tarefa_Cadastrada_Com_Os_Dois_Itens_Vinculados_E_Percentual_Em_0()
    {
        await Page.GotoAsync(UrlBase + "/Tarefa/Cadastrar");

        await Page.GetByLabel("Título").FillAsync("Lavar o Cachorro");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();

        await Page.GetByRole(AriaRole.Link, new() { Name = "Itens" }).ClickAsync();

        await Page.GetByLabel("Novo Item").FillAsync("Pegar Shampoo");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Adicionar" }).ClickAsync();

        await Page.GetByLabel("Novo Item").FillAsync("Pegar o Condicionador");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Adicionar" }).ClickAsync();

        await Expect(Page.GetByText("0%")).ToHaveCountAsync(2);
    }
    [TestMethod]
    public async Task TarefaCadastrada_SemTitulo_RetornaErro()
    {
        await Page.GotoAsync(UrlBase + "/Tarefa/Cadastrar");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();

        string rotaAbsoluta = new Uri(Page.Url).AbsolutePath;
        Assert.AreEqual("/Tarefa/Cadastrar", rotaAbsoluta);
        await Expect(Page.GetByText("O campo \"Título\" deve ser preenchido.")).ToBeVisibleAsync();
    }
    [TestMethod]
    public async Task TarefaConcluida_AlteraDataDeConclusao_E_Status()
    {
        await Page.GotoAsync(UrlBase + "/Tarefa/Cadastrar");

        await Page.GetByLabel("Título").FillAsync("Lavar o Cachorro");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();

        await Page.GetByRole(AriaRole.Button, new() { Name = "Concluir" }).ClickAsync();
        await Expect(Page.GetByText("100%")).ToBeInViewportAsync();
    }
    [TestMethod]
    public async Task EditarTarefa_Existente_RetornaNaListagem()
    {
        await Page.GotoAsync(UrlBase + "/Tarefa/Cadastrar");

        await Page.GetByLabel("Título").FillAsync("Lavar o Cachorro");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();

        await Page.GetByRole(AriaRole.Link, new() { Name = "Editar" }).ClickAsync();

        await Page.GetByLabel("Título").FillAsync("Pegar o Shampoo");
        await Page.GetByLabel("Prioridade").SelectOptionAsync("Alta");

        await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();

        await Expect(Page.GetByText("Pegar o Shampoo")).ToBeVisibleAsync();
        await Expect(Page.GetByText("Alta")).ToBeVisibleAsync();
    }
    [TestMethod]
    public async Task ExcluirTarefaApaga_Registro_E_Itens()
    {
        await Page.GotoAsync(UrlBase + "/Tarefa/Cadastrar");

        await Page.GetByLabel("Título").FillAsync("Lavar o Cachorro");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();

        await Page.GetByRole(AriaRole.Link, new() { Name = "Excluir" }).ClickAsync();

        await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();

        await Expect(Page.GetByText("Lavar o Cachorro")).Not.ToBeVisibleAsync();
    }
    [TestMethod]
    public async Task SelecionarTodosRetorna_Tarefas_EmAberto_E_Concluidas()
    {
        await Page.GotoAsync(UrlBase + "/Tarefa/Cadastrar");

        await Page.GetByLabel("Título").FillAsync("Lavar o Cachorro");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();
        await Page.GetByRole(AriaRole.Button, new() { Name = "Concluir" }).ClickAsync();

        await Page.GotoAsync(UrlBase + "/Tarefa/Cadastrar");
        await Page.GetByLabel("Título").FillAsync("Lavar o Gato");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();

        await Expect(Page.GetByText("Pendente", new() { Exact = true })).ToHaveCountAsync(2);
        await Expect(Page.GetByText("100%")).ToBeInViewportAsync();

    }
    [TestMethod]
    public async Task SelecionarTodosRetorna_Tarefas_Pendentes()
    {
        await Page.GotoAsync(UrlBase + "/Tarefa/Cadastrar");

        await Page.GetByLabel("Título").FillAsync("Lavar o Cachorro");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();
        await Page.GetByRole(AriaRole.Button, new() { Name = "Concluir" }).ClickAsync();

        await Page.GotoAsync(UrlBase + "/Tarefa/Cadastrar");
        await Page.GetByLabel("Título").FillAsync("Lavar o Gato");

        await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();

        await Page.GetByRole(AriaRole.Link, new() { Name = "Pendentes" }).ClickAsync();

        await Expect(Page.GetByText("Pendente", new() { Exact = true })).ToHaveCountAsync(2);
        await Expect(Page.GetByText("100%")).Not.ToBeVisibleAsync();
        await Expect(Page.GetByText("Concluída", new() { Exact = true })).Not.ToBeVisibleAsync();
    }
    [TestMethod]
    public async Task SelecionarTodosRetorna_Tarefas_Concluidas()
    {
        await Page.GotoAsync(UrlBase + "/Tarefa/Cadastrar");

        await Page.GetByLabel("Título").FillAsync("Lavar o Cachorro");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();
        await Page.GetByRole(AriaRole.Button, new() { Name = "Concluir" }).ClickAsync();

        await Page.GotoAsync(UrlBase + "/Tarefa/Cadastrar");
        await Page.GetByLabel("Título").FillAsync("Lavar o Gato");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();

        await Page.GetByRole(AriaRole.Link, new() { Name = "Concluídas" }).ClickAsync();

        await Expect(Page.GetByText("Pendente", new() { Exact = true })).Not.ToBeVisibleAsync();
        await Expect(Page.GetByText("100%")).ToBeVisibleAsync();
        await Expect(Page.GetByText("Concluída", new() { Exact = true })).ToBeVisibleAsync();
    }
    [TestMethod]
    public async Task SelecionarTodosRetorna_Tarefas_PorPrioridade()
    {
        await Page.GotoAsync(UrlBase + "/Tarefa/Cadastrar");

        await Page.GetByLabel("Título").FillAsync("Lavar o Cachorro");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();

        await Page.GotoAsync(UrlBase + "/Tarefa/Cadastrar");
        await Page.GetByLabel("Título").FillAsync("Lavar o Gato");
        await Page.GetByLabel("Prioridade").SelectOptionAsync("Alta");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();

        await Page.GotoAsync(UrlBase + "/Tarefa/Cadastrar");
        await Page.GetByLabel("Título").FillAsync("Lavar o Thanos");
        await Page.GetByLabel("Prioridade").SelectOptionAsync("Baixa");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();

        await Page.GetByRole(AriaRole.Link, new() { Name = "Prioridade" }).ClickAsync();
        await Expect(Page.GetByText("Normal")).ToHaveCountAsync(2);
        await Expect(Page.GetByText("Alta")).ToHaveCountAsync(2);
        await Expect(Page.GetByText("Baixa")).ToHaveCountAsync(2);

    }
}
