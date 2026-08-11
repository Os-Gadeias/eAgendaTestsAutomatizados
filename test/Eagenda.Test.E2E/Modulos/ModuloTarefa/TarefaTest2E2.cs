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
}
