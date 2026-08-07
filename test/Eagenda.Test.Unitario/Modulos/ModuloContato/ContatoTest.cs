using eAgenda.Dominio.Modulos.ModuloContato;

namespace Eagenda.Test.Unitario.Modulos.ModuloContato;

[TestClass]
public class ContatoTest
{
    [TestMethod]
    public void Cadastrar_Contato_Com_DadosValidos_NaoRetornaErros()
    {
        Contato contato = new("Thiago Kovalski", "Thiago@gmail.com", "(49) 98888-8888", "Dev", "NDD");

        List<string> erros = contato.Validar();

        Assert.HasCount(0, erros);
    }
    [TestMethod]
    public void Cadastrar_contato_apenas_com_os_campos_obrigatórios()
    {
        Contato contato = new("Thiago Kovalski", "Thiago@gmail.com", "(49) 98888-8888", null, null);

        List<string> erros = contato.Validar();

        Assert.HasCount(0, erros);
    }
    [TestMethod]
    public void Cadatrar_Contato_SemCampos_Obrigatorios_CarregaErros()
    {
        Contato contato = new(string.Empty, string.Empty, string.Empty, null, null);

        List<string> erros = contato.Validar();

        Assert.HasCount(3, erros);
        Assert.AreEqual("O campo \"Nome\" deve conter entre 2 e 100 caracteres.", erros[0]);
        Assert.AreEqual("O campo \"E-mail\" deve conter um endereço de e-mail válido.", erros[1]);
        Assert.AreEqual("O campo \"Telefone\" deve estar no formato (XX) XXXX-XXXX ou (XX) XXXXX-XXXX.", erros[2]);
    }
    [TestMethod]
    public void Cadastrar_Contato_Com_NomePequeno_RetornaErros()
    {
        Contato contato = new("T", "Thiago@gmail.com", "(49) 98888-8888", "Dev", "NDD");

        List<string> erros = contato.Validar();

        Assert.HasCount(1, erros);
        Assert.AreEqual("O campo \"Nome\" deve conter entre 2 e 100 caracteres.", erros.First());
    }
    [TestMethod]
    public void Cadastrar_Contato_Com_ComNomeTamanhoMinimo_NaoRetornaErros()
    {
        Contato contato = new("Th", "Thiago@gmail.com", "(49) 98888-8888", "Dev", "NDD");

        List<string> erros = contato.Validar();

        Assert.HasCount(0, erros);
    }
    [TestMethod]
    public void Cadastrar_Contato_Com_ComNomeAcimaDoTamanhoMaximo_RetornaErro()
    {
        Contato contato = new(new string('a', 101), "Thiago@gmail.com", "(49) 98888-8888", "Dev", "NDD");

        List<string> erros = contato.Validar();

        Assert.HasCount(1, erros);
        Assert.AreEqual("O campo \"Nome\" deve conter entre 2 e 100 caracteres.", erros.First());
    }
    [TestMethod]
    public void Cadastrar_Contato_Com_NomeLimiteMaximo_NaoRetornaErro()
    {
        Contato contato = new(new string('a', 100), "Thiago@gmail.com", "(49) 98888-8888", "Dev", "NDD");

        List<string> erros = contato.Validar();

        Assert.HasCount(0, erros);
    }
    [TestMethod]
    public void Cadastrar_Contato_ComEmail_ComFormatoInvalido_RetornaErro()
    {
        Contato contato = new(new string('a', 100), "sememail", "(49) 98888-8888", "Dev", "NDD");

        List<string> erros = contato.Validar();

        Assert.HasCount(1, erros);
        Assert.AreEqual("O campo \"E-mail\" deve conter um endereço de e-mail válido.", erros.First());
    }
    [TestMethod]
    public void Cadastrar_Contato_ComEmail_SemDominio_RetornaErro()
    {
        Contato contato = new(new string('a', 100), "Thiago@gmail", "(49) 98888-8888", "Dev", "NDD");

        List<string> erros = contato.Validar();

        Assert.HasCount(1, erros);
        Assert.AreEqual("O campo \"E-mail\" deve conter um endereço de e-mail válido.", erros.First());
    }
}
