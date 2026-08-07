using System.Text.RegularExpressions;
using Eagenda.Test.E2E.Compartilhado;
using Microsoft.Playwright.MSTest;

namespace Eagenda.Test.E2E;

[TestClass]
public sealed class Test1 : PageTest
{
    [TestMethod]
    public async Task TestMethod1()
    {
        await Page.GotoAsync("https://playwright.dev");

        await Expect(Page).ToHaveTitleAsync(new Regex("Playwright"));
    }
}
