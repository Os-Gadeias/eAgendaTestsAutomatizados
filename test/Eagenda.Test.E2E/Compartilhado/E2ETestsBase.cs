using Microsoft.Playwright.MSTest;

namespace Eagenda.Test.E2E.Compartilhado;

public abstract class E2ETestsBase : PageTest
{
    protected TestApplicationFactory Aplicacao = null!;
    protected string UrlBase { get; set; } = string.Empty;

    [TestInitialize]
    public async Task InicializarAplicacao()
    {
        Aplicacao = new TestApplicationFactory();

        UrlBase = Aplicacao.UrlBase!;
    }
    [TestCleanup]
    public async Task LiberarAplicacao()
    {
        try
        {
            if (Aplicacao is not null)
                await Aplicacao.DisposeAsync();

        }
        finally
        {
            Aplicacao = null!;
        }
    }
}
