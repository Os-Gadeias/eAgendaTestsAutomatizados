using Eagenda.Test.E2E.Compartilhado;
using Microsoft.Playwright;

namespace Eagenda.Test.E2E.Modulos.ModuloContato;

[TestClass]
public class ContatoTestE2E : E2ETestsBase
{
    [TestMethod]
    public async Task CadastarUsuario_ComDados_Validos_PersisteERetornaListagem()
    {
        const string nome = "Thiago Kovalski";
        const string email = "thiagokovalski5@gmail.com";
        const string telefone = "(49) 99999-9999";
        const string cargo = "senior";
        const string empresa = "NDD";

        await Page.GotoAsync(UrlBase + "/Contato/Cadastrar");

        await Page.GetByLabel("Nome").FillAsync(nome);
        await Page.GetByLabel("E-mail").FillAsync(email);
        await Page.GetByLabel("Telefone").FillAsync(telefone);
        await Page.GetByLabel("Cargo").FillAsync(cargo);
        await Page.GetByLabel("Empresa").FillAsync(empresa);

        await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();

        string rotaFinal = new Uri(Page.Url).AbsolutePath;
        Assert.AreEqual("/Contato/Listar", rotaFinal);
        await Expect(Page.GetByText("Thiago Kovalski")).ToBeVisibleAsync();
        await Expect(Page.GetByText("thiagokovalski5@gmail.com")).ToBeVisibleAsync();
        await Expect(Page.GetByText("(49) 99999-9999")).ToBeVisibleAsync();
        await Expect(Page.GetByText("senior")).ToBeVisibleAsync();
        await Expect(Page.GetByText("NDD")).ToBeVisibleAsync();
    }
}
