using eAgenda.Dominio.Modulos.ModuloContato;
using eAgenda.Infra.Modulos.ModuloContato;
using FizzWare.NBuilder;

namespace Eagenda.Test.Integracao.Compartilhado.ModuloContato;

[TestClass]
public class RepositorioContatoOrmTest : RepositorioOrmTestBase
{
    [TestMethod]
    public void CadastrarContato_Valido_PersisteDados()
    {
        Contato contato = new("Thiago Kovalski", "Thiago@gmail.com", "(49) 98888-8888", "Dev", "NDD");

        repositorioContato.Cadastrar(contato);

        dbContext.ChangeTracker.Clear();

        Contato? contatoSelecionado = repositorioContato.SelecionarPorId(contato.Id);

        Assert.IsNotNull(contatoSelecionado);
        Assert.AreEqual("Thiago Kovalski", contatoSelecionado.Nome);
        Assert.AreEqual("Thiago@gmail.com", contatoSelecionado.Email);
        Assert.AreEqual("(49) 98888-8888", contatoSelecionado.Telefone);
        Assert.AreEqual("(49) 98888-8888", contatoSelecionado.Telefone);
        Assert.AreEqual("Dev", contatoSelecionado.Cargo);
        Assert.AreEqual("NDD", contatoSelecionado.Empresa);

    }
}
