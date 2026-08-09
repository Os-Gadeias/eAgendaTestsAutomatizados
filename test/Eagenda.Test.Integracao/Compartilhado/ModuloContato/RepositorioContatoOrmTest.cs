using System.Formats.Cbor;
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
    [TestMethod]
    public void Cadastrar_contato_apenas_com_os_campos_obrigatórios()
    {
        Contato contato = new("Thiago Kovalski", "Thiago@gmail.com", "(49) 98888-8888", null, null);

        repositorioContato.Cadastrar(contato);

        dbContext.ChangeTracker.Clear();

        Contato? contatoSelecionado = repositorioContato.SelecionarPorId(contato.Id);

        Assert.IsNotNull(contatoSelecionado);
        Assert.AreEqual("Thiago Kovalski", contatoSelecionado.Nome);
        Assert.AreEqual("Thiago@gmail.com", contatoSelecionado.Email);
        Assert.AreEqual("(49) 98888-8888", contatoSelecionado.Telefone);
        Assert.IsNull(contatoSelecionado.Cargo);
        Assert.IsNull(contatoSelecionado.Empresa);
    }
    [TestMethod]
    public void Editar_Contato_Com_DadosValidos_Persiste()
    {
        Contato contato = new("Victor Jeremias", "Victor@gmail.com", "(49) 98888-8888", null, null);

        repositorioContato.Cadastrar(contato);

        Contato contatoEditado = new("Thiago Kovalski", "Thiago@gmail.com", "(49) 98888-7777", "Dev", "NDD");

        bool conseguiuEditar = repositorioContato.Editar(contato.Id, contatoEditado);

        dbContext.ChangeTracker.Clear();

        Contato? contatoSelecionado = repositorioContato.SelecionarPorId(contato.Id);

        Assert.IsNotNull(contatoSelecionado);
        Assert.IsTrue(conseguiuEditar);
        Assert.AreEqual("Thiago Kovalski", contatoSelecionado.Nome);
        Assert.AreEqual("Thiago@gmail.com", contatoSelecionado.Email);
        Assert.AreEqual("(49) 98888-7777", contatoSelecionado.Telefone);
        Assert.AreEqual("Dev", contatoSelecionado.Cargo);
        Assert.AreEqual("NDD", contatoSelecionado.Empresa);
    }
    [TestMethod]
    public void ExcluirContato_PersisteExclusao()
    {
        Contato contato = new("Victor Jeremias", "Victor@gmail.com", "(49) 98888-8888", null, null);

        repositorioContato.Cadastrar(contato);

        bool conseguiuExcluir = repositorioContato.Excluir(contato.Id);

        dbContext.ChangeTracker.Clear();

        Contato? contatoSelecionado = repositorioContato.SelecionarPorId(contato.Id);

        Assert.IsTrue(conseguiuExcluir);
        Assert.IsNull(contatoSelecionado);
    }
}
