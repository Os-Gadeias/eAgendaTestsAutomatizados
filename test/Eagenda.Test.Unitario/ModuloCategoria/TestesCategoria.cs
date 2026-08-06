using eAgenda.Dominio.Modulos.ModuloCategoria;

[TestClass]
public sealed class TestesCategoria
{
    [TestMethod]
    public void ValidarSem_Titulo_DeveRetornar_ErroCompativel()
    {
        Categoria categoria = new Categoria(null);

        List<string> erros = categoria.Validar();

        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Título\" deve ser preenchido!",
            erros.First());
    }

    [TestMethod]
    public void Validar_PoucasLetrasTituto_DeveRetornar_ErroCompativel()
    {
        Categoria categoria = new Categoria("I");

        List<string> erros = categoria.Validar();

        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Titulo\" deve conter mais que 2 caracteres!",
            erros.First()
        );
    }

    [TestMethod]
    public void Validar_MuitosLetrasTitulo_DeveRetornar_ErroCompativel()
    {
        Categoria categoria = new Categoria(new string('A', 101));

        List<string> erros = categoria.Validar();

        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Titulo\" deve comter menos que 100 caracteres!",
            erros.First()
        );
    }
}
