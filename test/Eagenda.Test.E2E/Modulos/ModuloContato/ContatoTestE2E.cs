using eAgenda.Dominio.Modulos.ModuloContato;
using Eagenda.Test.E2E.Compartilhado;
using Microsoft.Playwright;

namespace Eagenda.Test.E2E.Modulos.ModuloContato;

[TestClass]
public class ContatoTestE2E : E2ETestsBase
{
    private const string nome = "Thiago Kovalski";
    private const string email = "thiagokovalski5@gmail.com";
    private const string telefone = "(49) 99999-9999";
    private const string cargo = "senior";
    private const string empresa = "NDD";
    [TestMethod]
    public async Task CadastarUsuario_ComDados_Validos_PersisteERetornaListagem()
    {
        await CadastrarUsuario();

        string rotaFinal = new Uri(Page.Url).AbsolutePath;
        Assert.AreEqual("/Contato/Listar", rotaFinal);
        await Expect(Page.GetByText("Thiago Kovalski")).ToBeVisibleAsync();
        await Expect(Page.GetByText("thiagokovalski5@gmail.com")).ToBeVisibleAsync();
        await Expect(Page.GetByText("(49) 99999-9999")).ToBeVisibleAsync();
        await Expect(Page.GetByText("senior")).ToBeVisibleAsync();
        await Expect(Page.GetByText("NDD")).ToBeVisibleAsync();
    }
    [TestMethod]
    public async Task Cadastro_ComEmail_Duplicado_RetornaErro()
    {
        await CadastrarUsuario();

        await Page.GotoAsync(UrlBase + "/Contato/Cadastrar");

        await Page.GetByLabel("Nome").FillAsync("Victor Jeremias");
        await Page.GetByLabel("E-mail").FillAsync("thiagokovalski5@gmail.com"); //email ja cadastrado
        await Page.GetByLabel("Telefone").FillAsync("(49) 98888-8888");
        await Page.GetByLabel("Cargo").FillAsync("Pleno");
        await Page.GetByLabel("Empresa").FillAsync("Google");

        await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();

        string rotaFinal = new Uri(Page.Url).AbsolutePath;
        Assert.AreEqual("/Contato/Cadastrar", rotaFinal);
        await Expect(Page.GetByText("Já existe um contato com este email.")).ToBeVisibleAsync();

    }
    [TestMethod]
    public async Task EditarContato_ComDadosValidos_NaoRetornaErro()
    {
        await CadastrarUsuario();

        await Page.GetByRole(AriaRole.Link, new() { Name = "Editar" }).ClickAsync();

        await Page.GetByLabel("Nome").FillAsync("Victor Jeremias");
        await Page.GetByLabel("E-mail").FillAsync("jeremias@gmail.com");
        await Page.GetByLabel("Telefone").FillAsync("(49) 98888-8888");
        await Page.GetByLabel("Cargo").FillAsync("Pleno");
        await Page.GetByLabel("Empresa").FillAsync("Google");

        await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();

        string rotaFinal = new Uri(Page.Url).AbsolutePath;
        Assert.AreEqual("/Contato/Listar", rotaFinal);
        await Expect(Page.GetByText("Victor Jeremias")).ToBeVisibleAsync();
        await Expect(Page.GetByText("jeremias@gmail.com")).ToBeVisibleAsync();
        await Expect(Page.GetByText("(49) 98888-8888")).ToBeVisibleAsync();
        await Expect(Page.GetByText("Pleno")).ToBeVisibleAsync();
        await Expect(Page.GetByText("Google")).ToBeVisibleAsync();
    }
    private async Task CadastrarUsuario()
    {
        await Page.GotoAsync(UrlBase + "/Contato/Cadastrar");

        await Page.GetByLabel("Nome").FillAsync(nome);
        await Page.GetByLabel("E-mail").FillAsync(email);
        await Page.GetByLabel("Telefone").FillAsync(telefone);
        await Page.GetByLabel("Cargo").FillAsync(cargo);
        await Page.GetByLabel("Empresa").FillAsync(empresa);

        await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();
    }
}
