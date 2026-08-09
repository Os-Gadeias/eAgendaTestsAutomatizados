using eAgenda.Aplicacao.Modulos.ModuloContato;
using eAgenda.Dominio.Modulos.ModuloCompromisso;
using eAgenda.Dominio.Modulos.ModuloContato;
using FluentResults;
using Moq;

namespace Eagenda.Test.Unitario.Modulos.ModuloContato;

[TestClass]
public class ServicoContatoTest
{
    [TestMethod]
    public void CadastrarUsuario_NaoRetornaErros()
    {
        Mock<IRepositorioContato> repositorioContato = new();
        Mock<IRepositorioCompromisso> repositorioCompromisso = new();

        repositorioContato.Setup(r => r.SelecionarTodos()).Returns([]);

        ServicoContato servicoContato = new(repositorioContato.Object, repositorioCompromisso.Object);

        Contato? contatoCadastrado = null!;

        repositorioContato.Setup(r => r.Cadastrar(It.IsAny<Contato>())).Callback<Contato>(c => contatoCadastrado = c);

        Result resultado = servicoContato.Cadastrar(new("Thiago Kovalski", "Thiago@gmail.com", "(49) 98888-8888", "Dev", "NDD"));


        Assert.IsTrue(resultado.IsSuccess);
        Assert.IsNotNull(contatoCadastrado);
        repositorioContato.Verify(r => r.Cadastrar(It.IsAny<Contato>()), Times.Once);
    }
    [TestMethod]
    public void Cadastrar_contato_apenas_com_os_campos_obrigatórios()
    {
        Mock<IRepositorioContato> repositorioContato = new();
        Mock<IRepositorioCompromisso> repositorioCompromisso = new();

        repositorioContato.Setup(r => r.SelecionarTodos()).Returns([]);

        ServicoContato servicoContato = new(repositorioContato.Object, repositorioCompromisso.Object);

        Contato? contatoCadastrado = null!;

        repositorioContato.Setup(r => r.Cadastrar(It.IsAny<Contato>())).Callback<Contato>(c => contatoCadastrado = c);

        Result resultado = servicoContato.Cadastrar(new("Thiago Kovalski", "Thiago@gmail.com", "(49) 98888-8888", null, null));


        Assert.IsTrue(resultado.IsSuccess);
        Assert.IsNotNull(contatoCadastrado);
        repositorioContato.Verify(r => r.Cadastrar(It.IsAny<Contato>()), Times.Once);
    }
    [TestMethod]
    public void Cadatrar_Contato_SemNome_RetornaErro()
    {
        Mock<IRepositorioContato> repositorioContato = new();
        Mock<IRepositorioCompromisso> repositorioCompromisso = new();

        repositorioContato.Setup(r => r.SelecionarTodos()).Returns([]);

        ServicoContato servicoContato = new(repositorioContato.Object, repositorioCompromisso.Object);

        Result resultado = servicoContato.Cadastrar(new(string.Empty, "Thiago@gmail.com", "(49) 98888-8888", null, null));

        Assert.IsTrue(resultado.IsFailed);
        Assert.AreEqual("O campo \"Nome\" deve conter entre 2 e 100 caracteres.", resultado.Errors.First().Message);
        repositorioContato.Verify(r => r.Cadastrar(It.IsAny<Contato>()), Times.Never);

    }
    [TestMethod]
    public void Cadatrar_Contato_SemEmail_RetornaErro()
    {
        Mock<IRepositorioContato> repositorioContato = new();
        Mock<IRepositorioCompromisso> repositorioCompromisso = new();

        repositorioContato.Setup(r => r.SelecionarTodos()).Returns([]);

        ServicoContato servicoContato = new(repositorioContato.Object, repositorioCompromisso.Object);

        Result resultado = servicoContato.Cadastrar(new("Thiago Kovalski", string.Empty, "(49) 98888-8888", null, null));

        Assert.IsTrue(resultado.IsFailed);
        Assert.AreEqual("O campo \"E-mail\" deve conter um endereço de e-mail válido.", resultado.Errors.First().Message);
        repositorioContato.Verify(r => r.Cadastrar(It.IsAny<Contato>()), Times.Never);
    }
    [TestMethod]
    public void Cadatrar_Contato_SemTelefone_RetornaErro()
    {
        Mock<IRepositorioContato> repositorioContato = new();
        Mock<IRepositorioCompromisso> repositorioCompromisso = new();

        repositorioContato.Setup(r => r.SelecionarTodos()).Returns([]);

        ServicoContato servicoContato = new(repositorioContato.Object, repositorioCompromisso.Object);

        Result resultado = servicoContato.Cadastrar(new("Thiago Kovalski", "Thiago@gmail.com", string.Empty, null, null));

        Assert.IsTrue(resultado.IsFailed);
        Assert.AreEqual("O campo \"Telefone\" deve estar no formato (XX) XXXX-XXXX ou (XX) XXXXX-XXXX.", resultado.Errors.First().Message);
        repositorioContato.Verify(r => r.Cadastrar(It.IsAny<Contato>()), Times.Never);

    }
    [TestMethod]
    public void Cadastrar_ContatoComEmail_Duplicado_RetornaErro()
    {
        Mock<IRepositorioContato> repositorioContato = new();
        Mock<IRepositorioCompromisso> repositorioCompromisso = new();

        repositorioContato.Setup(r => r.SelecionarTodos()).Returns([new Contato("Victor Jeremias", "ThiagoKovalski@Gmail.com", "(49) 98888-7777", null, null)]);

        ServicoContato servicoContato = new(repositorioContato.Object, repositorioCompromisso.Object);

        Result resultado = servicoContato.Cadastrar(new("Thiago Kovalski", "ThiagoKovalski@Gmail.com", "(49) 98888-8888", null, null));

        Assert.IsTrue(resultado.IsFailed);
        Assert.AreEqual("Já existe um contato com este email.", resultado.Errors.First().Message);
        repositorioContato.Verify(r => r.Cadastrar(It.IsAny<Contato>()), Times.Never);
    }
    [TestMethod]
    public void Cadastrar_ContatoComTelefone_Duplicado_RetornaErro()
    {
        Mock<IRepositorioContato> repositorioContato = new();
        Mock<IRepositorioCompromisso> repositorioCompromisso = new();

        repositorioContato.Setup(r => r.SelecionarTodos()).Returns([new Contato("Victor Jeremias", "VictorJeremias@gmail.com", "(49) 98888-8888", null, null)]);

        ServicoContato servicoContato = new(repositorioContato.Object, repositorioCompromisso.Object);

        Result resultado = servicoContato.Cadastrar(new("Thiago Kovalski", "ThiagoKovalski@Gmail.com", "(49) 98888-8888", null, null));

        Assert.IsTrue(resultado.IsFailed);
        Assert.AreEqual("Já existe um contato com este telefone.", resultado.Errors.First().Message);
        repositorioContato.Verify(r => r.Cadastrar(It.IsAny<Contato>()), Times.Never);
    }
    [TestMethod]
    public void Editar_Contato_Com_DadosValidos_Persiste()
    {

        Mock<IRepositorioContato> repositorioContato = new();
        Mock<IRepositorioCompromisso> repositorioCompromisso = new();
        ServicoContato servicoContato = new(repositorioContato.Object, repositorioCompromisso.Object);

        Contato contato = new("Victor Jeremias", "VictorJeremias@gmail.com", "(49) 98888-7777", "Senior", "Google");

        repositorioContato.Setup(r => r.SelecionarTodos()).Returns([]);
        repositorioContato.Setup(r => r.SelecionarPorId(It.IsAny<Guid>())).Returns(new Contato("Victor Jeremias", "VictorJeremias@gmail.com", "(49) 98888-7777", "Senior", "Google"));

        repositorioContato.Setup(r => r.Editar(It.IsAny<Guid>(), It.IsAny<Contato>())).Returns(true);

        Result resultado = servicoContato.Editar(new(contato.Id, "Thiago Kovalski", "Thiago@gmail.com", "(49) 98888-8888", "Dev", "NDD"));

        Assert.IsTrue(resultado.IsSuccess);
        repositorioContato.Verify(r => r.Editar(It.IsAny<Guid>(), It.IsAny<Contato>()), Times.Once);
    }
    [TestMethod]
    public void EditarContato_ComEmail_JaCadastrado_RetornaErro_E_NaoEdita()
    {
        Mock<IRepositorioContato> repositorioContato = new();
        Mock<IRepositorioCompromisso> repositorioCompromisso = new();
        ServicoContato servicoContato = new(repositorioContato.Object, repositorioCompromisso.Object);

        Contato contato = new("Victor Jeremias", "VictorJeremias@gmail.com", "(49) 98888-7777", "Senior", "Google");

        repositorioContato.Setup(r => r.SelecionarTodos()).Returns([new Contato("Victor Jeremias", "VictorJeremias@gmail.com", "(49) 98888-7777", "Senior", "Google")]);

        Result resultado = servicoContato.Editar(new(contato.Id, "Thiago Kovalski", "VictorJeremias@gmail.com", "(49) 98888-8888", "Dev", "NDD"));

        Assert.IsTrue(resultado.IsFailed);
        Assert.Contains("Já existe", resultado.Errors.First().Message);
        repositorioContato.Verify(r => r.Editar(It.IsAny<Guid>(), It.IsAny<Contato>()), Times.Never);
    }
    [TestMethod]
    public void EditarContato_MantendoProprioEmail_E_Telefone_NaoRetornaErros()
    {
        Mock<IRepositorioContato> repositorioContato = new();
        Mock<IRepositorioCompromisso> repositorioCompromisso = new();
        ServicoContato servicoContato = new(repositorioContato.Object, repositorioCompromisso.Object);

        Contato contato = new("Victor Jeremias", "VictorJeremias@gmail.com", "(49) 98888-7777", "Senior", "Google");

        repositorioContato.Setup(r => r.SelecionarTodos()).Returns([contato]);
        repositorioContato.Setup(r => r.Editar(It.IsAny<Guid>(), It.IsAny<Contato>())).Returns(true);

        Result resultado = servicoContato.Editar(new(contato.Id, "Victor Augusto", "VictorJeremias@gmail.com", "(49) 98888-7777", "Pleno", "NDD"));

        Assert.IsTrue(resultado.IsSuccess);
        repositorioContato.Verify(r => r.Editar(It.IsAny<Guid>(), It.IsAny<Contato>()), Times.Once);
    }
}
