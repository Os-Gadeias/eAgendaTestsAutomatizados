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
}
