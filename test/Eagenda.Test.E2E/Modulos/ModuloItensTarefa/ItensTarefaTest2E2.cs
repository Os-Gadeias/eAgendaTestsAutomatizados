using eAgenda.Dominio.Modulos.ModuloTarefa;
using Eagenda.Test.E2E.Compartilhado;
using Microsoft.Playwright;

namespace Eagenda.Test.E2E.Modulos.ModuloItensTarefa;

[TestClass]
public class ItensTarefaTest2E2 : E2ETestsBase
{
    [TestMethod]
    public async Task AdicionarItem_NaTarefa_Persiste()
    {
        await Page.GotoAsync(UrlBase + "/Tarefa/Cadastrar");

        await Page.GetByLabel("Título").FillAsync("Lavar o Cachorro");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();

        await Page.GetByRole(AriaRole.Link, new() { Name = "Itens" }).ClickAsync();

        await Page.GetByLabel("Novo Item").FillAsync("Pegar Shampoo");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Adicionar" }).ClickAsync();

        string rotaAbsoluta = new Uri(Page.Url).AbsolutePath;

        Assert.Contains("/Tarefa/GerenciarItens/", rotaAbsoluta);
        await Expect(Page.GetByText("Pegar Shampoo")).ToBeVisibleAsync();
    }
    [TestMethod]
    public async Task ListaComItens_AtualizaAPorcentagem_DeAcordoCom_OsItensConcluidos()
    {
        await Page.GotoAsync(UrlBase + "/Tarefa/Cadastrar");

        await Page.GetByLabel("Título").FillAsync("Lavar o Cachorro");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();

        await Page.GetByRole(AriaRole.Link, new() { Name = "Itens" }).ClickAsync();

        await Page.GetByLabel("Novo Item").FillAsync("Pegar Shampoo");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Adicionar" }).ClickAsync();

        await Page.GetByLabel("Novo Item").FillAsync("Pegar o Condicionador");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Adicionar" }).ClickAsync();

        await Page.GetByLabel("Novo Item").FillAsync("Secar o Cachorro");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Adicionar" }).ClickAsync();

        await Page.GetByLabel("Novo Item").FillAsync("Passar Perfume");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Adicionar" }).ClickAsync();

        var item = Page
            .GetByText("Pegar o Condicionador")
            .Locator("../.."); //pega o elemento pai da div que o "Pegar Condicionador" está

        await item
            .GetByRole(AriaRole.Button, new() { Name = "Concluir" })
            .ClickAsync();

        await Expect(Page.GetByText("25%")).ToHaveCountAsync(2);
    }
    [TestMethod]
    public async Task UltimoItemConcluido_AtualizaAPorcentagem_Para100_Porcento()
    {
        await Page.GotoAsync(UrlBase + "/Tarefa/Cadastrar");

        await Page.GetByLabel("Título").FillAsync("Lavar o Cachorro");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();

        await Page.GetByRole(AriaRole.Link, new() { Name = "Itens" }).ClickAsync();

        await Page.GetByLabel("Novo Item").FillAsync("Pegar Shampoo");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Adicionar" }).ClickAsync();

        await Page.GetByLabel("Novo Item").FillAsync("Pegar o Condicionador");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Adicionar" }).ClickAsync();

        await Page.GetByLabel("Novo Item").FillAsync("Secar o Cachorro");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Adicionar" }).ClickAsync();

        await Page.GetByLabel("Novo Item").FillAsync("Passar Perfume");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Adicionar" }).ClickAsync();

        while (await Page.GetByRole(AriaRole.Button, new() { Name = "Concluir" }).CountAsync() > 0)
        {
            await Page
                .GetByRole(AriaRole.Button, new() { Name = "Concluir" })
                .First
                .ClickAsync();
        }

        await Expect(Page.GetByText("100%")).ToHaveCountAsync(2);
    }
}
