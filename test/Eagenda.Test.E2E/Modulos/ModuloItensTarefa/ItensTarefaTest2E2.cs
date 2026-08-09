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
}
