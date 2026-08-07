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
}
