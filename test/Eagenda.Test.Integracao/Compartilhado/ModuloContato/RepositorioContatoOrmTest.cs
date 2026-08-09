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
    [TestMethod]
    public void SelecionarTodos_RetornaDados()
    {
        Contato contato = new("Victor Jeremias", "Victor@gmail.com", "(49) 98888-8888", "Desenvolvedor", "Google");
        repositorioContato.Cadastrar(contato);

        Contato contato2 = new("Thiago Kovalski", "Thiago@gmail.com", "(49) 98888-8888", "Dev", "NDD");
        repositorioContato.Cadastrar(contato2);

        dbContext.ChangeTracker.Clear();

        Contato? contatoSelecionado = repositorioContato.SelecionarPorId(contato.Id);
        Contato? contatoSelecionado2 = repositorioContato.SelecionarPorId(contato2.Id);

        Assert.AreEqual("Victor Jeremias", contatoSelecionado!.Nome);
        Assert.AreEqual("Victor@gmail.com", contatoSelecionado.Email);
        Assert.AreEqual("(49) 98888-8888", contatoSelecionado.Telefone);
        Assert.AreEqual("Desenvolvedor", contatoSelecionado.Cargo);
        Assert.AreEqual("Google", contatoSelecionado.Empresa);

        Assert.AreEqual("Thiago Kovalski", contatoSelecionado2!.Nome);
        Assert.AreEqual("Thiago@gmail.com", contatoSelecionado2.Email);
        Assert.AreEqual("(49) 98888-8888", contatoSelecionado2.Telefone);
        Assert.AreEqual("Dev", contatoSelecionado2.Cargo);
        Assert.AreEqual("NDD", contatoSelecionado2.Empresa);
    }
    [TestMethod]
    public void Dados_SaoListados_Corretamente_NoVisualizar()
    {
        Contato contato = new("Victor Jeremias", "Victor@gmail.com", "(49) 98888-8888", "Desenvolvedor", "Google");
        repositorioContato.Cadastrar(contato);

        dbContext.ChangeTracker.Clear();

        Contato? contatoSelecionado = repositorioContato.SelecionarPorId(contato.Id);

        Assert.IsNotNull(contatoSelecionado);
        Assert.AreEqual("Victor Jeremias", contatoSelecionado.Nome);
        Assert.AreEqual("Victor@gmail.com", contatoSelecionado.Email);
        Assert.AreEqual("(49) 98888-8888", contatoSelecionado.Telefone);
        Assert.AreEqual("Desenvolvedor", contatoSelecionado.Cargo);
        Assert.AreEqual("Google", contatoSelecionado.Empresa);
    }
}
