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
}
